using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ModelGroup.Config.JObject
{
    public class Source
    {
        public string group { get; set; }
        [JsonProperty("enum")]
        public string _enum {get;set;}
        public string filter { get; set; }
        public int value { get; set; }
    }
    public class channel
    {
        public string name { get; set; }
        public string alias { get; set; }
        public string target { get; set; }
        public int dims { get; set; }
        public int type { get; set; }
        public string attach_to { get; set; }
        public List<string> flags { get; set; }
        public List<Source> source { get; set; }

        public channel()
        {
            dims = 1;
        }
    }
    public class ColorTarget
    {
        public string channel { get; set; }
        public string element { get; set; }
    }
    public class ColorChannel
    {
        public string name { get; set; }
        public string alias { get; set; }
        public List<ColorTarget> target { get; set; }
    }
    public class ChannelGroup
    {
        public string name { get; set; }
        public string alias { get; set; }
        public List<string> global_params { get; set; }
        public List<channel> channels { get; set; }
        public List<channel> textures { get; set; }
        public List<ColorChannel> colors { get; set; }
    }

    public class Channels
    {
        public List<string> flags { get; set; }
        public List<ChannelGroup> groups { get; set; }
    }
}
