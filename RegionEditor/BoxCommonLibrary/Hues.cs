using System;
using System.IO;
using System.Drawing;

namespace TheBox.Mul
{
	/// <summary>
	/// Summary description for Hues.
	/// </summary>
	public class Hues
	{
		[ System.Runtime.InteropServices.DllImport("kernel32", SetLastError=true) ]
		private static extern unsafe bool ReadFile(
			IntPtr hFile,                       // handle to file
			void* pBuffer,                      // data buffer
			int NumberOfBytesToRead,            // number of bytes to read
			int* pNumberOfBytesRead,            // number of bytes read
			int Overlapped                      // overlapped buffer
			);

		private HueGroup[] m_Groups;

		/// <summary>
		/// Gets the hue groups
		/// </summary>
		public HueGroup[] Groups
		{
			get { return m_Groups; }
		}

		/// <summary>
		/// Creates an empty Hues object
		/// </summary>
		private Hues()
		{
			m_Groups = new HueGroup[ 375 ];
		}

		/// <summary>
		/// Loads a Hues object from a file
		/// </summary>
		/// <param name="FileName">The hues.mul file to read from</param>
		/// <returns>A Hues object</returns>
		public static Hues Load( string FileName )
		{
			if ( ! File.Exists( FileName ) )
				return null;

			try
			{
				FileStream stream = new FileStream( FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
				BinaryReader reader = new BinaryReader( stream, System.Text.Encoding.ASCII );

				Hues hues = new Hues();

				for ( int i = 0; i < 375; i++ )
				{
					hues.m_Groups[ i ] = HueGroup.Read( reader );
				}
				stream.Close();
				
				return hues;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Gets or sets a hue
		/// </summary>
		public Hue this[int index]
		{
			get
			{
				if ( index < 1 || index > 3000 )
					return null;

				index--;

				int group = index / 8;
				int entry = index % 8;

				return m_Groups[ group ].HueList[ entry ];
			}
			set
			{
				if ( index < 1 || index > 3000 )
					return;

				int group = index / 8;
				int entry = index & 8;

				m_Groups[ group  ].HueList[ entry ] = value;
			}
		}
	}

	#region Internal Data

	/// <summary>
	/// A group of 8 hues
	/// </summary>
	public class HueGroup
	{
		private int m_Header;
		private Hue[] m_Hues;

		/// <summary>
		/// Gets the list of hues in this groupd
		/// </summary>
		public Hue[] HueList
		{
			get { return m_Hues; }
			set { m_Hues = value; }
		}

		/// <summary>
		/// Reads a HueGroup
		/// </summary>
		/// <param name="reader">The reader of the data stream</param>
		/// <returns>A HueGroupd item</returns>
		public static HueGroup Read( BinaryReader reader )
		{
			HueGroup group = new HueGroup();
			
			group.m_Header = reader.ReadInt32();
			group.m_Hues = new Hue[ 8 ];

			for ( int i = 0; i < 8; i++ )
				group.m_Hues[ i ] = Hue.Read( reader );

			return group;
		}
	}

	/// <summary>
	/// A UO Hue
	/// </summary>
	public class Hue
	{
		private short[] m_ColorTable;
		private short m_TableStart;
		private short m_TableEnd;
		private char[] m_Name;

		/// <summary>
		/// Gets or sets the color table for the hue
		/// </summary>
		public short[] ColorTable
		{
			get { return m_ColorTable; }
			set { m_ColorTable = value; }
		}

		/// <summary>
		/// Reads a Hue
		/// </summary>
		/// <param name="reader">The data reader</param>
		/// <returns>The hue read</returns>
		public static Hue Read( BinaryReader reader )
		{
			Hue hue = new Hue();

			hue.m_ColorTable = new short[ 32 ];
			hue.m_Name = new char[ 20 ];

			for ( int i = 0; i < 32; i++ )
				hue.m_ColorTable[ i ] = reader.ReadInt16();

			hue.m_TableStart = reader.ReadInt16();
			hue.m_TableEnd = reader.ReadInt16();
			
			hue.m_Name = reader.ReadChars( 20 );

			return hue;
		}

		/// <summary>
		/// Gets or sets the 
		/// </summary>
		public string Name
		{
			get { return new string( m_Name ); }
			set
			{
				string name = value;
				if ( name.Length > 20 )
				{
					name.Substring( 0, 20 );
				}

				m_Name = name.ToCharArray();
			}
		}

		/// <summary>
		/// Writes the hue to a stream
		/// </summary>
		/// <param name="writer">The writer used to write the hue</param>
		private void Write( BinaryWriter writer )
		{
			for ( int i = 0; i < 32; i++ )
				writer.Write( m_ColorTable[i] );

			writer.Write( m_TableStart );
			writer.Write( m_TableEnd );

			for ( int i = 0; i < 20; i++ )
				writer.Write( m_Name[i] );
		}

		/// <summary>
		/// Converts a RGB 555 value to a Color object
		/// </summary>
		/// <param name="rgb555color">The RGB 555 value to convert</param>
		/// <returns>A Color corresponding to the value</returns>
		public static Color ToColor( short rgb555color )
		{
			int Red = ( ( rgb555color >> 10 ) & 0x1F ) * 8;
			int Green = ( ( rgb555color >> 5 ) & 0x1F ) * 8;
			int Blue = ( rgb555color & 0x1F ) * 8;

			return Color.FromArgb( Red, Green, Blue);
		}

		/// <summary>
		/// Gets a spectrum corresponding to this hue
		/// </summary>
		/// <param name="imgSize">The size of the spectrum</param>
		/// <returns>A 32x1 bitmap with the spectrum</returns>
		public Bitmap GetSpectrum( Size imgSize )
		{
				Bitmap bmp = new Bitmap( 128, 10 );

				for ( int i = 0; i < 32; i++ )
				{
					for ( int x = 0; x < 4; x++ )
						for ( int y = 0; y < 10; y++ )
						{
							bmp.SetPixel( i * 4 + x, y, ToColor( m_ColorTable[ i ] ) );
						}
				}

			Bitmap bmp1 = new Bitmap( bmp, imgSize );

				return bmp1;
		}
	}

	#endregion
}
