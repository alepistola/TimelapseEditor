using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    /* Derived by Change, this class represents changes regardless
     * the preset values. It override the abstract method
     * declared in the base class (SaveChange)
     */
    public class PresetChange : Change
    {
        // preset file name
        public string FileName { get; set; }

        // list of key-value
        private string[] _rules;

        // hard-coded directory
        private const string _presetDirectory = "Presets\\";


        public PresetChange(List<IAdapterProxy> imgs, int first, int last, string presetFileName) : base(imgs, first, last)
        {
            FileName = presetFileName;
            try
            {
                _rules = LoadTagsFromPresetFile(presetFileName);
            }
            catch(FileNotFoundException)
            {
                throw;
            }
        }

        /* read all the lines (key-value pairs) from the txt file */
        private string[] LoadTagsFromPresetFile(string presetFileName)
        {
            Dictionary<string, string> rules = new Dictionary<string, string>();
            string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), _presetDirectory + presetFileName);
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                return lines;
            }
            else
                throw new FileNotFoundException($"[x] File \"{path}\" not found");
        }

        
        public string[] GetTags() => _rules;

        /* it applies the preset to every image in the change object */
        public override void SaveChange()
        {
            for (int i = _startImageNum; i <= _lastImageNum; i++)
            {
                IAdapterProxy current = _modifiedImages[i];

                current.ApplyPreset(this);
            }
        }
    }
}
