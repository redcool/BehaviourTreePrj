using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;

namespace PowerUtilities
{
    public class InspectorView : VisualElement
    {
        public static Action<NodeView> OnNodeSelected;

        Editor nodeEditor;
        public new class UxmlFactory : UxmlFactory<InspectorView> { }

        public void UpdateView(NodeView nodeView)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(nodeEditor);

            nodeEditor = Editor.CreateEditor(nodeView.node);
            var imguiContainer = new IMGUIContainer(() => {
                nodeEditor.DrawDefaultInspector();
            });
            Add(imguiContainer);
        }

    }
}
