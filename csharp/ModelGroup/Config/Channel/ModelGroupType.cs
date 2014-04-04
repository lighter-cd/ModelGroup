using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroup.Config.Channel
{
    public class ModelGroupType : NamedItem
    {
        private List<ModelChannel> vecModels;	// 模型频道
        private List<ModelChannel> vecTextures;	// 纹理频道
        private List<ColorChannel> vecColors;	// 颜色频道

        private int real_channels;		        // 真正的频道数量(包括数组频道的维度在内的数量)
        private int[] channelIndices;	        // 频道索引与真正的频道数组(包括数组频道的维度在内)下标的对应  UINT

        private List<Enum.Enumeration> globalParams;	// 全局参数

        private uint index;				        // 模型组编号


        public ModelGroupType(String name, String alias, uint _index)
            :base(name,alias)
        {
            globalParams = new List<Enum.Enumeration>();
            vecModels = new List<ModelChannel>();
            vecTextures = new List<ModelChannel>();
            vecColors = new List<ColorChannel>();
            index = _index;
        }

        public int ModelChannels
        {
            get {return vecModels.Count;}
        }

        public ModelChannel GetModelChannel(int n)
        {
            return vecModels[n];
        }

        public void AddModelChannel(ModelChannel m)
        {
            vecModels.Add(m);
        }

        public void BuildModelChannelIndices()
        {
            channelIndices = new int[vecModels.Count];

            for (int i = 0; i < vecModels.Count; i++)
            {
                ModelChannel channel = vecModels[i];

                if (channel.Target != null && channel.Target.Length != 0)
                {
                    channel.TargetChannel = GetChannelIndex(channel.Target);
                }

                channelIndices[i] = real_channels;
                real_channels += channel.Dims;
            }
        }

        public int GetChannelIndex(String name)
        {
            for (int i= 0; i < vecModels.Count; i++)
            {
                if (name == vecModels[i].Name)
                {
                    return i;
                }
            }
            return -1;
        }

        ///
        public int TextureChannels
        {
            get {return vecTextures.Count;}
        }

        public ModelChannel GetTextureChannel(int n)
        {
            return vecTextures[n];
        }

        public void AddTextureChannel(ModelChannel t)
        {
            vecTextures.Add(t);
        }

        ///
        public int ColorChannels
        {
            get {return vecColors.Count;}
        }

        public ColorChannel GetColorChannel(int n)
        {
            return vecColors[n];
        }

        public void AddColorChannel(ColorChannel c)
        {
            vecColors.Add(c);
        }


        public List<Enum.Enumeration> GlobalParamsVector
        {
            get { return globalParams; }
        }

        public int GlobalParams
        {
            get {return this.globalParams.Count;}
        }

        public Enum.Enumeration GetGlobalParam(int n)
        {
            return this.globalParams[n];
        }

        public void AddGlobalParam(Enum.Enumeration p)
        {
            this.globalParams.Add(p);
        }
    }
}
