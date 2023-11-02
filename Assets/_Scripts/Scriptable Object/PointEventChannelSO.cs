using System;
using UnityEngine;

namespace Apolos.SO
{
    [CreateAssetMenu(menuName = "Events/Point Event Channel")]
    public class PointEventChannelSO : ScriptableObject
    {
        public Action<float, Vector3> OnEventRaised;

        public void RaiseEvent(float value, Vector3 vector3)
        {
            OnEventRaised?.Invoke(value, vector3);
        }
    }
}


