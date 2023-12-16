using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public Tilemap tilemap;
    public Vector2 moveArea = new Vector2(10, 10);

    private Vector3 randomDestination;
    private bool isMoving = false;

    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToDestination());
        }
    }

    void SetRandomDestination()
    {
        float randomX = Random.Range(-moveArea.x / 2, moveArea.x / 2);
        float randomY = Random.Range(-moveArea.y / 2, moveArea.y / 2);

        Vector3Int cellPosition = tilemap.WorldToCell(new Vector3(randomX, randomY, 0));
        randomDestination = tilemap.GetCellCenterWorld(cellPosition);
    }

    bool CanMoveTo(Vector3 targetPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<TilemapCollider2D>() != null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator MoveToDestination()
    {
        isMoving = true;

        // Kiểm tra xem vị trí mới có thể điều hướng không
        while (!CanMoveTo(randomDestination))
        {
            SetRandomDestination();
        }

        while (Vector2.Distance(transform.position, randomDestination) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, randomDestination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

        SetRandomDestination();
        isMoving = false;
    }
}

