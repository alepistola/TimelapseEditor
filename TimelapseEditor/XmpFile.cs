﻿using System;
using System.Data;
using System.IO;

namespace TimelapseEditor
{
    /* 
     * This class is responsable for the reading and writing directly to and from the file
     * Any form of caching is provided at this level
     * For more informations about xmp file and the photoshop crs tag please refer to https://exiftool.org/TagNames/XMP.html#crs
     */

    class XmpFile
    {
        private string _path;
        private StreamReader _sr;
        private StreamWriter _sw;

        public XmpFile(string filename)
        {
            // checking if the photo exists
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"[-] Image {filename} not found");
            }
            // calculate the path for the xmp file
            _path = filename.Split('.')[0] + ".xmp";
            // checking if the xmp file already exists otherwise i'll create it
            if(!File.Exists(_path))
                File.Create(_path);

        }

        public string GetPath() => _path;

        public object ReadTag(string key)
        {
            object toRet = null;
            string row;

            if (!String.IsNullOrEmpty(key))
            {
                _sr = File.OpenText(_path);
                while (!_sr.EndOfStream && toRet == null)
                {
                    row = _sr.ReadLine().Trim();
                    if(row.StartsWith("crs:" + key))
                        toRet = row.Split(':')[1].Split('=')[1];
                }
            }
            else
                throw new InvalidConstraintException("[-] The specified key is not valid");

            return toRet;
        }


    }
}
