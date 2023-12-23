using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    private CinemachineConfiner _confiner;
    private PolygonCollider2D _polygonCollider2D;

    void Awake()
    {
        _polygonCollider2D = this.GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        UpdateBounds();
    }

    public void UpdateBounds()
    {
        _confiner = FindFirstObjectByType<CinemachineConfiner>();

        _confiner.m_BoundingShape2D = _polygonCollider2D;
    }
}
