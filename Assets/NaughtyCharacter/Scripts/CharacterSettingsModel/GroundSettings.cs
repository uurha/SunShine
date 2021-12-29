using System;
using UnityEngine;

namespace NaughtyCharacter.Scripts.CharacterSettingsModel
{
    [Serializable]
    public class GroundSettings
    {
        [SerializeField] private LayerMask groundLayers;         // Which layers are considered as ground
        [SerializeField] private float sphereCastRadius = 0.35f; // The radius of the sphere cast for the grounded check
        [SerializeField] private float sphereCastDistance = 0.15f; // The distance below the character's capsule used for the sphere cast grounded check
        
        public LayerMask GroundLayers => groundLayers;
        public float SphereCastRadius => sphereCastRadius;
        public float SphereCastDistance => sphereCastDistance;
    }
}
