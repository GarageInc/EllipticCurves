﻿using System;
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


		public static string getPointtoString(ECPoint p){
			string result = "";
			result += "[ " + p.x + " ; " + p.y + " ]"; 

			return result;
		}


		public static bool isFirst(List<ECPoint> list, ECPoint p){
			if (list.Count > 0) {
				if ((list [0].x == p.x && list [0].y == p.y)) {
					return true;
				}
				return false;
			} else {
				return false;
			}
		}

		public static bool Contains(List<ECPoint> list, ECPoint p){

			for (int i = 0; i < list.Count; i++) {
				if (list [i].x == p.x && list [i].y == p.y) {
					return true;
				}// pass
			}

			return false;
		}
	}
}

