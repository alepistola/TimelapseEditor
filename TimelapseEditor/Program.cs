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
            // used to locate the template txt file
            Dictionary<int, string> templateMap = new Dictionary<int, string>() { { 1, "FairyGreen.txt" },
                                                                                  { 2, "WalPyne.txt"},
                                                                                  { 3, "MountDramatic.txt"} };

            Welcome();
            int option = int.Parse(Console.ReadLine());

            /* exposure stabilization + preset */
            if (option == 3) 
            {
                /* printing the options */
                Console.WriteLine("which template do you want to apply? (specify the number) ");
                foreach(KeyValuePair<int, string> temp in templateMap)
                    Console.WriteLine($"{temp.Key} - {temp.Value}");
                int template = int.Parse(Console.ReadLine());

                /* if the specified template exists continue */
                if(template > 0 && templateMap.ContainsKey(template))
                {
                    /* reading the first image path */
                    Console.WriteLine("Please enter the full path of the first image");
                    firstImagePath = Console.ReadLine();

                    /* create the timelapse builder */
                    ITimelapseBuilder timelapseBuilder = new TimelapseBuilder(firstImagePath);
                    Director director = new Director();
                    director.SetBuilder(timelapseBuilder);

                    /* specify the configuration */
                    director.BuildStabilizedWithPresetTimelapse(templateMap[template]);
                    Console.WriteLine("[+] Operation terminated correctly!\n\nPress any key to exit");
                    Console.ReadLine();
                }
                else
                    Console.WriteLine("[x] Err: value not valid!");

            }
            /* exposure stabilization + vignette */
            else if (option == 2)
            {
                /* reading the intensity */
                Console.WriteLine("with what intensity value the vignette shall be? (1-5) ");
                int intensity = int.Parse(Console.ReadLine());
                if (intensity > 0 && intensity <= 5)
                {
                    /* reading the first image path */
                    Console.WriteLine("Please enter the full path of the first image");
                    firstImagePath = Console.ReadLine();

                    /* create the timelapse builder */
                    ITimelapseBuilder timelapseBuilder = new TimelapseBuilder(firstImagePath);
                    Director director = new Director();
                    director.SetBuilder(timelapseBuilder);

                    /* specify the configuration */
                    director.BuildStabilizedWithVignetteTimelapse(intensity);
                    Console.WriteLine("[+] Operation terminated correctly!\n\nPress any key to exit");
                    Console.ReadLine();
                }
                else
                    Console.WriteLine("[x] Err: value not valid!");
            }
            /* exposure stabilization only */
            else if (option == 1)
            {
                /* reading the first image path */
                Console.WriteLine("Please enter the full path of the first image");
                firstImagePath = Console.ReadLine();

                /* create the timelapse builder */
                ITimelapseBuilder timelapseBuilder = new TimelapseBuilder(firstImagePath);
                Director director = new Director();
                director.SetBuilder(timelapseBuilder);

                /* specify the configuration */
                director.BuildStabilizedTimelapse();
                Console.WriteLine("[+] Operation terminated correctly!\n\nPress any key to exit");
                Console.ReadLine();
            }
            else
                Console.WriteLine("[x] Err: value not valid!");
        }


        /* welcome message */
        static void Welcome()
        {
            Console.WriteLine("-------------------- Timelapse RAW editor --------------------\n");
            Console.WriteLine("This programme will let you choose the type of changes you may");
            Console.WriteLine("want to apply at your raw footage timelapse. These are the options available:");
            Console.WriteLine("1. Smoothly ramp the exposure throughout a timelapse");
            Console.WriteLine("2. Option 1 + the addition of a vignette with intensity from 1 to 5");
            Console.WriteLine("3. Option 1 + the addition of a preset style (from a predefined list)\n");
            Console.WriteLine("Please enter the number of the modification you want to perform: ");
        }
    }
}
