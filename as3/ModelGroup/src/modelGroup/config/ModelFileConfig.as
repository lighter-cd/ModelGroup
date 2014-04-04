package modelGroup.config
{
import modelGroup.config.enum.Enumeration;
import modelGroup.config.resource.Componets;
import modelGroup.config.resource.FormatFile;
import modelGroup.config.resource.ResourceType;
import modelGroup.config.resource.Resource;

import flash.utils.Dictionary;

/**
 * 所有模型组的资源和命名定义。
 * 装载 命名。json 和 资源.json
 */
public class ModelFileConfig
{
    private var mapEnumParams:Dictionary;			// 所有枚举列表的集合
    private var vecModelTypes:Vector.<ResourceType>;	// 所有资源的集合
    private var path:String;
    private static var instance:ModelFileConfig;

    public static function getInstance():ModelFileConfig
    {
        if (instance == null)
        {
            instance = new ModelFileConfig();
        }
        return instance;
    }

    public function ModelFileConfig()
    {
        if (instance != null)
        {
            return;
        }
        mapEnumParams = new Dictionary();
        vecModelTypes = new Vector.<ResourceType>();
    }

    public function get Path():String
    {
        return path;
    }

    public function set Path(_path:String):void
    {
        path = _path;
    }

    public function get MapEnumParams():Dictionary
    {
        return mapEnumParams;
    }

    public function get ModelTypes():int
    {
        return vecModelTypes.length;
    }

    public function getModelType(n:uint):ResourceType
    {
        return vecModelTypes[n];
    }

    public function getModelTypeByName(name:String):ResourceType
    {
        for (var i:int = 0; i < vecModelTypes.length; i++)
        {
            if (name == vecModelTypes[i].getName(false))
            {
                return vecModelTypes[i];
            }
        }
        return null;
    }

    public function loadConfig(enum:String, res:String):Boolean
    {
        if (loadEnums(enum))
            return loadRes(res);
        return false;
    }


    private function loadEnums(enum:String):Boolean
    {
        //todo:需要增加对json数据的严格检查。
        var json_in:Object = JSON.parse(enum);
        if (json_in is Array)
        {
            var array:Array = json_in as Array;
            for (var i:int = 0; i < array.length; i++)
            {
                var item:Object = array[i];
                var ep:Enumeration = new Enumeration(item.name, item.alias);

                var subArray:Array = (item.item is Array) ? item.item : new Array(item.item);
                for (var j:int = 0; j < subArray.length; j++)
                {
                    var subItem:Object = subArray[j];
                    ep.addItem(subItem.ID, subItem.name, subItem.alias);
                }
                mapEnumParams[item.name] = ep;
            }
        }

        return true;
    }

    private function loadRes(res:String):Boolean
    {
        //todo:需要增加对json数据的严格检查。
        var json_in:Object = JSON.parse(res);
        var arrayModelType:Array = json_in as Array;
        for (var i:int = 0; i < arrayModelType.length; i++)
        {
            var itemModelType:Object = arrayModelType[i];
            var mt:ResourceType = new ResourceType(itemModelType.name, itemModelType.alias);

            var arrayResource:Array = (itemModelType.type is Array) ? itemModelType.type : new Array(itemModelType.type);
            for (var j:int = 0; j < arrayResource.length; j++)
            {
                var resource:Object = arrayResource[j];
                var type:int = resource.hasOwnProperty("type") ? resource.type : FormatFile.FT_MODEL;

                var componets:uint = 0;
                if (resource.hasOwnProperty("bone") && resource.bone)
                {
                    componets |= Componets.Skeleton;
                }
                if (resource.hasOwnProperty("mesh") && resource.mesh)
                {
                    componets |= Componets.Mesh;
                }
                if (resource.hasOwnProperty("anim") && resource.anim)
                {
                    componets |= Componets.Animate;
                }
                if (resource.hasOwnProperty("ashook") && resource.ashook)
                {
                    componets |= Componets.Hook;
                }
                if (resource.hasOwnProperty("camera") && resource.camera)
                {
                    componets |= Componets.Camera;
                }
                mt.addResource(new Resource(resource.name, resource.alias, componets, resource.file, type));
            }
            vecModelTypes.push(mt);
        }
        return true;
    }
}
}