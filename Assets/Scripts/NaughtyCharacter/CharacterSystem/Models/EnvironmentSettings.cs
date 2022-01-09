using System;
using Base;
using UnityEngine;

namespace NaughtyCharacter.CharacterSystem.Models
{
    [Serializable]
    public class EnvironmentSettings : ICopyable<EnvironmentSettings>
    {
        /// <summary>
        /// Which layers are considered as ground
        /// </summary>
        [SerializeField] private LayerMask checkLayer;

        /// <summary>
        /// The radius of the sphere cast for the grounded check
        /// </summary>
        [SerializeField] private float sphereCastRadius = 0.35f;

        /// <summary>
        /// The distance below the character's capsule used for the sphere cast grounded check
        /// </summary>
        [SerializeField] private float sphereCastDistance = 0.15f;

        /// <summary>
        /// Environment speed reduction 0 is fully not movable
        /// </summary>
        [SerializeField] private float environmentDensity = 1;

        public LayerMask CheckLayer => checkLayer;
        public float SphereCastRadius => sphereCastRadius;
        public float SphereCastDistance => sphereCastDistance;
        public float EnvironmentDensity => environmentDensity;

        public EnvironmentSettings Copy()
        {
            return new EnvironmentSettings()
                   {
                       checkLayer = checkLayer,
                       sphereCastRadius = sphereCastRadius,
                       sphereCastDistance = sphereCastDistance,
                       environmentDensity = environmentDensity
                   };
        }
    }
}
