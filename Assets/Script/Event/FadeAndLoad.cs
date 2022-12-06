using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAndLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangScene()
    {
        SceneManager.LoadScene("boss");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
