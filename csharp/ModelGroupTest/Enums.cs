using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroupTest
{
    class Item
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
    }
    class Enum
    {
        public string name { get; set; }
        public string alias { get; set; }
        public List<Item> item { get; set; }
    }
    class Enums
    {
        public List<Enum> enums { get; set; } 
    }

    class EnumsValider
    {
        private bool validEnum(Enum _enum, IList<string> messages)
        {
            HashSet<string> nameSets = new HashSet<string>();
            HashSet<string> aliasSets = new HashSet<string>();
            HashSet<int> idSets = new HashSet<int>();
            foreach(Item item in _enum.item)
            {
                if (nameSets.Contains(item.name))
                {
                    messages.Add("枚举 " + _enum.name + " 中项目名 " + item.name + " 已经存在");
                }
                nameSets.Add(item.name);

                if (item.alias == null)
                {
                    messages.Add("枚举 " + _enum.name + " 中项目 " + item.name + " 别名为空");
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(item.alias, @"[\u4e00-\u9fa5]"))
                {
                    messages.Add("枚举 " + _enum.name + " 中项目 " + item.name + " 中项目别名 " + item.alias + " 不能包含汉字");
                }
                if (aliasSets.Contains(item.alias))
                {
                    messages.Add("枚举 " + _enum.name + " 中项目 " + item.name + " 中项目别名 " + item.alias + " 已经存在");
                }
                aliasSets.Add(item.alias);

                if (idSets.Contains(item.ID))
                {
                    messages.Add("枚举 " + _enum.name + " 中项目 " + item.name + " 中项目ID " + item.ID.ToString() + " 已经存在");
                }
                idSets.Add(item.ID);
            }
            return true;
        }

        public bool Valid(Enums enums, out IList<string> messages)
        {
            messages = new List<string>();
            HashSet<string> nameSets = new HashSet<string>();
            HashSet<string> aliasSets = new HashSet<string>();
            foreach(Enum _enum in enums.enums)
            {
                if(nameSets.Contains(_enum.name))
                {
                    messages.Add("枚举名 " + _enum.name + " 已经存在");
                }
                nameSets.Add(_enum.name);

                if(System.Text.RegularExpressions.Regex.IsMatch(_enum.alias, @"[\u4e00-\u9fa5]"))
                {
                    messages.Add("枚举别名 " + _enum.alias + " 不能包含汉字");
                }
                if (aliasSets.Contains(_enum.alias))
                {
                    messages.Add("枚举别名 " + _enum.alias + " 已经存在");
                }
                aliasSets.Add(_enum.alias);

                validEnum(_enum, messages);
            }
            return messages.Count == 0;
        }
    }
}
