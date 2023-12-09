using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using NOOD;
using NOOD.Sound;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BulletMono : MonoBehaviour
{
    BulletSO data;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private TextMeshPro _damageTextNormal;

    #region Unity Functions
    void OnEnable()
    {
        Invoke(nameof(DeactivateSelf), 10f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.Damage(this.GetDamage());
            FeedbackManager.Instance.PlayPlayerShootFB();
            SpawnDamageText(this.GetDamage());
        }
        PlayBulletEffect();
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.BulletExplode);
        Invoke(nameof(DeactivateSelf), 0.1f);
    }
    void OnDestroy()
    {
        NoodyCustomCode.StopAllUpdaterWithName("BulletUpdater");
    }
    void OnDisable()
    {
        CancelInvoke(nameof(DeactivateSelf));
    }
    #endregion

    #region Active, deactivate
    private void DeactivateSelf()
    {
        this.gameObject.SetActive(false);
    }
    #endregion

    #region Get Set
    public float GetDamage()
    {
        return data._damage;
    }
    public void SetData(BulletSO data)
    {
        this.data = data;
        _sr.sprite = data._bulletSprite;
    }
    #endregion

    #region Perform functions
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
                break;
            }
        }
        if(effect == null)
        {
            effect = Instantiate(data._bulletEffect, bulletEffectHolder);
        }
        effect.transform.position = this.transform.position;
        effect.Play();
    }
    private void SpawnDamageText(float damage)
    {
        Transform damageTextHolder = AbstractItemHolder.Instance.DamageTextTransformHolder;
        TextMeshPro damageText = null;
        foreach(Transform child in damageTextHolder)
        {
            if(child.gameObject.activeInHierarchy == false)
            {
                damageText = child.GetComponent<TextMeshPro>();
                break;
            }
        }
        if(damageText == null)
        {
            damageText = Instantiate(_damageTextNormal, damageTextHolder);
        }
        damageText.transform.position = this.transform.position;
        damageText.text = damage.ToString("0");
        damageText.GetComponent<MMF_Player>().PlayFeedbacks();
        damageText.transform.DOMoveY(this.transform.position.y + 1, 1f);
    }
    #endregion
}
