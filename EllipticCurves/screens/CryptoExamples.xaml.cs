using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class CryptoExamples : ContentPage
	{
		StartPage parent;

		EC curve;
        
        List<string> errors = new List<string>();

        public CryptoExamples (StartPage page, ECPoint p)
		{
			InitializeComponent ();

			parent = page;

            if (p != null)
            {
                curve = p.elliptic_curve;
                curve.generationPoint = p;
            }
            else
            {
                curve = new EC();
            }
            
			invalidateGenPoint ();
		}

		protected void invalidateErrors()
        {
            clearTracing();

            bool isError = false;

            if (curve.p != 0 && curve.isValidatedCoefs)
            {
                isError = !curve.isNotSingular;

                if (curve.isNotSingular)
                {
                    trace("Кривая не сингулярная(гладкая)", Color.Green);
                }
                else
                {
                    trace("Кривая сингулярная: не выполняется условие 4*a^3 + 27*b^2 != 0, криптографические операции не применимы", Color.Red);
                }
            }

            if (errors.Count > 0)
            {
                isError = true;

                foreach (var e in errors)
                {
                    trace(e, Color.Red);
                }

                errors.Clear();
            }

            frameResult.OutlineColor = isError ? Color.Red : Color.Green;
		}


		public void invalidateGenPoint(){
            if (curve.generationPoint.isBelongToCurve())
            {
                trace("Точка принадлежит прямой", Color.Green);

                var totalPointsCount = curve.GetAllPoints().Count + 1;
                BigInteger countInOrders = curve.generationPoint.countPointsInOrder();
                
                var h = totalPointsCount / countInOrders;


                if (!Functions.IsSimple(countInOrders))
                {
                    trace("Порядок точки - НЕ простое число!", Color.Red);
                    setButtonsState(false);

                    return ;
                }

                if ((h * countInOrders) == totalPointsCount && h > 0 && h < 6)
                {
                    trace(
                        "Условие выполняется: " + totalPointsCount + " = " + h + " * " + countInOrders +
                        ", где второй множитель - порядок генерирующей точки", Color.Green);

                    if (totalPointsCount != countInOrders)
                    {
                        trace("Проверка условия: " + totalPointsCount + " != " + countInOrders, Color.Green);
                        setButtonsState(true);
                    }
                    else
                    {
                        trace("Ошибка условия: ошибочно, что " + totalPointsCount + " != " + countInOrders, Color.Red);
                        setButtonsState(false);
                    }
                }
                else
                {
                    trace("Условие НЕ выполняется: " + totalPointsCount + " = " + h + " * " + countInOrders +
                        ", где второй множитель - порядок генерирующей точки", Color.Red);
                    setButtonsState(false);
                }
            }
            else
            {

                trace("Точка не принадлежит прямой!", Color.Red);
                setButtonsState(false);
            }
        }

        void clearPointEntries()
        {
            entryX.Text = "";
            entryY.Text = "";
        }

        public void setButtonsState( bool state ){
			gost3410Button.IsEnabled = state;
		}
        
        protected void clearTracing()
        {
            frameResult.OutlineColor = Color.Green;
            stackResults.Children.Clear();
        }

        protected void trace(string message, Color color)
        {
            frameResult.OutlineColor = color;
            stackResults.Children.Add(new Label { TextColor = color, Text = message, VerticalOptions = LayoutOptions.StartAndExpand });
        }
        

		// HANDLERS

		public void handler_changedPValidate(object sender, EventArgs e)
        {
            clearPointEntries();
            try
			{
			    Entry current = sender as Entry;
			    curve.p = !string.IsNullOrEmpty(current.Text) ? new BigInteger(entryP.Text, 10) : 0;
			}
			catch{
				errors.Add("Неверное значение 'p' = " + entryP.Text);
			} finally{
				invalidateErrors();
			}
		}
		public void handler_changedAValidate(object sender, EventArgs e)
        {
            clearPointEntries();
            try
			{
			    Entry current = sender as Entry;
			    curve.a = !string.IsNullOrEmpty(current.Text) ? new BigInteger(entryA.Text, 10) : 0;
			}
			catch{
                errors.Add("Неверное значение 'a' = " + entryA.Text);
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedBValidate(object sender, EventArgs e)
        {
            clearPointEntries();
            try
			{
			    Entry current = sender as Entry;
			    curve.b = !string.IsNullOrEmpty(current.Text) ? new BigInteger(entryB.Text, 10) : 0;
			}
			catch{
                errors.Add("Неверное значение 'b' = " + entryB.Text);
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedXValidate(object sender, EventArgs e)
        {
            try
            {
                Entry current = sender as Entry;
                curve.generationPoint.x = !string.IsNullOrEmpty(current.Text) ? new BigInteger(entryX.Text, 10) : 0;
            }
            catch{
                errors.Add("Неверное значение 'x' = " + entryX.Text);
			} finally{
				invalidateErrors();
				invalidateGenPoint ();
			}
		}

		public void handler_changedYValidate(object sender, EventArgs e)
        {
            try
            {
                Entry current = sender as Entry;
                curve.generationPoint.y = !string.IsNullOrEmpty(current.Text) ? new BigInteger(entryY.Text, 10) : 0;
            }
            catch{
                errors.Add("Неверное значение 'y' = " + entryY.Text);
			} finally{
				invalidateErrors();
				invalidateGenPoint ();
			}
		}
        
        
		public void handler_gost3410ButtonClick(object sender, EventArgs e){

            clearTracing();
			
			try
            {
                // Сначала нужно получить порядок точки эллиптической кривой
                BigInteger k = this.curve.generationPoint.countPointsInOrder();
				trace ("0) Порядок точки 'k' = " + k, Color.Green);

				string message = "'какой-то текст'";
				trace ("1) Проведём шифрование сообщения " + message, Color.Green);
			
				DSGost DS = new DSGost(curve.p, curve.a, curve.b, k, System.Text.Encoding.Unicode.GetBytes(message));
				BigInteger d=DS.GenPrivateKey(2);

				trace ("2) Сгенерирован приватный ключ 'd'=" + d, Color.Green);
				ECPoint Q = DS.GenPublicKey(d);  
				trace ("3) Сгенерирован публичный от 'd' ключ 'Q' = " + Q, Color.Green);

				trace("4) ...генерируем хеш ГОСТ...", Color.Green);
				GOST hash = new GOST(256);
				byte[] H = hash.GetHash(System.Text.Encoding.Unicode.GetBytes("Message"));

				trace ("5) Получен ГОСТ-хэш(от сообщения) 'H' = " + System.Text.Encoding.Unicode.GetString(H,0,H.Length), Color.Green);

				string sign  = DS.SignGen(H, d);
				trace ("6) Получена цифровая подпись (по секретному ключу 'd') 'sign' = " + sign, Color.Green);


				bool result = DS.SignVer(H, sign, Q);  
				trace ("7) Удалось ли провести верификацию(с открытым ключом 'Q')? :" + (result ? "да" : "нет"), Color.Green);
			} catch(Exception er){
				
				trace ("Случилась какая-то ошибка в формировании ключа. Вы всё ввели правильно?",Color.Red);
			}
		}
	}
}

