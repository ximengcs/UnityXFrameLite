using System.Text;
using System.Collections.Generic;

namespace UnityXFrame.Core.Diagnotics
{
    public struct CommandLine
    {
        private string m_Name;
        private string[] m_Params;

        public string[] Params => m_Params;
        public int ParamCount => m_Params.Length;
        public string Name => m_Name;
        public bool Empty => string.IsNullOrEmpty(m_Name);

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= m_Params.Length)
                    return default;
                return m_Params[index];
            }
        }

        internal CommandLine(string param)
        {
            param = param.Trim();
            if (string.IsNullOrEmpty(param))
            {
                m_Name = null;
                m_Params = null;
                return;
            }
            StringBuilder cur = null;
            List<StringBuilder> list = new List<StringBuilder>();
            bool hasAdd = false;
            bool valid = false;
            int index = 0;
            bool isString = false;
            foreach (char c in param)
            {
                if (list.Count <= index && cur == null)
                {
                    cur = new StringBuilder();
                }

                if (c == '\"')
                {
                    if (cur.Length == 0)
                    {
                        valid = true;
                        isString = true;
                    }
                    else
                    {
                        index++;
                        cur = null;
                        isString = false;
                        valid = false;
                        hasAdd = false;
                    }
                }
                else
                {
                    if (c == ' ' && !isString)
                    {
                        index++;
                        cur = null;
                        isString = false;
                        valid = false;
                        hasAdd = false;
                    }
                    else
                    {
                        valid = true;
                        cur.Append(c);
                    }
                }

                if (valid && !hasAdd)
                {
                    hasAdd = true;
                    list.Add(cur);
                }
            }

            m_Params = new string[list.Count - 1];
            m_Name = list[0].ToString();
            for (int i = 1; i < list.Count; i++)
                m_Params[i - 1] = list[i].ToString().Trim();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Name);
            foreach (string str in m_Params)
            {
                sb.Append(" [");
                sb.Append(str);
                sb.Append(']');
            }
            return sb.ToString();
        }
    }
}
