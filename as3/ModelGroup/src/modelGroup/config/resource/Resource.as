package modelGroup.config.resource
{
import modelGroup.config.NamedItem;

/**
 * 模型组频道所对应的一个资源。
 * 对应 资源.json 中每个Resource的一个子项
 */

public class Resource extends NamedItem
{
    private var componets:uint;
    private var file:FormatFile;

    public function Resource(name:String, alias:String, componets:uint, file:String, type:int)
    {
        super(name, alias);
        this.componets = componets;
        this.file = new FormatFile(file, type);
    }

    public function get Componets():uint
    {
        return componets;
    }

    public function get File():FormatFile
    {
        return file;
    }
}
}