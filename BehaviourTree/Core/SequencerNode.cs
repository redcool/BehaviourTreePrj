namespace PowerUtilities
{
    public class SequencerNode : CompositeNode
    {
        public int index;

        protected override void OnStart()
        {
            index = 0;
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            var child = children[index];
            var state = child.Update();

            if (state == State.Success)
            {
                index++;
            }

            if (state == State.Failed)
                return State.Failed;

            return index == children.Count? State.Success : State.Running;
        }
    }
}
