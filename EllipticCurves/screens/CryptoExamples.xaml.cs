using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class CryptoExamples : ContentPage
	{
		StartPage parent;

		ECPoint point;

		public CryptoExamples (StartPage page, ECPoint p = null)
		{
			InitializeComponent ();

			parent = page;

			if (p == null) {
				point = new ECPoint ();
			} else {
				point = p;
			}			

			// operationsButton.IsEnabled = point.validatedAll;
		}

	}
}

