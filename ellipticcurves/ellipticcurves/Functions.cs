using System;
using System.Collections.Generic;
using System.Linq;

namespace EllipticCurves
{
	public class Functions
	{
		public Functions ()
		{
		}

	    public static bool IsSimple(BigInteger p)
	    {
	        for (var i = 2; i < p.sqrt(); i++)
	        {
	            var div = p/i;
	            if (div*i==p)
	            {
	                return false;
	            }
	        }

	        return true;
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

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}

