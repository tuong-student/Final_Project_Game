using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        private Vector3 _playerMovement;

        void Awake()
        {
            GameInput.Init();
            GameInput.onPlayerPressMoveVector2 += GetPlayerMoveDir;
        }
        void Update()
        {
            Move();
        }

        private void GetPlayerMoveDir(Vector2 playerInput)
        {
             _playerMovement = playerInput.ToVector3XZ();
        }
        private void Move()
        {
            this.transform.position += _playerMovement * _speed * Time.deltaTime;
        }
    }
}
