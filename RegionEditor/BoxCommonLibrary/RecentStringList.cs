using System;
using System.Collections.Specialized;

namespace TheBox.Common
{
	/// <summary>
	/// Provides a list of strings recently used
	/// </summary>
	[ Serializable ]
	public class RecentStringList
	{
		private System.Collections.Specialized.StringCollection m_List;
		private int m_Capacity;

		/// <summary>
		/// Gets or sets the list of strings in the collection
		/// </summary>
		public StringCollection List
		{
			get { return m_List; }
			set { m_List = value; }
		}

		/// <summary>
		/// Gets or sets the capacity of the collection
		/// </summary>
		[ System.Xml.Serialization.XmlAttribute ]
		public int Capacity
		{
			get { return m_Capacity; }
			set
			{
				m_Capacity = value;
				if ( m_Capacity < 1 )
					m_Capacity = 1;

				while ( m_Capacity < m_List.Count )
				{
					m_List.RemoveAt( m_List.Count - 1 );
				}
			}
		}

		/// <summary>
		/// Creates a list of string that will feature the most recently used behaviour
		/// </summary>
		public RecentStringList()
		{
			m_List = new StringCollection();
			m_Capacity = 10;
		}

		/// <summary>
		/// Adds a string to the list and positions it in the recent list
		/// </summary>
		/// <param name="text">The string that should be added</param>
		public void AddString( string text )
		{
			if ( m_List.Contains( text ) )
			{
				m_List.Remove( text );
				m_List.Insert( 0, text );
			}
			else
			{
				if ( m_List.Count == m_Capacity )
				{
					m_List.RemoveAt( m_List.Count - 1 );
				}

				m_List.Insert( 0, text );
			}
		}

		/// <summary>
		/// Gets the string at a given position
		/// </summary>
		public string this[int index]
		{
			get
			{
				if ( index < m_List.Count )
					return m_List[ index ];
				else
					return null;
			}
		}

		/// <summary>
		/// Gets an array of strings
		/// </summary>
		/// <returns>A string[] object or null if no string is in the collection</returns>
		public string[] GetArray()
		{
			string[] list = new string[ m_List.Count ];

			for ( int i = 0; i < m_List.Count; i++ )
			{
				list[ i ] = m_List[ i ];
			}

			return list;
		}
	}
}