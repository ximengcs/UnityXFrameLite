using System;
using XFrame.Modules.Times;
using System.Collections.Generic;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debuger
    {
        private class TweenModule
        {
            private Dictionary<string, Tween> m_Tweens;
            private List<Tween> m_TweensList;

            public TweenModule()
            {
                m_TweensList = new List<Tween>();
                m_Tweens = new Dictionary<string, Tween>();
            }

            public void OnUpdate()
            {
                m_TweensList.Clear();
                m_TweensList.AddRange(m_Tweens.Values);
                foreach (Tween tween in m_TweensList)
                {
                    tween.Update();
                    if (tween.IsComplete)
                        m_Tweens.Remove(tween.Key);
                }
            }

            public void Do(string key, float endValue, float duration, Func<float> getter, Action<float> setter)
            {
                if (m_Tweens.ContainsKey(key))
                {
                    m_Tweens[key] = new Tween(key, endValue, duration, getter, setter);
                }
                else
                {
                    m_Tweens.Add(key, new Tween(key, endValue, duration, getter, setter));
                }
            }
        }

        private struct Tween
        {
            public string Key;
            public float EndValue;
            public float Duration;
            public Func<float> Getter;
            public Action<float> Setter;
            public bool IsComplete;

            private float m_StartValue;
            private float m_SuplusTime;
            private float m_LastTime;

            public Tween(string key, float endValue, float duration, Func<float> getter, Action<float> setter)
            {
                Key = key;
                EndValue = endValue;
                Duration = duration;
                Getter = getter;
                Setter = setter;
                IsComplete = false;
                m_SuplusTime = duration;
                m_LastTime = TimeModule.Inst.Time;
                m_StartValue = Getter();
            }

            public void Update()
            {
                float escapeTime = TimeModule.Inst.Time - m_LastTime;
                m_LastTime = TimeModule.Inst.Time;
                m_SuplusTime -= escapeTime;
                float rate = 1 - m_SuplusTime / Duration;

                float value = m_StartValue + (EndValue - m_StartValue) * rate;
                Setter(value);

                if (m_SuplusTime <= 0)
                    IsComplete = true;
            }
        }
    }
}
