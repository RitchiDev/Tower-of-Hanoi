using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private Vector2 m_RingSize;
    public Vector2 Size => m_RingSize;

    private void Awake()
    {
        m_RingSize = GetComponent<Collider2D>().bounds.size;
    }
}
