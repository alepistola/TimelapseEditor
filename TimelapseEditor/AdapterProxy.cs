using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    class AdapterProxy : IAdapterProxy
    {
        private IPhotoChanges _adapter;
        private double _exposure;
        private string _photoFilename;

        public AdapterProxy(string filename)
        {
            _photoFilename = filename;
            _exposure = Double.NaN;
        }

        private IPhotoChanges GetAdapter()
        {
            if (_adapter == null)
                _adapter = new CameraRawXmpAdapter(_photoFilename);
            return _adapter;
        }

        public void ApplyPreset()
        {
            throw new NotImplementedException();
        }

        public double GetExposure()
        {
            if (Double.IsNaN(_exposure))
                _exposure = GetAdapter().GetExposureFromFile();
            return _exposure;
        }

        public void SetExposure(double value)
        {
            if (_adapter == null)
            {
                _adapter = new CameraRawXmpAdapter(_photoFilename);
                _adapter.SetExposureToFile(value);
            }
            else
                _adapter.SetExposureToFile(value);

        }

        public string GetImagePath() => _photoFilename;
    }
}
