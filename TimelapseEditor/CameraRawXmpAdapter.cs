using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    class CameraRawXmpAdapter : IPhotoChanges, IAdapterProxy
    {

        private XmpFile _xmpFile;
        private readonly string _photoFilename;
        private Dictionary<string, string> _translationRules = new Dictionary<string, string> { { "Exposure", "crs:Exposure2012" } };

        public CameraRawXmpAdapter(string photoFilename)
        {
            _xmpFile = new XmpFile(photoFilename);
            _photoFilename = photoFilename;
        }

        public void ApplyPresetToFile(Preset preset)
        {
            throw new NotImplementedException();
        }

        public double? GetExposureFromFile()
        {
            double value = double.NaN;
            try
            {
                string read = _xmpFile.ReadTag(_translationRules["Exposure"]);
                if(!string.IsNullOrEmpty(read))
                {
                    string sub = read.Substring(1).Split("\"")[0];
                    if (sub.StartsWith("+"))
                        Double.TryParse(sub.Substring(1).Replace('.', ','), out value);
                    else
                    {
                        Double.TryParse(sub.Substring(1).Replace('.', ','), out value);
                        value *= (-1.00);
                    }
                }
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.Message); 
            }

            return value;
        }

        public void SetExposureToFile(double value)
        {
            string stringValue = string.Format("{0:N2}", value);
            string toWrite = (value >= 0) ? "+" + stringValue.Replace(',', '.') : stringValue.Replace(',', '.');
            _xmpFile.SaveTag(_translationRules["Exposure"], toWrite);
        }

        public string GetImagePath() => _photoFilename;

        public string GetFilePath() => _xmpFile.GetPath();

        public string GetImageFileName() => _xmpFile.GetImageFileName();

        public void SetExposure(double value)
        {
            SetExposureToFile(value);
        }

        public double GetExposure()
        {
            double? exp = GetExposureFromFile();
            return Double.IsNaN(exp.Value) ? 0.00 : exp.Value;
        }

        public void ApplyPreset()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, double> GetExifFromPhoto()
        {
            return _xmpFile.ReadExifFromPhoto();
        }

        public Dictionary<string, double> GetExif()
        {
            return GetExifFromPhoto();
        }

        public void SaveExposure()
        {

        }
    }
}
