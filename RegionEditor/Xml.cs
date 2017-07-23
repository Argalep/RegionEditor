using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using TheBox;
using TheBox.MapViewer;

namespace RegionEditor
{
    class XmlSupport
    {
        public static string GetAttribute( XmlElement xml, string attribute, bool mandatory )
        {
            if ( xml == null )
            {
                if ( mandatory )
                    return null;
            }
            else if ( xml.HasAttribute( attribute ) )
            {
                return xml.GetAttribute( attribute );
            }

            return null;
        }

        public static bool ReadXmlBool( XmlElement xml, string attribute, ref XmlBool value )
        {
            bool b = false;

            if (ReadBoolean(xml, attribute, ref b))
            {
                if (b)
                    value = XmlBool.True;
                else
                    value = XmlBool.False;
                return true;
            }
            else
                return false;
        }

        public static bool ReadBoolean( XmlElement xml, string attribute, ref bool value )
		{
			return ReadBoolean( xml, attribute, ref value, true );
		}

		public static bool ReadBoolean( XmlElement xml, string attribute, ref bool value, bool mandatory )
		{
			string s = GetAttribute( xml, attribute, mandatory );

			if ( s == null )
				return false;

			try
			{
				value = XmlConvert.ToBoolean( s );
			}
			catch
			{
				return false;
			}

			return true;
		}

        public static bool ReadDateTime( XmlElement xml, string attribute, ref DateTime value )
        {
            return ReadDateTime( xml, attribute, ref value, true );
        }

        public static bool ReadDateTime( XmlElement xml, string attribute, ref DateTime value, bool mandatory )
        {
            string s = GetAttribute( xml, attribute, mandatory );

            if ( s == null )
                return false;

            try
            {
				value = XmlConvert.ToDateTime( s, XmlDateTimeSerializationMode.Local );
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ReadEnum( XmlElement xml, string attribute, Type type, ref object value )
        {
            return ReadEnum( xml, attribute, type, ref value, true );
        }

        public static bool ReadEnum( XmlElement xml, string attribute, Type type, ref object value, bool mandatory )
        {
            string s = GetAttribute( xml, attribute, mandatory );

            if ( s == null )
                return false;

            try
            {
                value = Enum.Parse( type, s, true );
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ReadMap( XmlElement xml, string attribute, ref Maps value )
        {
            return ReadMap( xml, attribute, ref value, true );
        }

        public static bool ReadMap( XmlElement xml, string attribute, ref Maps value, bool mandatory )
        {
            string s = GetAttribute( xml, attribute, mandatory );

            if ( s == null )
                return false;

            try
            {
                value = (Maps) Enum.Parse( typeof( Maps ), s );
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ReadInt32( XmlElement xml, string attribute, ref int value )
        {
            return ReadInt32( xml, attribute, ref value, true );
        }

        public static bool ReadInt32( XmlElement xml, string attribute, ref int value, bool mandatory )
        {
            string s = GetAttribute( xml, attribute, mandatory );

            if ( s == null )
                return false;

            try
            {
                value = XmlConvert.ToInt32( s );
            }
            catch
            {
                   return false;
            }

            return true;
        }

        public static bool ReadPoint3D( XmlElement xml, ref Point3D value )
        {
            return ReadPoint3D( xml, ref value, true );
        }

        public static bool ReadPoint3D( XmlElement xml, ref Point3D value, bool mandatory )
        {
            int x = 0, y = 0, z = 0;

            bool xyOk = ReadInt32( xml, "x", ref x, mandatory ) & ReadInt32( xml, "y", ref y, mandatory );
            bool zOk = ReadInt32( xml, "z", ref z, mandatory );

            if ( xyOk && zOk )
            {
                if ( !zOk )
                    z = 0;

                value = new Point3D( x, y, z );
                return true;
            }

            return false;
        }

        public static bool ReadRectangle3D( XmlElement xml, int defaultMinZ, int defaultMaxZ, ref MapRect value )
        {
            return ReadRectangle3D( xml, defaultMinZ, defaultMaxZ, ref value, true );
        }

        public static bool ReadRectangle3D( XmlElement xml, int defaultMinZ, int defaultMaxZ, ref MapRect value, bool mandatory )
        {
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

            if ( xml.HasAttribute( "x" ) )
            {
                if ( ReadInt32( xml, "x", ref x1, mandatory )
                    & ReadInt32( xml, "y", ref y1, mandatory )
                    & ReadInt32( xml, "width", ref x2, mandatory )
                    & ReadInt32( xml, "height", ref y2, mandatory ) )
                {
                    x2 += x1;
                    y2 += y1;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ( !ReadInt32( xml, "x1", ref x1, mandatory )
                    | !ReadInt32( xml, "y1", ref y1, mandatory )
                    | !ReadInt32( xml, "x2", ref x2, mandatory )
                    | !ReadInt32( xml, "y2", ref y2, mandatory ) )
                {
                    return false;
                }
            }

            int minZ = defaultMinZ;
            int maxZ = defaultMaxZ;

            ReadInt32(xml, "zmin", ref minZ, false);
            ReadInt32(xml, "zmax", ref maxZ, false);

            value = new MapRect(new Point2D(x1, y1), new Point2D(x2, y2), minZ, maxZ);

            return true;
        }

        public static bool ReadString( XmlElement xml, string attribute, ref string value )
        {
            return ReadString( xml, attribute, ref value, true );
        }

        public static bool ReadString( XmlElement xml, string attribute, ref string value, bool mandatory )
        {
            string s = GetAttribute( xml, attribute, mandatory );

            if ( s == null )
                return false;

            value = s;
            return true;
        }

        public static bool ReadTimeSpan( XmlElement xml, string attribute, ref TimeSpan value )
        {
            return ReadTimeSpan( xml, attribute, ref value, true );
        }

        public static bool ReadTimeSpan( XmlElement xml, string attribute, ref TimeSpan value, bool mandatory )
        {
            string s = GetAttribute( xml, attribute, mandatory );

            if ( s == null )
                return false;

            try
            {
                value = XmlConvert.ToTimeSpan( s );
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}
