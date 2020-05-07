using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    class CameraRawXmpAdapter : IPhotoChanges
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

        public double GetExposureFromFile()
        {
            double value;
            string read = _xmpFile.ReadTag(_translationRules["Exposure"]);
            string sub = read.Substring(1).Split("\"")[0];
            if (sub.StartsWith("+"))
                Double.TryParse(sub.Substring(1), out value);
            else
            {
                Double.TryParse(sub.Substring(1), out value);
                value *= (-1);
            }

            return value;

        }

        public void SetExposureToFile(double value)
        {
            string toWrite = (value >= 0) ? "+" + value.ToString() : "-" + value.ToString();
            toWrite = $"\"{toWrite}\"";
            _xmpFile.SaveTag(_translationRules["Exposure"], toWrite);
        }

        public string GetImagePath() => _photoFilename;

        public string GetFilePath() => _xmpFile.GetPath();
    }
}
