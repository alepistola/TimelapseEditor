using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    /* 
     * this class is the actual proxy for adapters. It provides a form of caching and lazy loading when possible.
     */
    public class AdapterProxy : IAdapterProxy
    {
        // internal adapter
        private IAdapterProxy _adapter;

        // internal values (cached)
        private double _exposure;
        private Dictionary<string, double> _exif;
        private readonly string _photoPath;
        private string _imageFileName;

        public AdapterProxy(string photoPath)
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
            // if exposure is set
            if (!Double.IsNaN(_exposure))
                GetAdapter().SetExposure(_exposure);
            else
                throw new InvalidOperationException("Exposure value not set hence it is impossible to save it");
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

        public void ApplyPreset(PresetChange preset)
        {
            if (_adapter == null)
                GetAdapter().ApplyPreset(preset);
            else
                _adapter.ApplyPreset(preset);
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
