using System;
using UnityEngine;

namespace Apolos.SO
{
    [CreateAssetMenu(menuName = "Events/Float Event Channel")]
    public class FloatEventChannelSO : ScriptableObject
    {
        public Action<float> OnEventRaised;

        public void RaiseEvent(float value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}


