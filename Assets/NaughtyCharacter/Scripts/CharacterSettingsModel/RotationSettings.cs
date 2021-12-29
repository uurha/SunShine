using System;
using UnityEngine;

namespace NaughtyCharacter.Scripts.CharacterSettingsModel
{
    [Serializable]
    public class RotationSettings
    {
        [Header("Control Rotation")]
        [SerializeField] private float minPitchAngle = -45.0f;

        [SerializeField] private float maxPitchAngle = 75.0f;

        [Header("Character Orientation")]
        [SerializeField] private RotationBehavior rotationBehavior = RotationBehavior.OrientRotationToMovement;
        [SerializeField] private float minRotationSpeed = 600.0f; // The turn speed when the player is at max speed (in degrees/second)
        [SerializeField] private float maxRotationSpeed = 1200.0f; // The turn speed when the player is stationary (in degrees/second)
        
        public float MinPitchAngle => minPitchAngle;
        public float MaxPitchAngle => maxPitchAngle;
        public RotationBehavior RotationBehavior => rotationBehavior;
        public float MinRotationSpeed => minRotationSpeed;
        public float MaxRotationSpeed => maxRotationSpeed;
    }
}
