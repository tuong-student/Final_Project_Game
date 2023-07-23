using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.RolePlay.Attributes;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private ModifiableValue<float> _speed = new ModifiableValue<float>();
        private Vector3 _playerMovement;

        void Awake()
        {
            GameInput.Init();
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
             _playerMovement = playerInput.ToVector3XZ();
        }
        private void Move()
        {
            this.transform.position += _playerMovement * _speed.value * Time.deltaTime;
        }
    }
}
