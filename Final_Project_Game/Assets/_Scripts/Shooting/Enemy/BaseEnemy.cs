using System.Collections;
using System.Collections.Generic;
using Game;
using NOOD;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected Vector3 _targetPos;
    [SerializeField] protected bool _isTest;
    [SerializeField] protected float _moveSpeed = 4;
    [SerializeField] protected float _hp = 30;
    [SerializeField] protected ItemSO _reward; 
    [SerializeField] protected EnemyAnimation _enemyAnimation;
    [SerializeField] protected float _attackRate;
    [SerializeField] protected float _damage = 1;
    protected float _attackTime, _nextAttackTime;
    private Collider2D _myCollider;
    protected bool _isDead;
    protected bool _isAttacking;

    void Awake()
    {
        _myCollider = this.gameObject.GetComponent<Collider2D>();
        Init();
        NoodyCustomCode.CreateNewCoroutineLoop(() =>
        {
            FindPlayer();
            FindCrop();
        }, 0.2f);
    }

    private void Update()
    {
        _attackTime += Time.deltaTime;
        ChildUpdate();
    }

    protected virtual void ChildUpdate(){}

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
        ItemSpawnManager.instance.SpawnManyItem(this.transform.position, null, _reward, UnityEngine.Random.Range(0, 3));
        Destroy(this.gameObject, 2f);
    }
    protected virtual void DropReward()
    {

    }
    protected virtual void Attack()
    {
        if(PlayerManager.Instance.GetHealth() <= 0) 
        {
            
        }
        if(_attackTime >= _nextAttackTime && _isAttacking == false)
        {
            _enemyAnimation.PlayAttackAnimation();
            ChildAttack();
            _attackTime = Time.time;
            _nextAttackTime = Time.time + 1/_attackRate;
            _isAttacking = true;
            NoodyCustomCode.StartDelayFunction(() =>
            {
                _isAttacking = false;
            }, _enemyAnimation.GetAttackDuration());
        }
    }
    protected virtual void FindPlayer()
    {

    }
    protected virtual void FindCrop()
    {

    }
}
