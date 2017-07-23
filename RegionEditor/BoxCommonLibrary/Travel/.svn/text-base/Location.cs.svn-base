using System;
using System.Xml.Serialization;
using System.Windows.Forms;
// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
using System.Collections.Generic;
// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert

namespace TheBox.Data
{
	/// <summary>
	/// Describes a location entry for Pandora's travel agent
	/// </summary>
	public class Location : IComparable
	{
		private short m_X;
		private short m_Y;
		private sbyte m_Z;
		private byte m_Map;
		private string m_Name;

		/// <summary>
		/// Gets or sets the name of the location
		/// </summary>
		[ XmlAttribute ]
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Gets or sets the X coordinate of the location
		/// </summary>
		[ XmlAttribute ]
		public short X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Gets or sets the Y coordinate of the location
		/// </summary>
		[ XmlAttribute ]
		public short Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// Gets or sets the Z coordinate of the location
		/// </summary>
		[ XmlAttribute ]
		public sbyte Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		/// <summary>
		/// Gets or sets the map the location resides on
		/// </summary>
		public int Map
		{
			get { return m_Map; }
			set { m_Map = (byte) value; }
		}

		/// <summary>
		/// Converts a collection of Location structures to a TreeNodeCollection
		/// </summary>
		/// <param name="list">A collection of Location structures</param>
		/// <returns>A TreeNodeCollection object containing nodes for all the locations in the list</returns>
		// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
		public static TreeNode[] ArrayToNodes( List<object> list )
		// Issue 10 - End
		{
			TreeNode[] nodes = new TreeNode[ list.Count ];

			for ( int i = 0; i < list.Count; i++ )
			{
				Location loc = list[ i ] as Location;

				TreeNode node = new TreeNode( loc.Name );
				node.Tag = loc;

				nodes[ i ] = node;
			}

			return nodes;
		}

		/// <summary>
		/// Converts this location to a string
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format( "{0} ({1},{2},{3})", m_Name, m_X, m_Y, m_Z );
		}


		#region IComparable Members

		/// <summary>
		/// Compares this Location to another object
		/// </summary>
		/// <param name="obj">The object to compare to</param>
		/// <returns>The result value of the comparison</returns>
		public int CompareTo(object obj)
		{
			Location o = obj as Location;

			if ( o == null )
				throw new Exception( string.Format( "Cannot compare Location to {0}", obj.GetType().Name ) );

			return this.m_Name.CompareTo( o.m_Name );
		}

		#endregion
	}
}