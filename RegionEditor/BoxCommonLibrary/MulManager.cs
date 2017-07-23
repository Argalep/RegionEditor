using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Specialized;
// Issues 43 - Problems when the client path isn't found - http://code.google.com/p/pandorasbox3/issues/detail?id=43 - Smjert
using System.Windows.Forms;
using TheBox.CustomMessageBox;
using Microsoft.Win32;
// Issues 43 - End

namespace TheBox.Common
{
    /// <summary>
    /// Provides access to mul file locations
    /// </summary>
    public class MulManager
    {
        private static string[] m_Files = new string[]
            {
            "hues.mul",
            "radarcol.mul",
            "map0.mul", "mapdif0.mul", "mapdifl0.mul",
            "mapdif1.mul", "mapdifl1.mul",
            "map2.mul", "mapdif2.mul", "mapdifl2.mul",
            "map3.mul", "mapdif3.mul", "mapdifl3.mul",
            "staidx0.mul", "statics0.mul", "stadif0.mul", "stadifl0.mul", "stadifi0.mul",
            "stadif1.mul", "stadifl1.mul", "stadifi1.mul",
            "staidx2.mul", "statics2.mul", "stadif2.mul", "stadifl2.mul", "stadifi2.mul",
            "staidx3.mul", "statics3.mul", "stadif3.mul", "stadifl3.mul", "stadifi3.mul",
            "artidx.mul", "art.mul",
            "anim.idx", "anim.mul",
            "anim2.idx", "anim2.mul",
            "anim3.idx", "anim3.mul",
            "body.def", "bodyconv.def",
            "gumpidx.mul", "gumpart.mul",
            "verdata.mul",
            "map4.mul", "mapdif4.mul", "mapdifl4.mul",
            "staidx4.mul", "statics4.mul", "stadif4.mul", "stadifl4.mul", "stadifi4.mul",
            "anim4.idx", "anim4.mul"
        };

        // Issues 43 - Problems when the client path isn't found - http://code.google.com/p/pandorasbox3/issues/detail?id=43 - Smjert
        public static bool FixClientPath()
        {
            DialogResult reply = ErrMsgBox.Show("The client path has not been found, maybe your registry key is corrupted or is missing, choose a way to fix it:"
                + "\n\n Yes: Pandora will try to search where you installed the client automatically" +
                "\n No: You manually specify the client path", "Client path not found", MessageBoxButtons.YesNo);

            if (reply == DialogResult.Yes)
            {
                string directory = @"C:\\Program Files (x86)\\Electronic Arts\\Ultima Online Classic\\";

                if (Directory.Exists(directory))
                {
                    if (File.Exists(directory + @"\client.exe"))
                        WriteRegistryKey(directory);
                }
                else
                {
                    directory = @"C:\\Program Files\\Electronic Arts\\Ultima Online Classic";
                    if (Directory.Exists(directory + @"\client.exe"))
                        WriteRegistryKey(directory);
                    else
                    {
                        ErrMsgBox.Show("The automatic search failed to find the folder, please specify the path manually", "Folder not found");
                        if (!SetCustomPath())
                            return false;
                    }
                }
                return true;
            }
            else if (reply == DialogResult.No && SetCustomPath())
                return true;

            return false;

        }

        private static bool SetCustomPath()
        {
            FolderBrowserDialog fdialog = new FolderBrowserDialog();
            DialogResult result = fdialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (Directory.Exists(fdialog.SelectedPath) && File.Exists(fdialog.SelectedPath + @"\client.exe"))
                {
                    WriteRegistryKey(fdialog.SelectedPath);
                    return true;
                }
                else
                {
                    result = ErrMsgBox.Show("You selected the wrong folder, do you want to retry?", "Wrong Folder", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        return SetCustomPath();
                }
            }
            return false;
        }

        private static void WriteRegistryKey(string path)
        {
            try
            {
                using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Origin Worlds Online\Ultima Online\1.0"))
                {
                    key.SetValue("ExePath", path + @"\client.exe");
                    key.SetValue("InstCDPath", path);
                    key.SetValue("PatchExePath", path + @"\uopatch.exe");
                    key.SetValue("StartExeParg", path + @"\uo.exe");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        // Issues 43 - End

        /// <summary>
        /// Gets the files supported by Pandora's Box
        /// </summary>
        public string[] SupportedFiles
        {
            get { return m_Files; }
        }

        private string m_2DFolder;
        private string m_CustomFolder;
        private NameValueCollection m_Table;

        /// <summary>
        /// Creates a new MulManager object reading initial data from the registry
        /// </summary>
        public MulManager()
        {

            m_2DFolder = GetExePath("Ultima Online");

            // Issues 43 - Problems when the client path isn't found - http://code.google.com/p/pandorasbox3/issues/detail?id=43 - Smjert

            if (m_2DFolder == null && !FixClientPath())
            {
                ErrMsgBox.Show("Impossible to load .mul files", "Error");
                Environment.Exit(1);
            }
            // Issues 43 - End

            m_Table = new NameValueCollection();
        }

        /// <summary>
        /// Gets or sets the custom UO installation folder
        /// </summary>
        [XmlAttribute]
        public string CustomFolder
        {
            get { return m_CustomFolder; }
            set { m_CustomFolder = value; }
        }

        /// <summary>
        /// Gets the default UO folder as found in the registry
        /// </summary>
        [XmlIgnore]
        public string DefaultFolder
        {
            get
            {
                if (m_2DFolder != null)
                {
                    return m_2DFolder;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the current list of key/values
        /// </summary>
        public string[] Table
        {
            get
            {
                if (m_Table.Keys.Count == 0)
                {
                    return null;
                }

                string[] nodes = new string[m_Table.Keys.Count * 2];

                for (int i = 0; i < nodes.Length; i += 2)
                {
                    string key = m_Table.Keys[i / 2];
                    string file = m_Table[key];

                    nodes[i] = key;
                    nodes[i + 1] = file;
                }

                return nodes;
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    return;
                }

                for (int i = 0; i < value.Length; i += 2)
                {
                    string key = value[i];
                    string file = value[i + 1];

                    m_Table.Add(key, file);
                }
            }
        }

        /// <summary>
        /// Gets the path to the UO exe from the registry
        /// </summary>
        /// <param name="subName">The class name of the UO installation</param>
        /// <returns>A path to the installation folder, null if none is found</returns>
        private static string GetExePath(string subName)
        {
            try
            {
                String keyString;

                keyString = @"SOFTWARE\Origin Worlds Online\{0}\1.0";

                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(String.Format(keyString, subName)))
                {
                    if (key == null)
                        return null;

                    string v = key.GetValue("ExePath") as string;

                    if (v == null || v.Length <= 0)
                        return null;

                    if (!File.Exists(v))
                        return null;

                    v = Path.GetDirectoryName(v);

                    if (v == null)
                        return null;

                    return v;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the file corresponding to the specified file name
        /// </summary>
        [XmlIgnore]
        public string this[string multype]
        {
            get
            {
                multype = multype.ToLower();

                string file = m_Table[multype];

                if (file != null && File.Exists(file))
                {
                    return file;
                }

                if (m_CustomFolder != null)
                {
                    file = Path.Combine(m_CustomFolder, multype);

                    if (File.Exists(file))
                    {
                        return file;
                    }
                }

                if (m_2DFolder != null)
                {
                    file = Path.Combine(m_2DFolder, multype);

                    if (File.Exists(file))
                    {
                        return file;
                    }
                }

                return null;
            }
            set
            {
                multype = multype.ToLower();

                if (value == null)
                {
                    m_Table.Remove(multype);
                }
                else if (File.Exists(value))
                {
                    m_Table[multype] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a mul file, using string.Format() notation
        /// </summary>
        [XmlIgnore]
        public string this[string format, params object[] args]
        {
            get
            {
                return this[string.Format(format, args)];
            }
            set
            {
                this[string.Format(format, args)] = value;
            }
        }
    }
}