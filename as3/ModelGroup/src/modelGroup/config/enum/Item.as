package modelGroup.config.enum
{
import modelGroup.config.NamedItem;

/**
 * EnumParam 中的一个项目
 */
public class Item extends NamedItem
{
    public var id:uint;

    public function Item(id:uint, name:String, alias:String)
    {
        this.id = id;
        super(name, alias);
    }
}
}