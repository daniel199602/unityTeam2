using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomtest : MonoBehaviour
{
    public Mesh mesh;
    public float offset;
    public Material mat;
    public int matIndex;

    private void Start()
    {
        mesh = gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        mat = gameObject.GetComponent<SkinnedMeshRenderer>().materials[matIndex];



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(changShader());
        }
    }




    IEnumerator changShader()
    {
        offset = -3;
        mat.SetFloat("_offset", offset);
        yield return new WaitForSeconds(0.3f);
        offset = 5;
        mat.SetFloat("_offset", offset);
        yield return new WaitForSeconds(0.3f);
        offset = -3;
        mat.SetFloat("_offset", offset);
        yield return new WaitForSeconds(0.3f);
        offset = 5;
        mat.SetFloat("_offset", offset);
        yield return new WaitForSeconds(0.3f);
        offset = -3;
        mat.SetFloat("_offset", offset);
        yield return new WaitForSeconds(0.3f);
        offset = 5;
        mat.SetFloat("_offset", offset);
        yield return new WaitForSeconds(0.3f);
    }
}
