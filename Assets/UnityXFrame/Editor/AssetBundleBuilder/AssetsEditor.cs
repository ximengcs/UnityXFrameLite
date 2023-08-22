using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeViewExamples;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class AssetsEditor : EditorWindow
    {
        private Rect m_Rect;
        private SearchField m_SearchField;
        private TestTreeView m_Tree;
        private TreeViewState m_State;

        private void OnEnable()
        {
            m_State = new TreeViewState();
            m_Tree = new TestTreeView(m_State);
            m_SearchField = new SearchField();
            m_SearchField.downOrUpArrowKeyPressed += m_Tree.SetFocusAndEnsureSelectedItem;
        }

        private void OnGUI()
        {
            m_Tree.searchString = m_SearchField.OnGUI(new Rect(20f, 10f, position.width-40f, 20f), m_Tree.searchString);
            m_Tree.OnGUI(new Rect(20, 30, position.width-40, position.height-60));
        }
    }

    public class TestTreeView : TreeViewWithTreeModel<TreeElement>
    {
        public TestTreeView(TreeViewState state) : base(state,
            new MultiColumnHeader(new MultiColumnHeaderState(
                new MultiColumnHeaderState.Column[]
                {
                    new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByLabel"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. "),
                    contextMenuText = "Asset",
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 30,
                    minWidth = 30,
                    maxWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByType"), "Sed hendrerit mi enim, eu iaculis leo tincidunt at."),
                    contextMenuText = "Type",
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 30,
                    minWidth = 30,
                    maxWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = true
                }
                })),
            new TreeModel<TreeElement>(
                new List<TreeElement>()
                {
                    new TreeElement()
                    {
                        depth = -1,
                        name = "Test1",
                        id = -1
                    },
                    new TreeElement()
                    {
                        depth = 0,
                        name = "Test2",
                        id = 0
                    },
                    new TreeElement()
                    {
                        depth = 1,
                        name = "Test3",
                        id = 1
                    },
                    new TreeElement()
                    {
                        depth = 0,
                        name = "Test4",
                        id = 2
                    },
                    new TreeElement()
                    {
                        depth = 1,
                        name = "Test5",
                        id = 3
                    },
                    new TreeElement()
                    {
                        depth = 2,
                        name = "Test6",
                        id = 4
                    }
                }
            ))
        {
            showBorder = true;
            Reload();
        }
    }
}