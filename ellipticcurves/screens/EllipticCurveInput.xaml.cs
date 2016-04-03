using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class EllipticCurveInput: ContentPage
	{
		StartPage parent;

		ECPoint point;

		string errorP="";
		string errorA="";
		string errorB="";
		string errorX="";
		string errorY="";

		public EllipticCurveInput (StartPage page)
		{
			InitializeComponent ();

			parent = page;

			point = new ECPoint ();

			// operationsButton.IsEnabled = point.validatedAll;
		}



		protected void invalidateErrors(){
			stackResults.Children.Clear ();

			bool isError = false;

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

			operationsButton.IsEnabled = point.validatedAll && !isError;

			getCountButton.IsEnabled = point.validatedCoefs && !isError;
			genRandomPointButton.IsEnabled = point.validatedCoefs && !isError;

			if (isError) {
				frameResult.OutlineColor = Color.Red;
			} else {
				frameResult.OutlineColor = Color.Green;
			}

			labelCountPoints.Text = "";
		}

		// HANDLERS


		public void invalidateGenPoint(){
			if (point.FieldChar != 0) {

				if (point.y * point.y == ((point.x * point.x * point.x + point.x * point.a + point.b) % point.FieldChar)) {

					stackResults.Children.Add (new Label {
						TextColor = Color.Green,
						Text = "Точка принадлежит прямой.",
						VerticalOptions = LayoutOptions.StartAndExpand
					});	
					operationsButton.IsEnabled = false;
				} else {

					stackResults.Children.Add (new Label {
						TextColor = Color.Red,
						Text = "Точка не принадлежит прямой!",
						VerticalOptions = LayoutOptions.StartAndExpand
					});	
					operationsButton.IsEnabled = false;
				}
			} else {
				
				operationsButton.IsEnabled = false;
			}
		}

		public void handler_changedPValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					point.FieldChar = new BigInteger(entryP.Text, 10);
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

		BigInteger startGen = 0;
		private void handler_genRandomPointButtonClick(object sender, EventArgs e)
		{
			l1:
			BigInteger y = ((startGen * startGen * startGen + startGen * point.a + point.b) % point.FieldChar ).sqrt();

			if (y * y % point.FieldChar == ((startGen * startGen * startGen + startGen * point.a + point.b) % point.FieldChar)) {

				entryX.Text = startGen.ToString ();
				entryY.Text = y.ToString ();

				startGen++;
				startGen = startGen % point.FieldChar;
			} else {

				startGen++;
				startGen = startGen % point.FieldChar;
				goto l1;
			}
		}

		private void handler_getCountButtonClick(object sender, EventArgs e)
		{
			int count = 0;

			stackResults.Children.Clear ();

			string result = "";

			for (int i = 1; i <= 31; i++) {
				ECPoint p = ECPoint.multiply (i, point);

				result += Functions.getPointtoString (p) + " ";

				count++;
			}

			stackResults.Children.Add (new Label { TextColor=Color.Green, Text = result, VerticalOptions=LayoutOptions.StartAndExpand });

			labelCountPoints.Text = count.ToString();
		}

		private void handler_operationsButtonClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new Operations(parent, point));
		}
	}
}

