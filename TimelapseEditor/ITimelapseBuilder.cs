using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    /* interface that declares the methods used to modify the timelapse */
    public interface ITimelapseBuilder
    {
        void AnalyzeExposureTime();
        void AddPreset(string presetFileName);
        void AddVignetting(int intensity);
        Timelapse GetTimelapse();
    }
}
