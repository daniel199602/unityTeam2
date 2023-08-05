using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Connector: MonoBehaviour
{
    public Vector2 size = Vector2.one * 4.0f;
    public bool isConnected;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 offset = transform.position + transform.up;
        Gizmos.DrawLine(offset, offset+transform.forward*30.0f);
    }

    
}
