using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    /* Derived by Change, this class represents changes regardless
     * the exposure time value. It concretizes the abstract method
     * declared in the base class */
    class VignetteChange : Change
    {
        private int _intensity;

        /* note: first and last goes from 0 to num_real_images -1 */
        public VignetteChange(List<IAdapterProxy> imgs, int first, int last) : base(imgs, first, last)
        { }

        public int GetIntensity() => _intensity;

        public void SetIntensity(int intensity)
        {
            if (intensity > 0 && intensity <= 5)
                _intensity = intensity;
        }

        public override void SaveChange()
        {
            for (int i = _startImageNum; i <= _lastImageNum; i++)
            {
                IAdapterProxy current = _modifiedImages[i];

                current.ApplyVignette(_intensity);
            }
        }
    }
}
