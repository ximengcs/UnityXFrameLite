using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.Parser
{
    public class Vector2Parser : IParser<Vector2>
    {
        private Vector2 m_Value;
        public Vector2 Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        int IPoolObject.PoolKey => default;

        public Vector2 Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || !TryParse(pattern, out m_Value))
            {
                m_Value = default;
                Log.Print(LogLv, "XFrame", $"FloatParser parse failure. {pattern}");
            }

            return m_Value;
        }

        public static bool TryParse(string pattern, out Vector2 value)
        {
            string[] valueStr = pattern.TrimStart('(').TrimEnd(')').Split(',');
            if (valueStr.Length != 2)
            {
                value = default;
                return false;
            }
            else
            {
                if (FloatParser.TryParse(valueStr[0], out value.x) && FloatParser.TryParse(valueStr[1], out value.y))
                {
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }
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

        public static bool operator ==(Vector2Parser src, object tar)
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

        public static bool operator !=(Vector2Parser src, object tar)
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

        public static implicit operator Vector2(Vector2Parser parser)
        {
            return parser != null ? parser.m_Value : default;
        }

        public static implicit operator Vector2Parser(Vector2 value)
        {
            Vector2Parser parser = new Vector2Parser();
            parser.m_Value = value;
            return parser;
        }
    }
}
