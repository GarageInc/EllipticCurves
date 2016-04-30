using System;
using System.Collections.Generic;
using System.Linq;

namespace EllipticCurves
{
	public class EC
	{

		public BigInteger a{ get; set; }
		public BigInteger b{ get; set; }
		public BigInteger p {get; set;}

		public ECPoint generationPoint{ get; set; }
        

		public EC ()
		{
			a = 0;
			b = 0;
			p = 0;

			generationPoint = new ECPoint ();
		    generationPoint.elliptic_curve = this;
		}

		protected BigInteger startGen = 0;
		protected BigInteger getRandomPointCoord_y(){
			l1:
			BigInteger y = ((startGen * startGen * startGen + startGen * generationPoint.a + generationPoint.b) % p ).sqrt();

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

			generationPoint.y = getRandomPointCoord_y ();
			generationPoint.x = startGen - 1;

			return generationPoint;
		}

		public ECPoint createRandomGeneratingPoint(){
		
			if (p != 0) {

				generationPoint = getNext ();
			}

			return generationPoint;
		}

	    public List<ECPoint> GetAllPoints()
	    {
            List<ECPoint> points = new List<ECPoint>();

	        startGen = 0;
            points.Add( createRandomGeneratingPoint() );

	        do
	        {
	            points.Add(getNext());
	        } while (points[0] != points.Last());

	        points.Remove(points.Last());

	        return points;
	    }


	    public bool isNotSingular => (4 * a * a * a + 27 * b * b) % p != 0;
	}
}

