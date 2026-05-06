namespace McsCoreLib.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class McsRegionAttribute(string mRegionName) : Attribute
    {
        public string RegionName { get; private set; } = mRegionName;
    }
}