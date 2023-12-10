using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private BaseEnemy _baseEnemy;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Light2D _light2D;
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
            Color color = _sr.color;
            float intensity = _light2D.intensity;
            if(color.a >= 0)
            {
                intensity -= Time.deltaTime;
                color.a -= Time.deltaTime;
                _sr.color = color;
                _light2D.intensity = intensity;
                return false;
            }
            else
            {
                _baseEnemy.gameObject.SetActive(false);
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
