using System;
using System.Collections.Generic;

namespace UnityXFrame.Core
{
    public partial class TypeChecker
    {
        private static List<string> s_Types = new List<string>();
        private static List<string> s_ExcludeNameSpaceList = new List<string>();
        private static List<string> s_ExcludeClassList = new List<string>();
        private static List<Type> s_AllClassList = new List<Type>();

        public static void IncludeModule(string moduleName)
        {
            s_Types.Add(moduleName);
        }

        public static void ExcludeModule(string moduleName)
        {
            if (s_Types.Contains(moduleName))
                s_Types.Remove(moduleName);
        }

        public static void IncludeNameSpace(string namespaceName)
        {
            if (s_ExcludeNameSpaceList.Contains(namespaceName))
                s_ExcludeNameSpaceList.Remove(namespaceName);
        }

        public static void ExcludeNameSpace(string namespaceName)
        {
            s_ExcludeNameSpaceList.Add(namespaceName);
        }

        public static void IncludeClass(string className)
        {
            if (s_ExcludeClassList.Contains(className))
                s_ExcludeClassList.Remove(className);
        }

        public static void ExcludeClass(string className)
        {
            s_ExcludeClassList.Add(className);
        }

        public static void IncludeAllClass(Type className)
        {
            if (s_AllClassList.Contains(className))
                s_AllClassList.Remove(className);
        }

        public static void ExcludeAllClass(Type className)
        {
            s_AllClassList.Add(className);
        }
    }
}
