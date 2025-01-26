using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentObject : MonoBehaviour
{
    [SerializeField] Transform m_target;
    [SerializeField] Transform m_hazard;

    public Transform Target
    {
        get { return m_target; }
        set { m_target = value; }
    }

    public Transform Hazard
    {
        get { return m_hazard; }
        set { m_hazard = value; }
    }

    public void Start()
    {
        if (m_target == null)
        {
            Debug.Log("Target not assigned");
        }

        if (m_hazard == null)
        {
            Debug.Log("Hazard not assigned");
        }
    }
}