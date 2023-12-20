using Game;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public Tilemap tilemap;
    public Vector2 moveArea = new Vector2(10, 10);

    private Vector3 randomDestination;
    private bool isMoving = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sr;
    private bool _run, _left, _right, _up, _down;
    private Vector3 _originalScale;
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
        _animator.SetBool("Run", _run);
        _animator.SetBool("Left", _left);
        _animator.SetBool("Right", _right);
        _animator.SetBool("Up", _up);
        _animator.SetBool("Down", _down);
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
        Vector2 moveDirection = (randomDestination - (Vector3)transform.position).normalized;
            SetAnim(moveDirection);
            transform.position = Vector2.MoveTowards(transform.position, randomDestination, moveSpeed * Time.deltaTime);
            yield return null;
        }
        SetAnim(Vector2.zero);
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

        SetRandomDestination();
        isMoving = false;
    }

    private void SetAnim(Vector2 position)
    {

        _up = false;
        _down = false;
        _left = false;
        _right = false;
        _run = false;
        if (position == Vector2.zero)
        {
            _run = false;
            return;
        }
        else
        {
            _run = true;
        }

        if (position.x < 0)
        {
            _left = true;
            _right = false;
        }
        else if (position.x > 0)
        {
            _left = false;
            _right = true;
        }
        else
        {
            if (position.y > 0)
            {
                _up = true;
            }
            else
            {
                _down = true;
            }
        }
    }



}

