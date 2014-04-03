package modelGroup.config.resource
{
import modelGroup.config.NamedItem;

/**
 * 一个模型组所包含的的资源定义。
 * 代表了某类模型组所对应的资源列表
 * 对应 资源.json 中的一个Resource
 */

public class ModelType extends NamedItem
{
    private var vecResources:Vector.<Resource>;

    public function ModelType(name:String, alias:String)
    {
        super(name, alias);
        vecResources = new Vector.<Resource>();
    }

    public function get Resources():int
    {
        return vecResources.length;
    }

    public function getResource(n:uint):Resource
    {
        return vecResources[n];
    }

    public function addResource(r:Resource):void
    {
        vecResources.push(r);
    }
}
}