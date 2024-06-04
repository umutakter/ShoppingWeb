using System.Reflection;

namespace CoreLibrary.DbCore
{
    public static class DbHelpers
    {
        public static string GetTableName<T>()
        {
            Type type = typeof(T);
            return type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!.ToString()!;
        }
    }
}
