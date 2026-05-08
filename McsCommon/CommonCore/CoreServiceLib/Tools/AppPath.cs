namespace CoreServiceLib.Tools
{
    public class AppPath
    {
        public static string AppBaseDirectory { get; }
        public static DirectoryInfo ParentPath { get; }
        public static string ParentPathString { get; }

        static AppPath()
        {
            AppBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ParentPath = Directory.GetParent($"{AppBaseDirectory}");
            ParentPathString = $@"{ParentPath?.Parent?.FullName}\";
        }

        /// <summary>
        /// 获取文件夹路径，若不存在则创建
        /// </summary>
        /// <param name="mFolderName"></param>
        /// <returns></returns>
        public static string GetOrCreatePath(string mFolderName)
        {
            string path = $@"{ParentPathString}{mFolderName}";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
    }
}