using UnityEngine;

namespace PowerUtilities
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        protected override void OnStart()
        {
            Debug.Log($"onstart {message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop {message}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate {message}");
            return State.Success;
        }
    }

}
