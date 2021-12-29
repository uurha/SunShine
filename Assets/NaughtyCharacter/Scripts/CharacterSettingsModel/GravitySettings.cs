using System;
using UnityEngine;

namespace NaughtyCharacter.Scripts.CharacterSettingsModel
{
    [Serializable]
    public class GravitySettings
    {
        [SerializeField] private float gravity = 20f; // Gravity applied when the player is airborne
        [SerializeField] private float groundedGravity = 5.0f; // A constant gravity that is applied when the player is grounded
        [SerializeField] private float maxFallSpeed = 40.0f; // The max speed at which the player can fall

        public float Gravity => gravity;
        public float GroundedGravity => groundedGravity;
        public float MaxFallSpeed => maxFallSpeed;
    }
}
