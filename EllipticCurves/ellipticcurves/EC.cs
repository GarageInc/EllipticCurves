using System;
using System.Collections.Generic;

namespace EllipticCurves
{
	public class EC
	{

		public BigInteger a{ get; set; }
		public BigInteger b{ get; set; }
		public BigInteger p {get; set;}

		public ECPoint generation_point{ get; set; }
		public int count_points{ get; set; }

		protected List<ECPoint> points = new List<ECPoint> ();

		public EC ()
		{
			a = 0;
			b = 0;
			p = 0;

			generation_point = new ECPoint ();
		}

		protected BigInteger startGen = 0;
		protected BigInteger getRandomPointCoord_y(){
			l1:
			BigInteger y = ((startGen * startGen * startGen + startGen * generation_point.a + generation_point.b) % p ).sqrt();

			if (((y * y) % p) == ((startGen * startGen * startGen + startGen * a + b) % p)) {

				startGen = (startGen+1) % p;
				return y;
			} else {

				startGen++;
				startGen = startGen % p;
				goto l1;
			}
		}

		protected ECPoint getNext(){

			generation_point.y = getRandomPointCoord_y ();
			generation_point.x = startGen - 1;

			return generation_point;
		}

		public ECPoint createRandomGeneretingPoint(){
		
			if (p != 0) {

				generation_point = getNext ();
			}

			return generation_point;
		}

		public List<ECPoint> getAllPoints(){
		

			return points;
		}
	}
}

