using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDecor : MonoBehaviour
{
    //生成隨機裝飾品
    [SerializeField] GameObject[] decorPrefabs;
    Random_room myGenerator;
    bool isCompleted;
    public MobBoXBorn aa;
    void Awake()
    {
        //抓出Main_room物件底下的Script
        myGenerator = GameObject.Find("Main_room").GetComponent<Random_room>();
    }

    
    void Update()
    {
        //如果地成生成已完成就執行
        if(!isCompleted && myGenerator.dungeonState == DungeonState.completed)
        {
            isCompleted = true;
            int decorIndex = Random.Range(0, decorPrefabs.Length);
            GameObject goDecor = Instantiate(decorPrefabs[decorIndex], transform.position, Quaternion.identity*transform.rotation, transform) as GameObject;
            goDecor.name = decorPrefabs[decorIndex].name;
        }
    }
    
}
