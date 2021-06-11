using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private Stack<Ring> m_Rings;
    public Stack<Ring> RingsStack => m_Rings;
    public int RingsOnStick => m_Rings.Count;

    public void ResetStack()
    {
        m_Rings = new Stack<Ring>();
    }

    public void AddRing(Ring ring, Vector2 bottom)
    {
        ring.transform.position = new Vector2(transform.position.x, bottom.y + ring.Size.y * m_Rings.Count);
        m_Rings.Push(ring);
    }

    public Ring RemoveTopRing()
    {
        return m_Rings.Pop();
    }

    public Ring CheckTopRing()
    {
        if(m_Rings.Count >= 1)
        {
            return m_Rings.Peek();
        }
        else
        {
            return null;
        }
    }

    private void OnMouseDown()
    {
        GameManager.Instance.StickSelected(this);
    }
}
