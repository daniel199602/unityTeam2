using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private float gravity = -9.8f;
    private float dropRayDistance = 2f;
    private float dropRayRescueDistance = 15f;
    public LayerMask hitMask;

    Vector3 velocity = Vector3.zero;
    Vector3 deltaMove = Vector3.zero;
    //private CharacterController cc;

    // Use this for initialization
    void Start()
    {
        //cc = GetComponent<CharacterController>();
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
        Gizmos.DrawRay(transform.position, Vector3.down * dropRayDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up * dropRayRescueDistance, Vector3.down * dropRayRescueDistance);
    }

    /// <summary>
    /// �g�u����
    /// </summary>
    void UpdateRaycast()
    {
        Ray r = new Ray(transform.position, Vector3.down);//�a�������g�u
        
        if (Physics.Raycast(r, dropRayDistance, hitMask))
        {
            gravity = 0;
            velocity.y = 0;
        }
        else
        {
            Ray rRescue = new Ray(transform.position + Vector3.up * dropRayRescueDistance, Vector3.down);//�O�I�����g�u
            if (Physics.Raycast(rRescue, dropRayRescueDistance, hitMask))
            {
                gravity = 9.8f;
                velocity.y = 9.8f;
            }
            else
            {
                gravity = -9.8f;
            }
        }
    }
    
    /// <summary>
    /// �C��p��X��e�����O�[�t�צ첾�V�q
    /// </summary>
    /// <param name="deltatime"></param>
    void UpdateGravity(float deltatime)
    {
        velocity.y += gravity * deltatime;
        
        //�̤j�첾�V�q
        if (velocity.y < -30.0f)
        {
            velocity.y = -30.0f;
        }
    }

    /// <summary>
    /// �C����ܪ��a��e�y�Ц�m
    /// </summary>
    /// <param name="deltatime"></param>
    void UpdateMovement(float deltatime)
    {
        deltaMove = velocity * deltatime;
        transform.Translate(deltaMove, Space.World);
    }


}