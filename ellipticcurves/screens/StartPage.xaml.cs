using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class StartPage
	{
		public StartPage ()
		{
			InitializeComponent ();
		}

		protected void handler_calculatorButtonClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new Calculator(this));
		}

		protected void handler_ellipticButtonClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new EllipticCurveInput(this));
		}

		protected void handler_buttonCryptoClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new CryptoExamples(this));
		}
	}
}

