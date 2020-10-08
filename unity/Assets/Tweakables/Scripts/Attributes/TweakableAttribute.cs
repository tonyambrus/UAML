using System;

[AttributeUsage(AttributeTargets.Class)]
public class TweakableAttribute : Attribute
{
    public string Group { get; }

    public TweakableAttribute(string group = null)
    {
        Group = group;
    }
}