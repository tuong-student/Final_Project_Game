using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.RolePlay.Attributes;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private ModifiableValue<float> _speed = new ModifiableValue<float>();
        private Vector3 _playerMove;
        private Rigidbody2D _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            GameInput.onPlayerPressMoveVector2 += GetPlayerMoveDir;
            _speed.initial.value = PlayerConfig._speed;
        }
        void Update()
        {
            Move();
        }

        public void SetSpeed(float speed)
        {
            // _speed.value = speed;
        }

        private void GetPlayerMoveDir(Vector2 playerInput)
        {
            _playerMove = playerInput;
        }
        private void Move()
        {
            _rb.velocity = _playerMove * _speed.value;
        }
    }
}
