using System;
using UnityEngine;

namespace Apolos.SO
{
    [CreateAssetMenu(menuName = "Events/Ball Event Channel")]
    public class BallEventChannelSO : ScriptableObject
    {
        public Action<float, Vector3> OnEventRaised;

        public void RaiseEvent(float value, Vector3 vector3)
        {
            OnEventRaised?.Invoke(value, vector3);
        }
    }
}


