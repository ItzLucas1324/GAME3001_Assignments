using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentObject : MonoBehaviour
{
    [SerializeField] Transform m_target;

    public Transform Target
    {
        get { return m_target; }
        set { m_target = value; }
    }

    public void Start()
    {
        if (m_target == null)
        {
            Debug.Log("Target not assigned");
        }
    }
}