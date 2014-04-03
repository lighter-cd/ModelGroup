package modelGroup.config.channel
{
public class Flags
{
    public static const Sound:int = 1;			// 附带声音
    public static const BBox:int = 1 << 1;		// 参与边界盒计算
    public static const Pick:int = 1 << 2;		// 参与鼠标挑选
    public static const CastShadow:int = 1 << 3;	// 这个部位是否投射阴影
    public static const ShadowDecal:int = 1 << 4;	// 是否随地形变化的Decal
    public static const Must:int = 1 << 5;		// 是否未装载完这部分，整个模型都不显示
}
}