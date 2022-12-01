using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDecor : MonoBehaviour
{
    //�ͦ��H���˹��~
    [SerializeField] GameObject[] decorPrefabs;
    Random_room myGenerator;
    bool isCompleted;

    void Awake()
    {
        //��XMain_room���󩳤U��Script
        myGenerator = GameObject.Find("Main_room").GetComponent<Random_room>();
    }

    
    void Update()
    {
        //�p�G�a���ͦ��w�����N����
        if(!isCompleted && myGenerator.dungeonState == DungeonState.completed)
        {
            isCompleted = true;
            int decorIndex = Random.Range(0, decorPrefabs.Length);
            GameObject goDecor = Instantiate(decorPrefabs[decorIndex], transform.position, Quaternion.identity*transform.rotation, transform) as GameObject;
            goDecor.name = decorPrefabs[decorIndex].name;
        }
    }
    
}
