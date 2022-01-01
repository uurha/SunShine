using System;
using Base;
using NaughtyCharacter.Helpers;
using UnityEngine;

namespace NaughtyCharacter.CharacterSettingsModel
{
    [Serializable]
    public class RotationSettings : ICopyable<RotationSettings>
    {
        [Header("Control Rotation")]
        [SerializeField] private float minPitchAngle = -45.0f;

        [SerializeField] private float maxPitchAngle = 75.0f;

        [Header("Character Orientation")]
        [SerializeField] private RotationBehavior rotationBehavior = RotationBehavior.OrientRotationToMovement;

        /// <summary>
        /// The turn speed when the player is at max speed (in degrees/second)
        /// </summary>
        [SerializeField] private float minRotationSpeed = 600.0f;

        /// <summary>
        /// The turn speed when the player is stationary (in degrees/second)
        /// </summary>
        [SerializeField] private float maxRotationSpeed = 1200.0f;

        public float MinPitchAngle => minPitchAngle;
        public float MaxPitchAngle => maxPitchAngle;
        public RotationBehavior RotationBehavior => rotationBehavior;
        public float MinRotationSpeed => minRotationSpeed;
        public float MaxRotationSpeed => maxRotationSpeed;

        public RotationSettings Copy()
        {
            return new RotationSettings()
                   {
                       minPitchAngle = minPitchAngle,
                       maxPitchAngle = maxPitchAngle,
                       rotationBehavior = rotationBehavior,
                       minRotationSpeed = minRotationSpeed,
                       maxRotationSpeed = maxRotationSpeed
                   };
        }
    }
}
