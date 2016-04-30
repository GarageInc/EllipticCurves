using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class CryptoExamples : ContentPage
	{
		StartPage parent;

		ECPoint point;

		string errorP="";
		string errorA="";
		string errorB="";
		string errorX="";
		string errorY="";

		public CryptoExamples (StartPage page, ECPoint p = null)
		{
			InitializeComponent ();

			parent = page;

			if (p == null) {
				point = new ECPoint ();
			} else {
				point = p;
			}			

			this.BindingContext = point;

			invalidateGenPoint ();
		}

		protected void invalidateErrors(){
			stackResults.Children.Clear ();

			bool isError = point.elliptic_curve.isNotSingular;

			if ( point.elliptic_curve.isNotSingular ) {
				trace ("Кривая сингулярная: не выполняется условие 4*a^3 + 27*b^2 = 0, криптографические операции не применимы");
			} else {
				trace ("Кривая не сингулярная(гладкая)");
			}

			if (errorP != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorP,VerticalOptions=LayoutOptions.StartAndExpand });
				isError = true;
			}
			if (errorA != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorA, VerticalOptions=LayoutOptions.StartAndExpand });
				isError = true;			
			}
			if (errorB != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorB,VerticalOptions=LayoutOptions.StartAndExpand });	
				isError = true;
			}
			if (errorX != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorX,VerticalOptions=LayoutOptions.StartAndExpand });	
				isError = true;
			}
			if (errorY != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorY,VerticalOptions=LayoutOptions.StartAndExpand });	
				isError = true;
			}

			if (isError) {
				frameResult.OutlineColor = Color.Red;
			} else {
				frameResult.OutlineColor = Color.Green;
			}

		}


		public void invalidateGenPoint(){
			if (point!=null && point.p != 0) {

				stackResults.Children.Clear ();

				if (((point.y * point.y) % point.p ) == ((point.x * point.x * point.x + point.x * point.a + point.b) % point.p)) {

					frameResult.OutlineColor = Color.Green;
					stackResults.Children.Add (new Label {
						TextColor = Color.Green,
						Text = "Точка принадлежит прямой.",
						VerticalOptions = LayoutOptions.StartAndExpand
					});	
					setButtonsStates(true);
				} else {

					frameResult.OutlineColor = Color.Red;
					stackResults.Children.Add (new Label {
						TextColor = Color.Red,
						Text = "Точка не принадлежит прямой!",
						VerticalOptions = LayoutOptions.StartAndExpand
					});	
					setButtonsStates(false);
				}
			} else {

				setButtonsStates(false);
			}
		}

		public void setButtonsStates( bool state ){
			gost3410Button.IsEnabled = state;
		}


		public void trace(string message){

			stackResults.Children.Add (new Label { TextColor=Color.Green, Text = message,VerticalOptions=LayoutOptions.StartAndExpand });
		}

		public void traceError(string message){

			stackResults.Children.Add (new Label { TextColor=Color.Red, Text = message,VerticalOptions=LayoutOptions.StartAndExpand });
		}

		// HANDLERS

		public void handler_changedPValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					point.p = new BigInteger(entryP.Text, 10);
				}
				errorP = "";
			} catch{
				errorP = "Неверное значение 'p' = " + entryP.Text;
			} finally{
				invalidateErrors();
			}
		}
		public void handler_changedAValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					point.a = new BigInteger(entryA.Text, 10);
				}
				errorA = "";
			} catch{
				errorA = "Неверное значение 'a' = " + entryA.Text;
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedBValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					point.b = new BigInteger(entryB.Text, 10);
				}
				errorB = "";
			} catch{
				errorB = "Неверное значение 'b' = " + entryB.Text;
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedXValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					point.x = new BigInteger(entryX.Text, 10);
				}
				errorX = "";
			} catch{
				errorX = "Неверное значение 'x' = " + entryX.Text;
			} finally{
				invalidateErrors();
				invalidateGenPoint ();
			}
		}

		public void handler_changedYValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					point.y = new BigInteger(entryY.Text, 10);
				}
				errorY = "";
			} catch{
				errorY = "Неверное значение 'y' = " + entryY.Text;
			} finally{
				invalidateErrors();
				invalidateGenPoint ();
			}
		}

		public BigInteger getRandomPointCoord_y(){
			l1:
			BigInteger y = ((startGen * startGen * startGen + startGen * point.a + point.b) % point.p ).sqrt();

			if (((y * y) % point.p) == ((startGen * startGen * startGen + startGen * point.a + point.b) % point.p)) {

				startGen = (startGen+1) % point.p;
				return y;
			} else {

				startGen++;
				startGen = startGen % point.p;
				goto l1;
			}
		}

		BigInteger startGen = 0;
		public void handler_gost3410ButtonClick(object sender, EventArgs e){

			stackResults.Children.Clear ();

			// Сначала нужно получить порядок точки эллиптической кривой
			List<ECPoint> points = new List<ECPoint>();
			ECPoint p = new ECPoint(point);

			int i = 2;
			while( !Functions.isFirst(points,p) ) {

				if (!Functions.Contains (points, p)) {
					points.Add (p);
				}// pass

				p = ECPoint.multiply (i, point);
				i++;
			}

			try{
				int k = points.Count + 1;
				trace ("0) Порядок точки 'k' = " + k.ToString ());

				string message = "'какой-то текст'";
				trace ("1) Проведём шифрование сообщения " + message);
			
				DSGost DS = new DSGost(point.p, point.a, point.b, k, System.Text.Encoding.Unicode.GetBytes(message));
				BigInteger d=DS.GenPrivateKey(5);

				trace ("2) Сгенерирован приватный ключ 'd'=" + d.ToString ());
				ECPoint Q = DS.GenPublicKey(d);  
				trace ("3) Сгенерирован публичный от 'd' ключ 'Q' = " + Q.ToString ());

				trace("4) ...генерируем хеш ГОСТ...");
				GOST hash = new GOST(256);
				byte[] H = hash.GetHash(System.Text.Encoding.Unicode.GetBytes("Message"));

				trace ("5) Получен ГОСТ-хэш(от сообщения) 'H' = " + System.Text.Encoding.Unicode.GetString(H,0,H.Length));

				string sign  = DS.SignGen(H, d);
				trace ("6) Получена цифровая подпись (по секретному ключу 'd') 'sign' = " + sign);


				bool result = DS.SignVer(H, sign, Q);  
				trace ("7) Удалось ли провести верификацию(с открытым ключом 'Q')? :" + (result ? "да" : "нет"));
			} catch(Exception er){
				
				traceError ("Случилась какая-то ошибка в формировании ключа. Вы всё ввели правильно?");
			}
		}
	}
}

