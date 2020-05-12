using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    public class Director
    {
        private ITimelapseBuilder _builder;
        public ITimelapseBuilder SetBuilder(ITimelapseBuilder builder)
        {
            _builder = builder;
            return _builder;
        }

        public void BuildStabilizedTimelapse() { _builder.AnalyzeExposureTime(); }
        public void BuildStabilizedWithPresetTimelapse(Preset preset) 
        {
            BuildStabilizedTimelapse();
            _builder.AddPreset(preset);
        }
        public void BuildStabilizedWithVignetteTimelapse(int intensity)
        {
            BuildStabilizedTimelapse();
            _builder.AddVignetting(intensity);
        }
    }
}
