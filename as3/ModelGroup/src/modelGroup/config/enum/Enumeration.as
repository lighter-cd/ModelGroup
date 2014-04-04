package modelGroup.config.enum
{
import modelGroup.config.NamedItem;

import flash.utils.Dictionary;

/**
 * 模型组配置中的一个参数可能出现的ID和命名列举。
 * 对应 命名.json中的一个子项。
 */
public class Enumeration extends NamedItem
{
    private var mapItems:Dictionary = new Dictionary();
    private var _vecItems:Vector.<Item> = new Vector.<Item>();

    public function Enumeration(name:String, alias:String)
    {
        super(name, alias);
    }

    public function addItem(id:uint, name:String, alias:String):void
    {
        var item:Item = new Item(id, name, alias);
        mapItems[id] = item;
        _vecItems.push(item);
    }

    public function getItemName(id:uint, alias:Boolean):String
    {
        if (id in mapItems)
        {
            return (mapItems[id] as Item).getName(alias);
        }
        return null;
    }

    public function getItemIDByName(name:String):uint
    {
        for (var i:int = 0; i < vecItems.length; i++)
        {
            if (_vecItems[i].Name == name)
            {
                return _vecItems[i].id;
            }
        }
        return 0;
    }

    public function getItem(id:uint):Item
    {
        if (id in mapItems)
        {
            return mapItems[id];
        }
        return null;
    }

    public function get Size():int
    {
        return _vecItems.length;
    }

	public function get vecItems():Vector.<Item>
	{
		return _vecItems;
	}

}
}
