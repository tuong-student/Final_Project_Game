using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed = 5f;
    [SerializeField] float pickUpDistance = 2.25f; //1.5^2
    [SerializeField] float ttl = 10f;

    private void Awake()
    {
        player = GameManager.instance.player.transform;
    }


    // Update is called once per frame
    void Update()
    {
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
            Destroy(gameObject);
        }
    }
}
