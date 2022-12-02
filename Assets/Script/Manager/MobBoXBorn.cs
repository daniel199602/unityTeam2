using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBoXBorn : MonoBehaviour
{
    Collider m_Collider;
    public Transform spawnPoint;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    
    bool isInZone;
    public int GenerationTimes ;
    private void Start()
    {
        GenerationTimes = 0;
        m_Collider = GetComponent<Collider>();
        
    }
    void MobBord()
    {

        Vector3 startPos = spawnPoint.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
        Vector3 startPos1 = spawnPoint1.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
        Vector3 startPos2 = spawnPoint2.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
        Vector3 startPos3 = spawnPoint3.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
        int count = MobMain.Instance().pAliveObject.Count;
        if (count < 4)
        {
            //不知道為甚麼角色觸發後第二次重生偶爾會為NULL,所以加上if防當
            GameObject go = MobMain.Instance().oPool.LoadObjectFromPool(MobMain.Instance().pTestList);
            if (go != null)
            {
                go.SetActive(true);
                go.transform.position = startPos;
                MobMain.Instance().pAliveObject.Add(go);
            }

            GameObject go1 = MobMain.Instance().oPool.LoadObjectFromPool(MobMain.Instance().pTestList);
            if (go1 != null)
            {
                go1.SetActive(true);
                go1.transform.position = startPos1;
                MobMain.Instance().pAliveObject.Add(go1);
            }

            GameObject go2 = MobMain.Instance().oPool.LoadObjectFromPool(MobMain.Instance().pTestList);
            if (go2 != null)
            {
                go2.SetActive(true);
                go2.transform.position = startPos2;
                MobMain.Instance().pAliveObject.Add(go2);
            }

            GameObject go3 = MobMain.Instance().oPool.LoadObjectFromPool(MobMain.Instance().pTestList);
            if (go3 != null)
            {
                go3.SetActive(true);
                go3.transform.position = startPos3;
                MobMain.Instance().pAliveObject.Add(go3);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInZone = true;
            born();
            Debug.Log(isInZone);
            GenerationTimes = 0;
            Debug.Log(GenerationTimes);
            MobMain.Instance().MobBox = this;
            m_Collider.enabled = false;
        }

    }


    private float duration;

    
    void born()
    {
        int count = MobMain.Instance().pAliveObject.Count;
        Debug.Log($"{isInZone} {count} {GenerationTimes}");
        if (isInZone && count <= 0 && GenerationTimes < 2)
        {
            duration = 2;
            Invoke(nameof(MobBord), duration);

            GenerationTimes++;
            
            Debug.Log(GenerationTimes);
        }
    }
}

