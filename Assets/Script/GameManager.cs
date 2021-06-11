using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private List<Stick> m_Sticks;
    [SerializeField] private List<Ring> m_Rings;
    private Stick m_FirstSelectedStick;
    private Stick m_SecondSelectedStick;
    private Ring m_SelectedRing;
    private bool m_HasGrabbedRing;
    private Vector2 m_StoredRingPosition;

    [Header("Misc")]
    [SerializeField] private Transform m_StickBottom;
    private Vector3 m_MousePosition;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError(Instance.name + " Already exists!");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if(m_HasGrabbedRing)
        {
            HoldRing();
        }
    }

    private void ResetGame()
    {
        foreach (Stick stick in m_Sticks)
        {
            stick.ResetStack();
        }

        ResetSelected();
        SetRings();
    }

    private void SetRings()
    {
        for (int i = 0; i < m_Rings.Count; i++)
        {
            m_Sticks[0].AddRing(m_Rings[i], m_StickBottom.position);
        }
    }

    public void StickSelected(Stick stick)
    {
        if (m_FirstSelectedStick != null && stick == m_FirstSelectedStick && !m_HasGrabbedRing)
        {
            return;
        }

        SetStick(stick);

        if(m_FirstSelectedStick.CheckTopRing() == null)
        {
            ResetSelected();
            return;
        }
        else
        {
            if(m_FirstSelectedStick && m_SecondSelectedStick)
            {
                if(RingCanBeAdded())
                {
                    //Debug.Log("Allowed");

                    Ring ringOnTop = m_FirstSelectedStick.RemoveTopRing();
                    m_SecondSelectedStick.AddRing(ringOnTop, m_StickBottom.position);
                }

                ResetSelected();
                CheckForWin();
            }
        }
    }

    private void HoldRing()
    {
        float cameraDistance = Vector3.Dot(transform.position - m_Camera.transform.position, m_Camera.transform.forward);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cameraDistance;

        m_MousePosition = m_Camera.ScreenToWorldPoint(mousePos);

        Ring ringOnTop = m_FirstSelectedStick.CheckTopRing();
        ringOnTop.transform.position = m_MousePosition;
    }

    private void ResetGrabbedRing()
    {
        Ring grabbedRing = m_FirstSelectedStick.CheckTopRing();
        grabbedRing.transform.position = m_StoredRingPosition;
    }

    private void CheckForWin()
    {
        if(m_Sticks[2].RingsStack.Count >= 5)
        {
            Debug.Log("Game has been won!");
        }
    }

    private void SetStick(Stick stick)
    {
        if (m_FirstSelectedStick == null)
        {
            m_FirstSelectedStick = stick;

            m_HasGrabbedRing = true;
            m_StoredRingPosition = m_FirstSelectedStick.CheckTopRing().transform.position;
        }
        else
        {
            m_SecondSelectedStick = stick;

            m_HasGrabbedRing = false;
            ResetGrabbedRing();
        }
    }

    private void ResetSelected()
    {
        // Debug.Log("Stick Selection Reset");

        m_SecondSelectedStick = null;
        m_FirstSelectedStick = null;
    }

    private bool RingCanBeAdded()
    {
        return m_SecondSelectedStick.CheckTopRing() == null || m_FirstSelectedStick.CheckTopRing().Size.x < m_SecondSelectedStick.CheckTopRing().Size.x;
    }
}
