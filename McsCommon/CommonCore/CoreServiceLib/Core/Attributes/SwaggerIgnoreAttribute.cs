namespace CoreServiceLib.Core.Attributes
{
    /// <summary>
    /// 忽略参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SwaggerIgnoreAttribute : Attribute;
}