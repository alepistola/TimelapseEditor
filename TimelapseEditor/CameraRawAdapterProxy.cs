using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    class CameraRawAdapterProxy : IAdapterProxy
    {
        private IPhotoChanges _adapter;
        private double _exposure;
        private Dictionary<string, double> _exif;
        private readonly string _photoFilename;

        public CameraRawAdapterProxy(string filename)
        {
            _photoFilename = filename;
            _exposure = Double.NaN;
            _exif = null;
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
                _exposure = GetAdapter().GetExposureFromFile().GetValueOrDefault();
            return _exposure;
        }

        public void SetExposure(double value)
        {
            _exposure = value;
        }

        public void SaveExposure()
        {
            // if adapter exists and exposure is set
            if (!(_adapter == null) && !(Double.IsNaN(_exposure)))
                _adapter.SetExposureToFile(_exposure);
            // if exposure is set but adapter is not yet instantiated
            else if (_adapter == null && !(Double.IsNaN(_exposure)))
                GetAdapter().SetExposureToFile(_exposure);
            // if exposure is not set
            else
                throw new InvalidOperationException("Exposure value not set hence the adapter is not yet instantiated");
        }

        public string GetImagePath() => _photoFilename;

        public Dictionary<string, double> GetExif()
        {
            if (_exif == null)
                _exif = GetAdapter().GetExifFromPhoto();
            return _exif;
        }
    }
}
