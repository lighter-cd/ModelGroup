namespace ModelGroup
{
    namespace Channel
    {
        public enum Flags
        {
            Sound = 1,              // 附带声音
            BBox = 1 << 1,          // 参与边界盒计算
            Pick = 1 << 2,          // 参与鼠标挑选
            CastShadow = 1 << 3,    // 这个部位是否投射阴影
            ShadowDecal = 1 << 4,   // 是否随地形变化的Decal
            Must = 1 << 5           // 是否未装载完这部分，整个模型都不显示
        }
    }
}