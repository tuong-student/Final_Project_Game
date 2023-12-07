using NOOD;
using UnityEngine;

[Tooltip("This is use to rotate Item to the mouse")]
public class AbstractItemHolder : MonoBehaviorInstance<AbstractItemHolder>
{
    [SerializeField] private AbstractItem _currentGun;

    #region Pooling Transform
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
    private Transform _damageTextTransformHolder;
    public Transform DamageTextTransformHolder
    {
        get
        {
            if(_damageTextTransformHolder == null)
            {
                _damageTextTransformHolder = new GameObject("DamageTextTransformHolder").transform;
            }
            return _damageTextTransformHolder;
        }
    }
    #endregion

    #region Unity Functions
    void Update()
    {
        if(_currentGun.IsHasData)
            RotateToMouse();
    }
    #endregion

    #region Main Functions
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
            _currentGun.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            _currentGun.transform.localScale = Vector3.one;
        }
    }
    #endregion
}
