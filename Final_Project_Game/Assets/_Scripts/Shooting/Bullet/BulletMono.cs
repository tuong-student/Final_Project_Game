using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMono : MonoBehaviour
{
    BulletSO data;
    [SerializeField] private SpriteRenderer _sr;

    public float GetDamage()
    {
        return data._damage;
    }
    public void SetData(BulletSO data)
    {
        this.data = data;
        _sr.sprite = data._bulletSprite;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.Damage(GetDamage());
        }
        Destroy(this.gameObject);
    }
}
