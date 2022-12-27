#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PowerUtilities
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node node;
        
        public Port input;
        public Port output;

        public NodeView(Node node)
        {
            this.node = node;
            title= node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateOutputPorts()
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input,Port.Capacity.Single,typeof(bool));

            if(input != null )
            {
                //input.portName = "";
                inputContainer.Add(input);
            }
        }

        private void CreateInputPorts()
        {
            if (node is ActionNode)
                return;

            var capacity = node is CompositeNode ? Port.Capacity.Multi : Port.Capacity.Single;
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(bool));
            outputContainer.Add(output);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.position = new Vector2(newPos.xMin,newPos.yMin);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            BehaviourTreeEditor.UpdateInspectorView(this);
        }
    }
}
#endif