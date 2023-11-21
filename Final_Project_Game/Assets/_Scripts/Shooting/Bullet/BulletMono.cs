using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class BulletMono : MonoBehaviour
{
    #region Events
    public Action onDisable;
    #endregion

    BulletSO data;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private TrailRenderer _bulletTrail;
    private Rigidbody2D _myBody;

    void Awake()
    {
        _myBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Invoke(nameof(DeactivateSelf), 10f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.Damage(this.GetDamage());
            PlayBulletEffect();
            FeedbackManager.Instance.PlayPlayerBulletExplodeFB();
        }
        DeactivateSelf();
    }
    void OnDisable()
    {
        CancelInvoke(nameof(DeactivateSelf));
        onDisable?.Invoke();
    }
    private void DeactivateSelf()
    {
        _myBody.velocity = Vector3.zero;
        _bulletTrail.emitting = false;
        this.gameObject.SetActive(false);
    }
    public void ShowTrail()
    {
        StartCoroutine(ShowTrailCR());
    }
    private IEnumerator ShowTrailCR()
    {
        yield return null;
        _bulletTrail.emitting = true;
    }
    public float GetDamage()
    {
        return data._damage;
    }
    public void SetData(BulletSO data)
    {
        this.data = data;
        _sr.sprite = data._bulletSprite;
    }
    private void PlayBulletEffect()
    {
        GameObject bulletEffectHolderGO = GameObject.Find("BulletEffectHolder") ?? new GameObject("BulletEffectHolder");
        Transform bulletEffectHolder = bulletEffectHolderGO.transform;
        
        ParticleSystem effect = null;
        foreach(Transform child in bulletEffectHolder)
        {
            if(child.gameObject.activeInHierarchy == false)
            {
                effect = child.gameObject.GetComponent<ParticleSystem>();
            }
        }
        if(effect == null)
        {
            effect = Instantiate(data._bulletEffect, bulletEffectHolder);
        }
        effect.transform.position = this.transform.position;
        effect.Play();
    }
}
