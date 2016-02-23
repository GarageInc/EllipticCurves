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

			Label labelError = new Label { 
				Text = errorText, 
				Font = Font.SystemFontOfSize(30),
				XAlign = Xamarin.Forms.TextAlignment.Center,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand 
			};
						
			stackLayout.Children.Add(labelError);
		}
	}
}

