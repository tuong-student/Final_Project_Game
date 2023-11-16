using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 5f;
    [SerializeField] float pickUpDistance = 2.25f; //1.5^2
    [SerializeField] float ttl = 10f;

    public Item item;
    public int count = 1;
    private void Start()
    {
        player = GameManager.instance.GetTransform();
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }

    // Update is called once per frame
    void Update()
    {
        ttl -= Time.deltaTime;
        if (ttl < 0) Destroy(gameObject);
        float distance = (transform.position- player.position).sqrMagnitude;
        if (distance > pickUpDistance )
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
            );
        if(distance < 0.1f)
        {
            //Should be move into specified controller rather than being checked here.
            if (PlayerManager.instance.inventoryContainer != null)
            {
                Debug.Log("item_____" + item.icon + "______" + item.Name);
                PlayerManager.instance.inventoryContainer.Add(item, count);
            }
            else Debug.Log("No inventory container attached to the game manager");
            Destroy(gameObject);
        }
    }
}
