using System;

[AttributeUsage(AttributeTargets.Method)]
public class TweakButtonAttribute : Attribute
{
    public string Name { get; }

    public TweakButtonAttribute(string name = null)
    {
        Name = name;
    }
}
