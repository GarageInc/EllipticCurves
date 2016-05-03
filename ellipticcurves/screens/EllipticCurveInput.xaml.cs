using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class EllipticCurveInput: ContentPage
	{
		StartPage parent;

		EC curve;

		List<string> errors = new List<string> ();

		public EllipticCurveInput (StartPage page)
		{
			InitializeComponent ();

			parent = page;

			curve = new EC ();

			invalidateErrors ();
		}

	    protected void clearTracing()
	    {
            frameResult.OutlineColor = Color.Green;
            stackResults.Children.Clear();
        }

		protected void trace(string message, Color color){

			frameResult.OutlineColor = color;

			stackResults.Children.Add (new Label { TextColor=color, Text = message,VerticalOptions=LayoutOptions.StartAndExpand });
		}

		protected void invalidateErrors(){
            clearTracing();

		    bool isError = false;

		    if (curve.p != 0 && curve.isValidatedCoefs )
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

			if ( errors.Count > 0) {
				isError = true;

				foreach ( var e in errors ) {
					trace ( e, Color.Red );
				}

				errors.Clear ();
			}
            
            getCountButton.IsEnabled = curve.generationPoint.isValidatedAll && !isError;

			genRandomPointButton.IsEnabled = curve.generationPoint.isBelongToCurve() && !isError;

			operationsButton.IsEnabled = curve.generationPoint.isValidatedAll && !isError;
            
			labelCountPoints.Text = "";
		}

		public void invalidateGenPoint(){
			if (curve.generationPoint.isBelongToCurve()) {

				trace ("Точка принадлежит прямой", Color.Green);
            } else {

				trace ("Точка не принадлежит прямой!", Color.Red);
			}
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

		public void handler_changedXValidate(object sender, EventArgs e){
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

		public void handler_changedYValidate(object sender, EventArgs e){
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


	    void clearPointEntries()
	    {
	        entryX.Text = "";
	        entryY.Text = "";

	        labelOrderK.Text = "";
	        labelCountPoints.Text = "";
	    }

		private void handler_genRandomPointButtonClick(object sender, EventArgs e)
		{
			setPoint ();
		}

		public void setPoint(){

			ECPoint result = curve.getNext();

			entryX.Text = result.x.ToString ();
			entryY.Text = result.y.ToString ();
		}

	    protected void setGenerationPoint()
	    {
	        curve.generationPoint.x = !string.IsNullOrEmpty(entryX.Text) ? new BigInteger(entryX.Text, 10) : 0;
	        curve.generationPoint.y = !string.IsNullOrEmpty(entryY.Text) ? new BigInteger(entryY.Text, 10) : 0;
	    }

	    private void handler_getCountButtonClick(object sender, EventArgs e)
		{
            setGenerationPoint();
			List<ECPoint> points = curve.GetAllPoints ();

			string outputS = "";

			foreach(var p in points){
				outputS += p + " ";
			}

            clearTracing();

            trace (outputS, Color.Green);

	        var totalPointsCount = points.Count + 1;

            labelCountPoints.Text = totalPointsCount + "( 1 бесконечная )";

            setGenerationPoint();

	        BigInteger countInOrders = curve.generationPoint.countPointsInOrder();
            labelOrderK.Text = countInOrders.ToString();

	        if (!Functions.IsSimple(countInOrders))
	        {
	            trace("Порядок точки - НЕ простое число!", Color.Red);
	            return;
	        }

            var h = totalPointsCount / countInOrders;
	        if ((h * countInOrders) == totalPointsCount && h > 0 && h < 6)
	        {
	            trace(
	                "Условие выполняется: " + totalPointsCount + " = " + h + " * " + countInOrders +
	                ", где второй множитель - порядок генерирующей точки", Color.Green);

	            if (totalPointsCount != countInOrders)
	            {
                    trace("Проверка условия: " + totalPointsCount + " != " + countInOrders,Color.Green);
	            }
	            else
	            {
                    trace("Ошибка условия: ошибочно, что " + totalPointsCount + " != " + countInOrders, Color.Red);
                }
	        }
	        else
	        {
                trace("Условие НЕ выполняется: " + totalPointsCount + " = " + h + " * " + countInOrders +
                    ", где второй множитель - порядок генерирующей точки", Color.Red);
	        }
		}
			

		private void handler_operationsButtonClick(object sender, EventArgs e)
		{
		    setGenerationPoint();
            this.Navigation.PushAsync(new Operations(parent, curve.generationPoint));
		}
	}
}

