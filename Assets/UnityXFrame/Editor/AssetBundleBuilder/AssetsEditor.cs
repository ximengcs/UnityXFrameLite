using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeViewExamples;
using UnityEngine;
using static UnityEditor.Progress;

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

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var myTreeAsset = EditorUtility.InstanceIDToObject(instanceID).name;
            return false; // we did not handle the open
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
            else if (Event.current.type == EventType.DragPerform)
            {
                // To consume drag data.
                DragAndDrop.AcceptDrag();

                // GameObjects from hierarchy.
                if (DragAndDrop.paths.Length == 0 && DragAndDrop.objectReferences.Length > 0)
                {
                    Debug.Log("GameObject");
                    foreach (Object obj in DragAndDrop.objectReferences)
                    {
                        Debug.Log("- " + obj);
                    }
                }
                // Object outside project. It mays from File Explorer (Finder in OSX).
                else if (DragAndDrop.paths.Length > 0 && DragAndDrop.objectReferences.Length == 0)
                {
                    Debug.Log("File");
                    foreach (string path in DragAndDrop.paths)
                    {
                        Debug.Log("- " + path);
                    }
                }
                // Unity Assets including folder.
                else if (DragAndDrop.paths.Length == DragAndDrop.objectReferences.Length)
                {
                    Debug.Log("UnityAsset");
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        Object obj = DragAndDrop.objectReferences[i];
                        string path = DragAndDrop.paths[i];
                        Debug.Log(obj.GetType().Name);

                        // Folder.
                        if (obj is DefaultAsset)
                        {
                            Debug.Log(path);
                        }
                        // C# or JavaScript.
                        else if (obj is MonoScript)
                        {
                            Debug.Log(path + "\n" + obj);
                        }
                        else if (obj is Texture2D)
                        {

                        }

                    }
                }
                // Log to make sure we cover all cases.
                else
                {
                    Debug.Log("Out of reach");
                    Debug.Log("Paths:");
                    foreach (string path in DragAndDrop.paths)
                    {
                        Debug.Log("- " + path);
                    }

                    Debug.Log("ObjectReferences:");
                    foreach (Object obj in DragAndDrop.objectReferences)
                    {
                        Debug.Log("- " + obj);
                    }
                }
            }

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
                    width = 50,
                    minWidth = 50,
                    maxWidth = 1000000f,
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
                    width = 50,
                    minWidth = 50,
                    maxWidth = 1000000f,
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
            columnIndexForTreeFoldouts = 1;
            Reload();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var element = (TreeViewItem<TreeElement>)args.item;
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                InnerDrawRowGUI(args.GetCellRect(i), element, i, ref args);
            }
        }

        private void InnerDrawRowGUI(Rect cellRect, TreeViewItem<TreeElement> item, int culumn, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);
            //DefaultGUI.LabelRightAligned(cellRect, content, args.selected, args.focused);
            if (culumn == 0)
                GUI.DrawTexture(cellRect, EditorGUIUtility.FindTexture("Folder Icon"), ScaleMode.ScaleToFit);
            else
            {
                args.rowRect = cellRect;
                base.RowGUI(args);
            }
        }
    }
}