using System;

[AttributeUsage(AttributeTargets.Field)]
public class TweakAttribute : Attribute
{
    public string Group { get; }
    public string Notify { get; }

    public TweakAttribute(string notify = null, string group = null)
    {
        Group = group;
        Notify = notify;
    }
}
