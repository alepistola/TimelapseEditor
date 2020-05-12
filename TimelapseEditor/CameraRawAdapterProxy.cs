using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    public class CameraRawAdapterProxy : IAdapterProxy
    {
        private IAdapterProxy _adapter;
        private double _exposure;
        private Dictionary<string, double> _exif;
        private readonly string _photoPath;
        private string _imageFileName;

        public CameraRawAdapterProxy(string photoPath)
        {
            _photoPath = photoPath;
            _exposure = Double.NaN;
            _exif = null;
            _imageFileName = null;
        }

        private IAdapterProxy GetAdapter()
        {
            if (_adapter == null)
                _adapter = new CameraRawXmpAdapter(_photoPath);
            return _adapter;
        }


        public double GetExposure()
        {
            if (Double.IsNaN(_exposure))
                _exposure = GetAdapter().GetExposure();
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
                _adapter.SetExposure(_exposure);
            // if exposure is set but adapter is not yet instantiated
            else if (_adapter == null && !(Double.IsNaN(_exposure)))
                GetAdapter().SetExposure(_exposure);
            // if exposure is not set
            else
                throw new InvalidOperationException("Exposure value not set hence the adapter is not yet instantiated");
        }

        public string GetImagePath() => _photoPath;

        public string GetImageFileName()
        {
            if (String.IsNullOrEmpty(_imageFileName))
                _imageFileName = GetAdapter().GetImageFileName();
            return _imageFileName;
        }

        public Dictionary<string, double> GetExif()
        {
            if (_exif == null)
                _exif = GetAdapter().GetExif();
            return _exif;
        }

        public void ApplyPreset(Preset preset)
        {
            throw new NotImplementedException();
        }

        public void ApplyVignette(int intensity)
        {
            if (_adapter == null)
                GetAdapter().ApplyVignette(intensity);
            else
                _adapter.ApplyVignette(intensity);
        }
    }
}
