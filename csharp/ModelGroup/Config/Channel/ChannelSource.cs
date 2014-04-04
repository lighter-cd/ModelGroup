using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroup.Config.Channel
{
    public class ChannelSource
    {
        public Resource.ResourceType modelType;
        public Resource.Resource resource;
        public Enum.Enumeration filterParam;
        public int filterValue;

        public List<int> globalParamIndex;	    // 本频道的四个参数与全局函数的对应关系,下标是全局参数索引顺序
        public int filterParamIndex;	        // 本频道的过滤条件在四个参数中的位置
        public int channelParamIndex;	        // 本频道剩余的需要确定的参数在四个参数中的位置
        public Enum.Enumeration channelParam;         // 本频道剩余的需要确定的参数的类型

        public ChannelSource(String _modelType, String _resource, String _filterEnum, int _value)
        {
            ModelFileConfig mfc= ModelFileConfig.Instance;
            for (int i = 0; i < mfc.modelGroupTypes; i++)
            {
                Resource.ResourceType mt = mfc.getModelGroupType(i);
                if (_modelType == mt.Name)
                {
                    modelType = mt;
                    break;
                }
            }

            if (modelType != null)
            {
                for (int i = 0; i < modelType.Resources; i++)
                {
                    Resource.Resource res = modelType.getResource(i);
                    if (_resource == res.Name)
                    {
                        resource = res;
                        break;
                    }
                }
            }

            if (_filterEnum != null && _filterEnum.Length > 0)
            {
                filterParam = ModelFileConfig.Instance.getEnumParam(_filterEnum);
                if (filterParam != null)
                {
                     filterValue = _value;
                }
            }
        }    
        public void BuildParamIndex(List<Enum.Enumeration> vecGlobalParams)
        {
            globalParamIndex = new List<int>(vecGlobalParams.Count);
            for (int n = 0; n < vecGlobalParams.Count; n++)
            {
                globalParamIndex.Add(-1);
            }
            filterParamIndex = -1;
            channelParamIndex = -1;

            // 全局参数
            int nParams = resource.file.Params;
            List<bool> bParamIndexed = new List<bool>(nParams);
            for (int i = 0; i < vecGlobalParams.Count; i++)
            {
                String global= vecGlobalParams[i].Name;
                for (int p= 0; p < nParams; p++)
                {
                    Enum.Enumeration param = resource.file.getParam(p);
                    if (global == param.Name)
                    {
                        globalParamIndex[i] = p;
                        bParamIndexed[p] = true;
                        break;
                    }
                }
            }
            // 过滤参数
            if (filterParam != null)
            {
                String filter = filterParam.Name;
                for (int p = 0; p < nParams; p++)
                {
                    Enum.Enumeration param = resource.file.getParam(p);
                    if (filter == param.Name)
                    {
                        filterParamIndex = p;
                        bParamIndexed[p] = true;
                        break;
                    }
                }
            }
            // 剩余的参数就是等待模型组最后确定的参数,必须只能剩下一个。
            int nLeft = 0;
            for (int p = 0; p < nParams; p++)
            {
                if (!bParamIndexed[p])
                {
                    if (channelParamIndex < 0)
                    {
                        channelParamIndex = p;
                        channelParam = resource.file.getParam(p);
                        nLeft++;
                    }
                }
            }
        }    
    }
}
