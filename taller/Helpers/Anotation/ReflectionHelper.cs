namespace Helpers.Anotation
{
    public static class ReflectionHelper
    {
        public static object? GetNestedPropertyValue(object obj, string path)
        {
            foreach (var part in path.Split('.'))
            {
                if (obj == null) return null;
                var type = obj.GetType();
                var prop = type.GetProperty(part);
                obj = prop?.GetValue(obj);
            }
            return obj;
        }

        public static string PascalJoin(string a, string b)
        {
            return a + char.ToUpper(b[0]) + b.Substring(1);
        }
    }
}
