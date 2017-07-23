using System;
// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
using System.Collections.Generic;
// Issue 10 - End

namespace TheBox.Common
{
	/// <summary>
	/// Provides an implementation of a recently used objects list
	/// </summary>
	[ Serializable ]
	public class RecentList
	{
		private int m_Capacity = 10;
		// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
		private List<object> m_List;
		// Issue 10 - End

		/// <summary>
		/// Creates a new RecentList object
		/// </summary>
		public RecentList()
		{
			// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
			m_List = new List<object>();
			// Issue 10 - End
		}

		/// <summary>
		/// Creates a new RecentList object
		/// </summary>
		/// <param name="capacity"></param>
		public RecentList( int capacity ) : this()
		{
			m_Capacity = 10;
		}

		/// <summary>
		/// Gets or set the maximum capacity for the list
		/// </summary>
		[ System.Xml.Serialization.XmlAttribute ]
		public int Capacity
		{
			get { return m_Capacity; }
			set { m_Capacity = value; }
		}

		/// <summary>
		/// Gets or sets the items in the list
		/// </summary>
		// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
		public List<object> List
		// Issue 10 - End
		{
			get { return m_List; }
			set { m_List = value; }
		}

		/// <summary>
		/// Adds an object to the list
		/// </summary>
		/// <param name="o">The object to add</param>
		public void Add( object o )
		{
			if ( m_List.Contains( o ) )
			{
				// Move object to top
				m_List.Remove( o );
				m_List.Insert( 0, o );
			}
			else
			{
				m_List.Insert( 0, o );
				
				if ( m_List.Count > m_Capacity )
					m_List.RemoveRange( m_Capacity, m_List.Count );
			}
		}
	}
}