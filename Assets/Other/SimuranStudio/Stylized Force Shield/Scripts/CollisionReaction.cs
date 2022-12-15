using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Provides material with nice reaction on particle hits
public class CollisionReaction : MonoBehaviour
{
    //Connect your collision reaction material here. Authors provide ripple for that
    public GameObject ripplesVFX;

    private Material mat;

    private void OnCollisionEnter(Collision collision)
    {
        //Only works for collisions with objects tagged as Bullet. Can be changed here.
        if (collision.gameObject.tag == "Bullet")
        {
            //creating a copy of ripple material
            var ripples = Instantiate(ripplesVFX, transform) as GameObject;
            var psr = ripples.GetComponent<ParticleSystemRenderer>();
            mat = psr.material;
            //setting point of collision as center of ripple material
            mat.SetVector("_SphereCenter", collision.GetContact(0).point);
            //destruction of previously created copy
            Destroy(ripples, ripples.GetComponent<ParticleSystem>().startLifetime);
        }
    }
}