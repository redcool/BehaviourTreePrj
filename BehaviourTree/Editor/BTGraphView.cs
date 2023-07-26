#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

namespace PowerUtilities.BT
{
    public class BTGraphView : GraphView
    {
        public BehaviourTree Tree;
        public class UxmlFactor : UxmlFactory<BTGraphView, UxmlTraits> { }
        public BTGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BehaviourTreeEditor.RootPathFolder.Value+"/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndo;
        }


        private void OnUndo()
        {
            ShowTree(Tree);
            AssetDatabase.SaveAssets();
        }

        BTNodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.guid) as BTNodeView;
        }

        public void ShowTree(BehaviourTree tree)
        {
            Tree = tree;

            graphViewChanged -= OnViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnViewChanged;

            foreach (var item in tree.nodes)
            {
                CreateNodeView(item);
            }

            //edges
            tree.nodes.ForEach(n =>
            {
                var parent = FindNodeView(n);
                var children = Tree.GetChildren(n);
                children.ForEach(c =>
                {
                    var child = FindNodeView(c);
                    var edge = parent.output.ConnectTo(child.input);
                    AddElement(edge);
                });
            });
        }

        private GraphViewChange OnViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(item => {
                    var nodeView = item as BTNodeView;
                    if(nodeView != null)
                    {
                        Tree.DeleteNode(nodeView.node);
                    }

                    var edge = item as Edge;
                    if(edge != null)
                    {
                        var parent = edge.output.node as BTNodeView;
                        var child = edge.input.node as BTNodeView;

                        Tree.RemoveChild(parent.node, child.node);
                    }
                });
            }

            if(graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    var parent = edge.output.node as BTNodeView;
                    var child = edge.input.node as BTNodeView;

                    Tree.AddChild(parent.node, child.node);
                });
            }

            if(graphViewChange.movedElements != null)
            {
                nodes.ForEach(n =>
                {
                    var nv = n as BTNodeView;
                    if(nv != null)
                    {
                        nv.SortChildren();
                    }
                });
            }

            return graphViewChange;
        }

        public void CreateNode(Type nodeType)
        {
            var node = Tree.CreateNode(nodeType);
            CreateNodeView(node);
        }

        public void CreateNodeView(Node node)
        {
            AddElement(new BTNodeView(node));
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Set Default Action", (action) =>
            {
                if (selection.Count > 0)
                {
                    var nv = selection.First() as BTNodeView;
                    if (nv != null)
                        Tree.rootNode = nv.node;
                }
            });
            evt.menu.AppendSeparator();

            AppendToContextMenu(evt, TypeCache.GetTypesDerivedFrom(typeof(ActionNode)));
            AppendToContextMenu(evt, TypeCache.GetTypesDerivedFrom(typeof(DecoratorNode)));
            AppendToContextMenu(evt, TypeCache.GetTypesDerivedFrom(typeof(CompositeNode)));

            void AppendToContextMenu(ContextualMenuPopulateEvent evt, TypeCache.TypeCollection types)
            {
                foreach (var item in types)
                {
                    evt.menu.AppendAction($"[{item.BaseType.Name}] {item.Name}", (action) => CreateNode(item));
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.node != startPort.node &&
            endPort.direction != startPort.direction).ToList();
        }

        public void UpdateNodeViewStates()
        {
            nodes.ForEach( n => { 
                var nv = n as BTNodeView;
                if(nv != null)
                {
                    nv.UpdateState();
                }
            });
        }
    }
}
#endif