using System;
using Base;
using UnityEngine;

namespace NaughtyCharacter.CharacterSettingsModel
{
    [Serializable]
    public class GroundSettings : ICopyable<GroundSettings>
    {
        /// <summary>
        /// Which layers are considered as ground
        /// </summary>
        [SerializeField] private LayerMask groundLayers;

        /// <summary>
        /// The radius of the sphere cast for the grounded check
        /// </summary>
        [SerializeField] private float sphereCastRadius = 0.35f;

        /// <summary>
        /// The distance below the character's capsule used for the sphere cast grounded check
        /// </summary>
        [SerializeField] private float sphereCastDistance = 0.15f;

        public LayerMask GroundLayers => groundLayers;
        public float SphereCastRadius => sphereCastRadius;
        public float SphereCastDistance => sphereCastDistance;

        public GroundSettings Copy()
        {
            return new GroundSettings()
                   {
                       groundLayers = groundLayers,
                       sphereCastRadius = sphereCastRadius,
                       sphereCastDistance = sphereCastDistance
                   };
        }
    }
}
