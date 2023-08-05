using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform m_LookPoint;
    public Vector3 m_Pos = new Vector3(-145.0f, 220.0f,-100.0f);

    private void Update()
    {
        transform.position = new Vector3(m_LookPoint.position.x,0f,m_LookPoint.position.z) + m_Pos;
        //transform.position = Vector3.Lerp(transform.position, m_LookPoint.position + m_Pos, 0.1f);
        transform.LookAt(m_LookPoint);
    }

}
