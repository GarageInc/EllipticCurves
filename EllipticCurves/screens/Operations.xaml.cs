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

		public Operations (StartPage page, ECPoint param)
		{
			InitializeComponent ();

			parent = page;

			additionPoint = new ECPoint ();

			if (param != null) {

				point = param;
			} else {

				point = new ECPoint ();
			}

			additionPoint.a = point.a;
			additionPoint.b = point.b;
			additionPoint.p = point.p;

			this.BindingContext = point;

			buttonMult.IsEnabled = false;
		}


		protected void invalidateErrors(){
			stackResults.Children.Clear ();

			bool isError = false;

			if (errorX != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorX,VerticalOptions=LayoutOptions.StartAndExpand });	
				isError = true;
			}
			if (errorY != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorY,VerticalOptions=LayoutOptions.StartAndExpand });	
				isError = true;
			}
			if (errorZ != "") {
				stackResults.Children.Add (new Label { TextColor=Color.Red, Text = errorZ,VerticalOptions=LayoutOptions.StartAndExpand });	
				isError = true;
			}

			if (isError) {
				frameResult.OutlineColor = Color.Red;		
				//buttonDoubling.IsEnabled = false;
			} else {
				frameResult.OutlineColor = Color.Green;
				//buttonDoubling.IsEnabled = true;
			}
		}


		protected void addECtoFrame(ECPoint p){

			string result = getEtoString (p);

			stackResults.Children.Clear ();

			// stackResults.Children.Add (new Label { TextColor=Color.Green, Text = result, VerticalOptions=LayoutOptions.StartAndExpand });

			result = Functions.getPointtoString (p);
			stackResults.Children.Add (new Label { TextColor=Color.Green, Text = result, VerticalOptions=LayoutOptions.StartAndExpand });

			frameResult.OutlineColor = Color.Green;
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
				}
				errorX = "";
				buttonAddition.IsEnabled = true;
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
				}
				errorY = "";
				buttonAddition.IsEnabled = true;
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
					throw Exception("not valid");
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
			ECPoint doubling = ECPoint.Double (point);

			stackResults.Children.Add (new Label { TextColor=Color.Green, Text = "Удвоение:", VerticalOptions=LayoutOptions.StartAndExpand });
			addECtoFrame (doubling);
		}

		protected void handler_buttonAdditionClick(object sender, EventArgs e)
		{
			var resultPoint = point + additionPoint;

			stackResults.Children.Add (new Label { TextColor=Color.Green, Text = "Сложение точек:", VerticalOptions=LayoutOptions.StartAndExpand });
			addECtoFrame (resultPoint);
		}

		protected void handler_buttonMultClick(object sender, EventArgs e)
		{
			var resultPoint = ECPoint.multiply(z,point);

			stackResults.Children.Add (new Label { TextColor=Color.Green, Text = "Умножение на число:", VerticalOptions=LayoutOptions.StartAndExpand });
			addECtoFrame (resultPoint);
		}


		private void handler_buttonCryptoClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new CryptoExamples(parent, point));
		}

	}
}

