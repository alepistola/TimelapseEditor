using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    public interface ITimelapseBuilder
    {
        void AnalyzeExposureTime();
        void AddPreset(string presetFileName);
        void AddVignetting(int intensity);
        Timelapse GetTimelapse();
    }
}
