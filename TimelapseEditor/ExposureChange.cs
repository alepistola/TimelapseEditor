using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    /* Derived by Change, this class represents changes regardless
     * the exposure time value. It concretizes the abstract method
     * declared in the base class */
    class ExposureChange : Change
    {
        private double _increment;
        private double _exposureChange;

        /* note: first and last goes from 0 to num_real_images -1 */
        public ExposureChange(List<IAdapterProxy> imgs, int first, int last) : base(imgs, first, last)
        { }

        public double GetExposureChange() => _exposureChange;

        public void SetExposureChange(double exp)
        {
            _exposureChange = exp;
            // the increment is calculated considering the unit value (increment * total imgs number = total exp change)
            // so later we can use the relative postion of the image to increase or decrease the exposure value
            _increment = _exposureChange / _totalImagesNum;
        }

        // it calculates and applies the changes to every image
        public override void SaveChange()
        {
            // retriving the initial value
            double initialExposureTime = _modifiedImages[_startImageNum].GetExposure();

            for (int i = _startImageNum; i <= _lastImageNum; i++)
            {
                IAdapterProxy current = _modifiedImages[i];

                /* (relative position * unit increment) + initial value = new value
                 * using this methods we can obtain a smooth exposure transition */
                double newExposureTime = initialExposureTime + (_increment * (i - _startImageNum));

                // set & save
                current.SetExposure(newExposureTime);
                current.SaveExposure();
            }
        }
    }
}
