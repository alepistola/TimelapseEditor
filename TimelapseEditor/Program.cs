using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TimelapseEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            //double exposure;
            //Dictionary<string, double> exif;
            string firstImagePath = "C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus - 5 - D\\DSC_0004.NEF";
            List<CameraRawAdapterProxy> images;

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

            /* AdapterProxy test
            CameraRawAdapterProxy adapterProxy = new CameraRawAdapterProxy(imagePath);
            exposure = adapterProxy.GetExposure();
            adapterProxy.SetExposure(1.20);
            adapterProxy.SaveExposure();
            exif = adapterProxy.GetExif();
            Console.WriteLine($"[!] Exif data for {adapterProxy.GetImagePath()}");
            foreach(System.Collections.Generic.KeyValuePair<string, double> k in exif)
                Console.WriteLine($"  {k.Key} : {k.Value}");
            Console.WriteLine($"[!] Filename of image: {adapterProxy.GetImageFileName()}");
            */

            Console.WriteLine("Please enter the full filepath of the first image");
            firstImagePath = Console.ReadLine();
            images = GetAllImages(firstImagePath);


        }

        private static string GetNextImagePath(string path)
        {
            int position = path.Split('\\').Length - 1;
            string imageFileName = path.Split('\\')[position];
            string pathWithoutImageFileName = path.Substring(0, (path.Length - imageFileName.Length));
            string imageName = imageFileName.Split('.')[0];
            string imageNumber = "";
            string nameBeforeSequenceNumber = "";
            int nextImageNumber;
            string nextImagePath;
            string zeros = "";

            for(int i = 0; i < imageName.Length; i++)
            {
                char actualChar = imageName.ElementAt(i);
                if (Char.IsDigit(actualChar) && actualChar == '0' && imageNumber == "")
                    zeros += actualChar;
                else if (Char.IsDigit(actualChar))
                    imageNumber += actualChar;
                else
                    nameBeforeSequenceNumber += actualChar;
            }
            nextImageNumber = int.Parse(imageNumber) + 1;
            nextImagePath = pathWithoutImageFileName + nameBeforeSequenceNumber + zeros + nextImageNumber.ToString() + '.' + imageFileName.Split('.')[1];
            return nextImagePath;
        }

        // Consider creating LoadSequenceFromFirstPhoto as static method of Timelapse class
        private static List<CameraRawAdapterProxy> GetAllImages(string firstImagePath)
        {
            int found = 0;
            List<CameraRawAdapterProxy> imgs = new List<CameraRawAdapterProxy>();
            while (File.Exists(firstImagePath))
            {
                CameraRawAdapterProxy adapterProxy = new CameraRawAdapterProxy(firstImagePath);
                Console.WriteLine($"[+] Found: {adapterProxy.GetImagePath()}");
                imgs.Add(adapterProxy);
                firstImagePath = GetNextImagePath(firstImagePath);
                found++;
            }
            Console.WriteLine($"[!] Total {found} image to process");
            return imgs;
        }
    }
}
