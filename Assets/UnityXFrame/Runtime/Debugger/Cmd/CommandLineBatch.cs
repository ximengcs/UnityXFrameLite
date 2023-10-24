using System.Text;
using System.Collections;

namespace UnityXFrame.Core.Diagnotics
{
    internal class CommandLineBatch : IEnumerable
    {
        private CommandLineData[] Lines;

        public CommandLineBatch(string origin)
        {
            InnerAnalyze(origin);
        }

        public IEnumerator GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        private void InnerAnalyze(string param)
        {
            string[] listStr = param.Split('\n');
            Lines = new CommandLineData[listStr.Length];
            for (int i = 0; i < listStr.Length; i++)
                Lines[i] = new CommandLineData(listStr[i]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (CommandLineData line in Lines)
                sb.AppendLine(line.ToString());
            return sb.ToString();
        }
    }
}
