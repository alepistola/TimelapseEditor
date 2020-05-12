using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using MetadataExtractor.Formats.Exif;
using System.Linq;

namespace TimelapseEditor
{
    /* 
     * This class is responsable for the reading and writing directly to and from the file
     * Any form of caching is provided at this level
     * For more informations about xmp file and the camera raw (photoshop) crs tag please refer to https://exiftool.org/TagNames/XMP.html#crs
     */

    public class XmpFile
    {
        private string _filePath, _imageFileName;
        private StreamReader _sr;
        private const string _afterKey = "xmp:CreatorTool";
        private Dictionary<string, double> _exif;

        public XmpFile(string photoPath)
        {
            // checking if the photo exists
            if (!File.Exists(photoPath))
            {
                throw new FileNotFoundException($"[-] Image {photoPath} not found");
            }

            // calculate the path for the xmp file
            _filePath = photoPath.Split('.')[0] + ".xmp";

            // retrieve the image name
            int position = photoPath.Split('\\').Length - 1;
            _imageFileName = photoPath.Split('\\')[position];

            // read exif
            ReadExif(photoPath);

            // checking if the xmp file already exists otherwise i'll create it
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                CreateXmpTemplate(photoPath);
            }
        }

        public string GetPath() => _filePath;
        public string GetImageFileName() => _imageFileName;

        public Dictionary<string, double> ReadExifFromPhoto() => _exif;

        public string ReadTag(string key)
        {
            string toRet = null;
            string row;

            if (!String.IsNullOrEmpty(key))
            {
                _sr = File.OpenText(_filePath);
                while (!_sr.EndOfStream && toRet == null)
                {
                    row = _sr.ReadLine().Trim();
                    if (row.StartsWith(key))
                        toRet = row.Split('=')[1];
                }
                _sr.Close();
            }
            else
                throw new ArgumentNullException($"[-] File: {_filePath} the specified key: {key} is not valid");

            return toRet;
        }

        public void SaveTag(string key, string value)
        {
            string row;

            if(!String.IsNullOrEmpty(value))
            {
                try
                {
                    RemoveTag(key);
                }
                catch (ArgumentNullException) { throw; }
            }
            else
            {
                throw new ArgumentNullException($"[-] File: {_filePath} the value passed for key {key} is invalid");
            }
            string[] lines = File.ReadAllLines(_filePath);
            List<string> newlines = new List<string>();
            foreach (string line in lines)
            {
                newlines.Add(line);
                row = line.Trim();
                if (row.StartsWith(_afterKey))
                {
                    newlines.Add($"   {key}=\"{value}\"");
                }
            }
            File.WriteAllLines(_filePath, newlines.ToArray());
        }

        // if the tag exists is gonna be removed
        private void RemoveTag(string key)
        {
            string row;

            if (!String.IsNullOrEmpty(key))
            {
                string[] lines = File.ReadAllLines(_filePath);
                List<string> newlines = new List<string>();
                foreach (string line in lines)
                {
                    row = line.Trim();
                    if (!row.StartsWith(key))
                    {
                        newlines.Add(line);
                    }
                }
                File.WriteAllLines(_filePath, newlines.ToArray());
            }
            else
                throw new ArgumentNullException($"[-] File: {_filePath} the specified key: {key} is not valid");
        }

        private void CreateXmpTemplate(string photoPath)
        {
            string[] lines = new string[]
            {
                "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" x:xmptk=\"Adobe XMP Core 5.6-c128 79.159124, 2016/03/18-14:01:55        \">	",
                " <rdf:RDF xmlns:rdf = \"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" >",
                "  <rdf:Description rdf:about=\"\"",
                "   xmlns:xmp=\"http://ns.adobe.com/xap/1.0/\"",
                "   xmlns:tiff=\"http://ns.adobe.com/tiff/1.0/\"",
                "   xmlns:exif=\"http://ns.adobe.com/exif/1.0/\"",
                "   xmlns:aux=\"http://ns.adobe.com/exif/1.0/aux/\"",
                "   xmlns:photoshop=\"http://ns.adobe.com/photoshop/1.0/\"",
                "   xmlns:xmpMM=\"http://ns.adobe.com/xap/1.0/mm/\"",
                "   xmlns:stEvt=\"http://ns.adobe.com/xap/1.0/sType/ResourceEvent#\"",
                "   xmlns:dc=\"http://purl.org/dc/elements/1.1/\"",
                "   xmlns:crs=\"http://ns.adobe.com/camera-raw-settings/1.0/\"",
                "   xmp:CreatorTool=\"Ver.1.01\"",
                "   crs:RawFileName=\"" + _imageFileName + "\" >",
                "  </rdf:Description >",
                " </rdf:RDF>",
                "</x:xmpmeta>"
            };

            File.WriteAllLines(_filePath, lines);
        }

        private void ReadExif(string photoPath) 
        {
            _exif = new Dictionary<string, double>() { { "ExposureTime", Double.NaN }, { "Iso", Double.NaN }, { "F-number", Double.NaN } };

            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(photoPath);

            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().Where(s => s.ContainsTag(ExifDirectoryBase.TagFNumber)).FirstOrDefault();
            _exif["Iso"] = int.Parse(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagIsoEquivalent));
            string expression = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExposureTime).Split(' ')[0];
            if (expression.Contains('/'))
            {
                double num = Double.Parse(expression.Split('/')[0]);
                double den = Double.Parse(expression.Split('/')[1]);
                _exif["ExposureTime"] = (num / den);
            }
            else
                _exif["ExposureTime"] = Double.Parse(expression);

            _exif["F-number"] = double.Parse(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFNumber).Substring(2));
        }
    }
}
