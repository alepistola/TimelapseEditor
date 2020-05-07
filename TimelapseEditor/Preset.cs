using System;
using System.Collections.Generic;
using System.Text;

namespace TimelapseEditor
{
    class Preset
    {
        public string Name { get; set; }
        private Dictionary<string, string> _rules;


        public Preset(string name)
        {
            Name = name;
            _rules = new Dictionary<string, string>();
        }

        public void SetTags(Dictionary<string, string> rules)
        {
            _rules = rules;
        }

        // if the tag already exists the method updates it
        public void AddTag(string key, string value)
        {
            if (_rules.ContainsKey(key))
                _rules[key] = value;
            else
                _rules.Add(key, value);
        }

        public void RemoveTag(string key)
        {
            if (_rules.ContainsKey(key))
                _rules.Remove(key);
        }
    }
}
