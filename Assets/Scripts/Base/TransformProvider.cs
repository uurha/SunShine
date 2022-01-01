using System;
using CorePlugin.Logger;
using UnityEngine;

namespace Base
{
    public abstract class TransformProvider : MonoBehaviour, IProvider<Transform>
    {
        [SerializeField] private Transform provided;
        public Transform Provided => provided;

        private void Awake()
        {
            if (provided == null)
                DebugLogger.LogException(new NullReferenceException($"{nameof(provided)} is null on {nameof(Awake)}"),
                                         this);
        }

        private void Reset()
        {
            provided = transform;
        }
    }
}
