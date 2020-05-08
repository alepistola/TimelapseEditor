using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimelapseEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            double exposure;
            Dictionary<string, double> exif;
            string imagePath = "C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus - 5 - D\\DSC_0004.NEF";

            /* XmpFile test
            XmpFile file = new XmpFile(imagePath);
            exposure = file.ReadTag("crs:Exposure2012").ToString();
            Console.WriteLine($"[!] Retrieved Exposure2012 from {file.GetPath()}, value:{exposure}");
            file.SaveTag("crs:Contrast2012", "+1.00");
            contrast = file.ReadTag("crs:Contrast2012").ToString();
            Console.WriteLine($"[!] Retrieved Contrast2012 from {file.GetPath()}, value:{contrast}");
            */

            /* Camera raw adapter test 
            CameraRawXmpAdapter adapter = new CameraRawXmpAdapter(imagePath);
            exposure = adapter.GetExposureFromFile();
            Console.WriteLine($"[!] Retrieved Exposure2012 from {adapter.GetFilePath()}, value:{exposure}");
            */

            /* AdapterProxy test */
            CameraRawAdapterProxy adapterProxy = new CameraRawAdapterProxy(imagePath);
            exposure = adapterProxy.GetExposure();
            adapterProxy.SetExposure(1.20);
            adapterProxy.SaveExposure();
            exif = adapterProxy.GetExif();
            Console.WriteLine($"[!] Exif data for {adapterProxy.GetImagePath()}");
            foreach(System.Collections.Generic.KeyValuePair<string, double> k in exif)
                Console.WriteLine($"  {k.Key} : {k.Value}");
        }
    }
}
