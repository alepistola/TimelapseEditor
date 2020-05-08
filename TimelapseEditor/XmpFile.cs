﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TimelapseEditor
{
    /* 
     * This class is responsable for the reading and writing directly to and from the file
     * Any form of caching is provided at this level
     * For more informations about xmp file and the camera raw (photoshop) crs tag please refer to https://exiftool.org/TagNames/XMP.html#crs
     */

    class XmpFile
    {
        private string _path;
        private StreamReader _sr;
        private const string _afterKey = "xmp:CreatorTool";

        public XmpFile(string photoPath)
        {
            // checking if the photo exists
            if (!File.Exists(photoPath))
            {
                throw new FileNotFoundException($"[-] Image {photoPath} not found");
            }
            // calculate the path for the xmp file
            _path = photoPath.Split('.')[0] + ".xmp";

            // checking if the xmp file already exists otherwise i'll create it
            if(!File.Exists(_path))
            {
                File.Create(_path).Close();
                CreateXmpTemplate(photoPath);
            }
                

        }

        public string GetPath() => _path;

        public string ReadTag(string key)
        {
            string toRet = null;
            string row;

            if (!String.IsNullOrEmpty(key))
            {
                _sr = File.OpenText(_path);
                while (!_sr.EndOfStream && toRet == null)
                {
                    row = _sr.ReadLine().Trim();
                    if (row.StartsWith(key))
                        toRet = row.Split('=')[1];
                }
                _sr.Close();
            }
            else
                throw new ArgumentNullException($"[-] File: {_path} the specified key: {key} is not valid");

            if (toRet != null) return toRet;
            throw new InvalidConstraintException($"[-] File: {_path} does not contain key: {key}");
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
                throw new ArgumentNullException($"[-] File: {_path} the value passed for key {key} is invalid");
            }
            string[] lines = File.ReadAllLines(_path);
            List<string> newlines = new List<string>();
            foreach (string line in lines)
            {
                newlines.Add(line);
                row = line.Trim();
                if (row.StartsWith(_afterKey))
                {
                    newlines.Add($"\t\t{key}=\"{value}\"");
                }
            }
            File.WriteAllLines(_path, newlines.ToArray());
        }

        // if the tag exists is gonna be removed
        private void RemoveTag(string key)
        {
            string row;

            if (!String.IsNullOrEmpty(key))
            {
                string[] lines = File.ReadAllLines(_path);
                List<string> newlines = new List<string>();
                foreach (string line in lines)
                {
                    row = line.Trim();
                    if (!row.StartsWith(key))
                    {
                        newlines.Add(line);
                    }
                }
                File.WriteAllLines(_path, newlines.ToArray());
            }
            else
                throw new ArgumentNullException($"[-] File: {_path} the specified key: {key} is not valid");
        }

        private void CreateXmpTemplate(string photoPath)
        {
            int position = photoPath.Split('\\').Length - 1;
            string rawFileName = photoPath.Split('\\')[position];

            string[] lines = new string[]
            {
                "<x:xmpmeta xmlns:x=\"adobe:ns: meta / \" x:xmptk=\"Adobe XMP Core 5.6 - c128 79.159124, 2016 / 03 / 18 - 14:01:55        \">	",
                "  < rdf:RDF xmlns:rdf = \"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" >",
                "\t< rdf:Description rdf:about = \"\"",
                "\t\txmlns:xmp = \"http://ns.adobe.com/xap/1.0/\"",
                "\t\txmlns:tiff = \"http://ns.adobe.com/tiff/1.0/\"",
                "\t\txmlns:exif = \"http://ns.adobe.com/exif/1.0/\"",
                "\t\txmlns:aux = \"http://ns.adobe.com/exif/1.0/aux/\"",
                "\t\txmlns:photoshop = \"http://ns.adobe.com/photoshop/1.0/\"",
                "\t\txmlns:xmpMM = \"http://ns.adobe.com/xap/1.0/mm/\"",
                "\t\txmlns:stEvt = \"http://ns.adobe.com/xap/1.0/sType/ResourceEvent#\"",
                "\t\txmlns:dc = \"http://purl.org/dc/elements/1.1/\"",
                "\t\txmlns:crs = \"http://ns.adobe.com/camera-raw-settings/1.0/\"",
                "\t\txmp:CreatorTool = \"Ver.1.01\"",
                "\t\tcrs:RawFileName = \"" + rawFileName + "\" >",
                "\t</rdf:Description >",
                "  </ rdf:RDF >",
                "</ x:xmpmeta >"
            };

            File.WriteAllLines(_path, lines);
        }
    }
}
