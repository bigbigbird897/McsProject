namespace CoreServiceLib.Tools
{
    public static class EnumTool
    {
        public static Dictionary<int, string> GetEnums<T>() where T : Enum
        {
            var dict = new Dictionary<int, string>();
            var values = Enum.GetValues(typeof(T));
            foreach (var value in values)
            {
                var d = value?.ToString();
                if (!string.IsNullOrEmpty(d) && value != null)
                {
                    dict.Add(value.GetHashCode(), d);
                }
            }
            return dict;
        }

        public static IEnumerable<KeyValuePair<int, string>> GetKeyValues<T>() where T : Enum
        {
            var source = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                string value = item?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    source.Add((int)Enum.Parse(typeof(T), value), value);
                }
            }
            return source;
        }
    }
}