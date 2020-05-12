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
        {

        }

        public double GetExposureChange() => _exposureChange;

        public void SetExposureChange(double exp)
        {
            _exposureChange = exp;
            _increment = _exposureChange / _totalImagesNum;
        }

        public override void SaveChange()
        {
            double initialExposureTime = _modifiedImages[_startImageNum].GetExposure();

            /*if(_startImageNum - _lastImageNum == 0)
            {
                IAdapterProxy current = _modifiedImages[_startImageNum];

                double newExposureTime = initialExposureTime + (_increment * 1);

                current.SetExposure(newExposureTime);
                current.SaveExposure();
            }
            else
            {*/
                for (int i = _startImageNum; i <= _lastImageNum; i++)
                {
                    IAdapterProxy current = _modifiedImages[i];

                    double newExposureTime = initialExposureTime + (_increment * (i - _startImageNum));

                    current.SetExposure(newExposureTime);
                    current.SaveExposure();
                }
            //}
        }
    }
}
