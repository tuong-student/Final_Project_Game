using System.Collections;
using System.Collections.Generic;
using NOOD;
using NOOD.Sound;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] float spread = 0.7f;
    [SerializeField] ItemSO item;
    [SerializeField] int itemCountInOneDrop = 0;
    [SerializeField] private int dropCount = 1;
    [SerializeField] int _maxHealth = 8;
    [SerializeField] ResourceNodeType nodeType;
    [SerializeField] private CircleSlider _circleSlider;

    private int _currentHealth = 8;
    
    void Awake()
    {
        if(_circleSlider != null)
            _circleSlider.gameObject.SetActive(false);
    }

    public override void Hit()
    {
        _currentHealth--;
        if (nodeType == ResourceNodeType.Ore)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.StoneHit, GameManager.instance.gameStatus.soundVolume);
        else if (nodeType == ResourceNodeType.Tree)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.WoodHit, GameManager.instance.gameStatus.soundVolume);
        if (_currentHealth == 0)
        {
            if (nodeType == ResourceNodeType.Ore)
            {
                itemCountInOneDrop = 1;
                SoundManager.PlaySound(NOOD.Sound.SoundEnum.StoneFall, GameManager.instance.gameStatus.soundVolume);
            }
            else if (nodeType == ResourceNodeType.Tree)
                SoundManager.PlaySound(NOOD.Sound.SoundEnum.WoodFall, GameManager.instance.gameStatus.soundVolume);
            while (dropCount > 0)
            {
                dropCount--;
                itemCountInOneDrop = UnityEngine.Random.Range(1, 3);
                Vector3 position = transform.position;
                position.x += UnityEngine.Random.value - spread / 2;
                position.y += UnityEngine.Random.value - spread / 2;
                //GameObject go = Instantiate(pickUpDrop);
                ItemSpawnManager.instance.SpawnItem(position, null, item, itemCountInOneDrop);
            }
            Destroy(gameObject);
        }
        UpdateSlider();
    }

    public override bool CanBeHit(List<ResourceNodeType> canbehit)
    {
        return canbehit.Contains(nodeType);
    }

    private void UpdateSlider()
    {
        NoodyCustomCode.StopCoroutineLoop("NodeSlider");
        NoodyCustomCode.StopDelayFunction("NodeSliderDelay");
        if(_circleSlider.CanvasGroup != null)
            _circleSlider.CanvasGroup.alpha = 1;
        if(_circleSlider != null)
        {
            _circleSlider.gameObject.SetActive(true);
            _circleSlider.Init(_currentHealth, _maxHealth);
            NoodyCustomCode.StartDelayFunction(() =>
            {
                NoodyCustomCode.StartNewCoroutineLoop(() =>
                {
                    if (_circleSlider.CanvasGroup != null && _circleSlider.CanvasGroup.alpha > 0)
                    {
                        _circleSlider.CanvasGroup.alpha -= Time.deltaTime;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }, "NodeSlider");
            }, "NodeSliderDelay", 2f);
        }
    }
}

