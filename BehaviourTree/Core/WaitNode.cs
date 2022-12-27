using UnityEngine;

namespace PowerUtilities
{
    public class WaitNode : ActionNode
    {
        public float time = 1f;
        float startTime;

        protected override void OnStart()
        {
            startTime= Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Time.time - startTime > time)
                return State.Success;
            return State.Running;
        }
    }
}
