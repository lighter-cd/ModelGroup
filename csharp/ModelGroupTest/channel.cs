using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ModelGroup.Config.JObject;

namespace ModelGroupTest
{
    class ChannelValider
    {
        private Dictionary<string, Enumeration> enumsDict;
        private HashSet<string> flagSets;
        private Dictionary<string, Group> resourceGroups;

        private bool hasParam(string file, string _param)
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
                    if (param == _param)
                    {
                        return true;
                    }
                    offset = end + 1;
                }
                else
                {
                    break;
                }
            }
            return false;
        }

        private bool hasModelChannel(string target, ChannelGroup group)
        {
            foreach(channel c in group.channels)
            {
                if (c.name == target)
                {
                    return true;
                }
            }
            return false;
        }

        private void validFlags(string group, string c, IList<string> flags, IList<string> messages)
        {
            foreach (string flag in flags)
            {
                if (!flagSets.Contains(flag))
                {
                    messages.Add("模型组 " + group + " 频道 " + c + " 标记 " + flag + " 不在定义内");
                }
            }
        }

        private void validColorTarget(ChannelGroup group, string c, IList<ColorTarget> targets, IList<string> messages)
        {
            foreach (ColorTarget t in targets)
            {
                if(!hasModelChannel(t.channel,group))
                {
                    messages.Add("模型组 " + group.name + " 颜色频道 " + c + "的目标" + t.channel + "不存在");
                }
            }
        }

        private void validSource(string group, string c, IList<Source> sources, IList<string> messages)
        {
            foreach(Source src in sources)
            {
                // 必须是resource中有效的资源组
                if (!resourceGroups.Keys.Contains(src.group))
                {
                    messages.Add("模型组 " + group + " 频道 " + c + "source 的资源组" + src.group + "未定义");
                }
                // 必须是resource中有效的资源组中有效的资源
                Group g = resourceGroups[src.group];
                string file = null;
                foreach (ResourceType rt in g.type)
                {
                    if (rt.name == src._enum)
                    {
                        file = rt.file;
                        break;
                    }
                }
                if (file == null)
                {
                    messages.Add("模型组 " + group + " 频道 " + c + "source 的资源组" + src.group + "的资源类型" + src._enum + "未定义");
                }
 

                if(src.filter != null)
                {
                    // filter 必须是resource中有效的资源组中有效的资源的文件中有效的参数
                    if (!hasParam(file,src.filter))
                    {
                        messages.Add("模型组 " + group + " 频道 " + c + "source 的资源组" + src.group + "的过滤参数" + src.filter + "不是资源的有效参数");
                    }

                    // value 必须是 resource中有效的资源组中有效的资源的文件中有效的参数枚举中有效的数值
                    if(!enumsDict.Keys.Contains(src.filter))
                    {
                        messages.Add("模型组 " + group + " 频道 " + c + "source 的资源组" + src.group + "的过滤参数" + src.filter + "未定义");
                    }

                    Enumeration _enum = enumsDict[src.filter];
                    bool bExist = false;
                    foreach(Item item in _enum.item)
                    {
                        if(item.ID == src.value)
                        {
                            bExist = true;
                            break;
                        }
                    }
                    if(!bExist)
                    {
                        messages.Add("模型组 " + group + " 频道 " + c + "source 的资源组" + src.group + "的过滤参数" + src.filter + "的值"+src.value.ToString()+"不是资源的有效参数");
                    }
                }
            }
        }

        private void validGroup(ChannelGroup group, IList<string> messages)
        {
            HashSet<string> nameSets = new HashSet<string>();
            HashSet<string> aliasSets = new HashSet<string>();
            foreach (channel c in group.channels)
            {
                if (nameSets.Contains(c.name))
                {
                    messages.Add("模型组 " + group.name + " 频道 " + c.name + "已经存在");
                }
                nameSets.Add(c.name);

                if (System.Text.RegularExpressions.Regex.IsMatch(c.alias, @"[\u4e00-\u9fa5]"))
                {
                    messages.Add("模型组 " + group.name + " 频道 " + c.name + " 别名 " + c.alias + " 不能包含汉字");
                }
                if (aliasSets.Contains(c.alias))
                {
                    messages.Add("模型组 " + group.name + " 频道 " + c.name + " 别名 " + c.alias + " 已经存在");
                }
                aliasSets.Add(c.alias);

                // 目标频道必须存在。或者为空
                if(c.target != null && c.target.Length > 0){
                    if (!hasModelChannel(c.target, group))
                    {
                        messages.Add("模型组 " + group.name + " 频道 " + c.name + " 的目标频道 " + c.target + " 不存在");
                    }
                }
                // 维度不存在或者大于0
                if (c.dims <= 0)
                {
                    messages.Add("模型组 " + group.name + " 频道 " + c.name + " 的维度必须大于0");
                }
                else if(c.dims == 1)
                {
                    // 默认为是1
                }

                // 如果类型是 1,必须有目标骨骼存在
                if(c.type == 1 && (c.attach_to == null || c.attach_to.Length == 0))
                {
                    messages.Add("模型组 " + group.name + " 频道 " + c.name + " 类型type为1时，必须有目标骨骼 attach_to");
                }
                
                validSource(group.name, c.name, c.source, messages);

                if (c.flags != null)
                {
                    validFlags(group.name, c.name, c.flags, messages);
                }
            }

            if (group.textures != null)
            {
                nameSets.Clear();
                aliasSets.Clear();
                foreach (channel c in group.textures)
                {
                    if (nameSets.Contains(c.name))
                    {
                        messages.Add("模型组 " + group.name + " 纹理频道 " + c.name + "已经存在");
                    }
                    nameSets.Add(c.name);

                    if (System.Text.RegularExpressions.Regex.IsMatch(c.alias, @"[\u4e00-\u9fa5]"))
                    {
                        messages.Add("模型组 " + group.name + " 纹理频道 " + c.name + " 别名 " + c.alias + " 不能包含汉字");
                    }
                    if (aliasSets.Contains(c.alias))
                    {
                        messages.Add("模型组 " + group.name + " 纹理频道 " + c.name + " 别名 " + c.alias + " 已经存在");
                    }
                    aliasSets.Add(c.alias);

                    validSource(group.name, c.name, c.source, messages);
                }
            }

            if (group.colors != null)
            {
                nameSets.Clear();
                aliasSets.Clear();
                foreach (ColorChannel c in group.colors)
                {
                    if (nameSets.Contains(c.name))
                    {
                        messages.Add("模型组 " + group.name + " 颜色频道 " + c.name + "已经存在");
                    }
                    nameSets.Add(c.name);

                    if (System.Text.RegularExpressions.Regex.IsMatch(c.alias, @"[\u4e00-\u9fa5]"))
                    {
                        messages.Add("模型组 " + group.name + " 颜色频道 " + c.name + " 别名 " + c.alias + " 不能包含汉字");
                    }
                    if (aliasSets.Contains(c.alias))
                    {
                        messages.Add("模型组 " + group.name + " 颜色频道 " + c.name + " 别名 " + c.alias + " 已经存在");
                    }
                    aliasSets.Add(c.alias);

                    validColorTarget(group, c.name, c.target, messages);
                }
            }
        }
       
        public bool Valid(Enums enums, Resource resource, Channels channels, out IList<string> messages)
        {
            messages = new List<string>();

            enumsDict = new Dictionary<string, Enumeration>();
            foreach (Enumeration _enum in enums.enums)
            {
                enumsDict.Add(_enum.name,_enum);
            }
            resourceGroups = new Dictionary<string, Group>();
            foreach (Group group in resource.resources)
            {
                resourceGroups.Add(group.name,group);
            }

            flagSets = new HashSet<string>();

            if (channels.flags != null)
            {
                foreach (string flag in channels.flags)
                {
                    if (flagSets.Contains(flag))
                    {
                        messages.Add("flag " + flag + " 已经存在");
                    }
                    flagSets.Add(flag);
                }
            }

            if(channels.groups.Count == 0)
            {
                messages.Add("模型组数量不能为0");
            }
            
            HashSet<string> nameSets = new HashSet<string>();
            HashSet<string> aliasSets = new HashSet<string>();
            foreach (ChannelGroup group in channels.groups)
            {
                if (nameSets.Contains(group.name))
                {
                    messages.Add("模型组 " + group.name + " 已经存在");
                }
                nameSets.Add(group.name);

                if (System.Text.RegularExpressions.Regex.IsMatch(group.alias, @"[\u4e00-\u9fa5]"))
                {
                    messages.Add("模型组别名 " + group.alias + " 不能包含汉字");
                }
                if (aliasSets.Contains(group.alias))
                {
                    messages.Add("模型组别名 " + group.alias + " 已经存在");
                }
                aliasSets.Add(group.alias);

                if (group.channels.Count == 0)
                {
                    messages.Add("模型组 " + group.name + " 频道数量不能为0");
                }
                else
                {
                    validGroup(group, messages);
                }
            }

            return messages.Count == 0;
        }
    }
}
