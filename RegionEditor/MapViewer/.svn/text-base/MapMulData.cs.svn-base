/*/////////////////////////////////////////////////////////////////////////////////////////
 * 
 * This code is provided as is and without any warranties.
 * 
 * You can use the content of this file as you wish. Feel free to re use it
 * and embed it in your applications, modify it and re-distribute it.
 * 
 * In return I only ask you that if you use this code, you make the source of it
 * available to the public. This of course means only the part of your application
 * that directly uses my code.
 * 
 * Of course I appreciated being given the credit if you use this code in your
 * applications, but this isn't necessary.
 * 
 * Arya
 * arya@arya.distanthost.com
 * http://arya.distanthost.com/
 * 
 *///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace TheBox.MulData
{
	/// <summary>
	/// A single map cell
	/// </summary>
	[StructLayoutAttribute( LayoutKind.Sequential, Pack = 1, Size = 0 )]
	internal struct MapCell
	{
		/// <summary>
		/// The cell graphics, looked up in RadarCol
		/// </summary>
		public ushort Color;
		/// <summary>
		/// The altitude of the cell
		/// </summary>
		public sbyte Altitude;

		/// <summary>
		/// Creates a new MapCell
		/// </summary>
		public MapCell( ushort color, sbyte altitude )
		{
			Color = color;
			Altitude = altitude;
		}

		/// <summary>
		/// Gets the size of a MapCell object
		/// </summary>
		public static int Size
		{
			get { return 3;}
		}
	}

	/// <summary>
	/// Map block representing a matrix of 8x8 map cells
	/// </summary>
	internal class MapBlock
	{
		[DllImport("kernel32", SetLastError=true)]
		static extern unsafe bool ReadFile(
			IntPtr hFile,                       // handle to file
			void* pBuffer,                      // data buffer
			int NumberOfBytesToRead,            // number of bytes to read
			int* pNumberOfBytesRead,            // number of bytes read
			int Overlapped                      // overlapped buffer
			);

		/// <summary>
		/// The 64 map cells that are part of this block
		/// </summary>
		public MapCell[] Cells;

		/// <summary>
		/// Creates a new MapBlock
		/// </summary>
		private MapBlock()
		{
			Cells = new MapCell[ 64 ];
		}

		/// <summary>
		/// Gets the size of a MapBlock object
		/// </summary>
		public static int Size
		{
			get { return 196; }
		}

		/// <summary>
		/// Reads a MapBlock from a stream
		/// </summary>
		/// <param name="stream">The stream containing the data to read</param>
		/// <returns>The MapBlock object read from the stream</returns>
		public unsafe static MapBlock ReadFromStream( FileStream stream )
		{
			MapBlock block = new MapBlock();

			// Skip the int Header value, not used by the viewer
			stream.Seek( 4, SeekOrigin.Current );

			int n = 0;

			fixed ( MapCell* pCell = block.Cells )
			{
				// Issue 7 - Handle Warnings - http://code.google.com/p/pandorasbox3/issues/detail?id=7&can=1 - Kons
				ReadFile( stream.SafeFileHandle.DangerousGetHandle(), pCell, 192, &n, 0 );
				// Issue 7 - End
			}
			return block;
		}
	}

	/// <summary>
	/// Index entries for Statics.mul
	/// </summary>
	internal struct StaticIdx
	{
		[DllImport("kernel32", SetLastError=true)]
		static extern unsafe bool ReadFile(
			IntPtr hFile,                       // handle to file
			void* pBuffer,                      // data buffer
			int NumberOfBytesToRead,            // number of bytes to read
			int* pNumberOfBytesRead,            // number of bytes read
			int Overlapped                      // overlapped buffer
			);

		/// <summary>
		/// The start position. OxFFFFFFFF if there is no static object
		/// </summary>
		public int Start;
		/// <summary>
		/// The length of the data segment
		/// </summary>
		public int Length;
		/// <summary>
		/// The use of this field is still unknown
		/// </summary>
		public int Unkown;

		/// <summary>
		/// Gets the size of a StaticIdx object
		/// </summary>
		public static int Size
		{
			get
			{ return 12;}
		}

		/// <summary>
		/// Reads a StaticIdx object from a stream
		/// </summary>
		/// <param name="stream">The stream containing data about a StaticIdx object</param>
		/// <returns>The StaticIdx object read from the stream</returns>
		public unsafe static StaticIdx ReadFromStream( FileStream stream )
		{
			StaticIdx idx;

			int n = 0;

			StaticIdx* pIdx = &idx;
			// Issue 7 - Handle Warnings - http://code.google.com/p/pandorasbox3/issues/detail?id=7&can=1 - Kons
			ReadFile( stream.SafeFileHandle.DangerousGetHandle(), pIdx, 12, &n, 0 );
			// Issue 7 - End

			return idx;
		}
	}

	/// <summary>
	/// A static object
	/// </summary>
	[StructLayoutAttribute( LayoutKind.Sequential, Pack = 1, Size = 0 )]
	internal struct StaticData
	{
		[DllImport("kernel32", SetLastError=true)]
		static extern unsafe bool ReadFile(
			IntPtr hFile,                       // handle to file
			void* pBuffer,                      // data buffer
			int NumberOfBytesToRead,            // number of bytes to read
			int* pNumberOfBytesRead,            // number of bytes read
			int Overlapped                      // overlapped buffer
			);

		/// <summary>
		/// The graphics for this block. Look up in RadarCol adding an offset of 16384
		/// </summary>
		public ushort Color;
		/// <summary>
		/// X offset in the block
		/// </summary>
		public byte XOffest;
		/// <summary>
		/// Y offset in the block
		/// </summary>
		public byte YOffset;
		/// <summary>
		/// Altitude
		/// </summary>
		public sbyte Altitude;
		/// <summary>
		/// The use of this field is still unkown
		/// </summary>
		public ushort Hue;

		/// <summary>
		/// Gets the size of a StaticData object
		/// </summary>
		public static int Size
		{
			get { return 7;}
		}

		/// <summary>
		/// Reads StaticData from a stream
		/// </summary>
		/// <param name="stream">The stream containing information about StaticData</param>
		/// <param name="NumberOfStatics">The number of statics that should be read from the stream</param>
		/// <returns>The StaticData object read from the stream</returns>
		public unsafe static StaticData[] ReadFromStream( FileStream stream, int NumberOfStatics )
		{
			byte[] info = new byte[7];

			StaticData[] data = new StaticData[ NumberOfStatics ];

			fixed ( StaticData* pData = data )
			{
				int n = 0;
				// Issue 7 - Handle Warnings - http://code.google.com/p/pandorasbox3/issues/detail?id=7&can=1 - Kons
				ReadFile( stream.SafeFileHandle.DangerousGetHandle(), pData, NumberOfStatics * 7, &n, 0 );
				// Issue 7 - End
			}

			return data;
		}
	}
}