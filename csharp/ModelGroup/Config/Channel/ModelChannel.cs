using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroup.Config.Channel
{
    public class ModelChannel : NamedItem
    {
        static public Flags flagsDef;			// 频道行为定义

        private List<ChannelSource> sources;

        private String target;		    // 关联目标
        private int targetChannel;	    // 目标频道
        private PartType typeRelation;	// 关联类型
        private String attachTo;	    // 目标骨骼
        private int no; 				// 部位编号
        private int dims;			    // 频道的数组维度
        private uint flags;

        public ModelChannel(String name, String alias, String target, PartType type, String attachTo, int dims, uint flags)
            :base(name,alias)
        {
            this.target = target;
            this.typeRelation = type;

            if ((type == PartType.Rigid) || (type == PartType.blockup) || (type == PartType.Root))
            {
                this.attachTo = attachTo;
            } else
            { //if(eRelation != eRelationNone)
                try
                {
                    this.no = Convert.ToInt32(attachTo);
                }
                catch(Exception)
                {
                    this.no = 0;
                }
            }

            this.dims = dims;
            this.flags = flags;
            sources = new List<ChannelSource>();
        }


        public int Sources
        {
            get {return sources.Count;}
        }

        public ChannelSource getSource(int n)
        {
            return sources[n];
        }

        public void addSource(ChannelSource src)
        {
            sources.Add(src);
        }
        public int Dims
        {
            get {return dims;}
        }

        public String Target
        {
            get {return target;}
        }

        public int TargetChannel
        {
            get {return targetChannel;}
            set {targetChannel = value;}
        }

	    public PartType TypeRelation
        {
		    get {return typeRelation;}
	    }
	    public int No
        {
            get { return no; }
	    }
    }
}
