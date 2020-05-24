using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    /*
     * declares all the methods a Proxy should have. In order to make things work also the Adapter object
     * that is being proxied should implement this interface
     */
    public interface IAdapterProxy
    {
        public void SetExposure(double value);
        public double GetExposure();
        public void SaveExposure();
        public void ApplyPreset(PresetChange preset);
        public string GetImagePath();
        public string GetImageFileName();
        public Dictionary<string, double> GetExif();
        public void ApplyVignette(int intensity);
    }
}
