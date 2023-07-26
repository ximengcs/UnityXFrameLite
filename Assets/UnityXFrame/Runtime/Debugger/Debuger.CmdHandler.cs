using System;
using System.Reflection;
using XFrame.Modules.XType;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debuger
    {
        private struct CmdHandler
        {
            public string Name;
            public MethodInfo Method;
            public object Inst;

            public CmdHandler(object inst, string name, MethodInfo method)
            {
                Inst = inst;
                Name = name;
                Method = method;
            }

            public void Exec(string[] param)
            {
                Method.Invoke(Inst, new object[] { param });
            }
        }

        private Dictionary<Type, object> m_CmdInsts;
        private Dictionary<string, CmdHandler> m_CmdHandlers;

        private void InnerInitCmd()
        {
            m_CmdInsts = new Dictionary<Type, object>();
            m_CmdHandlers = new Dictionary<string, CmdHandler>();
            TypeSystem typeSys = TypeModule.Inst.GetOrNewWithAttr<DebugCommandClassAttribute>();
            foreach (Type type in typeSys)
            {
                object inst;
                if (type.IsSealed && type.IsAbstract)
                {
                    if (!m_CmdInsts.TryGetValue(type, out inst))
                    {
                        inst = null;
                        m_CmdInsts.Add(type, inst);
                    }
                }
                else
                {
                    if (!m_CmdInsts.TryGetValue(type, out inst))
                    {
                        inst = TypeModule.Inst.CreateInstance(type);
                        m_CmdInsts.Add(type, inst);
                    }
                }

                MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in methods)
                {
                    DebugCommandAttribute attr = method.GetCustomAttribute<DebugCommandAttribute>();
                    if (attr != null)
                    {
                        CmdHandler cmd = new CmdHandler(inst, method.Name, method);
                        if (!m_CmdHandlers.ContainsKey(cmd.Name))
                            m_CmdHandlers.Add(method.Name, cmd);
                    }
                }
            }
        }

        private void InnerRunCmd(string param)
        {
            if (string.IsNullOrEmpty(param))
                return;
            string[] strs = param.Split(' ');
            int count = strs.Length - 1;
            string cmdName = strs[count];

            if (m_CmdHandlers.TryGetValue(cmdName, out CmdHandler cmd))
            {
                string[] input = new string[count < 0 ? 0 : count];
                for (int i = 1; i < count; i++)
                    input[i - 1] = strs[i];
                Debuger.Tip(this, $"Run {cmdName}", Color.yellow);
                cmd.Exec(input);
            }
        }
    }
}
