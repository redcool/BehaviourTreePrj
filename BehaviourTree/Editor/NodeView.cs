#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PowerUtilities
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node node;
        
        public Port input;
        public Port output;

        public Orientation orientation = Orientation.Vertical;

        public NodeView(Node node) : base(BehaviourTreeEditor.RootPathFolder.Value+"/NodeView.uxml")
        {
            this.node = node;
            title= node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();

            SetupClasses();

            var description = this.Q<Label>("description");
            description.bindingPath = "description";
            description.Bind(new SerializedObject(node));
        }

        private void SetupClasses()
        {
            if (node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
        }

        private void CreateInputPorts ()
        {
            input = InstantiatePort(orientation, Direction.Input,Port.Capacity.Single,typeof(bool));

            if(input != null )
            {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);

            }
        }

        private void CreateOutputPorts()
        {
            if (node is ActionNode)
                return;

            var capacity = node is CompositeNode ? Port.Capacity.Multi : Port.Capacity.Single;
            output = InstantiatePort(orientation, Direction.Output, capacity, typeof(bool));
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Behaviour Tree SetPosition");

            node.position = new Vector2(newPos.xMin,newPos.yMin);

            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            BehaviourTreeEditor.UpdateInspectorView(this);
        }

        public void SortChildren()
        {
            var c = node as CompositeNode;
            if(c != null)
            {
                c.children.Sort((left, right) => (int)(left.position.x - right.position.x));
            }
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failed");

            if(Application.isPlaying)
            {
                switch (node.state)
                {
                    case Node.State.Running:
                        if(node.isStarted)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.Success:
                        AddToClassList("success");
                        break;
                    case Node.State.Failed:
                        AddToClassList("failed");
                        break;
                }
            }
        }
    }
}
#endif