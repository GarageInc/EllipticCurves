using System;
using System.ComponentModel;

namespace EllipticCurves
{
	public class ECPoint:INotifyPropertyChanged
	{
		protected EC elliptic_curve;

		protected BigInteger aa=0;
		protected BigInteger bb=0;
		protected BigInteger fieldchar=0;

		public BigInteger x
		{
			get { return elliptic_curve.x; }
			set
			{
				if (elliptic_curve.x != value)
				{
					elliptic_curve.x = value;
					OnPropertyChanged("x");
				}
			}
		}

		public BigInteger y
		{
			get { return elliptic_curve.y; }
			set
			{
				if (elliptic_curve.y != value)
				{
					elliptic_curve.y = value;
					OnPropertyChanged("y");
				}
			}
		}

		public BigInteger a
		{
			get { return aa; }
			set
			{
				if (aa != value)
				{
					aa = value;
					OnPropertyChanged("a");
				}
			}
		}

		public BigInteger b
		{
			get { return bb; }
			set
			{
				if (bb != value)
				{
					bb = value;
					OnPropertyChanged("b");
				}
			}
		}

		public BigInteger p
		{
			get { return fieldchar; }
			set
			{
				if (fieldchar != value)
				{
					fieldchar = value;
					OnPropertyChanged("FieldChar");
				}
			}
		}

		public ECPoint(ECPoint p)
		{
			x = p.x;
			y = p.y;
			a = p.a;
			b = p.b;
			p = p.p;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		public ECPoint()
		{
			x = new BigInteger();
			y = new BigInteger();
			a = new BigInteger();
			b = new BigInteger();
			p = new BigInteger();
		}

		public bool  validatedAll{
			get {
				return b != 0 && a != 0 && (x != 0 || y != 0) && p != 0;
			}
		}

		public bool  validatedCoefs{
			get {
				return b != 0 && a != 0 && p != 0;
			}
		}


		public bool isBelongToCurve(){

			if (p == 0) {
				return false;
			}


			if (((y * y) % p) == ((x * x * x + x * a + b) % p)) {
			
				return true;
			} else {
				
				return false;
			}
		}


		protected void allChanged(){

			OnPropertyChanged("x");
			OnPropertyChanged("y");
			OnPropertyChanged("a");
			OnPropertyChanged("b");
			OnPropertyChanged("FieldChar");
		}

		protected void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}







		//сложение двух точек P1 и P2
		public static ECPoint operator +(ECPoint p1, ECPoint p2)
		{
			ECPoint p3 = new ECPoint();
			p3.a = p1.a;
			p3.b = p1.b;
			p3.p = p1.p;

			BigInteger dy = p2.y - p1.y;
			BigInteger dx = p2.x - p1.x;

			if (dx < 0)
				dx += p1.p;
			if (dy < 0)
				dy += p1.p;

			BigInteger m = (dy * dx.modInverse(p1.p)) % p1.p;
			if (m < 0)
				m += p1.p;
			p3.x = (m * m - p1.x - p2.x) % p1.p;
			p3.y = (m * (p1.x - p3.x) - p1.y) % p1.p;
			if (p3.x < 0)
				p3.x += p1.p;
			if (p3.y < 0)
				p3.y += p1.p;
			return p3;
		}

		public static bool isEqualCoords(ECPoint p1, ECPoint p2)
		{			
			return p1.x==p2.x && p1.y==p2.y;
		}

		// сложение точки P c собой же
		public static ECPoint Double(ECPoint p)
		{
			ECPoint p2 = new ECPoint();
			p2.a = p.a;
			p2.b = p.b;
			p2.p = p.p;

			BigInteger dy = 3 * p.x * p.x + p.a;
			BigInteger dx = 2 * p.y;

			if (dx < 0)
				dx += p.p;
			if (dy < 0)
				dy += p.p;

			BigInteger m = (dy * dx.modInverse(p.p)) % p.p;
			p2.x = (m * m - p.x - p.x) % p.p;
			p2.y = (m * (p.x - p2.x) - p.y) % p.p;
			if (p2.x < 0)
				p2.x += p.p;
			if (p2.y < 0)
				p2.y += p.p;

			return p2;
		}

		//умножение точки на число x, по сути своей представляет x сложений точки самой с собой
		public static ECPoint multiply(BigInteger x, ECPoint p)
		{
			ECPoint temp = p;
			x = x - 1;
			while (x != 0)
			{

				if ((x % 2) != 0)
				{
					if ((temp.x == p.x) || (temp.y == p.y))
						temp = Double(temp);
					else
						temp = temp + p;
					x = x - 1;
				}
				x = x / 2;
				p = Double(p);
			}
			return temp;
		}
	}
}

