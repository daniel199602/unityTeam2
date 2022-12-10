using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMain : MonoBehaviour
{
    private static MobMain mInstance;
    public static MobMain Instance() { return mInstance; }

    GameObject player;
    private bool mobHave;
    //bool isInZone;

    

    //public Transform spawnPoint;
    //public Transform spawnPoint1;
    //public Transform spawnPoint2;
    //public Transform spawnPoint3;

    public ObjectPool oPool = null;
    public List<ObjectPool.ObjectPoolData> pTestList = null;
    public GameObject npcObject;
    public List<GameObject> pAliveObject = new List<GameObject>();
    public MobBoXBorn MobBox ;
    public GameObject PlayerStart;


    private void Awake()
    {
        mInstance = this;
        PlayerStart.SetActive(false);
        
    }

    void Start()
    {
        PlayerStart.SetActive(!PlayerStart.activeSelf);
        GameObject player = GameObject.FindWithTag("Player");
        mobHave = true;
        oPool = gameObject.AddComponent<ObjectPool>();
        pTestList = oPool.InitObjectPoolData(npcObject, 8);

        
    }
}