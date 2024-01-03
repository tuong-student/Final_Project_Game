using System.Collections;
using System.Collections.Generic;
using NOOD;
using NOOD.ModifiableStats;
using NOOD.Sound;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private ModifiableStats<float> _speed = new ModifiableStats<float>();
        private Vector3 _playerMove;
        private Rigidbody2D _rb;
        private bool _isMove;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            GameInput.onPlayerPressMoveVector2 += GetPlayerMoveDir;
            _speed.SetInitValue(PlayerConfig._speed);

            float footStepTime;
            footStepTime = SoundManager.GetSoundLength(NOOD.Sound.SoundEnum.FootStep);
            NoodyCustomCode.StartNewCoroutineLoop(() =>
            {
                if(_isMove)
                    SoundManager.PlaySound(NOOD.Sound.SoundEnum.FootStep);
            }, footStepTime);
        }
        void Update()
        {
            _isMove = false;
            if(GlobalConfig.s_IsUiOpen == true) return;
            if (PlayerManager.Instance.GetHealth() <= 0) return;
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
            _playerMove.Normalize();
            _rb.velocity = _playerMove * _speed.Value;
            if(_rb.velocity != Vector2.zero)
            {
                _isMove = true;
            }
        }
    }
}
