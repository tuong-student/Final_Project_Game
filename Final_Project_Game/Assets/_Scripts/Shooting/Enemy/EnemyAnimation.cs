using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _sr;
    private float _attackDuration = 0;

    public float GetAttackDuration()
    {
        return _attackDuration;
    }
    public void PlayHurtAnimation()
    {
        _anim.SetTrigger("Hit");
    }
    public void PlayDeadAnimation()
    {
        _anim.SetTrigger("Death");
        NoodyCustomCode.StartUpdater(() =>
        {
            Debug.Log("Updater");
            Color color = _sr.color;
            if(color.a >= 0)
            {
                color.a -= Time.deltaTime;
                _sr.color = color;
                return false;
            }
            else
            {
                Debug.Log("StopUpdater");
                return true;
            }
        });
    }
    public void PlayAttackAnimation()
    {
        _anim.SetTrigger("Attack");
        if(_attackDuration == 0)
            _attackDuration = _anim.GetCurrentAnimatorStateInfo(0).length;
    }
}
