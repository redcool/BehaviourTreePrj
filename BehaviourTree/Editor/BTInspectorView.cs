#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;

namespace PowerUtilities.BT
{
    public class BTInspectorView : VisualElement
    {
        public static Action<BTNodeView> OnNodeSelected;

        Editor nodeEditor;
        public new class UxmlFactory : UxmlFactory<BTInspectorView> { }

        public void UpdateView(BTNodeView nodeView)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(nodeEditor);

            nodeEditor = Editor.CreateEditor(nodeView.node);
            var imguiContainer = new IMGUIContainer(() => {
                if(nodeEditor.target)
                    nodeEditor.DrawDefaultInspector();
            });
            Add(imguiContainer);
        }

    }
}
#endif