using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace PowerUtilities.BT
{
    public class BTSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<BTSplitView, UxmlTraits> { }
    }
}
