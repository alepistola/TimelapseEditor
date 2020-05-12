﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    /*
     * Change is the main class specialized by others derived class
     * used to store information about general changes
     * It provides the more general interface used by Timelapse
     * to mantain a list of all changes needed to stabylize exposure,
     * WB and all the kind of changes that the user is able to make
     */
    abstract class Change
    {
        protected List<IAdapterProxy> _modifiedImages;
        protected int _startImageNum, _lastImageNum;
        protected int _totalImagesNum;

        public Change(List<IAdapterProxy> imgs, int first, int last)
        {
            _modifiedImages = imgs;
            _startImageNum = first;
            _lastImageNum = last;
            _totalImagesNum = (first - last) + 1;
        }

        public int GetStartImageNum() => _startImageNum;
        public int GetLastImageNum() => _lastImageNum;
        public int GetTotalImageNum() => _totalImagesNum;

        public abstract void SaveChange();


    }
}
