using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class HideIfAttribute : PropertyAttribute
{
    public readonly string conditionalField;

    public HideIfAttribute(string conditionalField)
    {
        this.conditionalField = conditionalField;
    }
}
