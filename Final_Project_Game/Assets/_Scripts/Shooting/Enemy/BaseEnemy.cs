using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected float _moveSpeed = 4;
    protected float _hp;
    protected Vector3 _targetPos;
    [SerializeField] protected Weapon _currentWeapon;
    [SerializeField] protected object _reward; // No define reward at current
    [SerializeField] protected EnemyAnimation _enemyAnimation;

    protected abstract void FindTarget();
    protected abstract void Move();
    protected abstract void PlayDeadEffect();
    protected virtual void DropReward()
    {

    }
    protected virtual void Attack()
    {

    }
    protected virtual void FindPlayer()
    {

    }
    protected virtual void FindCrop()
    {

    }
}
