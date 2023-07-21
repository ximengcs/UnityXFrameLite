using System.Collections.Generic;
using UnityEngine;

namespace UnityXFrame.Core.Diagnotics
{
    public static class DebugGUI
    {
        public static DebugStyle Style;
        private static string[] m_PowerText = new string[] { "On", "Off" };
        private static Dictionary<int, string> s_FloatTexts = new Dictionary<int, string>();


        public static GUILayoutOption Width(float width)
        {
            return GUILayout.Width(width * Debuger.Inst.m_FitWidth);
        }

        public static GUILayoutOption Height(float height)
        {
            return GUILayout.Height(height * Debuger.Inst.m_FitWidth);
        }


        public static void Progress(float value)
        {
            Progress(value, 0, 1);
        }

        public static void Progress(float value, float startValue, float endValue)
        {
            float rate = (value - startValue) / (endValue - startValue);
            rate = Mathf.Clamp(rate, 0, 1);
            Rect rect = GUILayoutUtility.GetAspectRect(10f / 1f);
            rect.width = rate * rect.width;
            rect.xMin += 10 * Debuger.Inst.m_FitWidth;
            rect.xMax -= 10 * Debuger.Inst.m_FitWidth;
            GUI.DrawTexture(rect, Style.ProgressSlider.normal.background, ScaleMode.StretchToFill);
        }

        public static void Line()
        {
            GUILayout.Label(string.Empty, GUILayout.Height(10));
            GUILayout.Button(string.Empty, GUILayout.Height(6));
        }

        public static Vector2 BeginScrollView(Vector2 pos, params GUILayoutOption[] options)
        {
            return GUILayout.BeginScrollView(pos, false, false, Style.HorizontalScrollbar, Style.VerticalScrollbar, options);
        }

        public static bool Button(string title, params GUILayoutOption[] options)
        {
            return GUILayout.Button(title, Style.Button, options);
        }

        public static string TextField(string text, params GUILayoutOption[] options)
        {
            return GUILayout.TextField(text, Style.Text, options);
        }

        public static int IntField(int value, params GUILayoutOption[] options)
        {
            string valueText = value.ToString();
            valueText = TextField(valueText, options);
            int.TryParse(valueText, out value);
            return value;
        }

        public static float FloatField(float value, params GUILayoutOption[] options)
        {
            int code = GUIUtility.GetControlID(nameof(FloatField).GetHashCode(), FocusType.Keyboard, GUILayoutUtility.GetLastRect());
            string valueText;
            if (!s_FloatTexts.TryGetValue(code, out valueText))
                valueText = value.ToString();
            valueText = TextField(valueText, options);
            s_FloatTexts[code] = valueText;
            float.TryParse(valueText, out value);
            return value;
        }

        public static void Label(string content, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, Style.Lable, options);
        }

        public static string TextArea(string content, params GUILayoutOption[] options)
        {
            return GUILayout.TextArea(content, Style.TextArea, options);
        }

        public static int Toolbar(int selectId, string[] texts, params GUILayoutOption[] options)
        {
            return GUILayout.Toolbar(selectId, texts, Style.Toolbar, options);
        }

        public static bool Power(bool on)
        {
            return Toolbar(on ? 0 : 1, m_PowerText) == 0;
        }
    }
}