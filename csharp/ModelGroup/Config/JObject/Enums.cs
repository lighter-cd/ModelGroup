using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroup.Config.JObject
{
    public class Item
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
    }
    public class Enumeration
    {
        public string name { get; set; }
        public string alias { get; set; }
        public List<Item> item { get; set; }
    }
    public class Enums
    {
        public List<Enumeration> enums { get; set; } 
    }
}
