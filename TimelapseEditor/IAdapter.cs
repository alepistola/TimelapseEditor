using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    /* this interface is used to wrap the xmpfile and expose a public and common interface used to access specified keys on the physical file
     * It lets the proxy work no matter the physical structure of the file containing the metadata because the proxy is going to invoke only IAdapter methods
     */
    public interface IAdapter
    {
        public void SetExposureToFile(double value);
        public double? GetExposureFromFile();
        public void ApplyPresetToFile(PresetChange preset);
        public string GetImagePath();
        public string GetFilePath();
        public string GetImageFileName();
        public Dictionary<string, double> GetExifFromPhoto();
        public void ApplyVignetteToFile(int intensity);
    }
}
