package modelGroup.config.channel
{
import modelGroup.config.NamedItem;
import modelGroup.config.enum.EnumParam;

public class ModelGroupType extends NamedItem
{
    private var vecModels:Vector.<ModelChannel>;	// 模型频道
    private var vecTextures:Vector.<ModelChannel>;	// 纹理频道
    private var vecColors:Vector.<ColorChannel>;	// 颜色频道

    private var real_channels:uint;		// 真正的频道数量(包括数组频道的维度在内的数量)
    private var channelIndices:Array;	// 频道索引与真正的频道数组(包括数组频道的维度在内)下标的对应  UINT

    private var globalParams:Vector.<EnumParam>;	// 全局参数

    private var index:uint;				// 模型组编号

    public function ModelGroupType(name:String, alias:String, _index:uint)
    {
        super(name, alias);
        globalParams = new Vector.<EnumParam>;
        vecModels = new Vector.<ModelChannel>;
        vecTextures = new Vector.<ModelChannel>;
        vecColors = new Vector.<ColorChannel>;
    }

    ///
    public function get ModelChannels():uint
    {
        return vecModels.length;
    }

    public function GetModelChannel(n:uint):ModelChannel
    {
        return vecModels[n];
    }

    public function AddModelChannel(m:ModelChannel):void
    {
        vecModels.push(m);
    }

    public function BuildModelChannelIndices():void
    {
        channelIndices = new Array(vecModels.length);
        for (var i:int = 0; i < vecModels.length; i++)
        {
            var channel:ModelChannel = vecModels[i];

            if (channel.Target && channel.Target != "")
            {
                channel.TargetChannel = GetChannelIndex(channel.Target);
            }

            channelIndices[i] = real_channels;
            real_channels += channel.Dims;
        }
    }

    ///
    public function get TextureChannels():uint
    {
        return vecTextures.length;
    }

    public function GetTextureChannel(n:uint):ModelChannel
    {
        return vecTextures[n];
    }

    public function AddTextureChannel(t:ModelChannel):void
    {
        vecTextures.push(t);
    }

    ///
    public function get ColorChannels():uint
    {
        return vecColors.length;
    }

    public function GetColorChannel(n:uint):ColorChannel
    {
        return vecColors[n];
    }

    public function AddColorChannel(c:ColorChannel):void
    {
        vecColors.push(c);
    }

    public function BuildChannels():void
    {

    }

    public function get GlobalParamsVector():Vector.<EnumParam>
    {
        return this.globalParams;
    }

    public function get GlobalParams():uint
    {
        return this.globalParams.length;
    }

    public function GetGlobalParam(n:uint):EnumParam
    {
        return this.globalParams[n];
    }

    public function AddGlobalParam(p:EnumParam):void
    {
        this.globalParams.push(p);
    }

    public function GetChannelIndex(name:String):int
    {
        for (var i:int = 0; i < vecModels.length; i++)
        {
            if (name == vecModels[i].Name)
            {
                return i;
            }
        }
        return -1;
    }
}
}