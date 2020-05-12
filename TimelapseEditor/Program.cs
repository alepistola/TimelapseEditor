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
            string firstImagePath;

            Welcome();
            int option = int.Parse(Console.ReadLine());
            if (option == 2)
            {
                Console.WriteLine("with what intensity value the vignette shall be? (1-5) ");
                int intensity = int.Parse(Console.ReadLine());
                if (intensity > 0 && intensity <= 5)
                {
                    Console.WriteLine("Please enter the full path of the first image");
                    firstImagePath = Console.ReadLine();
                    ITimelapseBuilder timelapseBuilder = new TimelapseBuilder(firstImagePath);
                    Director director = new Director();
                    director.SetBuilder(timelapseBuilder);
                    director.BuildStabilizedWithVignetteTimelapse(intensity);
                    Console.WriteLine("[+] Operation terminated correctly!");
                }
                else
                {
                    Console.WriteLine("[x] Err: value not valid!");
                }
            }
            else if (option == 1)
            {
                Console.WriteLine("Please enter the full path of the first image");
                firstImagePath = Console.ReadLine();
                ITimelapseBuilder timelapseBuilder = new TimelapseBuilder(firstImagePath);
                Director director = new Director();
                director.SetBuilder(timelapseBuilder);
                director.BuildStabilizedTimelapse();
                Console.WriteLine("[+] Operation terminated correctly!");
            }
            else
            {
                Console.WriteLine("[x] Err: value not valid!");
            }

            /* Timelapse test */
            //Console.WriteLine("Please enter the full filepath of the first image");
            //firstImagePath = Console.ReadLine();
            //Timelapse.Instance(firstImagePath);

            //CameraRawAdapterProxy adapterProxy = new CameraRawAdapterProxy(firstImagePath);
            //Console.WriteLine(adapterProxy.GetExif()["ExposureTime"]);

            /* Analyze Exp test */
            //Console.WriteLine("Please enter the full path of the first image");
            //firstImagePath = Console.ReadLine();
            //Timelapse.Instance(firstImagePath).AnalyzeExposure();


        }

        static void Welcome()
        {
            Console.WriteLine("-------------------- Timelapse RAW editor --------------------\n");
            Console.WriteLine("This programme will let you choose the type of changes you may");
            Console.WriteLine("want to apply at your raw footage timelapse. These are the options available:");
            Console.WriteLine("1. Smoothly ramp the exposure throughout a timelapse");
            Console.WriteLine("2. Option 1 + the addition of a vignette with intensity from 1 to 5\n");
            Console.WriteLine("Please enter the number of the modification you want to apply: ");
        }
    }
}
