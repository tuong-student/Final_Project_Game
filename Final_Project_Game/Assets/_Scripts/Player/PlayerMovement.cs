using System.Collections;
using System.Collections.Generic;
using NOOD.ModifiableStats;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private ModifiableStats<float> _speed = new ModifiableStats<float>();
        private Vector3 _playerMove;
        private Rigidbody2D _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            GameInput.onPlayerPressMoveVector2 += GetPlayerMoveDir;
            _speed.SetInitValue(PlayerConfig._speed);
        }
        void Update()
        {
            if(GlobalConfig._isBlockInput == true) return;
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
            _rb.velocity = _playerMove * _speed.Value;
        }
    }
}
