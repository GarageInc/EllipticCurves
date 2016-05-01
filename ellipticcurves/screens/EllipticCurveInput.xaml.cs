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

		protected void trace(string message, Color color){

			frameResult.OutlineColor = color;

			stackResults.Children.Add (new Label { TextColor=color, Text = message,VerticalOptions=LayoutOptions.StartAndExpand });
		}

		protected void invalidateErrors(){
			stackResults.Children.Clear ();

		    bool isError = false;

		    if (curve.p != 0)
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

			if (errors.Count > 0) {
				isError = true;

				foreach (string e in errors) {
					trace (e, Color.Red);
				}

				errors.Clear ();
			}
            
            getCountButton.IsEnabled =  !isError;

			genRandomPointButton.IsEnabled = curve.generationPoint.isBelongToCurve() && !isError;

			operationsButton.IsEnabled = curve.generationPoint.validatedAll && !isError;
            
			labelCountPoints.Text = "";
		}



		public void invalidateGenPoint(){
			if (curve.generationPoint.isBelongToCurve()) {

				trace ("Точка принадлежит прямой", Color.Green);
				operationsButton.IsEnabled = true;
			} else {

				trace ("Точка не принадлежит прямой!", Color.Red);
				operationsButton.IsEnabled = false;
			}
		}


		// HANDLERS


		public void handler_changedPValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					curve.p = new BigInteger(entryP.Text, 10);
				}
			} catch{
				errors.Add("Неверное значение 'p' = " + entryP.Text);
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedAValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					curve.a = new BigInteger(entryA.Text, 10);
				}
			} catch{
				errors.Add("Неверное значение 'a' = " + entryA.Text);
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedBValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					curve.b = new BigInteger(entryB.Text, 10);
				}
			} catch{
				errors.Add("Неверное значение 'b' = " + entryB.Text);
			} finally{
				invalidateErrors();
			}
		}

		public void handler_changedXValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					curve.generationPoint.x = new BigInteger(entryX.Text, 10);
				}
			} catch{
				errors.Add("Неверное значение 'x' = " + entryX.Text);
			} finally{
				invalidateErrors();
				invalidateGenPoint ();
			}
		}

		public void handler_changedYValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					curve.generationPoint.y = new BigInteger(entryY.Text, 10);
				}
			} catch{
				errors.Add("Неверное значение 'y' = " + entryY.Text);
			} finally{
				invalidateErrors();
				invalidateGenPoint ();
			}
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

		private void handler_getCountButtonClick(object sender, EventArgs e)
		{
			List<ECPoint> points = curve.GetAllPoints ();

			string outputS = "";

			foreach(var p in points){
				outputS += p + " ";
			}

			stackResults.Children.Clear ();

			trace (outputS, Color.Green);

			labelCountPoints.Text = points.Count + "( 1 бесконечная )";
		}
			

		private void handler_operationsButtonClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new Operations(parent, curve.generationPoint));
		}
	}
}

