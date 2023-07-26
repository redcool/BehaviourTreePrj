#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace PowerUtilities.BT
{
    public class BehaviourTreeEditor : EditorWindow
    {
        BTInspectorView inspectorView;
        BTGraphView treeGraphView;

        static BTInspectorView InspectorView { get; set; }

        public static Lazy<string> RootPathFolder = new Lazy<string>(() =>
        {
            var fileGUID = AssetDatabase.FindAssets("BehaviourTreeEditor").FirstOrDefault();
            return Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(fileGUID));
        });

        [MenuItem("PowerUtilities/BehaviourTreeEditor/Editor")]
        public static void Open()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
            if (wnd.position.width < 1000)
                wnd.position = new Rect(100, 100, 1000, 800);
        }

        [OnOpenAsset]
        static bool OnOpenAsset(int instanceId, int liine)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                Open();
                return true;
            }
            return false;
        }


        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(RootPathFolder.Value + "/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(RootPathFolder.Value + "/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            inspectorView = root.Q<BTInspectorView>();
            treeGraphView = root.Q<BTGraphView>();

            InspectorView = inspectorView;

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            var tree = Selection.activeObject as BehaviourTree;
            if (!tree && Selection.activeGameObject)
            {
                var run = Selection.activeGameObject.GetComponent<BehaviourTreeRun>();
                if (run)
                    tree = run.tree;
            }

            if (tree != null)
            {
                treeGraphView.ShowTree(tree);
            }
        }
        private void OnInspectorUpdate()
        {
            treeGraphView?.UpdateNodeViewStates();
        }
        public static void UpdateInspectorView(BTNodeView nv)
        {
            if (InspectorView != null)
            {
                InspectorView.UpdateView(nv);
            }
        }
    }
}
#endif