/***************************************************************************
 *                                Geometry.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: Geometry.cs,v 1.3 2005/01/22 04:25:04 krrios Exp $
 *   $Author: krrios $
 *   $Date: 2005/01/22 04:25:04 $
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using System.ComponentModel;
using TheBox;
using System.Drawing;
using TheBox.MapViewer.DrawObjects;
using TheBox.MapViewer;

namespace RegionEditor
{
    public class MapRectangle3D : MapRectangle
    {
        private int m_MinZ;
        private int m_MaxZ;

        public int MinZ
        {
            get { return m_MinZ; }
            set { m_MinZ = value; }
        }
        public int MaxZ
        {
            get { return m_MaxZ; }
            set { m_MaxZ = value; }
        }

        public MapRectangle3D(Rectangle location, Maps map, Color color, int minZ, int maxZ)
            : base(location, map, color)
        {
            m_MinZ = minZ;
            m_MaxZ = maxZ;
        }

        public MapRectangle3D(Rectangle location, Maps map, Color color, Color fillColor, int minZ, int maxZ)
            : base(location, map, color, fillColor)
        {
            m_MinZ = minZ;
            m_MaxZ = maxZ;
        }
    }

    public struct Point2D
    {
        internal int m_X;
        internal int m_Y;

        public static readonly Point2D Zero = new Point2D(0, 0);

        public Point2D(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public int X
        {
            get
            {
                return m_X;
            }
            set
            {
                m_X = value;
            }
        }

        public int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                m_Y = value;
            }
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", m_X, m_Y);
        }

        public static Point2D Parse(string value)
        {
            int start = value.IndexOf('(');
            int end = value.IndexOf(',', start + 1);

            string param1 = value.Substring(start + 1, end - (start + 1)).Trim();

            start = end;
            end = value.IndexOf(')', start + 1);

            string param2 = value.Substring(start + 1, end - (start + 1)).Trim();

            return new Point2D(Convert.ToInt32(param1), Convert.ToInt32(param2));
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is Point2D)) return false;

            Point2D p = (Point2D)o;

            return m_X == p.X && m_Y == p.Y;
        }

        public override int GetHashCode()
        {
            return m_X ^ m_Y;
        }

        public static bool operator ==(Point2D l, Point2D r)
        {
            return l.m_X == r.m_X && l.m_Y == r.m_Y;
        }

        public static bool operator !=(Point2D l, Point2D r)
        {
            return l.m_X != r.m_X || l.m_Y != r.m_Y;
        }

        public static bool operator >(Point2D l, Point2D r)
        {
            return l.m_X > r.m_X && l.m_Y > r.m_Y;
        }

        public static bool operator >(Point2D l, Point3D r)
        {
            return l.m_X > r.m_X && l.m_Y > r.m_Y;
        }

        public static bool operator <(Point2D l, Point3D r)
        {
            return l.m_X < r.m_X && l.m_Y < r.m_Y;
        }

        public static bool operator <(Point2D l, Point2D r)
        {
            return l.m_X < r.m_X && l.m_Y < r.m_Y;
        }

        public static bool operator >=(Point2D l, Point2D r)
        {
            return l.m_X >= r.m_X && l.m_Y >= r.m_Y;
        }

        public static bool operator >=(Point2D l, Point3D r)
        {
            return l.m_X >= r.m_X && l.m_Y >= r.m_Y;
        }

        public static bool operator <=(Point2D l, Point2D r)
        {
            return l.m_X <= r.m_X && l.m_Y <= r.m_Y;
        }

        public static bool operator <=(Point2D l, Point3D r)
        {
            return l.m_X <= r.m_X && l.m_Y <= r.m_Y;
        }
    }

    public class Point3D
    {
        internal int m_X;
        internal int m_Y;
        internal int m_Z;

        public static readonly Point3D Zero = new Point3D(0, 0, 0);

        public Point3D(int x, int y, int z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        [Description("The X coordinate of the point")]
        public int X
        {
            get
            {
                return m_X;
            }
            set
            {
                m_X = value;
            }
        }

        [Description("The Y coordinate of the point")]
        public int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                m_Y = value;
            }
        }

        [Description("The Z coordinate of the point")]
        public int Z
        {
            get
            {
                return m_Z;
            }
            set
            {
                m_Z = value;
            }
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", m_X, m_Y, m_Z);
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is Point3D)) return false;

            Point3D p = (Point3D)o;

            return m_X == p.X && m_Y == p.Y && m_Z == p.Z;
        }

        public override int GetHashCode()
        {
            return m_X ^ m_Y ^ m_Z;
        }

        public static Point3D Parse(string value)
        {
            int start = value.IndexOf('(');
            int end = value.IndexOf(',', start + 1);

            string param1 = value.Substring(start + 1, end - (start + 1)).Trim();

            start = end;
            end = value.IndexOf(',', start + 1);

            string param2 = value.Substring(start + 1, end - (start + 1)).Trim();

            start = end;
            end = value.IndexOf(')', start + 1);

            string param3 = value.Substring(start + 1, end - (start + 1)).Trim();

            return new Point3D(Convert.ToInt32(param1), Convert.ToInt32(param2), Convert.ToInt32(param3));
        }

        public static bool operator ==(Point3D l, Point3D r)
        {
            return l.m_X == r.m_X && l.m_Y == r.m_Y && l.m_Z == r.m_Z;
        }

        public static bool operator !=(Point3D l, Point3D r)
        {
            return l.m_X != r.m_X || l.m_Y != r.m_Y || l.m_Z != r.m_Z;
        }
    }

    public struct Rectangle2D
    {
        private Point2D m_Start;
        private Point2D m_End;

        public Rectangle2D(int x, int y, int width, int height)
        {
            m_Start = new Point2D(x, y);
            m_End = new Point2D(x + width, y + height);
        }

        public void Set(int x, int y, int width, int height)
        {
            m_Start = new Point2D(x, y);
            m_End = new Point2D(x + width, y + height);
        }

        public static Rectangle2D Parse(string value)
        {
            int start = value.IndexOf('(');
            int end = value.IndexOf(',', start + 1);

            string param1 = value.Substring(start + 1, end - (start + 1)).Trim();

            start = end;
            end = value.IndexOf(',', start + 1);

            string param2 = value.Substring(start + 1, end - (start + 1)).Trim();

            start = end;
            end = value.IndexOf(',', start + 1);

            string param3 = value.Substring(start + 1, end - (start + 1)).Trim();

            start = end;
            end = value.IndexOf(')', start + 1);

            string param4 = value.Substring(start + 1, end - (start + 1)).Trim();

            return new Rectangle2D(Convert.ToInt32(param1), Convert.ToInt32(param2), Convert.ToInt32(param3), Convert.ToInt32(param4));
        }

        public Point2D Start
        {
            get
            {
                return m_Start;
            }
            set
            {
                m_Start = value;
            }
        }

        public Point2D End
        {
            get
            {
                return m_End;
            }
            set
            {
                m_End = value;
            }
        }

        public int X
        {
            get
            {
                return m_Start.m_X;
            }
            set
            {
                m_Start.m_X = value;
            }
        }

        public int Y
        {
            get
            {
                return m_Start.m_Y;
            }
            set
            {
                m_Start.m_Y = value;
            }
        }

        public int Width
        {
            get
            {
                return m_End.m_X - m_Start.m_X;
            }
            set
            {
                m_End.m_X = m_Start.m_X + value;
            }
        }

        public int Height
        {
            get
            {
                return m_End.m_Y - m_Start.m_Y;
            }
            set
            {
                m_End.m_Y = m_Start.m_Y + value;
            }
        }

        public void MakeHold(Rectangle2D r)
        {
            if (r.m_Start.m_X < m_Start.m_X)
                m_Start.m_X = r.m_Start.m_X;

            if (r.m_Start.m_Y < m_Start.m_Y)
                m_Start.m_Y = r.m_Start.m_Y;

            if (r.m_End.m_X > m_End.m_X)
                m_End.m_X = r.m_End.m_X;

            if (r.m_End.m_Y > m_End.m_Y)
                m_End.m_Y = r.m_End.m_Y;
        }

        public bool Contains(Point3D p)
        {
            return (m_Start.m_X <= p.m_X && m_Start.m_Y <= p.m_Y && m_End.m_X > p.m_X && m_End.m_Y > p.m_Y);
            //return ( m_Start <= p && m_End > p );
        }

        public bool Contains(Point2D p)
        {
            return (m_Start.m_X <= p.m_X && m_Start.m_Y <= p.m_Y && m_End.m_X > p.m_X && m_End.m_Y > p.m_Y);
            //return ( m_Start <= p && m_End > p );
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})+({2}, {3})", X, Y, Width, Height);
        }
    }

    public class MapRect
    {
        private Point2D m_Start;
        private Point2D m_End;
        private int m_MinZ;
        private int m_MaxZ;

        public MapRect()
        {
        }

        public MapRect(Point2D start, Point2D end, int minZ, int maxZ)
        {
            m_Start = start;
            m_End = end;
            m_MinZ = minZ;
            m_MaxZ = maxZ;
        }

        public MapRect(Rectangle rectangle, int minZ, int maxZ)
            : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, minZ, maxZ)
        {
        }

        public MapRect(int x, int y, int width, int height, int minZ, int maxZ)
        {
            m_Start = new Point2D(x, y);
            m_End = new Point2D(x + width, y + height);
            m_MinZ = minZ;
            m_MaxZ = maxZ;
        }

        public Point2D Start
        {
            get
            {
                return m_Start;
            }
            set
            {
                m_Start = value;
            }
        }

        public Point2D End
        {
            get
            {
                return m_End;
            }
            set
            {
                m_End = value;
            }
        }

        public int X
        {
            get { return m_Start.X; }
            set
            {
                if (value < m_End.X)
                {
                    int delta = value - m_Start.X;
                    m_Start.X += delta;
                    m_End.X += delta;
                }
            }
        }

        public int Y
        {
            get { return m_Start.Y; }
            set
            {
                if (value < m_End.Y)
                {
                    int delta = value - m_Start.Y;
                    m_Start.Y += delta;
                    m_End.Y += delta;
                }
            }
        }

        public int Width
        {
            get
            {
                return m_End.X - m_Start.X;
            }
            set
            {
                if (value > 0)
                    m_End.X = m_Start.X + value;
            }
        }

        public int Height
        {
            get
            {
                return m_End.Y - m_Start.Y;
            }
            set
            {
                if (value > 0)
                    m_End.Y = m_Start.Y + value;
            }
        }

        public int MinZ
        {
            get { return m_MinZ; }
            set
            {
                if (value >= -128 && value <= 128)
                    m_MinZ = value;
            }
        }

        public int MaxZ
        {
            get { return m_MaxZ; }
            set
            {
                if (value >= -128 && value <= 128)
                    m_MaxZ = value;
            }
        }

        public bool Contains(Point3D p)
        {
            return (p.m_X >= m_Start.m_X)
                && (p.m_X < m_End.m_X)
                && (p.m_Y >= m_Start.m_Y)
                && (p.m_Y < m_End.m_Y)
                && (p.m_Z >= m_MinZ)
                && (p.m_Z < m_MaxZ);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})-({2},{3})",
                m_Start.X, m_Start.Y, Width, Height);
        }
    }
}