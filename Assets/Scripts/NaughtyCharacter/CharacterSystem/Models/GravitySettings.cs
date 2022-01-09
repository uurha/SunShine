using System;
using Base;
using UnityEngine;

namespace NaughtyCharacter.CharacterSystem.Models
{
    [Serializable]
    public class GravitySettings : ICopyable<GravitySettings>
    {
        /// <summary>
        /// Gravity applied when the player is airborne
        /// </summary>
        [SerializeField] private float gravity = 20f;

        /// <summary>
        /// A constant gravity that is applied when the player is grounded
        /// </summary>
        [SerializeField] private float groundedGravity = 5.0f;

        /// <summary>
        /// The max speed at which the player can fall
        /// </summary>
        [SerializeField] private float maxFallSpeed = 40.0f;

        public float Gravity => gravity;
        public float GroundedGravity => groundedGravity;
        public float MaxFallSpeed => maxFallSpeed;

        public GravitySettings Copy()
        {
            return new GravitySettings()
                   {
                       gravity = gravity,
                       groundedGravity = groundedGravity,
                       maxFallSpeed = maxFallSpeed
                   };
        }
    }
}
