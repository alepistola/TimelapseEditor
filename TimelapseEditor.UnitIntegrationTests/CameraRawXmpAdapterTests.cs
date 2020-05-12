using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor.UnitIntegrationTests
{
    [TestClass]
    public class CameraRawXmpAdapterTests
    {
        private CameraRawXmpAdapter adapter;

        private void Setup()
        {
            // Arrange
            string firstImagePath = "C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus - 5 - D\\DSC_0004.NEF";
            if (File.Exists("C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus -5 - D\\DSC_0004.xmp"))
            {
                File.Delete("C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus -5 - D\\DSC_0004.xmp");
            }
            adapter = new CameraRawXmpAdapter(firstImagePath);
        }

        [TestMethod]
        public void SetNGetExposureFromToFileTest_ReturnsDouble()
        {
            Setup();

            adapter.SetExposureToFile(1.25);
            double? exp = adapter.GetExposureFromFile();
            Assert.IsTrue(exp.Value == 1.25);
        }

        [TestMethod]
        public void GetExifFromPhotoTest_ReturnsDictionary()
        {
            Setup();

            var exif = adapter.GetExifFromPhoto();
            Assert.IsTrue(exif.Count == 3);
        }

        [TestMethod]
        public void GetImageFilenameTest_ReturnsFirstImagePath()
        {
            Setup();

            string imageName = adapter.GetImageFileName();
            Assert.IsTrue(imageName == "DSC_0004.NEF");
        }
    }
}
