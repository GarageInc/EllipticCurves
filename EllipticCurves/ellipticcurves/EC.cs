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

        public bool isValidatedCoefs => b != 0 && a != 0 && p != 0;

        public EC ()
		{
			a = 0;
			b = 0;
			p = 0;

            generationPoint = new ECPoint(this);
		}

	    protected BigInteger findY(BigInteger second)
	    {
	        BigInteger counter = 0;
	        while (counter < p)
	        {
	            var first = (counter*counter) % p;
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
                generationPoint.y = new BigInteger(getRandomPointCoord_y());
                generationPoint.x = new BigInteger(startGen - 1);// imprortant!
            }

		    var newPoint = new ECPoint
		    {
		        x = new BigInteger(generationPoint.x),
		        y = new BigInteger(generationPoint.y),
		        elliptic_curve = generationPoint.elliptic_curve
		    };

		    return newPoint;
		}
        
	    public List<ECPoint> GetAllPoints()
	    {
            List<ECPoint> points = new List<ECPoint>();

	        startGen = 0;

            var point = getNext();

            points.Add( point );
            points.Add(new ECPoint
            {
                x = new BigInteger(point.x),
                y = (-1) * new BigInteger(point.y)
            });
            
            BigInteger counter = 0;
            do
            {
                point = getNext();

                points.Add( point );
                points.Add(new ECPoint
                {
                    x= new BigInteger(point.x),
                    y=(-1)* new BigInteger(point.y)
                });

                counter++;

                if (points[points.Count - 2] == points.Last())
                {
                    points.Remove(points.Last());
                }
            } while ( points[0] != point && counter < p );

	        points.Remove( points.Last() );
            points.Remove(points.Last());

            return points;
	    }


	    public bool isNotSingular => (4 * a * a * a + 27 * b * b) % p != 0;
	}
}

