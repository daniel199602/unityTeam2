using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBoXBorn : MonoBehaviour
{
    Collider m_Collider;
    DoorOpen_MobLast doorOpen_MobLast;
    public Transform spawnPoint;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    GameObject door;
    bool isInZone;
    public int GenerationTimes ;
    int killMobLast = 0;
    public GameObject mob1,mob2;



    private void Start()
    {
        door = this.gameObject.transform.GetChild(0).gameObject;
        door.SetActive(false);
        GenerationTimes = 0;
        m_Collider = GetComponent<Collider>();
        doorOpen_MobLast = GetComponent<DoorOpen_MobLast>();
        killMobLast = 0;

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
                go.transform.position = startPos;
                go.SetActive(true);
                go.GetComponent<MubHpData>().Hp = 1000;//重生血量回滿
                MobMain.Instance().pAliveObject.Add(go);
            }

            GameObject go1 = MobMain.Instance().oPool.LoadObjectFromPool(MobMain.Instance().pTestList);
            if (go1 != null)
            {
                go1.transform.position = startPos1;
                go1.SetActive(true);
                go1.GetComponent<MubHpData>().Hp = 1000;//重生血量回滿
                MobMain.Instance().pAliveObject.Add(go1);
            }

            GameObject go2 = MobMain.Instance().oPool.LoadObjectFromPool(MobMain.Instance().pTestList);
            if (go2 != null)
            {
                go2.transform.position = startPos2;
                go2.SetActive(true);
                go2.GetComponent<MubHpData>().Hp = 1000;//重生血量回滿
                MobMain.Instance().pAliveObject.Add(go2);
            }

            GameObject go3 = MobMain.Instance().oPool.LoadObjectFromPool(MobMain.Instance().pTestList);
            if (go3 != null)
            {
                go3.transform.position = startPos3;
                go3.SetActive(true);
                go3.GetComponent<MubHpData>().Hp = 1000;//重生血量回滿
                MobMain.Instance().pAliveObject.Add(go3);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            door.SetActive(!door.activeSelf);
            isInZone = true;
            born();
            //Debug.Log(isInZone);
            GenerationTimes = 0;
            //Debug.Log(GenerationTimes);
            MobMain.Instance().MobBox = this;
            m_Collider.enabled = false;
            mob1.SetActive(true);
            mob2.SetActive(true);
        }

    }


    private float duration;

    
    void born()
    {
        int count = MobMain.Instance().pAliveObject.Count;
        //Debug.Log($"{isInZone} {count} {GenerationTimes}");
        killMobLast++;
        if (isInZone && count <= 0 && GenerationTimes < 1)
        {
            duration = 2;
            Invoke(nameof(MobBord), duration);

            GenerationTimes++;
            
            //Debug.Log(GenerationTimes);
        }
        //Debug.Log(killMobLast);
        
    }
    bool killAll = true;
    private void Update()
    {
        if(killMobLast>=3 && killAll==true)
        {
            for (int i = 0 ; i < 8;  i++)
            {
                Debug.Log(killMobLast);
                doorOpen_MobLast.killMob();
            }
            killAll = false;
        }
    }
}

