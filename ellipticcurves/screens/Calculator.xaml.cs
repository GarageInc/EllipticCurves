using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class Calculator
	{
		private StartPage parent;

		BigInteger x;
		BigInteger y;
		BigInteger n;

		String errorText = "Проверьте правильность введенных данных!";

		public Calculator(StartPage startPage)
		{
			InitializeComponent();
			parent = startPage;
		}

		protected void parseValues(){
			x = new BigInteger (entryX.Text,10);
			y = new BigInteger (entryY.Text,10);
			n = new BigInteger (entryN.Text,10);
		}

		protected void clearFields (){
			entryX.Text = String.Empty;
			entryY.Text = String.Empty;
			entryN.Text = String.Empty;
			labelResult.Text = String.Empty;
		}

		// HANDLERS

		private void handler_buttonAdditionClick(object sender, EventArgs e)
		{
			try{
				parseValues();
				labelResult.Text = ((x+y) % n).ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}
	}
}

