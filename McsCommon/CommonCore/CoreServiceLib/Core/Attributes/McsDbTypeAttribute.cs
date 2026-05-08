namespace CoreServiceLib.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class McsDbTypeAttribute(McsDbType type) : Attribute
    {
        public McsDbType McsDB { get; private set; } = type;
    }
}