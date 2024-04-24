using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.Parser
{
    public class ColorParser : IParser<Color>
    {
        private Color m_Value;
        public Color Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        int IPoolObject.PoolKey => default;
        public string MarkName { get; set; }

        IPool IPoolObject.InPool { get; set; }

        public Color Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || !TryParse(pattern, out m_Value))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"FloatParser parse failure. {pattern}");
            }

            return m_Value;
        }

        public static bool TryParse(string pattern, out Color value)
        {
            string[] valueStr = pattern.TrimStart('(', '#').TrimEnd(')').Split(',');
            value = new Color();
            if (valueStr.Length == 0)
                return false;

            if (valueStr.Length == 1 && valueStr[0].Length == 6)
            {
                return ColorUtility.TryParseHtmlString($"#{valueStr[0]}", out value);
            }

            if (valueStr.Length > 0)
                FloatParser.TryParse(valueStr[0], out value.r);
            if (valueStr.Length > 1)
                FloatParser.TryParse(valueStr[1], out value.g);
            if (valueStr.Length > 2)
                FloatParser.TryParse(valueStr[2], out value.b);
            if (valueStr.Length > 3)
                FloatParser.TryParse(valueStr[3], out value.a);
            return true;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public override string ToString()
        {
            return m_Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            if (parser != null)
            {
                return m_Value.Equals(parser.Value);
            }
            else
            {
                return m_Value.Equals(obj);
            }
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {
            m_Value = default;
            LogLv = LogLevel.Warning;
        }

        void IPoolObject.OnRelease()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        public static bool operator ==(ColorParser src, object tar)
        {
            if (ReferenceEquals(src, null))
            {
                return ReferenceEquals(tar, null);
            }
            else
            {
                return src.Equals(tar);
            }
        }

        public static bool operator !=(ColorParser src, object tar)
        {
            if (ReferenceEquals(src, null))
            {
                return !ReferenceEquals(tar, null);
            }
            else
            {
                return !src.Equals(tar);
            }
        }

        public static implicit operator Color(ColorParser parser)
        {
            return parser != null ? parser.m_Value : default;
        }

        public static implicit operator ColorParser(Color value)
        {
            ColorParser parser = References.Require<ColorParser>();
            parser.m_Value = value;
            return parser;
        }
    }
}
