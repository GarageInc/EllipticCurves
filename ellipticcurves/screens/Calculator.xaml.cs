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

		protected void handler_buttonAdditionClick(object sender, EventArgs e)
		{
			try{
				parseValues();
				labelResult.Text = ((x+y) % n).ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}

		protected void handler_buttonSubstractionClick(object sender, EventArgs e)
		{
			try{
				parseValues();
				labelResult.Text = ((x-y) % n).ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}

		protected void handler_buttonMultClick(object sender, EventArgs e)
		{
			try{
				parseValues();
				labelResult.Text = ((x*y) % n).ToString();

			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}

		protected void handler_buttonDivisionClick(object sender, EventArgs e)
		{
			try{
				parseValues();

				BigInteger inverse = Functions.GetInverseByModule(y,n);

				if(inverse<0){
					inverse = inverse + n;
				}

				labelResult.Text = ((x*inverse) % n).ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}

		protected void handler_buttonSqrtXClick(object sender, EventArgs e)
		{
			try{
				parseValues();
				labelResult.Text = (( x.sqrt() ) % n).ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}

		protected void handler_buttonPowXYClick(object sender, EventArgs e)
		{
			try{
				parseValues();

				labelResult.Text = ( x.modPow(y,n) ).ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}

		protected void handler_buttonInverseXClick(object sender, EventArgs e)
		{
			try{
				parseValues();

				BigInteger inverse = Functions.GetInverseByModule(x,n);
			
				if(inverse<0){
					inverse = inverse + n;
				}

				labelResult.Text = inverse.ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}

		protected void handler_buttonXModNClick(object sender, EventArgs e)
		{
			try{
				parseValues();

				labelResult.Text = (x % n).ToString();
			} catch{
				clearFields ();
				this.Navigation.PushAsync(new Error(this, errorText));
			}
		}
	}
}

