using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{

    /* it exposes all the operation that can be made to the timelapse object
     * it is responsable of the creation and modulation of the timelapse object (singleton)
     */
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
        public void AddPreset(string presetFileName) { _timelapse.ApplyPreset(presetFileName); }
        public void AddVignetting(int intensity) { _timelapse.AddVignetting(intensity); }
        public Timelapse GetTimelapse() => _timelapse;
    }
}
