using System;
using System.Collections;
using System.Xml;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RegionEditor
{
    public enum XmlBool
    {
        Unspecified,
        True,
        False
    }

    public class MapRegion
    {
        public const int DefaultMinZ = sbyte.MinValue;
        public const int DefaultMaxZ = sbyte.MaxValue;

        private string m_Type = string.Empty;
        private int m_Priority = 50;
        private string m_Name = string.Empty;
        private Point3D m_GoLocation;
        private Point3D m_Entrance;
        private String m_Music = string.Empty;
        private int m_MinZ = DefaultMinZ;
        private int m_MaxZ = DefaultMaxZ;
        private string m_Rune = string.Empty;
        private XmlBool m_SmartNoHousing = XmlBool.Unspecified;
        private XmlBool m_LogoutDelayActive = XmlBool.Unspecified;
        private ArrayList m_Area;
        private ArrayList m_SubRegions;
        private XmlBool m_GuardsDisabled;
        private XmlNodeList m_Spawning = null;

        #region Setters & Getters
        public XmlBool GuardsDisabled
        {
            get { return m_GuardsDisabled; }
            set { m_GuardsDisabled = value; }
        }

        public ArrayList SubRegions
        {
            get { return m_SubRegions; }
            set { m_SubRegions = value; }
        }

        public ArrayList Area
        {
            get { return m_Area; }
            set { m_Area = value; }
        }

        public XmlBool LogoutDelayActive
        {
            get { return m_LogoutDelayActive; }
            set { m_LogoutDelayActive = value; }
        }

        public XmlBool SmartNoHousing
        {
            get { return m_SmartNoHousing; }
            set { m_SmartNoHousing = value; }
        }

        public string RuneName
        {
            get { return m_Rune; }
            set { m_Rune = value; }
        }

        public int MaxZ
        {
            get { return m_MaxZ; }
            set
            {
                int oldValue = m_MaxZ;
                m_MaxZ = value;

                if (m_MaxZ > DefaultMaxZ)
                    m_MaxZ = DefaultMaxZ;

                if (Area != null)
                {
                    foreach (MapRect rect in Area)
                    {
                        if (rect.MaxZ == oldValue)
                            rect.MaxZ = m_MaxZ;
                    }
                }

                if (SubRegions != null)
                {
                    foreach (MapRegion subRegion in SubRegions)
                    {
                        if (subRegion.MaxZ == oldValue)
                            subRegion.MaxZ = m_MaxZ;
                    }
                }
            }
        }

        public int MinZ
        {
            get { return m_MinZ; }
            set
            {
                int oldValue = m_MinZ;
                m_MinZ = value;

                if (m_MinZ < DefaultMinZ)
                    m_MinZ = DefaultMinZ;

                if (Area != null)
                {
                    foreach (MapRect rect in Area)
                    {
                        if (rect.MinZ == oldValue)
                            rect.MinZ = m_MinZ;
                    }
                }

                if (SubRegions != null)
                {
                    foreach (MapRegion subRegion in SubRegions)
                    {
                        if (subRegion.MinZ == oldValue)
                            subRegion.MinZ = m_MinZ;
                    }
                }
            }
        }

        public String MusicName
        {
            get { return m_Music; }
            set { m_Music = value; }
        }

        public Point3D GoLocation
        {
            get { return m_GoLocation; }
            set { m_GoLocation = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public int Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }

        public string TypeName
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        public int AddArea(MapRectangle3D rectangle)
        {
            return m_Area.Add(new MapRect(rectangle.Rectangle, rectangle.MinZ, rectangle.MaxZ));
        }

        public MapRegion()
        {
            m_GoLocation = new Point3D(-1, -1, 0);
            m_Area = new ArrayList();
            m_SubRegions = new ArrayList();
            m_Entrance = new Point3D(int.MinValue, int.MinValue, int.MinValue);
        }

        public MapRegion(string name)
            : this()
        {
            m_Name = name;
        }

        public static MapRegion LoadRegion(XmlElement xml)
        {
            MapRegion r = new MapRegion();

            XmlSupport.ReadString(xml, "name", ref r.m_Name);
            XmlSupport.ReadInt32(xml, "priority", ref r.m_Priority);
            XmlSupport.ReadString(xml, "type", ref r.m_Type);

            XmlElement zrange = xml["zrange"];
            XmlSupport.ReadInt32(zrange, "min", ref r.m_MinZ, false);
            XmlSupport.ReadInt32(zrange, "max", ref r.m_MaxZ, false);

            foreach (XmlElement xmlRect in xml.SelectNodes("rect"))
            {
                MapRect rect = new MapRect();
                if (XmlSupport.ReadRectangle3D(xmlRect, r.m_MinZ, r.m_MaxZ, ref rect))
                    r.m_Area.Add(rect);
            }

            if (!XmlSupport.ReadPoint3D(xml["go"], ref r.m_GoLocation, false) && r.m_Area.Count > 0)
            {
                Point2D start = ((MapRect)r.m_Area[0]).Start;

                Point2D end = ((MapRect)r.m_Area[0]).End;

                int x = start.X + (end.X - start.X) / 2;
                int y = start.Y + (end.Y - start.Y) / 2;

                r.m_GoLocation = new Point3D(x, y, 0);
            }

            int entranceX = int.MinValue;
            int entranceY = int.MinValue;
            int entranceZ = int.MinValue;

            XmlElement entrance = xml["entrance"];
            if (entrance != null)
            {
                XmlSupport.ReadInt32(entrance, "x", ref entranceX);
                XmlSupport.ReadInt32(entrance, "y", ref entranceY);
                XmlSupport.ReadInt32(entrance, "z", ref entranceZ);
            }

            r.m_Entrance.X = entranceX;
            r.m_Entrance.Y = entranceY;
            r.m_Entrance.Z = entranceZ;

            XmlSupport.ReadString(xml["music"], "name", ref r.m_Music);

            // <rune name="Haven City" />
            XmlSupport.ReadString(xml["rune"], "name", ref r.m_Rune);
            // <smartNoHousing active="true" />
            XmlSupport.ReadXmlBool(xml["smartNoHousing"], "active", ref r.m_SmartNoHousing);
            // <logoutDelay active="false" />
            XmlSupport.ReadXmlBool(xml["logoutDelay"], "active", ref r.m_LogoutDelayActive);
            // <guards disabled="true" />
            XmlSupport.ReadXmlBool(xml["guards"], "disabled", ref r.m_GuardsDisabled);


            // spawning, for now just keep a record of the info so we can persist it.
            r.m_Spawning = xml.SelectNodes("spawning");

            // Subregions
            foreach (XmlElement xRegion in xml.SelectNodes("region"))
            {
                MapRegion sub = MapRegion.LoadRegion(xRegion);

                if (sub != null)
                    r.m_SubRegions.Add(sub);
            }

            return r;
        }

        public XmlElement GetXml(XmlDocument dom)
        {
            XmlElement x = dom.CreateElement("region");

            if (m_Type != string.Empty)
                x.SetAttribute("type", m_Type);

            if (m_Priority != 50)
                x.SetAttribute("priority", m_Priority.ToString());

            if (m_Name != string.Empty)
                x.SetAttribute("name", m_Name);

            if (m_MinZ != DefaultMinZ || m_MaxZ != DefaultMaxZ)
            {
                XmlElement xRange = dom.CreateElement("zrange");

                if (m_MinZ != DefaultMinZ)
                    xRange.SetAttribute("min", m_MinZ.ToString());
                if (m_MaxZ != DefaultMaxZ)
                    xRange.SetAttribute("max", m_MaxZ.ToString());

                x.AppendChild(xRange);
            }

            foreach (MapRect rect in m_Area)
            {
                XmlElement xRect = dom.CreateElement("rect");
                xRect.SetAttribute("x", rect.Start.X.ToString());
                xRect.SetAttribute("y", rect.Start.Y.ToString());
                xRect.SetAttribute("width", rect.Width.ToString());
                xRect.SetAttribute("height", rect.Height.ToString());

                if (rect.MinZ != m_MinZ)
                    xRect.SetAttribute("zmin", rect.MinZ.ToString());
                if (rect.MaxZ != m_MaxZ)
                    xRect.SetAttribute("zmax", rect.MaxZ.ToString());

                x.AppendChild(xRect);
            }

            if (m_Area != null && m_Area.Count > 0)
            {
                MapRect rect = m_Area[0] as MapRect;
                Point3D DefaultGoLocation = new Point3D(rect.Start.X + (rect.End.X - rect.Start.X) / 2, rect.Start.Y + (rect.End.Y - rect.Start.Y) / 2, 0);

                if (m_GoLocation != DefaultGoLocation && m_GoLocation.X != -1 && m_GoLocation.Y != -1)
                {
                    XmlElement xGoLocation = dom.CreateElement("go");
                    xGoLocation.SetAttribute("x", m_GoLocation.X.ToString());
                    xGoLocation.SetAttribute("y", m_GoLocation.Y.ToString());
                    xGoLocation.SetAttribute("z", m_GoLocation.Z.ToString());
                    x.AppendChild(xGoLocation);
                }
            }

            if (m_Entrance.X != int.MinValue && m_Entrance.Y != int.MinValue)
            {
                XmlElement xEntrance = dom.CreateElement("entrance");
                xEntrance.SetAttribute("x", m_Entrance.X.ToString());
                xEntrance.SetAttribute("y", m_Entrance.Y.ToString());
                if (m_Entrance.Z != int.MinValue)
                    xEntrance.SetAttribute("z", m_Entrance.Z.ToString());
                x.AppendChild(xEntrance);
            }

            if (m_Music != string.Empty)
            {
                XmlElement xMusic = dom.CreateElement("music");
                xMusic.SetAttribute("name", m_Music);
                x.AppendChild(xMusic);
            }

            // Begin other data

            // <rune name="Haven City" />
            if (m_Rune != null && m_Rune != string.Empty)
            {
                XmlElement xRune = dom.CreateElement("rune");
                xRune.SetAttribute("name", m_Rune);
                x.AppendChild(xRune);
            }

            // <smartNoHousing active="true" />
            if (m_SmartNoHousing != XmlBool.Unspecified)
            {
                XmlElement xHouse = dom.CreateElement("smartNoHousing");
                xHouse.SetAttribute("active", m_SmartNoHousing.ToString().ToLower());
                x.AppendChild(xHouse);
            }

            // <logoutDelay active="false" />
            if (m_LogoutDelayActive != XmlBool.Unspecified)
            {
                XmlElement xLogout = dom.CreateElement("logoutDelay");
                xLogout.SetAttribute("active", m_LogoutDelayActive.ToString().ToLower());
                x.AppendChild(xLogout);
            }

            // <guards disabled="true" />
            if (m_GuardsDisabled != XmlBool.Unspecified)
            {
                XmlElement xGuards = dom.CreateElement("guards");
                xGuards.SetAttribute("disabled", m_GuardsDisabled.ToString().ToLower());
                x.AppendChild(xGuards);
            }

            // spawning, for now just keep a record of the info so we can persist it.
            if (m_Spawning != null)
            {
                foreach (XmlElement spawning in m_Spawning)
                {
                    XmlNode xSpawn = dom.ImportNode(spawning, true);
                    x.AppendChild(xSpawn);
                }
            }

            // Subregions
            foreach (MapRegion sub in m_SubRegions)
            {
                XmlElement xSub = sub.GetXml(dom);
                x.AppendChild(xSub);
            }

            return x;
        }

        internal TreeNode GetTreeNode()
        {
            TreeNode node = new TreeNode(m_Name);

            if (node.Text == string.Empty)
                node.Text = "Unnamed";

            node.Tag = this;

            foreach (MapRegion sub in m_SubRegions)
                node.Nodes.Add(sub.GetTreeNode());

            return node;
        }
    }
}
