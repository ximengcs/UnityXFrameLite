using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Serialize;
using System.Collections.Generic;
using XFrame.Modules.XType;
using System.Reflection;

namespace UnityXFrame.Core.Serialize
{
    public partial class CsvSerializeHelper : ISerializeHelper
    {
        private Dictionary<string, TypeHandlerAction> m_Handlers;

        public string CommentsSymbol { get; set; } = "#";
        public string PropertyNameSymbol { get; set; } = "#P";
        public string PropertyTypeSymbol { get; set; } = "#T";

        public int HandleType => Constant.CSV_TYPE;

        public CsvSerializeHelper()
        {
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<ITypeHandler>();
            m_Handlers = new Dictionary<string, TypeHandlerAction>();
            foreach (Type type in typeSys)
            {
                ITypeHandler handler = (ITypeHandler)Activator.CreateInstance(type);
                foreach (var item in handler.TypeHandlers)
                    m_Handlers.Add(item.Key, item.Value);
            }
        }

        public string Serialize<T>(T obj)
        {
            return default;
        }

        public object Deserialize(string json, Type dataType)
        {
            Csv<string> csv = new Csv<string>(json, ParserModule.Inst.STRING);
            Csv<string>.Line nameLine = null;
            Csv<string>.Line typeLine = null;

            int row = 1;
            while (row <= csv.Row)
            {
                Csv<string>.Line line = csv.Get(row++);
                if (line.Count > 0)
                {
                    if (line[0] == CommentsSymbol)
                        continue;

                    if (line[0] == PropertyNameSymbol)
                        nameLine = line;

                    if (line[0] == PropertyTypeSymbol)
                        typeLine = line;
                }
                if (nameLine != null && typeLine != null)
                    break;
            }

            object list = Activator.CreateInstance(dataType);
            Type itemType = dataType.GetGenericArguments()[0];
            MethodInfo method = dataType.GetMethod("Add");

            while (row <= csv.Row)
            {
                Csv<string>.Line rowLine = csv.Get(row++);
                if (rowLine[0] == CommentsSymbol)
                    continue;

                object item = Activator.CreateInstance(itemType);
                for (int i = 0; i < nameLine.Count; i++)
                {
                    string name = nameLine[i];
                    if (string.IsNullOrEmpty(name))
                        continue;

                    object value;
                    if (m_Handlers.TryGetValue(typeLine[i], out TypeHandlerAction handler))
                        value = handler(rowLine[i]);
                    else
                        value = rowLine[i];

                    FieldInfo prop = itemType.GetField(name);
                    if (prop != null)
                        prop.SetValue(item, value);
                }
                method.Invoke(list, new object[] { item });
            }

            return list;
        }

        public T Deserialize<T>(string json)
        {
            return default;
        }
    }
}
