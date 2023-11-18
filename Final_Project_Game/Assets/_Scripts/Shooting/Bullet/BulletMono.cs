using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMono : MonoBehaviour
{
    BulletSO data;
    [SerializeField] private SpriteRenderer _sr;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.Damage(this.GetDamage());
            PlayBulletEffect();
            FeedbackManager.Instance.PlayPlayerBulletExplodeFB();
        }
        this.gameObject.SetActive(false);
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
