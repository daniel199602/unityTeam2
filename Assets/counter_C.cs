using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter_C : MonoBehaviour
{
    int count;
    // Start is called before the first frame update
    void Start()
    {
        count = 5;
        StartCoroutine(Counter());
    }

    // Update is called once per frame
    void Update()
    {
        if (count<=0)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator Counter()
    {
        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }              
    }
}