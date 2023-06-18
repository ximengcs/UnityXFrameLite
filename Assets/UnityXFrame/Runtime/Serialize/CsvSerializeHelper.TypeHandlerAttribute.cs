using System.Collections.Generic;

namespace UnityXFrame.Core.Serialize
{
    public partial class CsvSerializeHelper
    {
        public delegate object TypeHandlerAction(string content);

        public interface ITypeHandler
        {
            Dictionary<string, TypeHandlerAction> TypeHandlers { get; }
        }

        private class DefaultTypeHandler : ITypeHandler
        {
            private Dictionary<string, TypeHandlerAction> m_Handlers;

            public Dictionary<string, TypeHandlerAction> TypeHandlers
            {
                get
                {
                    if (m_Handlers == null)
                    {
                        m_Handlers = new Dictionary<string, TypeHandlerAction>()
                        {
                            { "bool", ParseBool },
                            { "int", ParseInt },
                            { "float", ParseFloat }
                        };
                    }
                    return m_Handlers;
                }
            }

            public object ParseFloat(string json)
            {
                if (string.IsNullOrEmpty(json))
                    return default;
                if (!float.TryParse(json, out float value))
                    value = default;
                return value;
            }

            public object ParseInt(string json)
            {
                if (string.IsNullOrEmpty(json))
                    return default;
                if (!int.TryParse(json, out int value))
                    value = default;
                return value;
            }

            public object ParseBool(string json)
            {
                if (string.IsNullOrEmpty(json))
                    return default;
                if (!bool.TryParse(json, out bool value))
                    value = default;
                return value;
            }
        }

    }
}
