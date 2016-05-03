using System;
using System.Collections.Generic;

namespace EllipticCurves
{
	public class Functions
	{
		public Functions ()
		{
		}

		// Расширенный алгоритм Евклида
		private static void AdvancedEuclidAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y, out BigInteger d)
		{
			BigInteger q, r, x1, x2, y1, y2;

			if (b == 0)
			{
				d = a;
				x = 1;
				y = 0;
				return;
			}

			x2 = 1;
			x1 = 0;
			y2 = 0;
			y1 = 1;

			while (b > 0)
			{
				q = a / b;
				r = a - q * b;
				x = x2 - q * x1;
				y = y2 - q * y1;
				a = b;
				b = r;
				x2 = x1;
				x1 = x;
				y2 = y1;
				y1 = y;
			}

			d = a;
			x = x2;
			y = y2;
		}

		public static BigInteger GetInverseByModule(BigInteger a, BigInteger n)
		{
			BigInteger x, y, d;

			AdvancedEuclidAlgorithm(a, n, out x, out y, out d);

			if (d == 1) return x;

			return 0;
		}
        
		public static bool Contains(List<ECPoint> list, ECPoint p)
		{
		    foreach (ECPoint point in list)
		    {
		        if ( point == p ) {
		            return true;
		        }// pass
		    }

		    return false;
		}
	}
}

