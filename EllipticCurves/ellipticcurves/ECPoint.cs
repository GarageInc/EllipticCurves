using System;
using System.ComponentModel;

namespace EllipticCurves
{
	public class ECPoint:INotifyPropertyChanged
	{
		protected EC elliptic_curve;

		protected BigInteger xx {get;set;}
		protected BigInteger yy {get;set;}



		public ECPoint(ECPoint p)
		{
			elliptic_curve = p.elliptic_curve;

			x = p.x;
			y = p.y;
		}

		public ECPoint(EC curve)
		{
			elliptic_curve = curve;

			y = new BigInteger();
			x = new BigInteger();
		}

		public BigInteger x
		{
			get { return x; }
			set
			{
				if (xx != value)
				{
					xx = value;
					OnPropertyChanged("x");
				}
			}
		}

		public BigInteger y
		{
			get { return y; }
			set
			{
				if (yy != value)
				{
					yy = value;
					OnPropertyChanged("y");
				}
			}
		}

		public BigInteger a
		{
			get { return elliptic_curve.a; }
			set
			{
				if (elliptic_curve.a != value)
				{
					elliptic_curve.a = value;
					OnPropertyChanged("a");
				}
			}
		}

		public BigInteger b
		{
			get { return elliptic_curve.b; }
			set
			{
				if (elliptic_curve.b != value)
				{
					elliptic_curve.b = value;
					OnPropertyChanged("b");
				}
			}
		}

		public BigInteger p
		{
			get { return elliptic_curve.p; }
			set
			{
				if (elliptic_curve.p != value)
				{
					elliptic_curve.p = value;
					OnPropertyChanged("p");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;


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

		public static bool isEqualCoords(ECPoint p1, ECPoint p2)
		{			
			return p1.x==p2.x && p1.y==p2.y;
		}

		protected void allChanged(){

			OnPropertyChanged("x");
			OnPropertyChanged("y");
			OnPropertyChanged("a");
			OnPropertyChanged("b");
			OnPropertyChanged("p");
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

