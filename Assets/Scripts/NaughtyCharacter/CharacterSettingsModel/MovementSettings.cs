﻿using System;
using Base;
using UnityEngine;

namespace NaughtyCharacter.CharacterSettingsModel
{
    [Serializable]
    public class MovementSettings : ICopyable<MovementSettings>
    {
        /// <summary>
        /// In meters/second
        /// </summary>
        [SerializeField] private float acceleration = 25.0f;

        /// <summary>
        /// In meters/second
        /// </summary>
        [SerializeField] private float deceleration = 25.0f;

        /// <summary>
        /// In meters/second
        /// </summary>
        [SerializeField] private float maxHorizontalSpeed = 8.0f;

        /// <summary>
        /// In meters/second
        /// </summary>
        [SerializeField] private float jumpSpeed = 10.0f;

        /// <summary>
        /// In meters/second
        /// </summary>
        [SerializeField] private float jumpAbortSpeed = 10.0f;

        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float MaxHorizontalSpeed => maxHorizontalSpeed;
        public float JumpSpeed => jumpSpeed;
        public float JumpAbortSpeed => jumpAbortSpeed;

        public MovementSettings Copy()
        {
            return new MovementSettings()
                   {
                       acceleration = acceleration,
                       deceleration = deceleration,
                       maxHorizontalSpeed = maxHorizontalSpeed,
                       jumpSpeed = jumpSpeed,
                       jumpAbortSpeed = jumpAbortSpeed
                   };
        }
    }
}
