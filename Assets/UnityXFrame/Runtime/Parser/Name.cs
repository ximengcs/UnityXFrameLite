using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.Parser
{
    public class Name : MapParser<IntOrHashParser, UniversalParser>
    {
        public static int AVATAR = 0;
        public static char SPLIT = '_';
        public static char SPLIT2 = '#';

        protected override void InnerParseItem(out IntOrHashParser kParser, out UniversalParser vParser, string[] pItem)
        {
            if (pItem.Length == 1)
            {
                IntOrHashParser key = AVATAR;
                if (Has(key))
                    Log.Error($"Name format error, multi type 0");
                kParser = key;
                vParser = References.Require<UniversalParser>();
                vParser.Parse(pItem[0]);
            }
            else
            {
                kParser = References.Require<IntOrHashParser>();
                vParser = References.Require<UniversalParser>();
                kParser.Parse(pItem[0]);
                vParser.Parse(pItem[1]);
            }
        }

        public bool Is(UniversalParser vParser, bool keyRelease = true, bool valueRelease = true)
        {
            return Is(AVATAR, vParser, keyRelease, valueRelease);
        }

        public bool Is(IntOrHashParser kParser, UniversalParser vParser, bool keyRelease = true, bool valueRelease = true)
        {
            bool result = false;
            if (TryGet(kParser, out UniversalParser target))
                result = vParser == target;
            if (keyRelease)
                kParser.Release();
            if (valueRelease)
                vParser.Release();
            return result;
        }

        public override bool Equals(object obj)
        {
            Name parser = obj as Name;
            if (parser == null)
            {
                string strValue = obj as string;
                if (strValue == m_Origin)
                    return true;

                if (!string.IsNullOrEmpty(strValue))
                    parser = Name.Create(strValue);

                if (parser != null)
                {
                    bool equals = InnerCompareName(this, parser);
                    parser.Release();
                    return equals;
                }
            }
            else
            {
                return InnerCompareName(this, parser);
            }

            return false;
        }

        private static bool InnerCompareName(Name name1, Name name2)
        {
            if (name1.Count != name2.Count)
                return false;
            foreach (var entry in name2.Value)
            {
                if (name1.TryGet(entry.Key, out UniversalParser value))
                {
                    if (entry.Value != value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int result = 0;
            foreach (var entry in Value)
                result += entry.GetHashCode();
            return result;
        }

        public static Name Create(string pattern)
        {
            Name name = References.Require<Name>();
            name.Split = SPLIT;
            name.Split2 = SPLIT2;
            name.Parse(pattern);
            return name;
        }

        public static bool operator ==(Name src, object tar)
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

        public static bool operator !=(Name src, object tar)
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

        public static implicit operator string(Name parser)
        {
            return parser != null ? parser.m_Origin : default;
        }

        public static implicit operator Name(string value)
        {
            return Create(value);
        }
    }
}
