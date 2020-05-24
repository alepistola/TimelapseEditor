using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    /*
     * the class director exposes the configuration of the product (timelapse)
     * that is gonna be modified through the builder object
     */
    public class Director
    {
        private ITimelapseBuilder _builder;
        public ITimelapseBuilder SetBuilder(ITimelapseBuilder builder)
        {
            _builder = builder;
            return _builder;
        }

        public void BuildStabilizedTimelapse() { _builder.AnalyzeExposureTime(); }
        public void BuildStabilizedWithPresetTimelapse(string presetFileName) 
        {
            BuildStabilizedTimelapse();
            _builder.AddPreset(presetFileName);
        }
        public void BuildStabilizedWithVignetteTimelapse(int intensity)
        {
            BuildStabilizedTimelapse();
            _builder.AddVignetting(intensity);
        }
    }
}
