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

		    generationPoint = new ECPoint
		    {
		        elliptic_curve = this
		    };
		}

	    protected BigInteger findY(BigInteger second)
	    {
	        BigInteger counter = 0;
	        while (counter < p)
	        {
	            var first = counter*counter % p;
	            if (first == second)
	            {
	                return counter;
	            }

	            counter++;
	        }

	        return counter;
	    }
		protected BigInteger startGen = 0;
		protected BigInteger getRandomPointCoord_y(){
			l1:
			BigInteger secondPart = (startGen * startGen * startGen + startGen * generationPoint.a + generationPoint.b) % p ;

			startGen++;
            startGen = startGen % p;

		    var y = findY(secondPart);

            if ( y != p )
			{
				return y;
			}
            
		    goto l1;
		}

		public ECPoint getNext(){

            if (p != 0)
            {
                generationPoint.x = startGen;
                generationPoint.y = getRandomPointCoord_y();
            }

		    var newPoint = new ECPoint
		    {
		        x = generationPoint.x,
		        y = generationPoint.y,
		        elliptic_curve = generationPoint.elliptic_curve
		    };

		    return newPoint;
		}
        
	    public List<ECPoint> GetAllPoints()
	    {
            List<ECPoint> points = new List<ECPoint>();

	        startGen = 0;
            points.Add( getNext() );

	        string outS = "";

	        outS += "" + points.Last();
            do
            {
                var point = getNext();
                points.Add( point );
                outS += "" + point;
            } while ( points[0] != points.Last() && startGen < p && startGen != 0);

	        points.Remove( points.Last() );

	        return points;
	    }


	    public bool isNotSingular => (4 * a * a * a + 27 * b * b) % p != 0;
	}
}

