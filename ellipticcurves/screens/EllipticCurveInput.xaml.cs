using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class EllipticCurveInput
	{
		StartPage parent;

		ECPoint curve;

		string errorP="";
		string errorA="";
		string errorB="";

		public EllipticCurveInput (StartPage page)
		{
			InitializeComponent ();

			parent = page;
			curve = new ECPoint ();
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

			if (isError) {
				frameResult.OutlineColor = Color.Red;		
			} else {
				frameResult.OutlineColor = Color.Green;
			}
		}

		// HANDLERS

		public void handler_changedPValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					curve.p = new BigInteger(entryP.Text, 10);
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
					curve.a = new BigInteger(entryA.Text, 10);
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
					curve.b = new BigInteger(entryB.Text, 10);
				}
				errorB = "";
			} catch{
				errorB = "Неверное значение 'b' = " + entryB.Text;
			} finally{
				invalidateErrors();
			}
		}
	}
}

