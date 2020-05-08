using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    interface IAdapterProxy
    {
        public void SetExposure(double value);
        public double GetExposure();
        public void ApplyPreset();
        public string GetImagePath();
        public Dictionary<string, double> GetExif();
    }
}
