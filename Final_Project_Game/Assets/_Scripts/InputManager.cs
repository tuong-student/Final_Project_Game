using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector2 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;

    public Vector2 GetSelectMapPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.gameObject.layer == placementLayerMask)
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
