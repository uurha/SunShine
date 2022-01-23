using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.Extensions;
using Base.Inject;
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Attributes.EditorAddons.SelectAttributes;
using CorePlugin.Cross.Events.Interface;
using NaughtyCharacter.AnimationSystem;
using NaughtyCharacter.AnimationSystem.Interfaces;
using NaughtyCharacter.CharacterSystem.Models;
using NaughtyCharacter.MovementModule.CameraSystem;
using NaughtyCharacter.MovementModule.CameraSystem.Interfaces;
using NaughtyCharacter.MovementModule.EnvironmentSystem;
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
        [SerializeField] private CharacterController characterController;

        [SerializeReference] [SelectImplementation]
        private ICameraMovement playerCamera;

        [SerializeReference] [SelectImplementation]
        private IMovementEnvironment movementEnvironment;

        [SerializeReference] [SelectImplementation]
        private IInputAnalyzer inputAnalyzer;

        [SerializeReference] [SelectImplementation]
        private ICharacterAnimator characterAnimator;

        private Transform _transform;
        private IInputAnalyzer.InputAnalyzedData _analyzedInputData;
        private RotationSettings _rotationSettings;
        private MovementSettings _movementSettings;
        private EnvironmentSettings _environmentSettings;
        private GravitySettings _gravitySettings;

        public readonly struct CharacterData
        {
            public Vector3 Position { get; }
            public Vector3 Center { get; }
            public float Radius { get; }
            public Quaternion Rotation { get; }
            public Vector3 Velocity { get; }
            public Vector3 HorizontalVelocity { get; }
            public Vector3 VerticalVelocity { get; }

            public CharacterData(CharacterController character)
            {
                var velocity = character.velocity;
                Radius = character.radius;
                Center = character.center;
                Velocity = velocity;
                HorizontalVelocity = velocity.SetY(0.0f);
                VerticalVelocity = Vector3.Scale(velocity, Vector3.up);
                var characterTransform = character.transform;
                Position = characterTransform.position;
                Rotation = characterTransform.rotation;
            }
        }

        private void SetReferences()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Awake()
        {
            _transform = transform;
            SetReferences();
            Set(new GroundAnimator());
            Set(new GroundMovement());
            playerCamera ??= new CameraMovement();
            inputAnalyzer ??= new DefaultInputAnalyzer();
        }

        private void Start()
        {
            Initialize();
            inputAnalyzer.Inject();
            playerCamera.Inject<ICameraPivotProvider>();
            playerCamera.Inject<ICameraRigProvider>();
        }

        private void Initialize()
        {
            InitializeMovementEnvironment();
            InitializeCharacterAnimator();
        }

        private void InitializeMovementEnvironment()
        {
            movementEnvironment.Inject(_movementSettings);
            movementEnvironment.Inject(_rotationSettings);
            movementEnvironment.Inject(_environmentSettings);
            movementEnvironment.Inject(_gravitySettings);
            movementEnvironment.Initialize();
            inputAnalyzer.SetRotationClampFunction(movementEnvironment.ClampRotationFunc);
            inputAnalyzer.SetMovementClampFunction(movementEnvironment.ClampMovementFunc);
        }

        private void InitializeCharacterAnimator()
        {
            characterAnimator.Inject(playerAnimator);
        }

        private void Reset()
        {
            SetReferences();
        }

        private void Update()
        {
            inputAnalyzer.OnUpdate();
            _analyzedInputData = inputAnalyzer.GetAnalyzedData();
            movementEnvironment.OnUpdate(_analyzedInputData);
        }

        private void FixedUpdate()
        {
            Tick();
            playerCamera.SetPosition(_transform.position);
            playerCamera.SetControlRotation(_analyzedInputData.RotationInput);
        }

        private void Tick()
        {
            movementEnvironment.OnPreMove(Time.deltaTime);
            characterController.Move(movementEnvironment.CalculateMove());
            _transform.rotation = movementEnvironment.CalculateRotation(_transform.rotation);
            movementEnvironment.OnPostMove(new CharacterData(characterController));
            characterAnimator.UpdateState(movementEnvironment.StateProvider);
        }

        private void OnDrawGizmos()
        {
            if (_environmentSettings == null) return;
            var transformPosition = transform.position;
            var center = characterController.center;
            var rotation = transform.rotation;
            var radius = characterController.radius;

            var positions = new Dictionary<Vector3, Vector3>
                            {
                                { Vector3.down, transformPosition },
                                { Vector3.up, transformPosition + center * 2 },
                                { Vector3.forward, transformPosition + center + rotation * Vector3.forward * radius },
                                { Vector3.right, transformPosition + center + rotation * Vector3.right * radius },
                                { Vector3.left, transformPosition + center + rotation * Vector3.left * radius },
                                { Vector3.back, transformPosition + center + rotation * Vector3.back * radius }
                            };

            foreach (var position in positions.Values)
            {
                GizmoExtensions.DrawGizmoSphere(position, _environmentSettings.SphereCastRadius);
            }
        }

        private void ReceivedSettings(MovementSettings settings)
        {
            _movementSettings = settings;
        }

        private void ReceivedSettings(RotationSettings settings)
        {
            _rotationSettings = settings;
        }

        private void ReceivedSettings(GravitySettings settings)
        {
            _gravitySettings = settings;
        }

        public Delegate[] GetSubscribers()
        {
            return new Delegate[]
                   {
                       (SettingsEvents.CharacterSettings<MovementSettings>)ReceivedSettings,
                       (SettingsEvents.CharacterSettings<RotationSettings>)ReceivedSettings,
                       (SettingsEvents.CharacterSettings<GravitySettings>)ReceivedSettings,
                   };
        }

        public void Set(IMovementEnvironment environment)
        {
            if (environment == null) return;
            if (environment.GetType() == movementEnvironment?.GetType()) return;
            var oldEnvironment = movementEnvironment;
            movementEnvironment = environment;
            InitializeMovementEnvironment();
            if (oldEnvironment == null) return;
            var state = oldEnvironment.GetTransferState();
            movementEnvironment.OnEnvironmentChanged(state);
        }

        public void Set(ICharacterAnimator newCharacterAnimator)
        {
            if (newCharacterAnimator == null) return;
            if (newCharacterAnimator.GetType() == characterAnimator?.GetType()) return;
            characterAnimator?.Exit();
            characterAnimator = newCharacterAnimator;
            InitializeCharacterAnimator();
        }

        public void Set(EnvironmentSettings environmentSettings)
        {
            if (environmentSettings == null) return;
            _environmentSettings = environmentSettings;
            movementEnvironment?.Inject(_movementSettings);
        }
    }
}
