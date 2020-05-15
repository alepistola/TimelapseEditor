using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseEditor
{
    public class PresetChange : Change
    {
        public string FileName { get; set; }
        private string[] _rules;
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
