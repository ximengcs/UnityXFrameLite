using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Editor
{
    public class CheckErrorEditor : EditorWindow
    {
        private string m_StringFormatTxt;

        private void OnGUI()
        {
            GUILayout.BeginVertical(StyleUtility.Style1);
            EditorGUILayout.LabelField("Check String Format", StyleUtility.Style2);
            m_StringFormatTxt = EditorGUILayout.TextField(m_StringFormatTxt);
            if (GUILayout.Button("Check String Format"))
                InnerCheckStringFormat(m_StringFormatTxt);
            GUILayout.EndVertical();
        }

        private void InnerCheckStringFormat(string path)
        {
            foreach (string file in Directory.EnumerateFiles(path))
            {
                string ext = Path.GetExtension(file);
                if (ext.Contains("meta"))
                    continue;
                if (ext.Contains("json") || ext.Contains("csv") || ext.Contains("txt"))
                {
                    InnerCheckStringFormatContent(file, File.ReadAllText(file));
                }
            }

            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                InnerCheckStringFormat(dir);
            }
        }

        private void InnerCheckStringFormatContent(string file, string txt)
        {
            EditorLog.Debug($"================== start check {file} ==================");
            char[] invalidSymbols = new char[] { '｛', '｝', '，', '；', '“', '”', '‘', '’', '（', '）' };
            foreach (char ch in invalidSymbols)
            {
                int index = txt.IndexOf(ch);
                if (index != -1)
                {
                    int min = Mathf.Clamp(index - 1, 0, txt.Length);
                    int length = 20;
                    if (min + length >= txt.Length)
                        length = txt.Length - min;
                    string context = txt.Substring(min, length);
                    EditorLog.Debug($"format warning: {file} {EditorLog.Color("Contains", Color.red)} {ch}\t {(int)ch}\t context {EditorLog.Color("->", Color.green)} {context}");
                }
            }
        }
    }
}
