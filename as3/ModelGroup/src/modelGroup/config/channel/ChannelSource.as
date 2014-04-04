package modelGroup.config.channel
{
import modelGroup.config.ModelFileConfig;
import modelGroup.config.enum.Enumeration;
import modelGroup.config.resource.ResourceType;
import modelGroup.config.resource.Resource;

import flash.utils.Dictionary;

public class ChannelSource
{
    public var modelType:ResourceType;
    public var resource:Resource;
    public var filterParam:Enumeration;
    public var filterValue:int;

    public var globalParamIndex:Array;	// 本频道的四个参数与全局函数的对应关系,下标是全局参数索引顺序
    public var filterParamIndex:int;	// 本频道的过滤条件在四个参数中的位置
    public var channelParamIndex:int;	// 本频道剩余的需要确定的参数在四个参数中的位置
    public var channelParam:Enumeration; // 本频道剩余的需要确定的参数的类型

    public function ChannelSource(_modelType:String, _resource:String, _filterEnum:String, _value:int)
    {
        var mfc:ModelFileConfig = ModelFileConfig.getInstance();
        var nTypes:int = mfc.ModelTypes;
        for (var i:int = 0; i < nTypes; i++)
        {
            var mt:ResourceType = mfc.getModelType(i);
            if (_modelType == mt.Name)
            {
                modelType = mt;
                break;
            }
        }

        if (modelType)
        {
            for (i = 0; i < modelType.Resources; i++)
            {
                var res:Resource = modelType.getResource(i);
                if (_resource == res.Name)
                {
                    resource = res;
                    break;
                }
            }
        }

        if (_filterEnum)
        {
            var MapEnumParams:Dictionary = ModelFileConfig.getInstance().MapEnumParams;
            if (_filterEnum in MapEnumParams)
            {
                filterParam = MapEnumParams[_filterEnum];
                filterValue = _value;
            }
        }

    }

    public function BuildParamIndex(vecGlobalParams:Vector.<Enumeration>):void
    {
        globalParamIndex = new Array(vecGlobalParams.length);
        for (var n:int = 0; n < vecGlobalParams.length; n++)
        {
            globalParamIndex[n] = -1;
        }
        filterParamIndex = -1;
        channelParamIndex = -1;

        // 全局参数
        var nParams:uint = resource.File.Params;
        var bParamIndexed:Array = new Array(nParams);
        for (var i:int = 0; i < vecGlobalParams.length; i++)
        {
            var global:String = vecGlobalParams[i].Name;
            for (var p:int = 0; p < nParams; p++)
            {
                var param:Enumeration = resource.File.getParam(p);
                if (global == param.Name)
                {
                    globalParamIndex[i] = p;
                    bParamIndexed[p] = true;
                    break;
                }
            }
        }
        // 过滤参数
        if (filterParam)
        {
            var filter:String = filterParam.Name;
            for (p = 0; p < nParams; p++)
            {
                param = resource.File.getParam(p);
                if (filter == param.Name)
                {
                    filterParamIndex = p;
                    bParamIndexed[p] = true;
                    break;
                }
            }
        }
        // 剩余的参数就是等待模型组最后确定的参数,必须只能剩下一个。
        var nLeft:int = 0;
        for (p = 0; p < nParams; p++)
        {
            if (!bParamIndexed[p])
            {
                if (channelParamIndex < 0)
                {
                    channelParamIndex = p;
                    channelParam = resource.File.getParam(p);
                }
            }
        }
    }
}
}