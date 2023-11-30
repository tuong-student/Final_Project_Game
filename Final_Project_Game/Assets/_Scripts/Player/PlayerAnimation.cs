using NOOD;
using Unity.Collections;
using UnityEngine;

namespace Game
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _sr;
        private bool _run, _left, _right, _up, _down;
        private Vector3 _originalScale;

        #region Unity Functions
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
            if (PlayerManager.Instance.GetHealth() <= 0) return;
            _animator.SetBool("Run", _run);
            _animator.SetBool("Left", _left);
            _animator.SetBool("Right", _right);
            _animator.SetBool("Up", _up);
            _animator.SetBool("Down", _down); 
        }
        #endregion

        #region Animation
        private void SetAnim(Vector2 playerInput)
        {
            _up = false;
            _down = false;
            _left= false;
            _right = false;
            _run = false;
            if (GlobalConfig._isBlockInput == true) return;
            if (PlayerManager.Instance.GetHealth() <= 0) return;
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
        public void PlayDeadAnimation()
        {
            NoodyCustomCode.StartUpdater(() =>
            {
                Color color = _sr.color;
                if(color.a > 0)
                {
                    color.a -= Time.deltaTime;
                    _sr.color = color;
                    return false;
                }
                else
                {
                    return true;
                }
            });
            FeedbackManager.Instance.PlayPlayerDeadFB();
        }
        #endregion
    }
}
