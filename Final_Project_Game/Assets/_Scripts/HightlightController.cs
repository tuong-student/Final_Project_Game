using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HightlightController : MonoBehaviour
{
    [SerializeField] GameObject highlighter;
    private GameObject currentTarget;
    public void Hightlight(GameObject target)
    {
        if (currentTarget == target)
        {
            return;
        }
        Vector3 position = target.transform.position;
        Hightlight(position);
    }
    public void Hightlight(Vector3 position)
    {
        highlighter.SetActive(true);
        highlighter.transform.position = position;
    }

    public void Hide()
    {
        currentTarget = null;
        highlighter.SetActive(false);
    }
}
