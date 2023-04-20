using System;

namespace UnityXFrame.Core.Diagnotics
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DebugHelpAttribute : Attribute
    {
        public string Content { get; }

        public DebugHelpAttribute(string content)
        {
            Content = content;
        }
    }
}
