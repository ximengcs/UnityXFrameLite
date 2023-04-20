using XFrame.Utility;
using System.Collections.Generic;

namespace UnityXFrame.Core.Resource
{
    internal class FileNode<T> where T : class
    {
        public string Name;
        public FileNode<T> Parent;
        public Dictionary<string, FileNode<T>> m_Children;
        public Dictionary<string, T> m_Files;

        public FileNode(string name)
        {
            Name = name;
            m_Children = new Dictionary<string, FileNode<T>>();
            m_Files = new Dictionary<string, T>();
        }

        public T GetFile(string fullPath)
        {
            return InnerGetFile(fullPath);
        }

        public bool TryGetFile(string fullPath, out T info)
        {
            info = InnerGetFile(fullPath);
            if (info == null)
                return false;
            else
                return true;
        }

        public FileNode<T> Add(string fullPath, T info)
        {
            if (info == null)
                return default;

            return InnerAdd(fullPath, info);
        }

        public void Remove(string fullPath)
        {
            InnerRemove(fullPath);
        }

        public void Clear()
        {
            m_Children.Clear();
            m_Files.Clear();
        }

        private T InnerGetFile(string fullPath)
        {
            if (PathUtility.CheckFileName(fullPath, out string thisName, out string suplusName) == 1)
            {
                if (m_Files.TryGetValue(thisName, out T info))
                {
                    return info;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                if (m_Children.TryGetValue(thisName, out FileNode<T> node))
                    return node.InnerGetFile(suplusName);
                else
                    return default;
            }
        }

        private FileNode<T> InnerAdd(string fullPath, T info)
        {
            if (PathUtility.CheckFileName(fullPath, out string thisName, out string suplusName) == 1)
            {
                m_Files.Add(thisName, info);
                return default;
            }
            else
            {
                FileNode<T> node;
                if (!m_Children.TryGetValue(thisName, out node))
                {
                    node = new FileNode<T>(thisName);
                    node.Parent = this;
                    m_Children.Add(node.Name, node);
                }

                node.InnerAdd(suplusName, info);
                return node;
            }
        }

        private void InnerRemove(string fullPath)
        {
            if (PathUtility.CheckFileName(fullPath, out string thisName, out string suplusName) == 1)
            {
                Parent.m_Files.Remove(thisName);
            }
            else
            {
                if (m_Children.TryGetValue(thisName, out FileNode<T> node))
                {
                    node.InnerRemove(suplusName);
                }
            }
        }
    }
}
