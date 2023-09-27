
#if ODIN_INSPECTOR_3
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace RazerCore.Utils.Attributes
{
    [System.Diagnostics.Conditional("UNITY_EDITOR"), AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ListElementInlineButtonAttribute : Attribute
    {
        public string Action;
        public string Label;
        public string ShowIf;
        public string ButtonColor;
        public string TextColor;
        public SdfIconType Icon;
        public IconAlignment IconAlignment;

        public ListElementInlineButtonAttribute(string action, string label = null)
        {
            this.Action = action;
            this.Label = label;
        }

        public ListElementInlineButtonAttribute(string action, string label, SdfIconType icon) : this(action, label)
        {
            Icon = icon;
        }
    }
}
#endif
