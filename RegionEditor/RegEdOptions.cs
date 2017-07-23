using System;
using System.IO;
using System.Xml.Serialization;

namespace RegionEditor
{
    /// <summary>
    /// Summary description for RegEdOptions.
    /// </summary>
    public class RegEdOptions
    {
        public string ClientPath = "";
        public bool AlwaysOnTop = false;
        public bool DrawStatics = true;
        public bool XRayVision = false;

        public RegEdOptions()
        {
        }

        public static void Save(RegEdOptions Options)
        {
            // Look for filename
            System.Reflection.Assembly theExe = Options.GetType().Assembly;

            string file = theExe.Location;

            string FileName = Path.Combine(Path.GetDirectoryName(file), "RegEdOptions.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(RegEdOptions));
            FileStream theStream = new FileStream(FileName, FileMode.Create);
            serializer.Serialize(theStream, (RegEdOptions)Options);
            theStream.Close();
        }

        public static RegEdOptions Load()
        {
            RegEdOptions Options = new RegEdOptions();

            // Look for filename
            System.Reflection.Assembly theExe = Options.GetType().Assembly;

            string file = theExe.Location;

            string FileName = Path.Combine(Path.GetDirectoryName(file), "RegEdOptions.xml");

            if (!System.IO.File.Exists(FileName))
                return Options;

            // File exists
            XmlSerializer serializer = new XmlSerializer(typeof(RegEdOptions));
            System.IO.FileStream theStream = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
            Options = (RegEdOptions)serializer.Deserialize(theStream);
            theStream.Close();

            return Options;
        }
    }
}