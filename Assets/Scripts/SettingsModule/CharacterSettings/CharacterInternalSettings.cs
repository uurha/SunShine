using System;
using Base;
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Cross.Events.Interface;
using CorePlugin.Extensions;
using NaughtyCharacter.CharacterSystem.Models;
using UnityEngine;

namespace SettingsModule.CharacterSettings
{
    [CoreManagerElement]
    public class CharacterInternalSettings : MonoBehaviour, IEventHandler
    {
        [SerializeField] private GravitySettings gravitySettings;
        [SerializeField] private EnvironmentSettings environmentSettings;
        [SerializeField] private MovementSettings movementSettings;
        [SerializeField] private RotationSettings rotationSettings;

        private event SettingsEvents.CharacterSettings<GravitySettings> OnGravitySettingsChanged;
        private event SettingsEvents.CharacterSettings<EnvironmentSettings> OnGroundSettingsChanged;
        private event SettingsEvents.CharacterSettings<MovementSettings> OnMovementSettingsChanged;
        private event SettingsEvents.CharacterSettings<RotationSettings> OnRotationSettingsChanged;

        [EditorButton("Update setting")]
        public void InvokeEvents()
        {
            OnGravitySettingsChanged?.Invoke(gravitySettings.Copy());
            OnGroundSettingsChanged?.Invoke(environmentSettings.Copy());
            OnMovementSettingsChanged?.Invoke(movementSettings.Copy());
            OnRotationSettingsChanged?.Invoke(rotationSettings.Copy());
        }

        public void Subscribe(params Delegate[] subscribers)
        {
            EventExtensions.Subscribe(ref OnGravitySettingsChanged, subscribers);
            EventExtensions.Subscribe(ref OnGroundSettingsChanged, subscribers);
            EventExtensions.Subscribe(ref OnMovementSettingsChanged, subscribers);
            EventExtensions.Subscribe(ref OnRotationSettingsChanged, subscribers);
        }

        public void Unsubscribe(params Delegate[] unsubscribers)
        {
            EventExtensions.Unsubscribe(ref OnGravitySettingsChanged, unsubscribers);
            EventExtensions.Unsubscribe(ref OnGroundSettingsChanged, unsubscribers);
            EventExtensions.Unsubscribe(ref OnMovementSettingsChanged, unsubscribers);
            EventExtensions.Unsubscribe(ref OnRotationSettingsChanged, unsubscribers);
        }
    }
}
