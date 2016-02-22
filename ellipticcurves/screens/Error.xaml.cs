using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class Error
	{
		Page parent;

		public Error (Page page, string errorText)
		{
			InitializeComponent ();

			parent = page;
			labelError.Text = errorText;
		}
	}
}

