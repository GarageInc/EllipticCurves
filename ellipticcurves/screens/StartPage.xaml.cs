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

		private void handler_calculatorButtonClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new Calculator(this));
		}
	}
}

