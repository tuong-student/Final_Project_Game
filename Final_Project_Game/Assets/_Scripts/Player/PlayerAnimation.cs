using UnityEngine;

namespace Game
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _sr;
        private bool _run, _left, _right, _up, _down;
        private Vector3 _originalScale;

        void Awake()
        {
            GameInput.onPlayerPressMoveVector2 += SetAnim;
        }
        void Start()
        {
            _originalScale = this.transform.parent.transform.localScale;
        }
        void Update()
        {
            if(GlobalConfig._isBlockInput == true) return;
            _animator.SetBool("Run", _run);
            _animator.SetBool("Left", _left);
            _animator.SetBool("Right", _right);
            _animator.SetBool("Up", _up);
            _animator.SetBool("Down", _down); 
        }

        private void SetAnim(Vector2 playerInput)
        {
            _up = false;
            _down = false;
            _left= false;
            _right = false;
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
                _left = true;
                _right = false;
            }
            else if(playerInput.x > 0)
            {
                _left = false;
                _right = true;
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
    }
}
