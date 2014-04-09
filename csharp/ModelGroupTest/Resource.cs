using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroupTest
{
    class ResourceType
    {
        public string name { get; set; }
        public string alias { get; set; }
        public string file { get; set; }
        public List<string> components { get; set; }
    }

    class Group
    {
        public string name { get; set; }
        public string alias { get; set; }
        public List<ResourceType> type { get; set; }
    }
    
    class Resource
    {
        public List<string> components { get; set; }
        public List<Group> resources { get; set; }
    }

    class ResourceValider
    {
        private HashSet<string> enumSets;
        private HashSet<string> componentSets;

        private void validFile(string group, string type, string file, IList<string> messages)
        {
            String current = file;
            int offset = 0;

            while (true)
            {
                int start = current.IndexOf("(", offset);
                int end = current.IndexOf(")", offset);
                if (start >= 0 && end >= 0)
                {
                    String param = current.Substring(start + 1, end - start - 1);
                    if(!enumSets.Contains(param))
                    {
                        messages.Add("资源组 " + group + " 类型 " + type + " 文件参数 " + param + " 不在定义内");
                    }
                    offset = end + 1;
                }
                else
                {
                    break;
                }
            }
        }

        private void validComponents(string group,string type,IList<string> components, IList<string> messages)
        {
            foreach(string component in components)
            {
                if(!componentSets.Contains(component))
                {
                    messages.Add("资源组 " + group + " 类型 " + type + " 成分 " + component + " 不在定义内");
                }
            }
        }
        private void validGroup(Group group, IList<string> messages)
        {
            HashSet<string> nameSets = new HashSet<string>();
            HashSet<string> aliasSets = new HashSet<string>();
            foreach (ResourceType type in group.type)
            {
                if (nameSets.Contains(type.name))
                {
                    messages.Add("资源组 " + group.name + " 类型 " + type.name + "已经存在");
                }
                nameSets.Add(type.name);

                if (System.Text.RegularExpressions.Regex.IsMatch(type.alias, @"[\u4e00-\u9fa5]"))
                {
                    messages.Add("资源组 " + group.name + " 类型 " + type.name + " 别名 " + type.alias + " 不能包含汉字");
                }
                if (aliasSets.Contains(type.alias))
                {
                    messages.Add("资源组 " + group.name + " 类型 " + type.name + " 别名 " + type.alias + " 已经存在");
                }
                aliasSets.Add(type.alias);

                validFile(group.name,type.name,type.file, messages);

                validComponents(group.name, type.name, type.components, messages);
            }
        }
        
        public bool Valid(Enums enums, Resource resource ,out IList<string> messages)
        {
            messages = new List<string>();

            enumSets = new HashSet<string>();
            foreach(Enum _enum in enums.enums)
            {
                enumSets.Add(_enum.name);
            }

            componentSets = new HashSet<string>();
            foreach(string component in resource.components)
            {
                if (componentSets.Contains(component))
                {
                    messages.Add("components " + component + " 已经存在");
                }
                componentSets.Add(component);
            }

            HashSet<string> nameSets = new HashSet<string>();
            HashSet<string> aliasSets = new HashSet<string>();
            foreach(Group group in resource.resources)
            {
                if (nameSets.Contains(group.name))
                {
                    messages.Add("资源组 " + group.name + " 已经存在");
                }
                nameSets.Add(group.name);

                if (System.Text.RegularExpressions.Regex.IsMatch(group.alias, @"[\u4e00-\u9fa5]"))
                {
                    messages.Add("资源组别名 " + group.alias + " 不能包含汉字");
                }
                if (aliasSets.Contains(group.alias))
                {
                    messages.Add("资源组别名 " + group.alias + " 已经存在");
                }
                aliasSets.Add(group.alias);

                validGroup(group, messages);
            }

            return messages.Count == 0;
        }
    }
}
