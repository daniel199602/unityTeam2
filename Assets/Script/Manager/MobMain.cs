using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMain : MonoBehaviour
{
    private static MobMain mInstance;
    public static MobMain Instance() { return mInstance; }

    GameObject player;
    private bool mobHave;
    bool isInZone;

    public Transform spawnPoint;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public ObjectPool oPool = null;
    public List<ObjectPool.ObjectPoolData> pTestList = null;
    public GameObject npcObject;
    public List<GameObject> pAliveObject = new List<GameObject>();

    private void Awake()
    {
        mInstance = this;
    }
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        mobHave = true;

        oPool = gameObject.AddComponent<ObjectPool>();
        pTestList = oPool.InitObjectPoolData(npcObject, 12);
    }

    IEnumerator SpawnFloatingActor()
    {
        while (true)
        {
            Vector3 startPos = spawnPoint.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
            Vector3 startPos1 = spawnPoint1.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
            Vector3 startPos2 = spawnPoint2.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
            Vector3 startPos3 = spawnPoint3.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-2.0f, 2.0f));
            int count = pAliveObject.Count;
            if (count < 20)
            {
                //不知道為甚麼角色觸發後第二次重生偶爾會為NULL,所以加上if防當
                GameObject go = oPool.LoadObjectFromPool(pTestList);
                if (go != null)
                {
                    go.SetActive(true);
                    go.transform.position = startPos;
                    pAliveObject.Add(go);
                }

                GameObject go1 = oPool.LoadObjectFromPool(pTestList);
                if (go1 != null)
                {
                    go1.SetActive(true);
                    go1.transform.position = startPos1;
                    pAliveObject.Add(go1);
                }

                GameObject go2 = oPool.LoadObjectFromPool(pTestList);
                if (go2 != null)
                {
                    go2.SetActive(true);
                    go2.transform.position = startPos2;
                    pAliveObject.Add(go2);
                }

                GameObject go3 = oPool.LoadObjectFromPool(pTestList);
                if (go3 != null)
                {
                    go3.SetActive(true);
                    go3.transform.position = startPos3;
                    pAliveObject.Add(go3);
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInZone = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isInZone&& mobHave==true)
        {
            StartCoroutine(SpawnFloatingActor());
        }
    }
}
