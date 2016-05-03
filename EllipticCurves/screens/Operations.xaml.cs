using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class Operations : ContentPage
	{
		StartPage parent;

		ECPoint point;
		ECPoint additionPoint;
		BigInteger z;

		string errorX="";
		string errorY="";
		string errorZ="";

		public Operations (StartPage page, ECPoint sended_point)
		{
			InitializeComponent ();

			parent = page;

			additionPoint = new ECPoint ();

			point = sended_point ?? new ECPoint ();

		    additionPoint.elliptic_curve = point.elliptic_curve; 

            additionPoint.elliptic_curve.a = point.elliptic_curve.a;
			additionPoint.elliptic_curve.b = point.elliptic_curve.b;
			additionPoint.elliptic_curve.p = point.elliptic_curve.p;

			this.BindingContext = point;

			buttonMult.IsEnabled = false;
			buttonAddition.IsEnabled = additionPoint.x != 0 && additionPoint.y != 0;

			string text = "EC: Y^2 = X^3";
			if (point.a != 0) {
				text += $"+{point.elliptic_curve.a:F0}*X";
			}

			if (point.b != 0) {
				text += $"+{point.elliptic_curve.b:F0}";
			}

			labelEC.Text = text;
			labelPoint.Text = $"Генерирующая точка [{point.x:F0} ; {point.y:F0}])";
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

        protected void invalidateErrors()
        {
            clearTracing();

            var isError = false;

			if (errorX != "") {
                trace(errorX, Color.Red);
                isError = true;
			}
			if (errorY != "")
            {
                trace(errorY, Color.Red);
                isError = true;
			}
			if (errorZ != "") {
                trace(errorZ, Color.Red);	
				isError = true;
			}

			frameResult.OutlineColor = isError ? Color.Red : Color.Green;
		}


		protected void addECtoFrame(ECPoint p){

            clearTracing();

            string result = getEtoString (p);
            
			result = p.ToString();

            trace(result,Color.Green);
		}

		// HANDLERS

		protected string getEtoString(ECPoint p){
			string result = "E: Y^2=X^3";

			if (p.a != 0)
				result += " + " + p.a.ToString () + "*X";

			if (p.b != 0)
				result += " + " + p.b.ToString ();

			result += " (mod " + p.p +  ")";

			return result;
		}

		public void handler_changedXValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					additionPoint.x = new BigInteger(entryX.Text, 10);
					buttonAddition.IsEnabled = true;
				}
				errorX = "";
			} catch{
				buttonAddition.IsEnabled = false;
				errorX = "Неверное значение 'x' = " + entryX.Text;
			} finally{
				invalidateErrors();
			}
		}


		public void handler_changedYValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					additionPoint.y = new BigInteger(entryY.Text, 10);
					buttonAddition.IsEnabled = true;
				}
				errorY = "";
			} catch{
				buttonAddition.IsEnabled = false;
				errorY = "Неверное значение 'y' = " + entryY.Text;
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedZValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					z = new BigInteger(entryZ.Text, 10);
				}

				if( z== 0){
					throw new Exception("not valid");
				}

				errorZ = "";
				buttonMult.IsEnabled = true;
			} catch{
				errorZ = "Неверное значение 'z' = " + entryZ.Text;
				buttonMult.IsEnabled = false;
			} finally{
				invalidateErrors();
			}
		}

		protected void handler_buttonDoublingClick(object sender, EventArgs e)
        {
            trace("Удвоение:", Color.Green);

            ECPoint doubling = ECPoint.Double (point);
            
			addECtoFrame (doubling);
		}

		protected void handler_buttonAdditionClick(object sender, EventArgs e)
        {
            trace("Сложение точек:", Color.Green);

            additionPoint.x = additionPoint.x % point.p;
			additionPoint.y = additionPoint.y % point.p;

			var resultPoint = point + additionPoint;

            addECtoFrame (resultPoint);
		}

		protected void handler_buttonMultClick(object sender, EventArgs e)
        {
            trace("Умножение на число:", Color.Green);

            var resultPoint = ECPoint.multiply(z,point);

			addECtoFrame (resultPoint);
		}


		private void handler_buttonCryptoClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync( new CryptoExamples( parent, point ) );
		}

	}
}

