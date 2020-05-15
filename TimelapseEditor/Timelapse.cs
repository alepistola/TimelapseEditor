using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TimelapseEditor
{
    public class Timelapse
    {
        private static Timelapse _instance = null;
        private List<IAdapterProxy> _images;
        private List<ExposureChange> _exposureChanges;
        private VignetteChange _vignetteChange;
        private PresetChange _presetChange;

        private Timelapse(string photoPath)
        {
            _images = LoadImagesFromFirstPhoto(photoPath);
            _exposureChanges = new List<ExposureChange>();
        }

        public static Timelapse Instance(string photoPath)
        {
            if (_instance == null)
                _instance = new Timelapse(photoPath);
            return _instance;
        }

         
        private List<IAdapterProxy> LoadImagesFromFirstPhoto(string photoPath)
        {
            int found = 0;
            List<IAdapterProxy> imgs = new List<IAdapterProxy>();
            while (File.Exists(photoPath))
            {
                IAdapterProxy adapterProxy = new AdapterProxy(photoPath);
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
            if (int.Parse(imageNumber) == 9 || int.Parse(imageNumber) == 99 || int.Parse(imageNumber) == 999)
                zeros = zeros.Substring(1);
            nextImagePath = pathWithoutImageFileName + nameBeforeSequenceNumber + zeros + nextImageNumber.ToString() + '.' + imageFileName.Split('.')[1];
            return nextImagePath;
        }

        /* Analyzes exposure, searching for changes and creating "exposureChanges"
	     * lists, to store images data in change sequence. Then Calculate the
	     * exposure offset required to match differences in images.	
        */
        public void AnalyzeExposure()
        {
            int startExpChange;

            // foreach image in _images
            for(int i = 0; i < _images.Count -1; i++)
            {
                IAdapterProxy curr = _images[i];
                IAdapterProxy next = _images[i + 1];
                
                // Sets start image of exposure change
                if (_exposureChanges.Count == 0)
                    startExpChange = 0;
                else
                    startExpChange = _exposureChanges.Last().GetLastImageNum() + 1;

                if (IsExposureChanged(curr, next))
                {
                    ExposureChange newChange = new ExposureChange(_images, startExpChange, i);
                    double exposure = 0;
                    Dictionary<string, double> exif1 = curr.GetExif();
                    Dictionary<string, double> exif2 = next.GetExif();

                    // If the shutter speed was changed
                    if (exif1["ExposureTime"] != exif2["ExposureTime"])
                    {
                        // Use log and doubling function to calculate stops
                        if (exif1["ExposureTime"] < exif2["ExposureTime"])
                            exposure +=  Math.Log2(exif1["ExposureTime"] / exif2["ExposureTime"]);
                        else
                            exposure += (-1) * Math.Log2(exif1["ExposureTime"] / exif2["ExposureTime"]);
                    }

                    // If the aperture was changed
                    if (exif1["F-number"] != exif2["F-number"])
                    {
                        // Use log function to calculate stops
                        exposure += (2) * Math.Log2(exif1["F-number"] / exif2["F-number"]);
                    }

                    // If iso was changed
                    if (exif1["Iso"] != exif2["Iso"])
                    {
                        // Use log function to calculate stops
                        exposure += (-1) * Math.Log2(exif1["Iso"] / exif2["Iso"]);
                    }

                    // If the "xmp" exposure setting is changed..
                    if (curr.GetExposure() != next.GetExposure())
                    {
                        exposure += (next.GetExposure() - curr.GetExposure());
                    }

                    // Set calculated change to object
                    newChange.SetExposureChange(exposure);

                    // Save exposure change to exposureChanges list!
                    // Allowing multiple exposure changes to occur and be analyzed independently
                    _exposureChanges.Add(newChange);
                }
            }
            Console.WriteLine("[!] Finished analyzing the exposure time");
            _exposureChanges.ForEach(expChange => expChange.SaveChange());
            Console.WriteLine("[!] Finished saving the exposure changes to files");
        }

        private bool IsExposureChanged(IAdapterProxy img1, IAdapterProxy img2)
        {
            if((img1.GetExif()["ExposureTime"] != img2.GetExif()["ExposureTime"]) || (img1.GetExif()["Iso"] != img2.GetExif()["Iso"]) || (img1.GetExif()["F-number"] != img2.GetExif()["F-number"]))
            {
                return true;
            }
            return false;
        }

        public void RemoveChanges() 
        {
            _exposureChanges = new List<ExposureChange>();
            _vignetteChange = null;
            _presetChange = null;
        }
        public void ApplyPreset(string presetFileName) 
        {
            try
            {
                _presetChange = new PresetChange(_images, 0, _images.Count - 1, presetFileName);
                _presetChange.SaveChange();
                Console.WriteLine("[!] Correctly applied the preset to all the images");
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AddVignetting(int intensity) 
        {
            _vignetteChange = new VignetteChange(_images, 0, _images.Count - 1);
            _vignetteChange.SetIntensity(intensity);
            _vignetteChange.SaveChange();
        }

    }
}
