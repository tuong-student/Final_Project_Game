using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _anim;

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
    }
}
