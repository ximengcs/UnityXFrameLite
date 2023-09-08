using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.Parser
{
    public class Name : MapParser<IntOrHashParser, UniversalParser>
    {
        public static int AVATAR = 0;
        public static char SPLIT = '@';
        public static char SPLIT2 = '#';

        private Name() { }

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

        public bool Is(UniversalParser vParser)
        {
            return Is(AVATAR, vParser);
        }

        public bool Is(IntOrHashParser kParser, UniversalParser vParser)
        {
            bool result = false;
            if (TryGet(kParser, out UniversalParser target))
                result = kParser == target;
            vParser.Release();
            kParser.Release();
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
    }
}
