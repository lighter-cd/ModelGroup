using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace ModelGroup.Config
{
    public class ModelGroupConfig
    {
        private static ModelGroupConfig instance = null;

        private List<Channel.ModelGroupType> vecGroupTypes;
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

        private bool LoadChannel(String channel_string)
        {
			JsonData jd = JsonMapper.ToObject (channel_string);
			if (jd.IsArray)
			{
                uint i = 0;
                foreach(JsonData item in jd) 
				{
                    Channel.ModelGroupType mt = new Channel.ModelGroupType((String)item["name"], (String)item["alias"], i);

                    // 全局参数
                    JsonData global_params_root = item["global_params"];
                    JsonData global_params = global_params_root["param"];
                    if(global_params.IsArray)
                    {
                        foreach(JsonData param in global_params)
                        {
                            Enum.Enumeration p = modelFileConfig.getEnumParam((String)param["name"]);
                            if (p != null)
                            {
                                mt.AddGlobalParam(p);
                            }
                        }
                    }
                    else
                    {
                        Enum.Enumeration p = modelFileConfig.getEnumParam((String)global_params["name"]);
                        if (p != null)
                        {
                            mt.AddGlobalParam(p);
                        }
                    }

                    JsonData channels_root = item["channels"];
                    JsonData model_channels = channels_root["channel"];
                    if (model_channels.IsArray)
                    {
                        foreach (JsonData channel in model_channels)
                        {
                            Channel.Flags flags = 0;
                            if (channel.Keys.Contains("sound") && (int)channel["sound"]==1)
                            {
                                flags |= Channel.Flags.Sound;
                            }
                            if (channel.Keys.Contains("bbox") && (int)channel["bbox"] == 1)
                            {
                                flags |= Channel.Flags.BBox;
                            }
                            if (channel.Keys.Contains("pick") && (int)channel["pick"] == 1)
                            {
                                flags |= Channel.Flags.Pick;
                            }
                            if (channel.Keys.Contains("castshadow") && (int)channel["castshadow"] == 1)
                            {
                                flags |= Channel.Flags.CastShadow;
                            }
                            if (channel.Keys.Contains("shadowdecal") && (int)channel["shadowdecal"] == 1)
                            {
                                flags |= Channel.Flags.ShadowDecal;
                            }
                            if (!channel.Keys.Contains("asyncLoad") || (int)channel["asyncLoad"] != 1)
                            {
                                flags |= Channel.Flags.Must;
                            }

                            int dims = channel.Keys.Contains("dims") ? (int)channel["dims"] : 1;
                            String attach_to = channel.Keys.Contains("attach_to") ? channel["attach_to"].ToString():"";
                            // attach_to 不能确定类型，统一转换成字符串
                            Channel.ModelChannel mc = new Channel.ModelChannel(
                                    (String)channel["name"], (String)channel["alias"], (String)channel["target"],
                                    (PartType)(int)channel["type"], attach_to, dims, flags);
                            
                            JsonData sources = channel["source"];
                            if(sources.IsArray)
                            {
                                foreach(JsonData source in sources)
                                {
                                    String filter = source.Keys.Contains("filter") ? (String)source["filter"] : "";
                                    int value = source.Keys.Contains("value") ? (int)source["value"] : 0;
                                    
                                    Channel.ChannelSource src = new Channel.ChannelSource(
                                        (String)source["group"], (String)source["enum"], filter, value);
                                    src.BuildParamIndex(mt.GlobalParamsVector);
                                    mc.addSource(src);
                                }
                            }
                            mt.AddModelChannel(mc);
                        }
                    }
                    mt.BuildModelChannelIndices();

                    if (item.Keys.Contains("colors"))
                    {
                        JsonData colors_root = item["colors"];
                        JsonData color_channels = colors_root["color"];                       
                        if(color_channels.IsArray)
                        {
                            foreach(JsonData channel in color_channels)
                            {
                                Channel.ColorChannel cc = new Channel.ColorChannel((String)channel["name"], (String)channel["alias"], 0xffffffff);
                                
                                JsonData targetArray = channel["target"];
                                if(targetArray.IsArray)
                                {
                                    foreach(JsonData target in targetArray)
                                    {
                                        int channel_index = mt.GetChannelIndex((String)target["channel"]);
                                        cc.AddModelElement(channel_index, (String)target["element"]);
                                    }
                                }
                                else
                                {
                                    int channel_index = mt.GetChannelIndex((String)targetArray["channel"]);
                                    cc.AddModelElement(channel_index, (String)targetArray["element"]);
                                }
                                
                                mt.AddColorChannel(cc);
                            }
                        }
                    }

                    if (item.Keys.Contains("textures"))
                    {
                        JsonData textures_root = item["textures"];
                        JsonData texture_channels = textures_root["channel"];  
                        if(texture_channels.IsArray)
                        {
                            foreach(JsonData channel in texture_channels)
                            {
                                Channel.ModelChannel mc = new Channel.ModelChannel(
                                        (String)channel["name"], (String)channel["alias"], (String)channel["target"], 
                                        (PartType)(int)channel["type"], (String)channel["attach_to"],1,0);
                                JsonData sources = channel["source"];
                                if(sources.IsArray)
                                {
                                    foreach(JsonData source in sources)
                                    {
                                        Channel.ChannelSource src = new Channel.ChannelSource(
                                            (String)source["group"], (String)source["enum"], (String)source["filter"], (int)source["value"]);
                                        src.BuildParamIndex(mt.GlobalParamsVector);
                                        mc.addSource(src);
                                    }
                                }
                                mt.AddModelChannel(mc);
                            }
                        }
                    }


                    vecGroupTypes.Add(mt);
                    i++;
                }
            }
            return true;
        }

        public bool LoadConfig(String _enum, String res, String channel, String modelPath)
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
