using System;
using UnityEngine;
using System.Reflection;
using XFrame.Modules.XType;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Conditions;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debuger
    {
        private struct CmdHandler
        {
            private ParameterInfo[] m_ParamInfos;
            private int m_ParamCount;

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
            }

            public void Exec(CommandLine param)
            {
                if (m_ParamCount == 0)
                {
                    Method.Invoke(Inst, null);
                }
                else if (m_ParamCount > 0)
                {
                    if (m_ParamCount == 1)
                    {
                        if (m_ParamInfos[0].ParameterType == typeof(CommandLine))
                        {
                            Method.Invoke(Inst, new object[] { param });
                            return;
                        }
                        else if (m_ParamInfos[0].ParameterType == typeof(string[]))
                        {
                            Method.Invoke(Inst, new object[] { param.Params });
                            return;
                        }
                    }
                    object[] paramList = new object[m_ParamCount];
                    for (int i = 0; i < m_ParamCount; i++)
                    {
                        ParameterInfo info = m_ParamInfos[i];
                        if (info.ParameterType == typeof(string))
                        {
                            paramList[i] = i < param.ParamCount ? param[i] : null;
                        }
                        else
                        {
                            string value = i < param.ParamCount ? param[i] : null;
                            if (string.IsNullOrEmpty(value))
                            {
                                Log.Debug("XFrame", $"cmd exec warning, param is null, type is {info.ParameterType}, param Count is {param.ParamCount}, index is {i}");
                                paramList[i] = default;
                            }
                            else
                            {
                                if (Debuger.Inst.m_CmdParsers.TryGetValue(info.ParameterType, out IParser parser))
                                {
                                    paramList[i] = parser.Parse(value);
                                }
                                else
                                {
                                    Log.Debug("XFrame", $"cmd exec warning, can not find {info.ParameterType.Name} parser");
                                    paramList[i] = default;
                                }
                            }
                        }
                    }
                    Method.Invoke(Inst, paramList);
                }
            }
        }

        private Dictionary<Type, IParser> m_CmdParsers;
        private Dictionary<Type, object> m_CmdInsts;
        private Dictionary<string, CmdHandler> m_CmdHandlers;
        private string m_CmdHelpInfo;

        public void SetCmdHelpInfo(string info)
        {
            m_CmdHelpInfo = info;
        }

        public void RegisterCmdParser(Type dataType, Type parserType)
        {
            if (dataType == null || parserType == null)
                return;

            if (!m_CmdParsers.ContainsKey(dataType))
            {
                m_CmdParsers.Add(dataType, (IParser)References.Require(parserType));
            }
        }

        private void InnerInitCmd()
        {
            m_CmdParsers = new Dictionary<Type, IParser>()
            {
                { typeof(int), References.Require<IntParser>() },
                { typeof(bool), References.Require<BoolParser>() },
                { typeof(float), References.Require<FloatParser>() },
                { typeof(ConditionData), References.Require<ConditionParser>() }
            };
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

        internal void InnerClearCmd()
        {
            m_Cmd = null;
        }

        private void InnerRunCmd(string param)
        {
            if (string.IsNullOrEmpty(param))
                return;
            CommandLineBatch batch = new CommandLineBatch(param);
            foreach (CommandLine cmdline in batch)
            {
                if (cmdline.Empty)
                    continue;
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
