package modelGroup.config
{
public class NamedItem
{
    private var name:Array;

    public function NamedItem(name:String, alias:String)
    {
        this.name = new Array(name, alias);
    }

    public function getName(alias:Boolean):String
    {
        return name[alias?1:0];
    }

    public function get Name():String
    {
        return name[0];
    }

    public function get Alias():String
    {
        return name[1];
    }
}
}