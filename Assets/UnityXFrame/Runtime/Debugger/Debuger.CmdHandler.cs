using System;
using UnityEngine;
using System.Reflection;
using XFrame.Modules.XType;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debuger
    {
        private struct CmdHandler
        {
            private ParameterInfo[] m_ParamInfos;
            private int m_ParamCount;
            private bool m_AllString;

            public string Name;
            public MethodInfo Method;
            public object Inst;

            public CmdHandler(object inst, string name, MethodInfo method)
            {
                Inst = inst;
                Name = name;
                Method = method;
                m_ParamInfos = Method.GetParameters();
                m_ParamCount = m_ParamInfos.Length;

                m_AllString = true;
                foreach (ParameterInfo info in m_ParamInfos)
                {
                    if (info.ParameterType != typeof(string))
                    {
                        m_AllString = false;
                        break;
                    }
                }
            }

            public void Exec(CommandLine param)
            {
                if (m_ParamCount == 0)
                    Method.Invoke(Inst, null);
                else if (m_ParamCount > 0)
                {
                    if (m_ParamCount == 1 && !m_AllString)
                    {
                        if (m_ParamInfos[0].ParameterType == typeof(CommandLine))
                            Method.Invoke(Inst, new object[] { param });
                        else if (m_ParamInfos[0].ParameterType == typeof(string[]))
                            Method.Invoke(Inst, new object[] { param.Params });
                        else
                            Log.Debug("XFrame", $"Exec cmd {param.Name} failure, param no match, {m_ParamInfos[0].ParameterType}");
                    }
                    else
                    {
                        if (m_AllString)
                        {
                            object[] paramList = new object[m_ParamCount];
                            for (int i = 0; i < m_ParamCount; i++)
                                paramList[i] = i < param.ParamCount ? param[i] : null;
                            Method.Invoke(Inst, paramList);
                        }
                        else
                        {
                            Log.Debug("XFrame", $"Exec cmd {param.Name} failure, param no match, must be CommandLine, empty, string or string list");
                        }
                    }
                }
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
                        else
                            Log.Debug("XFrame", $"cmd {cmd.Name} is duplicate, ignore after");
                    }
                }
            }
        }

        private void InnerRunCmd(string param)
        {
            if (string.IsNullOrEmpty(param))
                return;
            CommandLineBatch batch = new CommandLineBatch(param);
            foreach (CommandLine cmdline in batch)
            {
                if (m_CmdHandlers.TryGetValue(cmdline.Name, out CmdHandler cmd))
                {
                    Debuger.Tip($"Run {cmdline.Name}", Color.yellow);
                    cmd.Exec(cmdline);
                }
                else
                {
                    Log.Debug("XFrame", $"Exec cmd {cmdline.Name} failure");
                }
            }
        }
    }
}
