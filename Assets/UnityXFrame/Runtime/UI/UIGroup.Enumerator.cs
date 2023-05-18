using System.Collections;
using XFrame.Collections;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    public partial class UIGroup
    {
        public struct Enumerator : IEnumerator<IUI>
        {
            private IEnumerator<XLinkNode<UIInfo>> m_ListIt;

            public IUI Current => m_ListIt.Current.Value.UI;

            object IEnumerator.Current => Current;

            internal Enumerator(XLinkList<UIInfo> list)
            {
                m_ListIt = list.GetEnumerator();
            }

            public void Dispose()
            {
                m_ListIt.Dispose();
            }

            public bool MoveNext()
            {
                return m_ListIt.MoveNext();
            }

            public void Reset()
            {
                m_ListIt.Reset();
            }
        }
    }
}
