package modelGroup.config.resource
{
import modelGroup.config.ModelFileConfig;
import modelGroup.config.enum.Enumeration;

import flash.utils.Dictionary;

/**
 * 模型组频道所对应的一个文件。
 * 资源.json中每个Resource所对应的一个文件
 */
public class FormatFile
{
    public static const FT_MODEL:int = 0;
    public static const FT_TEXTURE:int = 1;

    private var file:String;	// 文件字符串
    private var vecParams:Vector.<Param>;	// 频道使用的参数
    private var type:int;	// 是模型还是纹理

    private function ParseFileString():void
    {
        var current:String = file;
        var offset:int = 0;
        var enums:Dictionary = ModelFileConfig.getInstance().MapEnumParams;

        while (true)
        {
            var start:int = current.indexOf("(", offset);
            var end:int = current.indexOf(")",offset);
            if (start >= 0 && end >= 0)
            {
                var param:String = current.substr(start+1, end - start -1);
                if (param in enums)
                {
                    var enum:Enumeration = enums[param];
                    var p:Param = new Param(start, end, enum);
                    vecParams.push(p);
                }
                offset = end+1;
            } else
            {
                break;
            }
        }
    }

    public function FormatFile(file:String, type:int)
    {
        this.file = file;
        this.type = type;
		this.vecParams = new Vector.<Param>();
        ParseFileString();
    }

    public function get File():String
    {
        return file;
    }

    public function get Type():int
    {
        return type;
    }

    public function get Params():int
    {
        return vecParams ? vecParams.length : 0;
    }

    public function getParam(n:uint):Enumeration
    {
        return vecParams ? vecParams[n].enumParam : null;
    }

    public function getFormatFile(params:Vector.<int>):String
    {
        // todo:字符串替换 vs 字符串搜素，效率要比较后才知道。
        if (params.length == vecParams.length)
        {
            var result:String = file;
            for (var i:int = 0; i < vecParams.length; i++)
            {
                var p:Param = vecParams[i];
                var s:String = p.enumParam.getItemName(params[i], true);
				if(s == null){
					return null;
				}
				result = result.replace("(" + p.enumParam.Name + ")", s);
            }
            return ModelFileConfig.getInstance().Path + result;
        }
        return null;
    }
}
}

import modelGroup.config.enum.Enumeration;

internal class Param
{
    public var nStart:int;
    public var nEnd:int;
    public var enumParam:Enumeration;

    public function Param(start:int, end:int, _enumParam:Enumeration)
    {
        nStart = start;
        nEnd = end;
        enumParam = _enumParam;
    }
}