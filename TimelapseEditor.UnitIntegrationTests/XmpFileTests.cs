using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace TimelapseEditor.UnitIntegrationTests
{
    [TestClass]
    public class XmpFileTests
    {
        private XmpFile file;

        private void Setup()
        {
            // Arrange
            string firstImagePath = "C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus - 5 - D\\DSC_0004.NEF";
            if(File.Exists("C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus -5 - D\\DSC_0004.xmp"))
            {
                File.Delete("C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus -5 - D\\DSC_0004.xmp");
            }
            file = new XmpFile(firstImagePath);
        }

        [TestMethod]
        public void ReadTagTest_ReadExposure_ReturnsString()
        {
            Setup();

            // Act
            string exposure = file.ReadTag("crs:RawFileName");

            // Assert
            Assert.IsInstanceOfType(exposure, typeof(string));
        }

        [TestMethod]
        public void SaveTagTest_SaveConstrat_ReturnsString()
        {
            Setup();

            file.SaveTag("crs:Contrast2012", "+1.00");
            string contrast = file.ReadTag("crs:Contrast2012");

            Assert.IsTrue(contrast == "\"+1.00\"");
        }

        [TestMethod]
        public void GetExifTest_GetAllInfos_Returns3()
        {
            Setup();

            Dictionary<string, double> dic = file.ReadExifFromPhoto();

            Assert.IsTrue(dic.Count == 3);
        }
    }
}
