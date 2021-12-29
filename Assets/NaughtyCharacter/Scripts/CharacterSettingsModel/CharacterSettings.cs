using System;
using UnityEngine;

namespace NaughtyCharacter.Scripts.CharacterSettingsModel
{
    [Serializable]
    public class CharacterSettings
    {
        [SerializeField] private GravitySettings gravitySettings;
        [SerializeField] private GroundSettings groundSettings;
        [SerializeField] private MovementSettings movementSettings;
        [SerializeField] private RotationSettings rotationSettings;
    }
}
