using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _sr;
        private bool _run, _side, _up, _down;

        void Awake()
        {
            GameInput.onPlayerPressMoveVector2 += SetAnim;
        }
        void Start()
        {
            
        }
        void Update()
        {
            _animator.SetBool("Run", _run);
            _animator.SetBool("Side", _side);
            _animator.SetBool("Up", _up);
            _animator.SetBool("Down", _down); 
        }

        private void SetAnim(Vector2 playerInput)
        {
            _side = false;
            _up = false;
            _down = false;
            FlipX(false);
            if(playerInput == Vector2.zero) 
            {
                _run = false;
                return;
            }
            else
            {
                _run = true;
            }

            if(playerInput.x < 0)
            {
                FlipX(true);
            }
            if(playerInput.x != 0)
            {
                _side = true;
            }
            else
            {
                if(playerInput.y > 0)
                {
                    _up = true;
                }
                else
                {
                    _down = true;
                }
            }
        }

        private void FlipX (bool value)
        {
            _sr.flipX = value;
        }
    }
}
