package modelGroup.config
{
import modelGroup.config.channel.ChannelSource;
import modelGroup.config.channel.ColorChannel;
import modelGroup.config.channel.Flags;
import modelGroup.config.channel.ModelChannel;
import modelGroup.config.channel.ModelGroupType;
import modelGroup.config.enum.Enumeration;

public class ModelGroupConfig
{
    private static var instance:ModelGroupConfig;
    private var vecGroupTypes:Vector.<ModelGroupType>;
    private var modelFileConfig:ModelFileConfig;

    private function LoadChannel(channel_string:String):Boolean
    {
        var json_in:Object = JSON.parse(channel_string);
        var array:Array = json_in as Array;
        for (var i:int = 0; i < array.length; i++)
        {
            var item:Object = array[i];
            var mt:ModelGroupType = new ModelGroupType(item.name, item.alias, i);

            var global_params:Array = (item.global_params.param is Array) ? item.global_params.param : new Array(item.global_params.param);
            for (var j:int = 0; j < global_params.length; j++)
            {
                var param:Object = global_params[j];
				if(param.name in modelFileConfig.MapEnumParams){
					mt.AddGlobalParam(modelFileConfig.MapEnumParams[param.name]);
				}
            }

            var model_channels:Array = (item.channels.channel is Array) ? item.channels.channel : new Array(item.channels.channel);
            for (j = 0; j < model_channels.length; j++)
            {
                var channel:Object = model_channels[j];
                var flags:uint = 0;
                if (channel.hasOwnProperty("sound") && channel.sound)
                {
                    flags |= Flags.Sound;
                }
                if (channel.hasOwnProperty("bbox") && channel.bbox)
                {
                    flags |= Flags.BBox;
                }
                if (channel.hasOwnProperty("pick") && channel.pick)
                {
                    flags |= Flags.Pick;
                }
                if (channel.hasOwnProperty("castshadow") && channel.castshadow)
                {
                    flags |= Flags.CastShadow;
                }
                if (channel.hasOwnProperty("shadowdecal") && channel.castshadow)
                {
                    flags |= Flags.ShadowDecal;
                }
                if (!channel.hasOwnProperty("asyncLoad") || !channel.asyncLoad)
                {
                    flags |= Flags.Must;
                }
                var mc:ModelChannel = new ModelChannel(
                        channel.name, channel.alias, channel.target, channel.type, channel.attach_to,
                        channel.dims, flags);

                var sources:Array = (channel.source is Array) ? channel.source : new Array(channel.source);
                for (var k:int = 0; k < sources.length; k++)
                {
                    var source:Object = sources[k];
                    var src:ChannelSource = new ChannelSource(source.group, source.enum, source.filter, source.value);
                    src.BuildParamIndex(mt.GlobalParamsVector);
                    mc.addSource(src);
                }
                mt.AddModelChannel(mc);
            }
            mt.BuildModelChannelIndices();

            if (item.hasOwnProperty("colors"))
            {
                var color_channels:Array = (item.colors.color is Array) ? item.colors.channel : new Array(item.colors.color);
                for (j = 0; j < color_channels.length; j++)
                {
                    channel = color_channels[j];
                    var cc:ColorChannel = new ColorChannel(channel.name, channel.alias, 0xffffffff);
                    var targetArray:Array = (channel.target is Array) ? channel.target : new Array(channel.target);
                    for (k = 0; k < targetArray.length; k++)
                    {
                        var target:Object = targetArray[k];
                        var channel_index:int = mt.GetChannelIndex(target.channel);
                        cc.AddModelElement(channel_index, target.element);
                    }
                    mt.AddColorChannel(cc);
                }
            }

            if (item.hasOwnProperty("textures"))
            {
                var texture_channels:Array = (item.textures.channel is Array) ? item.textures.channel : new Array(item.textures.channel);
                for (j = 0; j < texture_channels.length; j++)
                {
                    channel = texture_channels[j];
                    mc = new ModelChannel(channel.name, channel.alias, channel.target, channel.type, channel.attach_to, 1, 0);
                    sources = (channel.source is Array) ? channel.source : new Array(channel.source);
                    for (k = 0; k < sources.length; k++)
                    {
                        source = sources[k];
                        src = new ChannelSource(source.group, source.enum, source.filter, source.value);
                        src.BuildParamIndex(mt.GlobalParamsVector);
                        mc.addSource(src);
                    }
                    mt.AddModelChannel(mc);
                }
            }

            vecGroupTypes.push(mt);
        }
        return true;
    }

    public static function getInstance():ModelGroupConfig
    {
        if (instance == null)
        {
            instance = new ModelGroupConfig();
        }
        return instance;
    }

    public function ModelGroupConfig()
    {
        vecGroupTypes = new Vector.<ModelGroupType>();
    }

    public function LoadConfig(enum:String, res:String, channel:String, modelPath:String):Boolean
    {
        ModelFileConfig.getInstance().Path = modelPath;
        modelFileConfig = ModelFileConfig.getInstance();
        if (ModelFileConfig.getInstance().loadConfig(enum, res))
        {
            return LoadChannel(channel);
        }
        return false;
    }

    public function get GroupTypes():uint
    {
        return vecGroupTypes.length;
    }

    public function GetGroupType(n:uint):ModelGroupType
    {
        return vecGroupTypes[n];
    }
	
	public function GetEnumParam(name:String):Enumeration
	{
		if(name in modelFileConfig.MapEnumParams){
			return modelFileConfig.MapEnumParams[name];
		}
		return null;
	}
}
}