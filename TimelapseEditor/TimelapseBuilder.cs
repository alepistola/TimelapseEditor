using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    public class TimelapseBuilder : ITimelapseBuilder
    {
        private Timelapse _timelapse;

        public TimelapseBuilder(string firstImagePath)
        {
            _timelapse = Timelapse.Instance(firstImagePath);
            this.Reset();
        }

        private void Reset()
        {
            _timelapse.RemoveChanges();
        }

        public void AnalyzeExposureTime() { _timelapse.AnalyzeExposure(); }
        public void AddPreset(Preset preset) { _timelapse.ApplyPreset(preset); }
        public void AddVignetting(int intensity) { _timelapse.AddVignetting(intensity); }
        public Timelapse GetTimelapse() => _timelapse;
    }
}
