using System;
using Base;
using Base.Inject;
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Attributes.EditorAddons.SelectAttributes;
using CorePlugin.Cross.Events.Interface;
using CorePlugin.Cross.SceneData;
using NaughtyCharacter.CharacterSettingsModel;
using NaughtyCharacter.Helpers;
using NaughtyCharacter.MovementModule.CameraSystem;
using NaughtyCharacter.MovementModule.CameraSystem.Interfaces;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using NaughtyCharacter.MovementModule.PlayerSystem;
using NaughtyCharacter.MovementModule.PlayerSystem.Interfaces;
using NaughtyCharacter.Utility;
using UnityEngine;

namespace NaughtyCharacter.CharacterSystem
{
    [CoreManagerElement]
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, IEventSubscriber
    {
        [SerializeField] private Animator playerAnimator;

        [SerializeReference] [SelectImplementation]
        private ICameraMovement playerCamera;

        [SerializeReference] [SelectImplementation]
        private IMovementEnvironment movementEnvironment;

        [SerializeReference] [SelectImplementation]
        private IInputAnalyzer inputAnalyzer;

        [SerializeReference] [SelectImplementation]
        private ICharacterAnimator characterAnimator;

        private CharacterController _characterController;

        private Data _data;
        private Transform _transform;

        public readonly struct Data
        {
            public Vector3 Velocity { get; }
            public Vector3 HorizontalVelocity { get; }
            public Vector3 VerticalVelocity { get; }
            public bool IsGrounded { get; }

            public Data(CharacterController character, bool isGrounded)
            {
                var velocity = character.velocity;
                Velocity = velocity;
                IsGrounded = isGrounded;
                HorizontalVelocity = velocity.SetY(0.0f);
                VerticalVelocity = Vector3.Scale(velocity, Vector3.up);
            }
        }

        private void Awake()
        {
            _transform = transform;
            _characterController = GetComponent<CharacterController>();
            playerCamera ??= new CameraMovement();
            characterAnimator ??= new CharacterAnimator();
            movementEnvironment ??= new GroundMovement();
        }

        private void Start()
        {
            _data = new Data(_characterController, true);
            movementEnvironment.Initialize();
            inputAnalyzer.Inject();
            inputAnalyzer.SetRotationClampFunction(movementEnvironment.ClampRotationFunc);
            inputAnalyzer.SetMovementClampFunction(movementEnvironment.ClampMovementFunc);
            characterAnimator.Inject(playerAnimator);
            playerCamera.Inject<ICameraPivotProvider>();
            playerCamera.Inject<ICameraRigProvider>();
        }

        private void Update()
        {
            inputAnalyzer.OnUpdate();

            movementEnvironment.OnUpdate(inputAnalyzer.GetMovementInput(), inputAnalyzer.GetRotationInput(),
                                         inputAnalyzer.GetJumpInput());
        }

        private void FixedUpdate()
        {
            Tick();
            playerCamera.SetPosition(_transform.position);
            playerCamera.SetControlRotation(inputAnalyzer.GetRotationInput());
        }

        private void Tick()
        {
            movementEnvironment.OnPreMove(Time.deltaTime);
            _characterController.Move(movementEnvironment.CalculateMove());
            _transform.rotation = movementEnvironment.CalculateRotation(_transform.rotation);
            movementEnvironment.OnPostMove(_transform.position);
            _data = new Data(_characterController, movementEnvironment.IsGrounded);
            characterAnimator.UpdateState(_data);
        }

        private void ReceivedSettings(MovementSettings settings)
        {
            movementEnvironment.Inject(settings);
            characterAnimator.Inject(settings);
        }

        private void ReceivedSettings(RotationSettings settings)
        {
            movementEnvironment.Inject(settings);
        }

        private void ReceivedSettings(GroundSettings settings)
        {
            movementEnvironment.Inject(settings);
        }

        private void ReceivedSettings(GravitySettings settings)
        {
            movementEnvironment.Inject(settings);
        }

        public Delegate[] GetSubscribers()
        {
            return new Delegate[]
                   {
                       (SettingsEvents.CharacterSettings<MovementSettings>)ReceivedSettings,
                       (SettingsEvents.CharacterSettings<RotationSettings>)ReceivedSettings,
                       (SettingsEvents.CharacterSettings<GroundSettings>)ReceivedSettings,
                       (SettingsEvents.CharacterSettings<GravitySettings>)ReceivedSettings,
                   };
        }
    }
}
