using System;
using UnityEngine;

namespace NaughtyCharacter.Scripts.CharacterSettingsModel
{
    [Serializable]
    public class MovementSettings
    {
        [SerializeField] private float acceleration = 25.0f;      // In meters/second
        [SerializeField] private float deceleration = 25.0f;      // In meters/second
        [SerializeField] private float maxHorizontalSpeed = 8.0f; // In meters/second
        [SerializeField] private float jumpSpeed = 10.0f;         // In meters/second
        [SerializeField] private float jumpAbortSpeed = 10.0f;    // In meters/second
        
        
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float MaxHorizontalSpeed => maxHorizontalSpeed;
        public float JumpSpeed => jumpSpeed;
        public float JumpAbortSpeed => jumpAbortSpeed;
    }
}
