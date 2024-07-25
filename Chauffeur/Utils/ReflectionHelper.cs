using System.Reflection;

namespace Chauffer.Utils;

public static class ReflectionHelper
{
    private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;

    public static T GetPrivateField<T>(this object o, string fieldName)
    {
        return (T)o.GetType().GetField(fieldName, Flags)!.GetValue(o);
    }
    
    public static void SetPrivateField(this object o, string fieldName, object value)
    {
        o.GetType().GetField(fieldName, Flags)!.SetValue(o, value);
    }

    public static void InvokeMethod(this object o, string methodName, object[] parameters = null)
    {
        o.GetType().GetMethod(methodName, Flags)!.Invoke(o, parameters ?? []);
    }
}