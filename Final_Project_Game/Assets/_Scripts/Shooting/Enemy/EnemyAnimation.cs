using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _anim;
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
    }
    public void PlayAttackAnimation()
    {
        _anim.SetTrigger("Attack");
        if(_attackDuration == 0)
            _attackDuration = _anim.GetCurrentAnimatorStateInfo(0).length;
    }
}
