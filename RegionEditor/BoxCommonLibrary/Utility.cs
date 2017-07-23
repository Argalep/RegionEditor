using System;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace TheBox.Common
{
	/// <summary>
	/// Provides general purpose static functions
	/// </summary>
	public class Utility
	{
		#region Imports

		private const int WM_CHAR = 0x0102;

		const byte VK_SHIFT = 0x10;
		const byte VK_CONTROL = 0x11;
		const byte VK_ALT = 0xA4;

		const byte KEYEVENTF_EXTENDEDKEY = 0x1;
		const byte KEYEVENTF_KEYUP = 0x2;

		[DllImport( "User32" )]
		private static extern IntPtr FindWindow(string lpszClassName, string lpszWindowName);

		[DllImport("user32.dll")]
		private static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam );

		[DllImport("user32.dll")]
		private static extern int SetForegroundWindow( int hWnd );

		[DllImport("User32.dll")]
		private static extern int SetFocus( int hwnd );

		[DllImport("User32.dll")]
		private static extern void keybd_event( byte VK, byte Scan, long Flags, long ExtraInfo );

		#endregion

		private static string m_CustomClientPath;
		private static Process m_ClientProcess;

		/// <summary>
		/// Gets or sets the custom client path used when sending commands to the UO window
		/// </summary>
		public static string CustomClientPath
		{
			get { return m_CustomClientPath; }
			set
			{
				if ( value == null || File.Exists( value ) )
				{
					m_CustomClientPath = value;
				}

				if ( value == null && m_ClientProcess != null )
				{
					m_ClientProcess = null;
				}
			}
		}

		/// <summary>
		/// Sends a string message to the UO window
		/// </summary>
		/// <param name="message">A string message that will be sent to UO</param>
		// Issue 38:  	 Message when client not found - Tarion
		// Changed return value to boolean
		// End Issue 38
		public static bool SendToUO( string message )
		{
			IntPtr handle = GetClientWindow();

			if ( handle.ToInt32() == 0 )
			{
				return false;
			}

			// We have a window
			foreach ( char c in message.ToCharArray() )
				SendMessage( handle.ToInt32(), WM_CHAR, c, 0 );

			SetForegroundWindow( handle.ToInt32() );
			return true;
		}

		/// <summary>
		/// Finds the process related to the
		/// </summary>
		private static void FindCustomClient()
		{
			if ( m_ClientProcess != null )
				return; // Current client process is valid

			if ( m_CustomClientPath != null && File.Exists( m_CustomClientPath  ) )
			{
				Process[] processList = Process.GetProcesses();

				foreach( Process p in processList )
				{
					try
					{
						if ( p.MainModule.FileName.ToLower() == m_CustomClientPath.ToLower() )
						{
							m_ClientProcess = p;
							m_ClientProcess.EnableRaisingEvents = true;
							m_ClientProcess.Exited += new EventHandler( ClientClosed );
						}
					}
					catch {} // Some processes won't allow access to the MainModule
				}
			}
		}

		/// <summary>
		/// Client closed, reset the process variable
		/// </summary>
		private static void ClientClosed( object sender, EventArgs e )
		{
			m_ClientProcess = null;
		}

		/// <summary>
		/// Sends a macro to the UO window
		/// </summary>
		/// <param name="macroKey">The character representing the macro</param>
		/// <param name="ctrl">Specifies whether ctrl is part of the macro</param>
		/// <param name="shift">Specifies whether ctrl is part of the macro</param>
		/// <param name="alt">Specifies whether ctrl is part of the macro</param>
		public static void SendMacro( Keys macroKey, bool ctrl, bool shift, bool alt )
		{
			int key = (int) macroKey;

			if ( key > byte.MaxValue )
			{
				return;
			}

			byte macroChar = (byte) key;

			int hwnd = GetClientWindow().ToInt32();

			if ( hwnd != 0 )
			{
				SetForegroundWindow( hwnd );
				SetFocus( hwnd );

				if ( ctrl ) keybd_event(VK_CONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);
				if ( shift ) keybd_event(VK_SHIFT, 0, KEYEVENTF_EXTENDEDKEY, 0);
				if ( alt ) keybd_event(VK_ALT, 0, KEYEVENTF_EXTENDEDKEY, 0);

				keybd_event(macroChar, 0, 0, 0);
				keybd_event(macroChar, 0, KEYEVENTF_KEYUP, 0);

				if ( ctrl ) keybd_event(VK_CONTROL, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
				if ( shift ) keybd_event(VK_SHIFT, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
				if ( alt ) keybd_event(VK_ALT, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
			}
		}

		/// <summary>
		/// Finds a pointer to the client window handle
		/// </summary>
		/// <returns>An IntPtr window handle</returns>
		public static IntPtr GetClientWindow()
		{
			IntPtr handle = FindWindow( "Ultima Online", null );

			if ( handle.ToInt32() == 0 )
			{
				handle = FindWindow( "Ultima Online Third Dawn", null );
			}

			if ( handle.ToInt32() == 0 )
			{
				FindCustomClient();

				if ( m_ClientProcess != null )
				{
					handle = m_ClientProcess.MainWindowHandle;
				}
			}

			return handle;
		}

		/// <summary>
		/// Brings the client window to front
		/// </summary>
		public static void BringClientToFront()
		{
			IntPtr handle = GetClientWindow();

			if ( handle.ToInt32() == 0 )
				return;

			SetForegroundWindow( handle.ToInt32() );
		}

		/// <summary>
		/// Brings the specified window to the front of the Z order
		/// </summary>
		/// <param name="handle">A pointer to the window handle</param>
		public static void BringWindowToFront( IntPtr handle )
		{
			SetForegroundWindow( handle.ToInt32() );
		}

		/// <summary>
		/// Verifies if a directory exists and if it doesn't it creates it
		/// </summary>
		/// <param name="path">The folder to ensure</param>
		public static void EnsureDirectory( string path )
		{
			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );
		}

		/// <summary>
		/// Evaluates a folder for the presence of valid UO files
		/// </summary>
		/// <param name="path">The folder to examine</param>
		/// <returns>True if mul files are present in the folder</returns>
		public static bool ValidateUOFolder( string path )
		{
			string map = Path.Combine( path, "map0.mul" );

			return
				File.Exists( map );
		}

		/// <summary>
		/// Validates a number ensuring it's withing the supplied bounds
		/// </summary>
		/// <param name="value">The numder to validate</param>
		/// <param name="min">The lower bound for the value</param>
		/// <param name="max">The upper bound for the value</param>
		/// <returns>The validated number</returns>
		public static int ValidateNumber( int value, int min, int max )
		{
			if ( value < min )
				return min;

			if ( value > max )
				return max;

			return value;
		}

		/// <summary>
		/// Saves a serializable XML object to a XML file
		/// </summary>
		/// <param name="obj">The object to save</param>
		/// <param name="filename">The filename where the object should be saved</param>
		/// <returns>True if succesful, false otherwise</returns>
		public static bool SaveXml( object obj, string filename )
		{
			FileStream stream = null;

			try
			{
				stream = new FileStream( filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite );
				XmlSerializer serializer = new XmlSerializer( obj.GetType() );
				serializer.Serialize( stream, obj );
				stream.Close();

				return true;
			}
			catch
			{
				if ( stream != null )
				{
					stream.Close();
				}

				return false;
			}
		}

		/// <summary>
		/// Loads an object from a XML file
		/// </summary>
		/// <param name="type">The type of the object stored in the XML file</param>
		/// <param name="filename">The filename to load</param>
		/// <returns>The object read, null if failed</returns>
		public static object LoadXml( Type type, string filename )
		{
			FileStream stream = null;

			try
			{
				stream = new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.Read );
				XmlSerializer serializer = new XmlSerializer( type );
				object obj = serializer.Deserialize( stream );
				stream.Close();

				return obj;
			}
			catch
			{
				if ( stream != null )
				{
					stream.Close();
				}

				return null;
			}
		}

		/// <summary>
		/// Draws a border around a control
		/// </summary>
		/// <param name="c">The control that should be applied the border</param>
		/// <param name="g">The graphics on which to draw</param>
		public static void DrawBorder( Control c, Graphics g )
		{
			Pen pen = new Pen( SystemColors.ControlDark );
			g.DrawRectangle( pen, 0, 0, c.Width - 1, c.Height - 1 );
			pen.Dispose();
		}

		/// <summary>
		/// Extracts an embedded resource and saves it to file
		/// </summary>
		/// <param name="assembly">The assmebly containing the resource</param>
		/// <param name="resource">The resource name</param>
		/// <param name="filename">The target filename for the resource</param>
		public static void ExtractEmbeddedResource( Assembly assembly, string resource, string filename )
		{
			Stream stream = assembly.GetManifestResourceStream( resource );
			FileStream fStream = new FileStream( filename, FileMode.Create, FileAccess.Write, FileShare.Read );

			byte[] data = new byte[ stream.Length ];
			stream.Read( data, 0, (int) stream.Length );

			fStream.Write( data, 0, data.Length );

			stream.Close();
			fStream.Flush();
			fStream.Close();
		}

		/// <summary>
		/// Retrieves an embedded xml-serialized object from a stream
		/// </summary>
		/// <param name="type">The type of the object</param>
		/// <param name="resource">The name of the resource</param>
		/// <param name="asm">The assembly containing the resource</param>
		/// <returns>The embedded object</returns>
		public static object GetEmbeddedObject( Type type, string resource, Assembly asm )
		{
			if ( resource == null || resource.Length == 0 )
				return null;

			Stream stream = asm.GetManifestResourceStream( resource );

			if ( stream == null )
				return null;

			XmlSerializer serializer = new XmlSerializer( type );
			object obj = null;

			try { obj = serializer.Deserialize( stream ); }
			catch {}

			stream.Close();
			return obj;
		}

		/// <summary>
		/// Retrieves the name of a directory
		/// </summary>
		/// <param name="path">The directory</param>
		/// <returns>A string containing the name of a directory</returns>
		public static string GetDirectoryName( string path )
		{
			string[] parts = path.Split( Path.DirectorySeparatorChar );

			if ( parts.Length == 0 )
				return null;
			else
				return parts[ parts.Length - 1 ];
		}

		/// <summary>
		/// Copies a directory and all of its contents
		/// </summary>
		/// <param name="source">The source directory</param>
		/// <param name="dest">The destination directory</param>
		/// <remarks>This function exists because the Directory.Move() function doesn't
		/// allow directories to be moved between volumes</remarks>
		/// <returns>True if the operation has been successful</returns>
		public static bool CopyDirectory( string source, string dest )
		{
			string[] files = Directory.GetFiles( source );
			string[] folders = Directory.GetDirectories( source );

			EnsureDirectory( dest );

			foreach ( string file in files )
			{
				string newFile = Path.Combine( dest, Path.GetFileName( file ) );

				try
				{
					File.Copy( file, newFile );
				}
				catch
				{
					return false;
				}
			}

			foreach( string folder in folders )
			{
				string name = GetDirectoryName( folder );

				if ( name != null )
				{
					string destDir = Path.Combine( dest, name );
					
					if ( ! CopyDirectory( folder, destDir ) )
						return false;
				}
				else
				{
					return false;
				}
			}

			return true;
		}
	}
}