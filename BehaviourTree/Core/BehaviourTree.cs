namespace PowerUtilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
    using Codice.Client.BaseCommands.CheckIn;

    public partial class BehaviourTree
    {
        public Node CreateNode(Type t)
        {
            var node = ScriptableObject.CreateInstance(t) as Node;
            node.guid = GUID.Generate().ToString();
            node.name = t.Name;

            Undo.RecordObject(this, "Behaviour Tree (CreateNode)");

            nodes.Add(node);

            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");

            nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);

            AssetDatabase.SaveAssets();
        }
        public void AddChild(Node parent,Node child)
        {
            var d = parent as DecoratorNode;
            if(d != null)
            {
                Undo.RecordObject(d, "Behaviour Tree(addChild)");
                
                d.child = child;

                EditorUtility.SetDirty(d);
            }
            var c = parent as CompositeNode;
            if(c!= null)
            {
                Undo.RecordObject(c, "Behaviour Tree(addChild)");

                c.children.Add(child);

                EditorUtility.SetDirty(c);
            }
        }

        public void RemoveChild(Node parent,Node child)
        {
            var d = parent as DecoratorNode;
            if (d != null)
            {
                Undo.RecordObject(d, "Behaviour Tree(Remove Child)");

                d.child = null;

                EditorUtility.SetDirty(d);  
            }
            var c = parent as CompositeNode;
            if (c != null)
            {
                Undo.RecordObject(c, "Behaviour Tree(Remove Child)");

                c.children.Remove(child);

                EditorUtility.SetDirty(c);
            }
        }
        public List<Node> GetChildren(Node parent)
        {
            var list = new List<Node>();

            var d = parent as DecoratorNode;
            if (d != null && d.child != null)
                list.Add(d.child);

            var c = parent as CompositeNode;
            if (c != null)
                return c.children;

            return list;
        }
    }
#endif


    [CreateAssetMenu()]
    public partial class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState;

        public List<Node> nodes = new List<Node>();

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                treeState = rootNode.Update();
            }
            return treeState;
        }

        void Traverse(Node n,Action<Node> action)
        {
            if (n)
            {
                action(n);
                var children = GetChildren(n);
                children.ForEach(c => Traverse(c, action));
            }
        }

        public BehaviourTree Clone()
        {
            var tree = Instantiate(this);
            tree.rootNode = rootNode.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.rootNode,n=> tree.nodes.Add(n));

            return tree;
        }
    }
}
