namespace PowerUtilities
{
    public abstract class DecoratorNode : Node
    {
        public Node child;

        public override Node Clone()
        {
            var inst = Instantiate(this);
            inst.child = child.Clone();
            return inst;
        }
    }
}
