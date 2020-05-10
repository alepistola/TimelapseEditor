using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TimelapseEditor
{
    class Timelapse
    {
        private static Timelapse _instance = null;
        private List<CameraRawAdapterProxy> _images;

        private Timelapse(string photoPath)
        {
            _images = LoadImagesFromFirstPhoto(photoPath);
        }

        public static Timelapse Instance(string photoPath)
        {
            if (_instance == null)
                _instance = new Timelapse(photoPath);
            return _instance;
        }

         
        private List<CameraRawAdapterProxy> LoadImagesFromFirstPhoto(string photoPath)
        {
            int found = 0;
            List<CameraRawAdapterProxy> imgs = new List<CameraRawAdapterProxy>();
            while (File.Exists(photoPath))
            {
                CameraRawAdapterProxy adapterProxy = new CameraRawAdapterProxy(photoPath);
                Console.WriteLine($"[+] Found: {adapterProxy.GetImagePath()}");
                imgs.Add(adapterProxy);
                photoPath = GetNextImagePath(photoPath);
                found++;
            }
            Console.WriteLine($"[!] Total {found} image to process");
            return imgs;
        }

        private string GetNextImagePath(string prevImagePath)
        {
            int position = prevImagePath.Split('\\').Length - 1;
            string imageFileName = prevImagePath.Split('\\')[position];
            string pathWithoutImageFileName = prevImagePath.Substring(0, (prevImagePath.Length - imageFileName.Length));
            string imageName = imageFileName.Split('.')[0];
            string imageNumber = "";
            string nameBeforeSequenceNumber = "";
            int nextImageNumber;
            string nextImagePath;
            string zeros = "";

            for (int i = 0; i < imageName.Length; i++)
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
    }
}
