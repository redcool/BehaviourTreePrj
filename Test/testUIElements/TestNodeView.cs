#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Assets.testUIElements
{
    public class TestNodeView : Node
    {
        public Port input;
        public Port output;

        public TestNodeView()
        {
            this.title = "TestNodeView";
            this.viewDataKey = GUID.Generate().ToString();

            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            inputContainer. Add(input);

            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputContainer. Add(output);
        }
    }
}
#endif