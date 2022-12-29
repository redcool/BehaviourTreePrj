using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerUtilities
{
    public abstract class Node : ScriptableObject
    {
        [HideInInspector]public string guid;
        [HideInInspector] public Vector2 position;

        public enum State
        {
            Running,Success,Failed
        }
        [HideInInspector] public State state = State.Running;

        public bool isStarted=false;
        [TextArea]public string description = "Node Description";
        public State Update()
        {
            if(!isStarted)
            {
                OnStart();
                isStarted = true;
            }
            state = OnUpdate();
            if(state != State.Running)
            {
                OnStop();
                isStarted= false;
            }
            return state;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
        public virtual Node Clone()
        {
            return Instantiate(this);
        }
    }
}
