#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Reflection;
using Assets.testUIElements;
using System.Linq;

public class TestGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<TestGraphView, UxmlTraits> { };

	public TestGraphView()
	{
		Insert(0, new GridBackground());

		this.AddManipulator(new ContentDragger());
		this.AddManipulator(new ContentZoomer());
		this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/testUIElements/TestWind1.uss");
        styleSheets.Add(styleSheet);
	}

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
		evt.menu.AppendAction("Add TestNode", (action) => {
			AddElement(new TestNodeView());
		});
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
		return ports.ToList().Where(p => p.direction != startPort.direction).ToList();
        //return base.GetCompatiblePorts(startPort, nodeAdapter);
    }
}
#endif