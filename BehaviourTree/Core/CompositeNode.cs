using System.Collections.Generic;

namespace PowerUtilities
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();

        public override  Node Clone()
        {
            var inst = Instantiate(this);
            inst.children = children.ConvertAll(x => x.Clone());
            return inst;
        }
    }
}
