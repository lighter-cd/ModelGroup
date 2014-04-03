package modelGroup.config.channel
{
import modelGroup.config.NamedItem;

public class ColorChannel extends NamedItem
{
    private var vecElements:Vector.<Element>;
    private var defaultColor:uint;

    public function ColorChannel(name:String, alias:String, _default:uint)
    {
        super(name, alias);
        defaultColor = _default;
        vecElements = new Vector.<Element>();
    }

    public function AddModelElement(channel:int, element:String):void
    {
        var e:Element = new Element(channel, element);
        vecElements.push(e);
    }

    public function GetModelElementChannel(n:uint):int
    {
        return vecElements[n].nChannel;
    }

    public function GetModelElementTarget(n:uint):String
    {
        return vecElements[n].sElement;
    }
}
}
internal class Element
{
    public var nChannel:int;
    public var sElement:String;

    public function Element(_channel:int, _element:String)
    {
        nChannel = _channel;
        sElement = _element;
    }
}