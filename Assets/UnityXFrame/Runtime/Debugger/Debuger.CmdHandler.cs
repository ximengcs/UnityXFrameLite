using System;
using UnityEngine;
using System.Reflection;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Conditions;
using XFrame.Modules.Reflection;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debugger
    {
        private interface ICmdHandler
        {
            void Exec(CommandLineData param);
        }

        #region Method Command Handler
        private struct MethodCmdHandler : ICmdHandler
        {
            private Debugger Debugger;
            private ParameterInfo[] m_ParamInfos;
            private int m_ParamCount;

            public string Name;
            public MethodInfo Method;
            public object Inst;

            public MethodCmdHandler(Debugger debugger, object inst, string name, MethodInfo method)
            {
                Inst = inst;
                Name = name;
                Method = method;
                Debugger = debugger;
                m_ParamInfos = Method.GetParameters();
                m_ParamCount = m_ParamInfos.Length;
            }

            public void Exec(CommandLineData param)
            {
                if (m_ParamCount == 0)
                {
                    Method.Invoke(Inst, null);
                }
                else if (m_ParamCount > 0)
                {
                    if (m_ParamCount == 1)
                    {
                        if (m_ParamInfos[0].ParameterType == typeof(CommandLineData))
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
                                if (Debugger.m_CmdParsers.TryGetValue(info.ParameterType, out IParser parser))
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
        #endregion

        #region Class Command Handler
        private class ClassCmdHandler : ICmdHandler
        {
            public string Name;
            public MethodInfo Method;
            public IDebugCommandLine Inst;

            public ClassCmdHandler(IDebugCommandLine inst, string name, MethodInfo method)
            {
                Inst = inst;
                Name = name;
                Method = method;
            }

            public void Exec(CommandLineData param)
            {
                if (param.ParamCount > 0)
                    CommandLine.Parser.Default.ParseArguments<object>(() => Inst, param.Params);
                Method.Invoke(Inst, null);
            }
        }
        #endregion

        private Dictionary<Type, IParser> m_CmdParsers;
        private Dictionary<Type, object> m_CmdInsts;
        private Dictionary<string, ICmdHandler> m_CmdHandlers;
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
            m_CmdHandlers = new Dictionary<string, ICmdHandler>();

            #region Method Command
            TypeSystem typeSys = Domain.TypeModule.GetOrNewWithAttr<DebugCommandClassAttribute>();
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
                        inst = Domain.TypeModule.CreateInstance(type);
                        m_CmdInsts.Add(type, inst);
                    }
                }

                IDebugCommandLine instCmdLine = inst as IDebugCommandLine;
                MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in methods)
                {
                    DebugCommandAttribute attr = method.GetCustomAttribute<DebugCommandAttribute>();
                    if (attr != null)
                    {
                        string name = method.Name;
                        ICmdHandler cmd;
                        if (instCmdLine != null)
                        {
                            cmd = new ClassCmdHandler(instCmdLine, method.Name, method);
                        }
                        else
                        {
                            cmd = new MethodCmdHandler(this, inst, method.Name, method);
                        }

                        if (!m_CmdHandlers.ContainsKey(name))
                            m_CmdHandlers.Add(name, cmd);
                        else
                            Log.Debug("XFrame", $"cmd {name} is duplicate, ignore after");
                    }
                }
            }
            #endregion
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
            foreach (CommandLineData cmdline in batch)
            {
                if (cmdline.Empty)
                    continue;
                if (m_CmdHandlers.TryGetValue(cmdline.Name, out ICmdHandler cmd))
                {
                    SetTip($"Run {cmdline.Name}", Color.yellow);
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
