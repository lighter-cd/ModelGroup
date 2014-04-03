package modelGroup.config.channel
{
import modelGroup.config.NamedItem;
import modelGroup.config.PartType;

public class ModelChannel extends NamedItem
{
    private var sources:Vector.<ChannelSource>;

    private var target:String;		// 关联目标
    private var targetChannel:int;	// 目标频道
    private var typeRelation:int;	// 关联类型
    private var attachTo:String;	// 目标骨骼
    private var no:int;				// 部位编号
    private var dims:int;			// 频道的数组维度
    private var flags:uint;			// 频道行为定义

    public function ModelChannel(name:String, alias:String, target:String, type:int, attachTo:String, dims:int, flags:uint)
    {
        super(name, alias);
        this.target = target;
        this.typeRelation = type;

        if ((type == PartType.Rigid) || (type == PartType.Blockup) || (type == PartType.Root))
        {
            this.attachTo = attachTo;
        } else
        { //if(eRelation != eRelationNone)
            this.no = int(attachTo);
        }

        this.dims = dims;
        this.flags = flags;
        sources = new Vector.<ChannelSource>;
    }

    public function get Sources():uint
    {
        return sources.length;
    }

    public function getSource(n:uint):ChannelSource
    {
        return sources[n];
    }

    public function addSource(src:ChannelSource):void
    {
        sources.push(src);
    }

    public function get Dims():uint
    {
        return dims;
    }

    public function get Target():String
    {
        return target;
    }

    public function get TargetChannel():int
    {
        return targetChannel;
    }

    public function set TargetChannel(c:int):void
    {
        targetChannel = c;
    }
	public function get TypeRelation():int{
		return typeRelation;
	}
	public function get No():int{
		return no;
	}
}
}