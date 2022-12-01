using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private float gravity = -9.8f;
    [SerializeField] private float dropRayDistance = 2.5f;
    public LayerMask hitMask;

    Vector3 velocity = Vector3.zero;
    Vector3 deltaMove = Vector3.zero;
    private CharacterController cc;

    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
        velocity.y = 0;
    }

    void LateUpdate()
    {
        float deltatime = Time.deltaTime;
        UpdateRaycast();
        UpdateGravity(deltatime);
        UpdateMovement(deltatime);
        velocity = deltaMove / deltatime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down*dropRayDistance);
        Gizmos.DrawRay(transform.position + Vector3.forward*0.1f, Vector3.down*dropRayDistance);
    }

    void UpdateRaycast()
    {
        Ray r = new Ray(transform.position, Vector3.down);
        Ray r2 = new Ray(transform.position + Vector3.forward*0.1f, Vector3.down);
        if (Physics.Raycast(r, dropRayDistance, hitMask)|| Physics.Raycast(r2, dropRayDistance, hitMask))
        {
            gravity = 0;
            velocity.y = 0;
        }
        else
        {
            gravity = -9.8f;
        }
    }

    void UpdateGravity(float deltatime)
    {
        velocity.y += gravity * deltatime;
        if (velocity.y < -50.0f)
        {
            velocity.y = -50.0f;
        }
    }

    void UpdateMovement(float deltatime)
    {
        deltaMove = velocity * deltatime*1.1f;
        transform.Translate(deltaMove, Space.World);
    }


}