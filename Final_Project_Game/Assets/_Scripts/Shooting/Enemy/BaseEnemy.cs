using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected Vector3 _targetPos;
    [SerializeField] protected bool _isTest;
    [SerializeField] protected float _moveSpeed = 4;
    [SerializeField] protected float _hp = 30;
    [SerializeField] protected object _reward; // No define reward at current
    [SerializeField] protected EnemyAnimation _enemyAnimation;
    [SerializeField] protected float _attackRate;
    protected float _attackTime, _nextAttackTime;
    private Collider2D _myCollider;
    protected bool _isDead;

    void Awake()
    {
        _myCollider = this.gameObject.GetComponent<Collider2D>();
        Init();
    }

    protected virtual void Update()
    {
        _attackTime += Time.deltaTime;
    }


    protected void Init()
    {
        _myCollider.enabled = true;
    }

    protected abstract void Move();
    protected abstract void ChildAttack();
    public virtual void Damage(float damage)
    {
        _enemyAnimation.PlayHurtAnimation();
        _hp -= damage;
        if(_hp <= 0 && _isDead == false)
        {
            Dead();
        }
    }
    protected virtual void Dead()
    {
        _isDead = true;
        _enemyAnimation.PlayDeadAnimation();
        _myCollider.enabled = false;
        Destroy(this.gameObject, 1f);
    }
    protected virtual void DropReward()
    {

    }
    protected virtual void Attack()
    {

        if(_attackTime >= _nextAttackTime)
        {
            _enemyAnimation.PlayAttackAnimation();
            ChildAttack();
            _attackTime = Time.time;
            _nextAttackTime = Time.time + 1/_attackRate;
        }
    }
    protected virtual void FindPlayer()
    {

    }
    protected virtual void FindCrop()
    {

    }
}
