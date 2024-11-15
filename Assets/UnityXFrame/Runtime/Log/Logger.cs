﻿using System;
using System.Threading;
using UnityEngine;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Logger : XFrame.Modules.Diagnotics.ILogger
    {
        private bool m_MustRegister;
        private Formater m_Formater;

        public Logger()
        {
            m_MustRegister = false;
            m_Formater = new Formater();
        }

        public void Register(string name, Color color)
        {
            m_Formater.Register(name, color);
        }

        public void Debug(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
            {
                if (Global.Fiber.CurrentIsMain)
                {
                    UnityEngine.Debug.Log(result);
                }
                else
                {
                    Global.Fiber.MainFiber.Post((state) =>
                    {
                        string msg = (string)state;
                        UnityEngine.Debug.Log(msg);
                    }, result);
                }
            }
        }

        public void Error(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
            {
                if (Global.Fiber.CurrentIsMain)
                {
                    UnityEngine.Debug.LogError(result);
                }
                else
                {
                    Global.Fiber.MainFiber.Post((state) =>
                    {
                        string msg = (string)state;
                        UnityEngine.Debug.LogError(msg);
                    }, result);
                }
            }
        }

        public void Fatal(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
            {
                if (Global.Fiber.CurrentIsMain)
                {
                    UnityEngine.Debug.LogError(result);
                }
                else
                {
                    Global.Fiber.MainFiber.Post((state) =>
                    {
                        string msg = (string)state;
                        UnityEngine.Debug.LogError(msg);
                    }, result);
                }
            }
        }

        public void Warning(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
            {
                if (Global.Fiber.CurrentIsMain)
                {
                    UnityEngine.Debug.LogWarning(result);
                }
                else
                {
                    Global.Fiber.MainFiber.Post((state) =>
                    {
                        string msg = (string)state;
                        UnityEngine.Debug.LogWarning(msg);
                    }, result);
                }
            }
        }

        private bool InnerFormat(out string result, params object[] content)
        {
            if (content.Length > 1)
            {
                string realContent;
                if (content.Length > 2)
                {
                    object[] contentList = new object[content.Length - 2];
                    for (int i = 0; i < contentList.Length; i++)
                        contentList[i] = content[i + 2];
                    realContent = string.Format((string)content[1], contentList);
                }
                else
                {
                    realContent = content[1].ToString();
                }
                bool success = m_Formater.Format(content[0].ToString(), realContent, out result);
                result = $"[{Thread.CurrentThread.ManagedThreadId,5}]" + result;
                if (success)
                    return true;
                else
                    return false;
            }
            else
            {
                if (content.Length == 1)
                    m_Formater.Format(string.Empty, content[0].ToString(), out result);
                else
                    result = string.Concat(content);
                result = $"[{Thread.CurrentThread.ManagedThreadId,5}]" + result;
                return true;
            }
        }

        public void Exception(Exception e)
        {
            if (Global.Fiber.CurrentIsMain)
            {
                UnityEngine.Debug.LogException(e);
            }
            else
            {
                Global.Fiber.MainFiber.Post((state) =>
                {
                    Exception exp = (Exception)state;
                    UnityEngine.Debug.LogException(exp);
                }, e);
            }
        }
    }
}
