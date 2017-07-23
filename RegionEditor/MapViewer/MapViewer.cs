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
// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
using System.Collections.Generic;
// Issue 10 - End
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using TheBox.MapViewer.DrawObjects;
using TheBox.MulData;
using TheBox.Common;
using System.Drawing.Drawing2D;
// Issues 43 - Problems when the client path isn't found - http://code.google.com/p/pandorasbox3/issues/detail?id=43 - Smjert
using TheBox.CustomMessageBox;
// Issues 43 - End

namespace TheBox.MapViewer
{
	/// <summary>
	/// List of supported maps as of Age of Shadows
	/// </summary>
	public enum Maps
	{
		/// <summary>
		/// This identifies all the maps. Should be used only to create draw objects that display on all maps
		/// </summary>
		AllMaps = -1,
		/// <summary>
		/// Map defined by the 0 index
		/// </summary>
		Felucca = 0,
		/// <summary>
		/// Map defined by the 0 index for the base files and 1 for the patch files
		/// </summary>
		Trammel = 1,
		/// <summary>
		/// Map defined by the 2 index
		/// </summary>
		Ilshenar = 2,
		/// <summary>
		/// Map defined by the 3 index
		/// </summary>
		Malas = 3,
		/// <summary>
		/// Map defined by index 4, the Tokuno Islands
		/// </summary>
		Tokuno = 4,
        /// <summary>
		/// Map defined by index 4, the Termur
		/// </summary>
        Termur = 5
	}

	/// <summary>
	/// Specifies the map navigation mode
	/// </summary>
	public enum MapNavigation
	{
		/// <summary>
		/// No built in navigation is used
		/// </summary>
		None,
		/// <summary>
		/// Navigation by left mouse button
		/// </summary>
		LeftMouseButton,
		/// <summary>
		/// Navigation by right mouse button
		/// </summary>
		RightMouseButton,
		/// <summary>
		/// Navigation by middle mouse button
		/// </summary>
		MiddleMouseButton,
		/// <summary>
		/// Navigation by any mouse button
		/// </summary>
		AnyMouseButton
	}

	/// <summary>
	/// Ultima Online map viewer
	/// </summary>
	public class MapViewer : System.Windows.Forms.Control
	{
		/// <summary>
		/// Creates a MapViewer control
		/// </summary>
		public MapViewer()
		{
			// Set styles needed to reduce flickering
			SetStyle( ControlStyles.DoubleBuffer, true );
			SetStyle( ControlStyles.UserPaint, true );
			SetStyle( ControlStyles.AllPaintingInWmPaint, true );

			// Initialize variables
			//m_MapArray = new ArrayList();
			m_ColorMap = new short[ 65536 ];
			// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
			m_DrawObjects = new List<IMapDrawable>();
			// Issue 10 - End

			// Generate the Difl data
			GeneratePatchData();

			// Read the colors
			// Issues 43 - Problems when the client path isn't found - http://code.google.com/p/pandorasbox3/issues/detail?id=43 - Smjert
			if (!LoadRadarcol())
			{
				ErrMsgBox.Show("Impossible to load .mul files", "Error");
				Environment.Exit(1);
			}
			// Issues 43 - End
			

			// Create the default view. This will be most likely changed by the frame
			m_ViewInfo = new MapViewInfo( this );
		}

		static MapViewer()
		{
			m_MulManager = new MulManager();
		}

		#region Events

		/// <summary>
		/// The coordinates of the center of the control have been changed
		/// </summary>
		public event EventHandler MapLocationChanged;

		/// <summary>
		/// The map displayed by the control has changed
		/// </summary>
		public event EventHandler MapChanged;

		/// <summary>
		/// Occurs when the zoom level in the map viewer has been changed
		/// </summary>
		public event EventHandler ZoomLevelChanged;

		/// <summary>
		/// Occurs when the location corresponding to the center of the control changes
		/// </summary>
		/// <param name="e">Standard EventArgs</param>
		protected virtual void OnMapLocationChanged( EventArgs e )
		{
			if ( MapLocationChanged != null )
			{
				MapLocationChanged( this, e );
			}
		}

		/// <summary>
		/// Occurs when the map is changed
		/// </summary>
		/// <param name="e">Standard EventArgs</param>
		protected virtual void OnMapChanged( EventArgs e )
		{
			if ( MapChanged != null )
			{
				MapChanged( this, e );
			}
		}

		/// <summary>
		/// Fires the ZoomLevelChanged event
		/// </summary>
		protected virtual void OnZoomLevelChanged( EventArgs e )
		{
			if ( ZoomLevelChanged != null )
			{
				ZoomLevelChanged( this, e );
			}
		}

		#endregion

		#region Extract Map Image

		/// <summary>
		/// Scale for the extracted image from the map
		/// </summary>
		public enum MapScale
		{
			/// <summary>
			/// Each pixel is extracted
			/// </summary>
			Full = 1,
			/// <summary>
			/// One pixel in two is drawn
			/// </summary>
			Half = 2,
			/// <summary>
			/// One pixel in four is drawn
			/// </summary>
			Fourth = 4,
			/// <summary>
			/// One pixel in eight is drawn
			/// </summary>
			Eigth = 8,
			/// <summary>
			/// One pixel in two blocks is drawn
			/// </summary>
			Sixteenth = 16
		}

		/// <summary>
		/// Extracts an image displaying the full map
		/// </summary>
		/// <param name="map">The map that should be extracted</param>
		/// <param name="scale">The scale value</param>
		/// <param name="FileName">The target filename</param>
		public static unsafe void ExtractMap( Maps map, MapScale scale, string FileName )
		{
			if ( map == Maps.AllMaps )
			{
				throw new Exception( "Cannot extract image for Maps.AllMaps" );
			}

			int index = (int) map;

			string mapfile = m_MulManager[ "map{0}.mul", index ];
			string mapdif =m_MulManager[ "mapdif{0}.mul", index ];
			string mapdifl = m_MulManager[ "mapdifl{0}.mul", index ];
			string sta = m_MulManager[ "statics{0}.mul", index ];
			string staidx = m_MulManager[ "staidx{0}.mul", index ];
			string stadif = m_MulManager[ "stadif{0}.mul", index ];
			string stadifi = m_MulManager[ "stadifi{0}.mul", index ];
			string stadifl = m_MulManager[ "stadifl.mul", index ];
			string col = m_MulManager[ "radarcol.mul", index ];

			// Issue 15 - Trammel has own map after ML - http://code.google.com/p/pandorasbox3/issues/detail?id=15 - Kons
			if (index == 1 && (!File.Exists(mapfile)))
			{
				mapfile = m_MulManager["map0.mul"];
				sta = m_MulManager["statics0.mul"];
				staidx = m_MulManager["staidx0.mul"];
			}
			// Issue 15 - End.

			if ( !File.Exists( col ) )
			{
				throw new FileNotFoundException( "radarcol.mul not found." );
			}

			if ( !File.Exists( mapfile ) )
			{
				throw new FileNotFoundException( string.Format( "File {0} doens't exist. Impossible to extract the map.", mapfile ) );
			}

			bool mappatch = false;

			if ( File.Exists( mapdif ) && File.Exists( mapdifl ) )
				mappatch = true;

			bool statics = false;

			if ( File.Exists( sta ) && File.Exists( staidx ) )
				statics = true;

			bool stapatch = false;

			if ( File.Exists( stadif ) && File.Exists( stadifi ) && File.Exists( stadifl ) )
				stapatch = true;

			if ( scale == MapScale.Eigth || scale == MapScale.Sixteenth )
				statics = false;

			int blocksDelta = 1;
			if ( scale == MapScale.Sixteenth )
				blocksDelta = 2;

			#region ColorMap
			short[] ColorMap = new short[ 65536 ];

			FileStream colstream = new FileStream( col, FileMode.Open );

			int n = 0;

            
			fixed ( short* pColorMap = ColorMap )
			{
				// Issue 7 - Handle Warnings - http://code.google.com/p/pandorasbox3/issues/detail?id=7&can=1 - Kons
				ReadFile(colstream.SafeFileHandle.DangerousGetHandle(), pColorMap, 131072, &n, 0);
				// Issue 7 - End
			}

			colstream.Close();
			#endregion

			#region Streams, MapPatch and StaPatch

			FileStream mapstream = new FileStream( mapfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
			FileStream stastream = null;
			FileStream staidxstream = null;
			FileStream mapdifstream = null;
			FileStream mapdiflstream = null;
			FileStream stadifstream = null;
			FileStream stadifistream = null;
			FileStream stadiflstream = null;

			// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
			Dictionary<int,int> MapPatch = null;
			Dictionary<int,int> StaPatch = null;
			// Issue 10 - End

			if ( statics )
			{
				stastream = new FileStream( sta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
				staidxstream = new FileStream( staidx, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );

				if ( stapatch )
				{
					stadifstream = new FileStream( stadif, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
					stadifistream = new FileStream( stadifi, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
					stadiflstream = new FileStream( stadifl, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );

					// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
					StaPatch = new Dictionary<int, int>();
					// Issue 10 - End

					BinaryReader reader = new BinaryReader( stadiflstream );

					int i = 0;

					// Issue 1 - Char buffer too small Exception - http://code.google.com/p/pandorasbox3/issues/detail?id=1 - Smjert
					while ( reader.BaseStream.Position < reader.BaseStream.Length )
					// Issue 1 - End
					{
						int key = reader.ReadInt32();
						StaPatch[ key ] = i++;
					}
				}
			}

			if ( mappatch )
			{
				mapdifstream = new FileStream( mapdif, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
				mapdiflstream = new FileStream( mapdifl, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );

				// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
				MapPatch = new Dictionary<int, int>();
				// Issue 10 - End

				BinaryReader reader = new BinaryReader( mapdiflstream );

				int i = 0;

				// Issue 1 - Char buffer too small Exception - http://code.google.com/p/pandorasbox3/issues/detail?id=1 - Smjert
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				// Issue 1 - End
				{
					int key = reader.ReadInt32();
					
					MapPatch[ key ] = i++;
				}

				mapdiflstream.Close();
			}

			#endregion

			int PPB = 8 / (int) scale;
			if ( PPB < 1 )
				PPB = 1;

			int step = (int) scale;
			Size size = MapSizes.GetSize( index );

			int xblocks = size.Width / 8;
			int yblocks = size.Height / 8;
			
			size.Width = size.Width / step;
			size.Height = size.Height / step;

			Bitmap bmp = new Bitmap( size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555 );
			System.Drawing.Imaging.BitmapData bData = bmp.LockBits( new Rectangle( new Point( 0,0 ), size ), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format16bppRgb555 );

			short* pixelPtr = ( short* ) bData.Scan0;

			for ( int x = 0; x < xblocks; x += blocksDelta )
			{
				for ( int y = 0; y < yblocks; y += blocksDelta )
				{
					int bindex = x * yblocks + y;

					MapBlock bMap = null;

					// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
					if ( mappatch && MapPatch.ContainsKey( bindex ) )
					{
						// Patched map block
						mapdifstream.Seek( MapPatch[ bindex ] * MapBlock.Size, SeekOrigin.Begin );
						// Issue 10 - End
						bMap = MapBlock.ReadFromStream( mapdifstream );
					}
					else
					{
						mapstream.Seek( bindex * MapBlock.Size, SeekOrigin.Begin );
						bMap = MapBlock.ReadFromStream( mapstream );
					}

					#region Statics

					if ( statics )
					{
						StaticIdx idx;
						StaticData[] sData = null;
						int NumOfStatics = 0;

						// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
						if ( stapatch && StaPatch.ContainsKey( bindex ) )
						{
							// Patch
							stadifistream.Seek( StaPatch[ bindex ] * StaticIdx.Size, SeekOrigin.Begin );
							// Issue 10 - End
							staidxstream.Seek( StaticIdx.Size, SeekOrigin.Current );

							idx = StaticIdx.ReadFromStream( stadifistream );

							if ( idx.Start != -1 )
							{
								NumOfStatics = idx.Length / StaticData.Size;
								stadifstream.Seek( idx.Start, SeekOrigin.Begin );
								sData = StaticData.ReadFromStream( stadifstream, NumOfStatics );
							}
						}
						else
						{
							// No patch
							idx = StaticIdx.ReadFromStream( staidxstream );

							if ( idx.Start != -1 )
							{
								NumOfStatics = idx.Length / StaticData.Size;
								stastream.Seek( idx.Start, SeekOrigin.Begin );
								sData = StaticData.ReadFromStream( stastream, NumOfStatics );
							}
						}

						if ( NumOfStatics > 0 )
						{
							foreach ( StaticData sd in sData )
							{
								int sindex = sd.YOffset * 8 + sd.XOffest;

								if ( bMap.Cells[ sindex ].Altitude <= sd.Altitude )
								{
									bMap.Cells[ sindex ].Altitude = sd.Altitude;
									bMap.Cells[ sindex ].Color = (ushort) (sd.Color + StaticOffset);
								}
							}
						}
					}

					#endregion

					#region Drawing

					if ( scale == MapScale.Sixteenth )
					{
						int x0 = x / 2;
						int y0 = y / 2;

						int delta = y0 * size.Width + x0;

						pixelPtr[ delta ] = ColorMap[ bMap.Cells[0].Color ];
					}
					else
					{
						int x0 = x * PPB;
						int y0 = y * PPB;

						for ( int xc = 0; xc < PPB; xc++ )
						{
							for ( int yc = 0; yc < PPB; yc++ )
							{
								int cell = yc * step * 8 + xc * step;

								int delta = ( y0 + yc ) * size.Width + x0 + xc;

								pixelPtr[ delta ] = ColorMap[ bMap.Cells[ cell ].Color ];
							}
						}
					}

					#endregion
				}
			}

			bmp.UnlockBits( bData );

			bmp.Save( FileName, System.Drawing.Imaging.ImageFormat.Jpeg );

			bmp.Dispose();
		}

		#endregion
		
		#region Drawing

		/// <summary>
		/// This does the actual drawing of the control
		/// </summary>
		/// <param name="e">The Event Args for the OnPaint event</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if ( ! ValidatePath() )
			{
				base.OnPaint( e );

				// Display error about not being able to find the map files

				Font font = new Font( FontFamily.GenericSansSerif, 8 );

				Color color = SystemColors.ControlText;

				Brush brush = new SolidBrush( color );

				StringFormat format = new StringFormat();

				format.LineAlignment = StringAlignment.Near;

				int mapnumber = GetMapIndex();
				if ( mapnumber == 1 )
					mapnumber = 0;

				string error = "Couldn't find the required files";

				e.Graphics.DrawString( error, font, brush, this.Bounds, format );

				return;
			}

			if ( ! m_isRefreshed )
				try
				{
					CalculateMap();
				}
				catch ( Exception err )
				{
					base.OnPaint( e );

					// Create a log and write the exception
					string filename = this.GetType().Assembly.Location;
					string folder = Path.GetDirectoryName( filename );
					string crashlog = Path.Combine( folder, "MapViewer crashlog.txt" );

					using ( StreamWriter writer = new StreamWriter( crashlog ) )
					{
						writer.WriteLine( "Log generated on {0}", DateTime.Now.ToString() );
						writer.WriteLine( err.ToString() );
					}

					// Display the error message
					Font font = new Font( FontFamily.GenericSansSerif, 8 );
					Color color = SystemColors.ControlText;
					Brush brush = new SolidBrush( color );
					StringFormat format = new StringFormat();
					format.LineAlignment = StringAlignment.Center;

					string error = string.Format( "An unexpected error occurred. A crash log has been generated at {0}. You can submit it to http://arya.distanthost.com/ for support", crashlog );

					e.Graphics.DrawString( error, font, brush, Bounds, format );

					return;
				}

			//
			// Rotate control
			//

			e.Graphics.ResetTransform();

			if ( m_ViewInfo.RotateView )
			{
				int xtrans = Size.Width / 2;
				int ytrans = Size.Height / 2;

				e.Graphics.TranslateTransform( -xtrans, -ytrans, MatrixOrder.Append );
				e.Graphics.RotateTransform( 45, MatrixOrder.Append );
				e.Graphics.TranslateTransform( xtrans, ytrans, MatrixOrder.Append );
			}

			// Determine origin for image
			int x0 = 0;
			int y0 = 0;

			if ( m_ViewInfo.RotateView )
			{
				x0 = - ( m_ViewInfo.ImageSize.Width - Width ) / 2;
				y0 = - ( m_ViewInfo.ImageSize.Height - Height ) / 2;
			}

			e.Graphics.DrawImage( m_Image, x0, y0 );

			//
			// DRAW OBJECTS
			//

			foreach ( IMapDrawable drawObject in m_DrawObjects )
				if ( drawObject.IsVisible( m_ViewInfo.Bounds, m_Map ) )
					drawObject.Draw( e.Graphics, m_ViewInfo );

			//
			// CROSS
			//

			if ( m_ShowCross )
			{
				Brush crossBrush = new SolidBrush( Color.FromArgb( 180, Color.White ) );
				Pen crossPen = new Pen( crossBrush );

				int xc = Size.Width / 2;
				int yc = Size.Height / 2;

				e.Graphics.DrawLine( crossPen, xc - 4, yc, xc + 4, yc );
				e.Graphics.DrawLine( crossPen, xc, yc - 4, xc, yc + 4 );

				crossBrush.Dispose();
				crossPen.Dispose();
			}
		}

		/// <summary>
		/// Creates the image used for drawing the control
		/// </summary>
		private unsafe void CreateImage()
		{
			try
			{
				int width = m_ViewInfo.ImageSize.Width;
				int height = m_ViewInfo.ImageSize.Height;

				if ( ( m_Image == null ) || ( m_Image.Size != new Size( width, height ) ) )
					m_Image = new Bitmap( width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555 );	// Use the UO 16 bit format right away

				// Lock image
				System.Drawing.Imaging.BitmapData bData = m_Image.LockBits( new Rectangle( 0, 0, m_Image.Width, m_Image.Height ), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format16bppRgb555 );

				// Get a pointer to the image pixels
				short* pixelPtr = ( short* ) bData.Scan0;

				m_ViewInfo.Initialize();

				for ( int y = 0; y < height; y++ )
				{
					for ( int x = 0; x < width; x++ )
					{
						PixelCoordinate info = m_ViewInfo.GetInfo();
					
						if ( info.Block == -1 )
							pixelPtr[0] = (short) 0;
						else
						{
							pixelPtr[0] = m_ColorMap[ m_MapBlocks[ info.Block ].Cells[ info.Cell ].Color ];
						}

						pixelPtr++;
					}
				}

				m_Image.UnlockBits( bData );
			}
			catch ( Exception err )
			{
				MessageBox.Show( err.ToString() );
			}
		}

		#endregion

		#region Enums

		/// <summary>
		/// Types of mul files used to describe maps
		/// </summary>
		private enum MulFileType
		{
			Map,
			Statics,
			Staidx,
			MapDif,
			MapDifl,
			StaDif,
			StaDifl,
			StaDifi,
			RadarCol
		}

		#endregion

		#region Constants

		/// <summary>
		/// The offset used to lookup colors for static objects in Radarcol.mul
		/// </summary>
		private const int StaticOffset = 16384;

		#endregion

		#region Variables

		/// <summary>
		/// The map currently displayed
		/// </summary>
		private Maps m_Map = Maps.Felucca;

		/// <summary>
		/// Specifies whether the control has been refreshed and doesn't need to be redrawn
		/// </summary>
		private bool m_isRefreshed = false;

		/// <summary>
		/// Specifies whether the map should draw statics or not
		/// </summary>
		private bool m_drawStatics = false;

		/// <summary>
		/// Array of byte values used to display the map
		/// </summary>
		// Non usato
		//private ArrayList m_MapArray;

		/// <summary>
		/// The list of the colors for the map display
		/// </summary>
		private short[] m_ColorMap;

		/// <summary>
		/// The blocks used to display the current map part
		/// </summary>
		private MapBlock[] m_MapBlocks;

		/// <summary>
		/// The image used to draw the control
		/// </summary>
		private Bitmap m_Image;

		/// <summary>
		/// The leftmost valid block read from file
		/// </summary>
		private int m_LeftBlock;

		/// <summary>
		/// The rightmost block read from file
		/// </summary>
		private int m_RightBlock;

		/// <summary>
		/// The topmost block read from file
		/// </summary>
		private int m_TopBlock;

		/// <summary>
		/// The bottom block read from file
		/// </summary>
		private int m_BottomBlock;

		/// <summary>
		/// Patch information for the map
		/// </summary>
		// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
		private Dictionary<int,int> m_MapPatch;
		// Issue 10 - End

		/// <summary>
		/// Patch information for the statics
		/// </summary>
		// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
		private Dictionary<int,int> m_StaPatch;
		// Issue 10 - End

		/// <summary>
		/// Error display on the control surface
		/// </summary>
		private bool m_DisplayErrors = true;

		/// <summary>
		/// The list of the objects drawn on the map
		/// </summary>
		// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
		private List<IMapDrawable> m_DrawObjects;
		// Issue 10 - End

		/// <summary>
		/// Displays the cross at the center of the map
		/// </summary>
		private bool m_ShowCross = false;

		/// <summary>
		/// Contains all the information about the current view of the map on the control
		/// </summary>
		private MapViewInfo m_ViewInfo;

		/// <summary>
		/// The file manager to use with Pandora's Box
		/// </summary>
		private static MulManager m_MulManager;

		/// <summary>
		/// Specifies whether the map viewer should zoom when using the mouse wheel
		/// </summary>
		private bool m_WheelZoom = false;

		/// <summary>
		/// Specifies the built in navigation mode
		/// </summary>
		private MapNavigation m_Navigation = MapNavigation.None;

		/// <summary>
		/// Specifies the X-Ray mode where statics below the map are displayed
		/// </summary>
		private bool m_XRayView = false;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the navigation style
		/// </summary>
		[ Category( "Settings" ), Description( "Specifies the navigation style on the map viewer" ) ]
		public MapNavigation Navigation
		{
			get { return m_Navigation; }
			set { m_Navigation = value; }
		}

		/// <summary>
		/// States whether the view should be rotated
		/// This feature is NOT supported and currently buggy because it doesn't work properly on all sizes
		/// </summary>
		[ Category( "Settings" ), Description( "Specifies whether the view should be rotated" ) ]
		public bool RotateView
		{
			get
			{
				return m_ViewInfo.RotateView;
			}
			set
			{
				if ( m_ViewInfo.RotateView != value )
				{
					m_ViewInfo.RotateView = value;
					
					InvalidateMap();
				}
			}
		}

		/// <summary>
		/// The zoom level of the map. Acceptable values are from -3 to 4.
		/// </summary>
		[
		Category( "Settings" ),
		Description( "The zoom level for the map. Values range from -3 to 4." )
		]
		public int ZoomLevel
		{
			get
			{
				return m_ViewInfo.ZoomLevel;
			}
			set
			{
				if ( m_ViewInfo.ZoomLevel == value )
					return;

				int zoomLevel = value;

				if ( zoomLevel > 4 )
					zoomLevel = 4;
				if ( zoomLevel < -3 )
					zoomLevel = -3;

				// Calculate the map view
				m_ViewInfo.ZoomLevel = zoomLevel;

				InvalidateMap();

				OnZoomLevelChanged( new EventArgs() );
			}
		}

		/// <summary>
		/// Gets or sets a value specifying whether the map viewer should display statics or not
		/// </summary>
		[
		Category( "Settings" ),
		Description( "Controls the display of statics on the control." )
		]
		public bool DrawStatics
		{
			get
			{
				return m_drawStatics;
			}
			set
			{
				if ( m_drawStatics != value )
				{
					m_drawStatics = value;

					InvalidateMap();
				}
			}
		}

		/// <summary>
		/// Gets the width of the current map
		/// </summary>
		[ Browsable( false ) ]
		public int MapWidth
		{
			get
			{
				return m_ViewInfo.MapSize.Width;
			}
		}

		/// <summary>
		/// Gets the height of the current map
		/// </summary>
		[ Browsable( false ) ]
		public int MapHeight
		{
			get
			{
				return m_ViewInfo.MapSize.Height;
			}
		}

		/// <summary>
		/// Gets the number of horizontal map blocks for the current map
		/// </summary>
		[ Browsable( false ) ]
		private int XBlocks
		{
			get
			{
				return m_ViewInfo.MapSize.Width / 8;
			}
		}

		/// <summary>
		/// Gets the number of vertical map blocks for the current map
		/// </summary>
		[ Browsable( false ) ]
		private int YBlocks
		{
			get
			{
				return m_ViewInfo.MapSize.Height / 8;
			}
		}

		/// <summary>
		/// Gets or sets the coordinates of the center of the map
		/// </summary>
		[
		Category( "Settings" ),
		Description( "The map coordinates of the center point of the control." )
		]
		public Point Center
		{
			set
			{
				if ( ( m_ViewInfo.Center.X != value.X ) || m_ViewInfo.Center.Y != value.Y )
				{
					m_ViewInfo.Center = value;
					InvalidateMap();

					OnMapLocationChanged( new EventArgs() );
				}
			}
			get
			{
				return m_ViewInfo.Center;
			}
		}

		/// <summary>
		/// Gets or sets the displayed map
		/// </summary>
		[
		Category( "Settings" ),
		Description( "The map type displayed." )
		]
		public Maps Map
		{
			get
			{
				return m_Map;
			}
			set
			{
				if ( value != m_Map )
				{
					m_Map = value;

					m_ViewInfo.Map = m_Map;

					GeneratePatchData();
					InvalidateMap();

					OnMapChanged( new EventArgs() );
				}
			}
		}

		/// <summary>
		/// Gets or sets a value stating whether error messages get displayed on the control surface
		/// </summary>
		[
		Category( "Settings" ),
		Description( "Specifies whether the control displays error messages on its surface" )
		]
		public bool DisplayErrors
		{
			get
			{
				return m_DisplayErrors;
			}
			set
			{
				m_DisplayErrors = value;
			}
		}

		/// <summary>
		/// Controls the display of a small cross at the center of the control
		/// </summary>
		[
		Category( "Settings" ),
		Description( "Controls the display of a small cross at the center of the control" )
		]
		public bool ShowCross
		{
			get
			{
				return m_ShowCross;
			}
			set
			{
				if ( m_ShowCross != value )
				{
					m_ShowCross = value;
					Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the list of drawing objects on the map
		/// </summary>
		[ Browsable( false ) ]
		// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
		public List<IMapDrawable> DrawObjects
		// Issue 10 - End
		{
			get { return m_DrawObjects; }
			set { m_DrawObjects = value; }
		}

		/// <summary>
		/// Gets or sets the mul file manager
		/// </summary>
		[ Browsable( false ) ]
		public MulManager MulManager
		{
			get
			{
				if ( m_MulManager == null )
				{
					m_MulManager = new MulManager();
				}

				return m_MulManager;
			}
			set { m_MulManager = value; }
		}

		/// <summary>
		/// Gets the mul file manager
		/// </summary>
		[ Browsable( false ) ]
		public static MulManager MulFileManager
		{
			get 
			{
				if ( m_MulManager == null )
				{
					m_MulManager = new MulManager();
				}

				return m_MulManager;
			}
			set { m_MulManager = value; }
		}

		/// <summary>
		/// States whether the map viewer can use the mouse wheel for zoom purposes
		/// </summary>
		[ Category( "Settings" ), Description( "States whether the map viewer will use mouse wheel zoom input for zooming." ) ]
		public bool WheelZoom
		{
			get { return m_WheelZoom; }
			set { m_WheelZoom = value; }
		}

		/// <summary>
		/// Specifies the X-Ray view mode
		/// </summary>
		[ Category( "Settings" ), Description( "Enables X-Ray view where statics below the ground are displayed" ) ]
		public bool XRayView
		{
			get { return m_XRayView; }
			set
			{
				if ( m_XRayView != value )
				{
					m_XRayView = value;

					InvalidateMap();
				}
			}
		}

		#endregion

		#region Handlers

		/// <summary>
		/// Overriden OnResize event handler
		/// </summary>
		/// <param name="e">Provided by the framework</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			// Update the size on the view
			m_ViewInfo.ControlSize = this.Size;

			if ( this.Created )
			{
				InvalidateMap();
			}
		}

		/// <summary>
		/// Handles zooming with the wheel
		/// </summary>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if ( m_WheelZoom )
			{
				if ( e.Delta > 0 )
				{
					ZoomIn();
				}
				else if ( e.Delta < 0 )
				{
					ZoomOut();
				}
			}
		}

		/// <summary>
		/// Handles the mouse down event to provide built in map navigation
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			bool navigate = false;

			switch ( m_Navigation )
			{
				case MapNavigation.AnyMouseButton:

					navigate = e.Button != MouseButtons.None;
					break;

				case MapNavigation.LeftMouseButton:

					navigate = e.Button == MouseButtons.Left;
					break;

				case MapNavigation.RightMouseButton:

					navigate = e.Button == MouseButtons.Right;
					break;

				case MapNavigation.MiddleMouseButton:

					navigate = e.Button == MouseButtons.Middle;
					break;
			}

			if ( navigate )
			{
				Point c = m_ViewInfo.ControlToMap( new Point( e.X, e.Y ) );
				Center = c;
			}

			base.OnMouseDown (e);
		}

		#endregion

		#region Methods

		[System.Runtime.InteropServices.DllImport("kernel32", SetLastError=true)]
		static extern unsafe bool ReadFile(
			IntPtr hFile,                       // handle to file
			void* pBuffer,                      // data buffer
			int NumberOfBytesToRead,            // number of bytes to read
			int* pNumberOfBytesRead,            // number of bytes read
			int Overlapped                      // overlapped buffer
			);

		/// <summary>
		/// Gets the string corresponding to the specified file for a given map
		/// </summary>
		/// <param name="MulType">The MulFileType value stating which file is being requested</param>
		/// <param name="map">The map referenced by the requested file</param>
		/// <returns></returns>
		private string GetMulFile( MulFileType MulType, Maps map )
		{
			if ( m_MulManager != null )
			{
				if ( MulType == MulFileType.RadarCol )
				{
					return m_MulManager[ "{0}.mul", MulType.ToString() ];
				}
				else
				{
					if ( map == Maps.Trammel && ( MulType == MulFileType.Map || MulType == MulFileType.Staidx || MulType == MulFileType.Statics ) )
					{
						map = Maps.Felucca;
					}

					return m_MulManager[ "{0}{1}.mul", MulType.ToString(), (int) map ];
				}
			}

			return null;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		/// <summary>
		/// Gets the value corresponding to the map currently selected. 1 for Trammel
		/// </summary>
		/// <returns>The progressive number of the selected map</returns>
		private int GetMapIndex()
		{
			switch ( m_Map )
			{
				case Maps.Felucca:
					return 0;
				case Maps.Trammel:
					return 1;
				case Maps.Ilshenar:
					return 2;
				case Maps.Malas:
					return 3;
				case Maps.Tokuno:
					return 4;
                case Maps.Termur:
                    return 5;
            }

			throw new Exception( "Cannot find index of the map currently selected" );
		}

		/// <summary>
		/// Gets the full path of the specified mul file
		/// </summary>
		/// <param name="MulType">The type of mul file</param>
		/// <param name="MapIndex">The map index that will be appended in the filename</param>
		/// <returns>The full path of the chosen file</returns>
		private string GetMulFile( MulFileType MulType, int MapIndex )
		{
			return GetMulFile( MulType, (Maps) MapIndex );
		}

		/// <summary>
		/// Retrieves the string corresponding to the given MulFileType
		/// </summary>
		/// <param name="MulType">The MulFileType that must be converted to string</param>
		/// <returns>A string that represents the initial name of the mul file specified by MulFileType</returns>
		private string MulTypeToString( MulFileType MulType )
		{
			switch ( MulType )
			{
				case MulFileType.Map:
					return "map";
				case MulFileType.MapDif:
					return "mapdif";
				case MulFileType.MapDifl:
					return "mapdifl";
				case MulFileType.StaDif:
					return "stadif";
				case MulFileType.StaDifi:
					return "stadifi";
				case MulFileType.StaDifl:
					return "stadifl";
				case MulFileType.Staidx:
					return "staidx";
				case MulFileType.Statics:
					return "statics";
			}

			throw new Exception( "Cannot recognize mul file type: " + MulType.ToString() );
		}

		/// <summary>
		/// Ensures that the currently selected path includes the files necessary to display the map
		/// </summary>
		/// <returns>True if the required files are on the path, false otherwise</returns>
		private bool ValidatePath()
		{
			// Check only for map.mul, statics.mul and staidx.mul
			if ( File.Exists( GetMulFile( MulFileType.Map, m_Map ) ) && File.Exists( GetMulFile( MulFileType.Statics, m_Map ) ) && File.Exists( GetMulFile( MulFileType.Staidx, m_Map ) ) )
				return true;
			else
				return true;
		}

		/// <summary>
		/// Reads the Difl files needed for patching purposes
		/// </summary>
		private void GeneratePatchData()
		{
			//
			// MAP: Read from mapdiflX.mul
			//
			// Note: Some blocks are duplicated.
			// Always use the second block or the map will miss pieces

			// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
			m_MapPatch = new Dictionary<int,int>();
			// Issue 10 - End

			if ( File.Exists( GetMulFile( MulFileType.MapDifl, m_Map ) ) )
			{
				FileStream stream = new FileStream( GetMulFile( MulFileType.MapDifl, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );
				BinaryReader reader = new BinaryReader( stream );

				int index = 0;

				// Issue 1 - Char buffer too small Exception - http://code.google.com/p/pandorasbox3/issues/detail?id=1 - Smjert
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				// Issue 1 - End
				{
					// Read data. Data stored in the file is the key
					// The index of the key is the value
					int key = reader.ReadInt32();

					m_MapPatch[ key ] = index++;
				}

				stream.Close();
			}

			//
			// STATICS: Read from stadiflX.mul
			//
			// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
			m_StaPatch = new Dictionary<int,int>();
			// Issue 10 - End

			if ( File.Exists( GetMulFile( MulFileType.StaDifl, m_Map ) ) )
			{
				FileStream stream = new FileStream( GetMulFile( MulFileType.StaDifl, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );
				BinaryReader reader = new BinaryReader( stream );

				int index = 0;

				// Issue 1 - Char buffer too small Exception - http://code.google.com/p/pandorasbox3/issues/detail?id=1 - Smjert
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				// Issue 1 - End
				{
					int key = reader.ReadInt32();

					m_StaPatch[ key ] = index++;
				}

				stream.Close();
			}
		}

		/// <summary>
		/// Reads the Radarcol.mul file to retrieve the color
		/// </summary>
		// Issues 43 - Problems when the client path isn't found - http://code.google.com/p/pandorasbox3/issues/detail?id=43 - Smjert
		private unsafe bool LoadRadarcol()
		{
			//  Issue 37:  	 Profile error - Tarion
			string mulFile = GetMulFile( MulFileType.RadarCol, m_Map );
			if (mulFile == null || mulFile == String.Empty)
                if (!MulManager.FixClientPath())
                    
					return false;

			FileStream stream = new FileStream(mulFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			// End  Issue 37

			int n = 0;

			fixed ( short* pColorMap = m_ColorMap )
			{
				// Issue 7 - Handle Warnings - http://code.google.com/p/pandorasbox3/issues/detail?id=7&can=1 - Kons
				ReadFile( stream.SafeFileHandle.DangerousGetHandle(), pColorMap, 131072, &n, 0 );
				// Issue 7 - End
			}

			stream.Close();

			return true;
		}
		// Issues 43 - End

		/// <summary>
		/// Converts a point on the control surface to map coordinates
		/// </summary>
		/// <param name="point">A point on the surface of the control</param>
		/// <returns>The point in map coordinates</returns>
		public Point ControlToMap( Point point )
		{
			return m_ViewInfo.ControlToMap( point );
		}

		/// <summary>
		/// Converts a horizontal coordinate from the control to map
		/// </summary>
		/// <param name="x">The x coordinate on the control</param>
		/// <returns>The x coordinate on the map</returns>
		public int ControlToMapX( int x )
		{
			return m_ViewInfo.ControlToMap( new Point( x,0 ) ).X;
		}

		/// <summary>
		/// Converts a vertical coordinate on the control to map coordinates
		/// </summary>
		/// <param name="y">The y coordinate on the control</param>
		/// <returns>The corresponding y coordinate on the map</returns>
		public int ControlToMapY( int y )
		{
			return m_ViewInfo.ControlToMap( new Point( 0,y ) ).Y;
		}

		private void CalculateMap()
		{
			// Get the pixel info for the left-top and right-bottom points
			
			BlockInfo TopLeft = m_ViewInfo.TopLeft;
			BlockInfo BottomRight = m_ViewInfo.BottomRight;

			// Verify if the blocks are valid and there's a portion of map actually visible
			if ( ( BottomRight.XBlock < 0 ) ||
				( BottomRight.YBlock < 0 ) ||
				( TopLeft.XBlock >= XBlocks ) ||
				( TopLeft.YBlock >= YBlocks ) )
			{
				CreateImage();
				return;
			}

			TopLeft.Validate();
			BottomRight.Validate();

			m_LeftBlock = TopLeft.XBlock;
			m_RightBlock = BottomRight.XBlock;
			m_TopBlock = TopLeft.YBlock;
			m_BottomBlock = BottomRight.YBlock;

			//
			// There's a map piece to draw
			//

			// Open the streams
			FileStream mapStream = new FileStream( GetMulFile( MulFileType.Map, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );
			FileStream staidxStream = new FileStream( GetMulFile( MulFileType.Staidx, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );
			FileStream staticsStream = new FileStream( GetMulFile( MulFileType.Statics, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );

			//
			// Map patch files
			//
			// The map can be displayed even if those don't exist
			//

			bool mappatch = false;

			FileStream mapDifStream = null;

			try
			{
				mapDifStream = new FileStream( GetMulFile( MulFileType.MapDif, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );

				mappatch = true;
			}
			catch {}

			//
			// Statics patch files
			//
			// Not required

			bool stapatch = false;

			FileStream staDifiStream = null;
			FileStream staDifStream = null;

			try
			{
				staDifiStream = new FileStream( GetMulFile( MulFileType.StaDifi, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );
				staDifStream = new FileStream( GetMulFile( MulFileType.StaDif, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );

				stapatch = true;
			}
			catch {}

			// Create the blocks array
			m_MapBlocks = new MapBlock[ m_ViewInfo.ValidXBlocks * m_ViewInfo.ValidYBlocks ];

			// This is the index for the array
			int index = 0;

			// Avoid properties to improve performance
			int t_YBlocks = YBlocks;
			int t_MapBlockSize = MapBlock.Size;
			int t_StaticIdxSize = StaticIdx.Size;
			int t_StaticDataSize = StaticData.Size;

			// Blocks are stored top to bottom -> left to right, so read column by column
			for ( int x = m_LeftBlock; x <= m_RightBlock; x++ )
			{
				// Get the posizion in the stream
				int streamSeek = ( x * t_YBlocks + m_TopBlock );

				// Position streams
				mapStream.Seek( streamSeek * t_MapBlockSize, SeekOrigin.Begin );
				staidxStream.Seek( streamSeek * t_StaticIdxSize, SeekOrigin.Begin );				

				for ( int i = 0; i < ( m_BottomBlock - m_TopBlock + 1 ); i++ )
				{
					int BlockNumber = x * t_YBlocks + m_TopBlock + i;

					// m_MapBlocks[ index ] = MapBlock.Read( mapReader );
					m_MapBlocks[ index ] = MapBlock.ReadFromStream( mapStream );

					// Verify if this block is patched
					// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
					if ( mappatch && m_MapPatch.ContainsKey( BlockNumber ) )
					{
						mapDifStream.Seek( m_MapPatch[ BlockNumber ] * t_MapBlockSize, SeekOrigin.Begin );
						// Issue 10 - End
						m_MapBlocks[ index ] = MapBlock.ReadFromStream( mapDifStream );
					}

					// This is used to determine the X-Ray view later
					int[] altitudes = new int[ 64 ];
					for ( int h = 0; h < 64; h++ )
					{
						altitudes[ h ] = m_MapBlocks[ index ].Cells[ h ].Altitude;
					}

					// STATICS
					if ( m_drawStatics && ( m_ViewInfo.ZoomLevel > -2 ) )
					{
						// StaticIdx idx = StaticIdx.Read( staidxReader );
						StaticIdx idx = StaticIdx.ReadFromStream( staidxStream );

						// BinaryReader muleStaReader = staticsReader;
						FileStream muleStaStream = staticsStream;

						// Verify if the statics are patched
						// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
						if ( stapatch && m_StaPatch.ContainsKey( BlockNumber ) )
						{
							staDifiStream.Seek( m_StaPatch[ BlockNumber ] * t_StaticIdxSize, SeekOrigin.Begin );
							// Issue 10 - End
	
							idx = StaticIdx.ReadFromStream( staDifiStream );

							// Update the mule variables to read from the patch file for this block
							muleStaStream = staDifStream;
						}

						// Check for statics
						if ( idx.Start != -1 )
						{
							int NumOfStatics = idx.Length / t_StaticDataSize;

							// Seek the statics stream
							muleStaStream.Seek( idx.Start, SeekOrigin.Begin );

							StaticData[] Statics = StaticData.ReadFromStream( muleStaStream, NumOfStatics );

							foreach ( StaticData sData in Statics )
							{
								// The cell number corresponding to this static object in the current map block
								int cellPosition = sData.YOffset * 8 + sData.XOffest;

								if ( m_XRayView && sData.Altitude < altitudes[ cellPosition ] )
								{
									// Here we are under the map
									if ( m_MapBlocks[ index ].Cells[ cellPosition ].Altitude == altitudes[ cellPosition ] || sData.Altitude >= m_MapBlocks[ index ].Cells[ cellPosition ].Altitude )
									{
										m_MapBlocks[ index ].Cells[ cellPosition ].Color = ( ushort ) ( sData.Color + StaticOffset );
										m_MapBlocks[ index ].Cells[ cellPosition ].Altitude = sData.Altitude;
									}
								}
								else if ( sData.Altitude >= m_MapBlocks[ index ].Cells[ cellPosition ].Altitude )
								{
									// Display the static
									m_MapBlocks[ index ].Cells[ cellPosition ].Color = (ushort) ( sData.Color + StaticOffset );

									// Update the altitude 
									m_MapBlocks[ index ].Cells[ cellPosition ].Altitude = sData.Altitude;
								}
							}
						}
					}
					index++;
				}
			}

			// Close the streams
			mapStream.Close();

			if ( mappatch )
				mapDifStream.Close();

			staidxStream.Close();
			staticsStream.Close();

			if ( stapatch )
			{
				staDifiStream.Close();
				staDifStream.Close();
			}

			// Create the image
			CreateImage();

			// Set the refreshed flag to true
			m_isRefreshed = true;
		}

		/// <summary>
		/// Invalidates the control and forces it to redraw
		/// </summary>
		private void InvalidateMap()
		{
			m_isRefreshed = false;
			Refresh();
		}

		/// <summary>
		/// Increases the zoom level by one point
		/// </summary>
		public void ZoomIn()
		{
			ZoomLevel++;
		}

		/// <summary>
		/// Decreases the zoom level by one point
		/// </summary>
		public void ZoomOut()
		{
			ZoomLevel--;
		}

		/// <summary>
		/// Adds a new draw object to the map and redraws the map
		/// </summary>
		/// <param name="drawObject">The IMapDrawable object that should be added</param>
		public void AddDrawObject( IMapDrawable drawObject )
		{
			m_DrawObjects.Add( drawObject );

			// Redraw the map
			Refresh();
		}

		/// <summary>
		/// Adds a new draw object to the map
		/// </summary>
		/// <param name="drawObject">The IMapDrawable object that is being added to the map</param>
		/// <param name="refresh">Specifies whether the map should be redrawn after the object is added</param>
		public void AddDrawObject( IMapDrawable drawObject, bool refresh )
		{
			m_DrawObjects.Add( drawObject );

			if ( refresh )
				Refresh();
		}

		/// <summary>
		/// Removes a draw object from the list of displayed objects
		/// </summary>
		/// <param name="drawObject">The draw object to be removed</param>
		public void RemoveDrawObject( IMapDrawable drawObject )
		{
			if ( m_DrawObjects.Contains( drawObject ) )
				m_DrawObjects.Remove( drawObject );
		}

		/// <summary>
		/// Removes all draw objects
		/// </summary>
		public void RemoveAllDrawObjects()
		{
			m_DrawObjects.Clear();
			Refresh();
		}

		/// <summary>
		/// Gets the height of the map at the center point of the control
		/// </summary>
		/// <returns>The height of the map at its center location</returns>
		public int GetMapHeight()
		{
			return GetMapHeight( m_ViewInfo.Center );
		}

		/// <summary>
		/// Gets the height of the map at a given point in the control and on a given map
		/// </summary>
		/// <param name="point">The point to search for height</param>
		/// <param name="mapIndex">The map on which the point is</param>
		/// <returns>The height corresponding to the point</returns>
		public int GetMapHeight( Point point, int mapIndex )
		{
			if ( mapIndex == (int) m_Map )
			{
				return GetMapHeight( point );
			}

			Size size = MapSizes.GetSize( mapIndex );

			// Make sure the point is valid for the current map
			if (
				( point.X < 0 ) || ( point.X > size.Height ) ||
				( point.Y < 0 ) || ( point.Y > size.Height ) )
			{
				return 0;
			}

			// Get file coordinates
			int xBlock = point.X / 8;
			int yBlock = point.Y / 8;
			
			int xCell = point.X % 8;
			int yCell = point.Y % 8;

			int yblocks = size.Height / 8;

			int BlockNumber = xBlock * yblocks + yBlock;
			int CellNumber = yCell * 8 + xCell;

			int height = 0;

			// Verify if the block is patched
			try
			{
				FileStream difl = new FileStream( GetMulFile( MulFileType.MapDifl, mapIndex ), FileMode.Open, FileAccess.Read );
				BinaryReader diflReader = new BinaryReader( difl );
				int index = 0;
				bool patch = false;

				// Issue 1 - Char buffer too small Exception - http://code.google.com/p/pandorasbox3/issues/detail?id=1 - Smjert
				while (diflReader.BaseStream.Position < diflReader.BaseStream.Length)
				// Issue 1 - End
				{
					int block = diflReader.ReadInt32();

					if ( block == BlockNumber )
					{
						patch = true;
						break;
					}

					index++;
				}

				difl.Close();

				if ( patch )
				{
					FileStream dif = new FileStream( GetMulFile( MulFileType.MapDif, mapIndex ), FileMode.Open, FileAccess.Read );
					dif.Seek( index * MapBlock.Size, SeekOrigin.Begin );

					MapBlock mp = MapBlock.ReadFromStream( dif );
					height = mp.Cells[ CellNumber ].Altitude;
					dif.Close();

					return height;
				}
			}
			catch {}

			// Not patched
			try
			{
				FileStream map = new FileStream( GetMulFile( MulFileType.Map, mapIndex ), FileMode.Open, FileAccess.Read );
				map.Seek( MapBlock.Size * BlockNumber, SeekOrigin.Begin );

				MapBlock mb = MapBlock.ReadFromStream( map );
				map.Close();

				height = mb.Cells[ CellNumber ].Altitude;
			}
			catch{}

			return height;
		}

		/// <summary>
		/// Calculates the height of the map at a given location
		/// </summary>
		/// <param name="point">The point of the map (in map units)</param>
		/// <returns>The height of the given point</returns>
		public int GetMapHeight( Point point )
		{
			// Make sure the point is valid for the current map
			if (
				( point.X < 0 ) || 
				( point.X > MapWidth ) ||
				( point.Y < 0 ) ||
				( point.Y > MapHeight ) )
				return 0;

			// Get file coordinates
			int xBlock = point.X / 8;
			int yBlock = point.Y / 8;

			int xCell = point.X % 8;
			int yCell = point.Y % 8;

			int BlockNumber = xBlock * YBlocks + yBlock;
			int CellNumber = yCell * 8 + xCell;

			// Get the file
			FileStream mapStream = null;

			try
			{
				mapStream = new FileStream( GetMulFile( MulFileType.Map, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );
			}
			catch
			{
				return 0;
			}

			mapStream.Seek( MapBlock.Size * BlockNumber , SeekOrigin.Begin );

			// BinaryReader mapReader = new BinaryReader( mapStream );

			// MapBlock theBlock = MapBlock.Read( mapReader );
			MapBlock theBlock = MapBlock.ReadFromStream( mapStream );

			mapStream.Close();

			int Height = theBlock.Cells[ CellNumber ].Altitude;

			// Verify if the block is patched
			if ( ! m_MapPatch.ContainsKey( BlockNumber ) )
				return Height;

			// The block is patched, read patch data
			// Issue 10 - Update the code to Net Framework 3.5 - http://code.google.com/p/pandorasbox3/issues/detail?id=10 - Smjert
			int DifIndex;

			if (!m_MapPatch.TryGetValue(BlockNumber, out DifIndex))
				return 0;
			// Issue 10 - End

			FileStream mapDifStream = null;

			try
			{
				mapDifStream = new FileStream( GetMulFile( MulFileType.MapDif, m_Map ), FileMode.Open, FileAccess.Read, FileShare.Read );
			}
			catch
			{
				// Return the original block height, more accurate than 0
				return Height;
			}

			mapDifStream.Seek( DifIndex * MapBlock.Size, SeekOrigin.Begin );

			theBlock = MapBlock.ReadFromStream( mapDifStream );

			mapDifStream.Close();

			return theBlock.Cells[ CellNumber ].Altitude;
		}

		/// <summary>
		/// Finds the draw object located at a given spot
		/// </summary>
		/// <param name="location">A point on the control surface</param>
		/// <param name="range">The number of pixels to consider as range for the search</param>
		/// <returns>A IMapDrawable object, or null</returns>
		public IMapDrawable FindDrawObject( Point location, int range )
		{
			Point c = ControlToMap( location );

			Rectangle rect = new Rectangle( c.X - ( range / 2 ), c.Y - ( range / 2 ), range, range );

			foreach ( IMapDrawable obj in m_DrawObjects )
			{
				if ( obj.IsVisible( rect, m_Map ) )
					return obj;
			}
			
			return null;
		}

		#endregion
	}
}