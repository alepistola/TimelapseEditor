using System;

namespace TimelapseEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            /* XmpFile test */
            string exposure, contrast;
            string imagePath = "C:\\Users\\Alessandro\\Pictures\\viaggio Erasmus - 5 - D\\DSC_0003.NEF";
            XmpFile file = new XmpFile(imagePath);
            exposure = file.ReadTag("Exposure2012").ToString();
            Console.WriteLine($"[!] Retrieved Exposure2012 from {file.GetPath()}, value:{exposure}");
            file.SaveTag("Contrast2012", "+1.00");
            contrast = file.ReadTag("Contrast2012").ToString();
            Console.WriteLine($"[!] Retrieved Contrast2012 from {file.GetPath()}, value:{contrast}");
        }
    }
}
