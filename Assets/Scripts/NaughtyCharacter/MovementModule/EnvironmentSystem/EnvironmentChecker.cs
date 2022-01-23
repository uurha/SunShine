using System;
using Base;
using Base.Extensions;
using CorePlugin.Attributes.EditorAddons;
using NaughtyCharacter.CharacterSystem;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem
{
    [CoreManagerElement]
    public class EnvironmentChecker : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private EnvironmentPreset defaultPreset;
        [SerializeField] private float checkSize = 0.3f;
        [SerializeField] private LayerMask layerMask;

        private bool _isHit;
        private readonly Collider[] _results = new Collider[1];
        private EnvironmentPreset _currentLiquidPreset;

        private void Awake()
        {
            character.Set(defaultPreset);
        }

        private void FixedUpdate()
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, checkSize, _results, layerMask, QueryTriggerInteraction.Collide);
            var hit = false;
            if(size > 0)
            {
                hit = _results[0].TryGetComponent<IProvider<EnvironmentPreset>>(out var component);
                _currentLiquidPreset = component.Provided;
            }

            if (hit)
            {
                if(_isHit) return;
                character.Set(_currentLiquidPreset);
            }
            else
            {
                if(!_isHit) return;
                character.Set(defaultPreset);
                _results[0] = null;
                _currentLiquidPreset = null;
            }
            _isHit = hit;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _isHit ? Color.green : Color.red;
            var position = transform.position;
            GizmoExtensions.DrawGizmoSphere(position, checkSize);
        }
    }
}
