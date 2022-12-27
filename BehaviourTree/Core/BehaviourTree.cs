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
    public partial class BehaviourTree
    {
        public Node CreateNode(Type t)
        {
            var node = ScriptableObject.CreateInstance(t) as Node;
            node.guid = GUID.Generate().ToString();
            node.name = t.Name;
            nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
        public void AddChild(Node parent,Node child)
        {
            var d = parent as DecoratorNode;
            if(d != null)
            {
                d.child = child;
            }
            var c = parent as CompositeNode;
            if(c!= null)
                c.children.Add(child);
        }

        public void RemoveChild(Node parent,Node child)
        {
            var d = parent as DecoratorNode;
            if (d != null)
            {
                d.child = null;
            }
            var c = parent as CompositeNode;
            if (c != null)
                c.children.Remove(child);
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

        public BehaviourTree Clone()
        {
            var tree = Instantiate(this);
            tree.rootNode = rootNode.Clone();
            return tree;
        }
    }
}
