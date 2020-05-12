using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    interface IPhotoChanges
    {
        public void SetExposureToFile(double value);
        public double? GetExposureFromFile();
        public void ApplyPresetToFile(Preset preset);
        public string GetImagePath();
        public string GetFilePath();
        public string GetImageFileName();
        public Dictionary<string, double> GetExifFromPhoto();
        public void ApplyVignetteToFile(int intensity);
    }
}
