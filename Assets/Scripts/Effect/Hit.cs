using System;
using UnityEngine;

namespace Effect
{
    public class Hit : MonoBehaviour
    {
        private Action _releaseAction;

        public void SetDeactivateAction(Action releaseObj)
        {
            _releaseAction = releaseObj;
        }

        public void Recycle()
        {
            _releaseAction.Invoke();
        }
    }
}