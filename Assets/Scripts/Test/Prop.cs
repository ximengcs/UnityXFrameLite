
using XFrame.Modules.Datas;

namespace Test
{
    public class Prop : IDataRaw
    {
        public int Id;

        public override string ToString()
        {
            return $"Prop {Id}";
        }
    }
}
