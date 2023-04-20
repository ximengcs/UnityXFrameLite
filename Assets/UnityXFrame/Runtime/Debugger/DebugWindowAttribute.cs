using System;

namespace UnityXFrame.Core.Diagnotics
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DebugWindowAttribute : Attribute
    {
        public int Order;
        public string Name;
        public bool AlwaysRun;

        public DebugWindowAttribute()
        {
            Order = 0;
            Name = default;
            AlwaysRun = false;
        }

        public DebugWindowAttribute(int order)
        {
            Order = order;
            Name = default;
            AlwaysRun = false;
        }

        public DebugWindowAttribute(string name)
        {
            Order = 0;
            Name = name;
            AlwaysRun = false;
        }

        public DebugWindowAttribute(bool alwaysRun)
        {
            Order = 0;
            Name = default;
            AlwaysRun = alwaysRun;
        }

        public DebugWindowAttribute(int order, string name)
        {
            Order = order;
            Name = name;
            AlwaysRun = false;
        }

        public DebugWindowAttribute(int order, string name, bool alwaysRun)
        {
            Order = order;
            Name = name;
            AlwaysRun = alwaysRun;
        }
    }
}