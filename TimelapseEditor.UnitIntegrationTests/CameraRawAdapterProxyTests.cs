using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor.UnitIntegrationTests
{
    [TestClass]
    public class CameraRawAdapterProxyTests
    {
        private CameraRawAdapterProxy proxy;

        private void Setup()
        {
            // Arrange
            string firstImagePath = "C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus - 5 - D\\DSC_0004.NEF";
            if (File.Exists("C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus -5 - D\\DSC_0004.xmp"))
            {
                File.Delete("C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus -5 - D\\DSC_0004.xmp");
            }
            proxy = new CameraRawAdapterProxy(firstImagePath);
        }

        [TestMethod]
        public void SetNGetExposure_ReturnsExposure()
        {
            Setup();

            proxy.SetExposure(2.56);
            double exp = proxy.GetExposure();

            Assert.IsTrue(exp == 2.56);
        }

        [TestMethod]
        public void SaveExposure_ReturnsVoid()
        {
            Setup();

            proxy.SetExposure(2.01);
            proxy.SaveExposure();

            CameraRawXmpAdapter adap = new CameraRawXmpAdapter("C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus - 5 - D\\DSC_0004.NEF");
            double exp = adap.GetExposureFromFile().Value;
            Assert.IsTrue(exp == 2.01);
        }
    }
}
