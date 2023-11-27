using UnityEngine;

namespace Game
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _sr;
        private bool _run, _side, _up, _down, _hold;
        private Vector3 _originalScale;

        void Awake()
        {
            // GameInput.onPlayerPressMoveVector2 += SetAnim;
        }
        void Start()
        {
            // _originalScale = this.transform.parent.transform.localScale;
        }
        void Update()
        {
            if(GlobalConfig._isBlockInput == true) return;
            // _animator.SetBool("Run", _run);
            // _animator.SetBool("Side", _side);
            // _animator.SetBool("Up", _up);
            // _animator.SetBool("Down", _down); 
            // _animator.SetBool("Hold", _hold);
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

        public void SetHold(bool value)
        {
            _hold = value;
        }

        private void FlipX (bool value)
        {
            if(value == true)
            {
                Vector3 temp = this.transform.parent.transform.localScale;
                temp.x *= -1;
                this.transform.parent.transform.localScale = temp;
            }
            else
            {
                this.transform.parent.transform.localScale = _originalScale;
            }
        }
    }
}
