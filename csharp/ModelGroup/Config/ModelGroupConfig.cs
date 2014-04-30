using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ModelGroup.Config.JObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace ModelGroup.Config
{
    public class ModelGroupConfig
    {
        private static ModelGroupConfig instance = null;

        private List<Channel.ModelGroupType> vecGroupTypes;
        private Flags flags;
        private ModelFileConfig modelFileConfig;

        public static ModelGroupConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModelGroupConfig();
                }
                return instance;
            }
        }

        public ModelGroupConfig()
        {
            vecGroupTypes = new List<Channel.ModelGroupType>();
        }

        private bool LoadChannel(byte[] channelBson)
        {
            MemoryStream ms = new MemoryStream(channelBson);
            using (BsonReader reader = new BsonReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                JObject.Channels cc = serializer.Deserialize<JObject.Channels>(reader);

                flags = new Flags(cc.flags);
                Channel.ModelChannel.flagsDef = flags;

                uint i = 0;
                foreach (JObject.ChannelGroup cg in cc.groups)
                {
                    Channel.ModelGroupType mg = new Channel.ModelGroupType(cg.name,cg.alias,i);

                    // 全局参数
                    foreach(string gp in cg.global_params)
                    {
                        mg.AddGlobalParam(modelFileConfig.getEnumParam(gp));
                    }
                    // 模型频道
                    foreach(JObject.channel mc in cg.channels)
                    {
                        Channel.ModelChannel _mc = new Channel.ModelChannel(mc.name,mc.alias,mc.target,(PartType)mc.type,mc.attach_to,mc.dims,flags.GetBits(mc.flags));
                        foreach (JObject.Source src in mc.source)
                        {
                            _mc.addSource(new Channel.ChannelSource(src.group,src._enum,src.filter,src.value));
                        }
                        mg.AddModelChannel(_mc);
                    }
                    // 纹理频道
                    if (cg.textures != null)
                    {
                        foreach (JObject.channel tex in cg.textures)
                        {
                            Channel.ModelChannel _mc = new Channel.ModelChannel(tex.name, tex.alias, tex.target, (PartType)tex.type, tex.attach_to, tex.dims, flags.GetBits(tex.flags));
                            foreach (JObject.Source src in tex.source)
                            {
                                _mc.addSource(new Channel.ChannelSource(src.group, src._enum, src.filter, src.value));
                            }
                            mg.AddTextureChannel(_mc);
                        }
                    }
                    // 颜色频道
                    if (cg.colors != null)
                    {
                        foreach (JObject.ColorChannel jcc in cg.colors)
                        {
                            Channel.ColorChannel _cc = new Channel.ColorChannel(jcc.name, jcc.alias, 0xffffffff);
                            foreach (JObject.ColorTarget ct in jcc.target)
                            {
                                int channel_index = mg.GetChannelIndex(ct.channel);
                                _cc.AddModelElement(channel_index, ct.element);
                            }
                            mg.AddColorChannel(_cc);
                        }
                    }
                    vecGroupTypes.Add(mg);
                    i++;
                }
            }
            return true;
        }

        public bool LoadConfig(byte[] _enum, byte[] res, byte[] channel, String modelPath)
        {
            ModelFileConfig.Instance.path = modelPath;
            modelFileConfig = ModelFileConfig.Instance;
            if (ModelFileConfig.Instance.loadConfig(_enum, res))
            {
                return LoadChannel(channel);
            }
            return false;
        }

        public int GroupTypes
        {
            get {return vecGroupTypes.Count;}
        }

        public Channel.ModelGroupType GetGroupType(int n)
        {
            return vecGroupTypes[n];
        }
	
	    public Enum.Enumeration GetEnumParam(String name)
	    {
		    return modelFileConfig.getEnumParam(name);
	    }
    }
}
