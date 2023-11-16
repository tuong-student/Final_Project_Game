using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Gun _currentGun;
    private Transform _bulletPoolingHolder;
    public Transform BulletTransformHolder  
    {
        get
        {
            if(_bulletPoolingHolder == null)
            {
                _bulletPoolingHolder = new GameObject("BulletPoolingHolder").transform;
            }
            return _bulletPoolingHolder;
        }
    }

    void Update()
    {
        RotateToMouse();
        if(Input.GetMouseButton(0))
        {
            _currentGun.Shoot();
        }
    }

    private void RotateToMouse()
    {
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosWorld.z = this.transform.position.z;
        Vector3 direction = (mousePosWorld - this.transform.position).normalized;

        this.transform.right = direction;
        ScaleWhenBackWard();
    }
    private void ScaleWhenBackWard()
    {
        if(this.transform.localEulerAngles.z < 270 && this.transform.localEulerAngles.z > 155)
        {
            this.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            this.transform.localScale = Vector3.one;
        }
    }
}
