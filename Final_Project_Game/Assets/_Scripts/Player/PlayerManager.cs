using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.RolePlay.Attributes;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;

        private ModifiableValue<float> _health = new ModifiableValue<float>();
        private ModifiableValue<float> _strength = new ModifiableValue<float>();
        private ModifiableValue<float> _speed = new ModifiableValue<float>();

        void Start()
        {
            _health.initial.value = PlayerConfig._maxHealth;
            _strength.initial.value = PlayerConfig._strength;
            _speed.initial.value = PlayerConfig._speed;
        }
        void Update()
        {
        }

        public void SetSpeed(float speed)
        {
            _playerMovement.SetSpeed(speed);
        }

        public void MinusHealth(float amount)
        {

        }
    }

}
