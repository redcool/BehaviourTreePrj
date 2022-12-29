#if UNITY_EDITOR
using PowerUtilities;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;


public class BehaviourTreeEditor : EditorWindow
{
    InspectorView inspectorView;
    BehaviourTreeGraphView treeGraphView;

    static InspectorView InspectorView { get; set; }

    public static Lazy<string> RootPathFolder = new Lazy<string>(() => {
        var fileGUID = AssetDatabase.FindAssets("BehaviourTreeEditor").FirstOrDefault();
        return Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(fileGUID));
    });

    [MenuItem("BehaviourTreeEditor/Editor")]
    public static void Open()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    [OnOpenAsset]
    static bool OnOpenAsset(int instanceId,int liine)
    {
        if(Selection.activeObject is BehaviourTree)
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

        inspectorView = root.Q<InspectorView>();
        treeGraphView = root.Q<BehaviourTreeGraphView>();

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

        if(tree != null)
        {
            treeGraphView.ShowTree(tree);
        }
    }
    private void OnInspectorUpdate()
    {
        treeGraphView?.UpdateNodeViewStates();
    }
    public static void UpdateInspectorView(NodeView nv)
    {
        if(InspectorView != null) {
            InspectorView.UpdateView(nv);
        }
    }
}
#endif