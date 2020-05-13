using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    public class CameraRawXmpAdapter : IAdapter, IAdapterProxy
    {

        private XmpFile _xmpFile;
        private readonly string _photoFilename;
        private Dictionary<string, string> _translationRules = new Dictionary<string, string> { { "Exposure", "crs:Exposure2012" } };
        private Dictionary<string, string> _vignetteRules = new Dictionary<string, string> { { "crs:PostCropVignetteHighlightContrast", "0" },
                                                                                             { "crs:PostCropVignetteStyle", "1" },
                                                                                             { "crs:PostCropVignetteRoundness", "+4" },
                                                                                             { "crs:PostCropVignetteFeather", "100" },
                                                                                             { "crs:PostCropVignetteMidpoint", "38" }        };

        public CameraRawXmpAdapter(string photoFilename)
        {
            try
            {
                _xmpFile = new XmpFile(photoFilename);
                _photoFilename = photoFilename;
            }
            catch(FileNotFoundException e)
            { Console.WriteLine(e.Message); }
        }

        #region adapter interface (IAdapter)

        public void SetExposureToFile(double value)
        {
            string stringValue = string.Format("{0:N2}", value);
            string toWrite = (value >= 0) ? "+" + stringValue.Replace(',', '.') : stringValue.Replace(',', '.');
            _xmpFile.SaveTag(_translationRules["Exposure"], toWrite);
        }

        public double? GetExposureFromFile()
        {
            double value = double.NaN;
            try
            {
                string read = _xmpFile.ReadTag(_translationRules["Exposure"]);
                if (!string.IsNullOrEmpty(read))
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

        public void ApplyPresetToFile(Preset preset)
        {
            throw new NotImplementedException();
        }

        public string GetFilePath() => _xmpFile.GetPath();

        public Dictionary<string, double> GetExifFromPhoto()
        {
            return _xmpFile.ReadExifFromPhoto();
        }

        public void ApplyVignetteToFile(int intensity)
        {
            switch (intensity)
            {
                case 1:
                    {
                        _vignetteRules.Add("crs:PostCropVignetteAmount", "-10");
                        break;
                    }
                case 2:
                    {
                        _vignetteRules.Add("crs:PostCropVignetteAmount", "-20");
                        break;

                    }
                case 3:
                    {
                        _vignetteRules.Add("crs:PostCropVignetteAmount", "-25");
                        break;
                    }
                case 4:
                    {
                        _vignetteRules.Add("crs:PostCropVignetteAmount", "-30");
                        break;
                    }
                case 5:
                    {
                        _vignetteRules.Add("crs:PostCropVignetteAmount", "-40");
                        break;
                    }
            }
            foreach(KeyValuePair<string, string> k in _vignetteRules)
            {
                _xmpFile.SaveTag(k.Key, k.Value);
            }
        }

        #endregion




        #region proxy interface (IAdapterProxy)

        public void SetExposure(double value)
        {
            SetExposureToFile(value);
        }

        public double GetExposure()
        {
            double? exp = GetExposureFromFile();
            return Double.IsNaN(exp.Value) ? 0.00 : exp.Value;
        }

        // It will not be invoked, never, because of the implementation of proxy
        public void SaveExposure()
        {

        }

        public void ApplyPreset(Preset preset)
        {
            ApplyPresetToFile(preset);
        }

        public Dictionary<string, double> GetExif()
        {
            return GetExifFromPhoto();
        }

        public void ApplyVignette(int intensity)
        {
            ApplyVignetteToFile(intensity);
        }

        #endregion




        #region shared methods between IPhotoCanges and IAdapterProxy

        public string GetImagePath() => _photoFilename;

        public string GetImageFileName() => _xmpFile.GetImageFileName();

        #endregion
    }
}
