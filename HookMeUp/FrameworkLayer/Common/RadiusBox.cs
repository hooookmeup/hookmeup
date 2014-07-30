using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Framework.Common
{
    /// <summary>
    /// A RadiusBox encloses a "box" area around a location, where each side of the square is
    ///  radius miles away from the location.  Doing it this way includes ~22% more area than if we
    ///  used a proper circle, but using a box simplifies the query.
    /// </summary>
    public class RadiusBox
    {

        #region PROPERTIES
        /// <summary>
        /// Represents the Southern latitude line.
        /// </summary>
        public Double BottomLine
        {
            get
            {
                return _dBottomLatLine;
            }
            set
            {
                _dBottomLatLine = value;
            }
        }


        /// <summary>
        /// Represents the Northern latitude line.
        /// </summary>
        public Double TopLine
        {
            get
            {
                return _dTopLatLine;
            }
            set
            {
                _dTopLatLine = value;
            }
        }


        /// <summary>
        /// Represents the Western longitude line.
        /// </summary>
        public Double LeftLine
        {
            get
            {
                return _dLeftLongLine;
            }
            set
            {
                _dLeftLongLine = value;
            }
        }


        /// <summary>
        /// Represents the Eastern longitude line.
        /// </summary>
        public Double RightLine
        {
            get
            {
                return _dRightLongLine;
            }
            set
            {
                _dRightLongLine = value;
            }
        }


        /// <summary>
        /// Represents the radius of the search area.
        /// </summary>
        public Double Radius
        {
            get
            {
                return _dRadius;
            }
            set
            {
                _dRadius = value;
            }
        }
        #endregion


        #region MEMBER DATA
        private double _dBottomLatLine;
        private double _dTopLatLine;
        private double _dLeftLongLine;
        private double _dRightLongLine;
        private double _dRadius;
        #endregion


        #region CLASS METHODS
        /// <summary>
        /// Creates a box that encloses the specified location, where the sides of the square
        ///  are inRadius miles away from the location at the perpendicular.  Note that we do
        ///  not actually generate lat/lon pairs; we only generate the coordinate that 
        ///  represents the side of the box.
        /// </summary>
        /// <remarks>
        /// <para>Formula obtained from Dr. Math at http://www.mathforum.org/library/drmath/view/51816.html.</para>
        /// </remarks>
        /// <param name="inLocation"></param>
        /// <param name="inRadius"></param>
        /// <returns></returns>
        public static RadiusBox Create(double latitude , double longitude ,  Double inRadius)
        {
            /*
                A point {lat,lon} is a distance d out on the tc radial from point 1 if:
             
                lat = asin (sin (lat1) * cos (d) + cos (lat1) * sin (d) * cos (tc))
                dlon = atan2 (sin (tc) * sin (d) * cos (lat1), cos (d) - sin (lat1) * sin (lat))
                lon = mod (lon1 + dlon + pi, 2 * pi) - pi
             
                Where:
                    * d is the distance in radians (an arc), so the desired radius divided by
                        the radius of the Earth.
                    * tc = 0 is N, tc = pi is S, tc = pi/2 is E, tc = 3*pi/2 is W.
            */
            double lat;
            double dlon;
            double dLatInRads = latitude * (Math.PI / 180.0);
            double dLongInRads = longitude * (Math.PI / 180.0);
            double dDistInRad = inRadius / 3956.0;
            RadiusBox box = new RadiusBox();
            box.Radius = inRadius;

            //  N (tc == 0):
            //      lat = asin (sin(lat1)*cos(d) + cos(lat1)*sin(d))
            //          = asin (sin(lat1 + d))
            //          = lat1 + d
            //  Unused:
            //      lon = lon1, because north-south lines follow lines of longitude.
            box.TopLine = dLatInRads + dDistInRad;
            box.TopLine *= (180.0 / Math.PI);

            //  S (tc == pi):
            //      lat = asin (sin(lat1)*cos(d) - cos(lat1)*sin(d))
            //          = asin (sin(lat1 - d))
            //          = lat1 - d
            //  Unused:
            //      lon = lon1, because north-south lines follow lines of longitude.
            box.BottomLine = dLatInRads - dDistInRad;
            box.BottomLine *= (180.0 / Math.PI);

            //  E (tc == pi/2):
            //      lat  = asin (sin(lat1)*cos(d))
            //      dlon = atan2 (sin(tc)*sin(d)*cos(lat1), cos(d) - sin(lat1)*sin(lat))
            //      lon  = mod (lon1 + dlon + pi, 2*pi) - pi
            lat = Math.Asin(Math.Sin(dLatInRads) * Math.Cos(dDistInRad));
            dlon = Math.Atan2(Math.Sin(Math.PI / 2.0) * Math.Sin(dDistInRad) * Math.Cos(dLatInRads), Math.Cos(dDistInRad) - Math.Sin(dLatInRads) * Math.Sin(lat));
            box.RightLine = ((dLongInRads + dlon + Math.PI) % (2.0 * Math.PI)) - Math.PI;
            box.RightLine *= (180.0 / Math.PI);

            //  W (tc == 3*pi/2):
            //      lat  = asin (sin(lat1)*cos(d))
            //      dlon = atan2 (sin(tc)*sin(d)*cos(lat1), cos(d) - sin(lat1)*sin(lat))
            //      lon  = mod (lon1 + dlon + pi, 2*pi) - pi
            dlon = Math.Atan2(Math.Sin(3.0 * Math.PI / 2.0) * Math.Sin(dDistInRad) * Math.Cos(dLatInRads), Math.Cos(dDistInRad) - Math.Sin(dLatInRads) * Math.Sin(lat));
            box.LeftLine = ((dLongInRads + dlon + Math.PI) % (2.0 * Math.PI)) - Math.PI;
            box.LeftLine *= (180.0 / Math.PI);

            return box;
        }
        #endregion
    }

    /// <summary>
    /// The Distance class takes two <see cref="SagaraSoftware.ZipCodeUtil.Location" /> objects and
    ///  uses their Latitude and Longitude to determine the distance between them.  Uses the
    ///  Haversine formula.
    /// </summary>
    public class Distance
    {
        #region CLASS METHODS
        /// <summary>
        /// Returns the distance in miles between two locations, calculated using the Haversine
        ///  forumula.
        /// </summary>
        /// <param name="inLoc1"></param>
        /// <param name="inLoc2"></param>
        /// <returns></returns>
        public static Double GetDistance(double Loc1Lat, double Loc1Long, double Loc2Lat, double Loc2long)
        {

            return Haversine(Loc1Lat, Loc1Long, Loc2Lat, Loc2long);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inLoc1"></param>
        /// <param name="inLoc2"></param>
        /// <returns></returns>
        private static double Haversine(double Loc1Lat, double Loc1Long, double Loc2Lat, double Loc2long)
        {
            /*
                The Haversine formula according to Dr. Math.
                http://mathforum.org/library/drmath/view/51879.html
                 
                dlon = lon2 - lon1
                dlat = lat2 - lat1
                a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
                c = 2 * atan2(sqrt(a), sqrt(1-a)) 
                d = R * c
                 
                Where
                    * dlon is the change in longitude
                    * dlat is the change in latitude
                    * c is the great circle distance in Radians.
                    * R is the radius of a spherical Earth.
                    * The locations of the two points in spherical coordinates (longitude and 
                        latitude) are lon1,lat1 and lon2, lat2.
            */
            double dDistance = Double.MinValue;
            double dLat1InRad = Loc1Lat * (Math.PI / 180.0);
            double dLong1InRad = Loc1Long * (Math.PI / 180.0);
            double dLat2InRad = Loc2Lat * (Math.PI / 180.0);
            double dLong2InRad = Loc2long * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) + Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) * Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // Distance.
            dDistance = 3956.0 * c;

            return dDistance;
        }
        #endregion
    }
}
