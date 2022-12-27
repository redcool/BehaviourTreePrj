using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestSplitView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<TestSplitView, UxmlTraits> { };
}
