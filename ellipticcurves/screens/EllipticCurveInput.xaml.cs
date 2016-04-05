using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EllipticCurves
{
	public partial class EllipticCurveInput: ContentPage
	{
		StartPage parent;

		ECPoint current_point;

		List<string> errors = new List<string> ();

		public EllipticCurveInput (StartPage page)
		{
			InitializeComponent ();

			parent = page;

			current_point = new ECPoint ();
		}

		protected void trace(string message, Color color){

			frameResult.OutlineColor = color;

			stackResults.Children.Add (new Label { TextColor=Color.Red, Text = message,VerticalOptions=LayoutOptions.StartAndExpand });
		}

		protected void invalidateErrors(){
			stackResults.Children.Clear ();

			bool isError = false;

			if (errors.Count > 0) {
				isError = true;

				foreach (string e in errors) {
					trace (e, Color.Red);
				}

				errors.Clear ();
			}

			operationsButton.IsEnabled = current_point.validatedAll && !isError;

			getCountButton.IsEnabled = current_point.validatedCoefs && !isError;
			genRandomPointButton.IsEnabled = current_point.validatedCoefs && !isError;

			labelCountPoints.Text = "";
		}

		public BigInteger getRandomPointCoord_y(){
			l1:
			BigInteger y = ((startGen * startGen * startGen + startGen * current_point.a + current_point.b) % current_point.p ).sqrt();

			if (((y * y) % current_point.p) == ((startGen * startGen * startGen + startGen * current_point.a + current_point.b) % current_point.p)) {
				
				startGen = (startGen+1) % current_point.p;
				return y;
			} else {

				startGen++;
				startGen = startGen % current_point.p;
				goto l1;
			}
		}

		public void invalidateGenPoint(){
			if (current_point.isBelongToCurve()) {

				trace("Точка принадлежит прямой", Color.Green)
				operationsButton.IsEnabled = true;
			} else {

				trace("Точка не принадлежит прямой!", Color.Red)
				operationsButton.IsEnabled = false;
			}
		}


		// HANDLERS


		public void handler_changedPValidate(object sender, EventArgs e){
			try{
				Entry current = sender as Entry;
				if ( current.Text != null && current.Text != ""){
					current_point.p = new BigInteger(entryP.Text, 10);
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
					current_point.a = new BigInteger(entryA.Text, 10);
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
					current_point.b = new BigInteger(entryB.Text, 10);
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
					current_point.x = new BigInteger(entryX.Text, 10);
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
					current_point.y = new BigInteger(entryY.Text, 10);
				}
			} catch{
				errors.Add("Неверное значение 'y' = " + entryY.Text);
			} finally{
				invalidateErrors();
				invalidateGenPoint ();
			}
		}

		BigInteger startGen = 0;
		private void handler_genRandomPointButtonClick(object sender, EventArgs e)
		{
			setPoint ();
		}

		public void setPoint(){
			current_point.y = getRandomPointCoord_y ();
			current_point.x = startGen - 1;

			entryX.Text = current_point.x.ToString ();
			entryY.Text = current_point.y.ToString ();
		}

		public List<ECPoint> points = new List<ECPoint>();

		private void handler_getCountButtonClick(object sender, EventArgs e)
		{
			points.Clear ();


			string result = "";

			int i = 2;

			startGen = 0;
			setPoint ();

			ECPoint p = new ECPoint(current_point);

			while( !isFirst(points,p) ) {

				if (!Contains (points, p)) {
					points.Add (p);
					result += Functions.getPointtoString (p) + " ";
				}// pass

				p = ECPoint.multiply (i, current_point);
				i++;
			}

			stackResults.Children.Clear ();
			trace (result, Color.Green);

			labelCountPoints.Text = points.Count.ToString() + " + 1 бесконечная";
		}


		public bool isFirst(List<ECPoint> list, ECPoint p){
			if (list.Count > 0) {
				if ((points [0].x == p.x && points [0].y == p.y)) {
					return true;
				}
				return false;
			} else {
				return false;
			}
		}

		public bool Contains(List<ECPoint> list, ECPoint p){
		
			for (int i = 0; i < list.Count; i++) {
				if (list [i].x == p.x && list [i].y == p.y) {
					return true;
				}// pass
			}

			return false;
		}


		private void handler_operationsButtonClick(object sender, EventArgs e)
		{
			this.Navigation.PushAsync(new Operations(parent, current_point));
		}
	}
}

