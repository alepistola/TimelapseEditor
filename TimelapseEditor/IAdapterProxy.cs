using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    interface IAdapterProxy
    {
        public void SetExposure(double value);
        public double GetExposure();
        public void SaveExposure();
        public void ApplyPreset(Preset preset);
        public string GetImagePath();
        public string GetImageFileName();
        public Dictionary<string, double> GetExif();
        public void ApplyVignette(int intensity);
    }
}
