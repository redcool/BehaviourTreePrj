using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerUtilities
{
    public abstract class Node : ScriptableObject
    {
        public string guid;
        public Vector2 position;

        public enum State
        {
            Running,Success,Failed
        }
        public State state = State.Running;

        bool isStarted=false;
        public State Update()
        {
            if(!isStarted)
            {
                OnStart();
                isStarted = true;
            }
            state = OnUpdate();
            if(state == State.Success || state == State.Failed)
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
