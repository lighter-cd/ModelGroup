using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroup.Config.JObject
{
    public class ResourceType
    {
        public string name { get; set; }
        public string alias { get; set; }
        public string file { get; set; }
        public List<string> components { get; set; }
    }

    public class Group
    {
        public string name { get; set; }
        public string alias { get; set; }
        public List<ResourceType> type { get; set; }
    }

    public class Resource
    {
        public List<string> components { get; set; }
        public List<Group> resources { get; set; }
    }
}
