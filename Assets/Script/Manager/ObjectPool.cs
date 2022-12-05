using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public class ObjectPoolData
    {
        public GameObject go;
        public bool bUsing;
    }

    private List<List<ObjectPoolData>> pObjectsTypes;

    private void Awake()
    {
        pObjectsTypes = new List<List<ObjectPoolData>>();
    }

    List<ObjectPoolData> OueryEmptyList()
    {
        List<ObjectPoolData> pList = new List<ObjectPoolData>();
        pObjectsTypes.Add(pList);
        return pList;
    }

    public List<ObjectPoolData> InitObjectPoolData(Object o, int iCount)
    {
        List<ObjectPoolData> pList = OueryEmptyList();
        for (int i = 0; i < iCount; i++)
        {
            GameObject go = Instantiate(o) as GameObject;
            ObjectPoolData data = new ObjectPoolData();
            data.bUsing = false;
            data.go = go;
            go.SetActive(false);
            go.transform.SetParent(this.transform);
            pList.Add(data);
        }
        return pList;
    }

    public void ClearObjectPoolList(List<ObjectPoolData> pList)
    {
        pList.Clear();
        pObjectsTypes.Remove(pList);
    }

    public GameObject LoadObjectFromPool(List<ObjectPoolData> pList)
    {
        int iCount = pList.Count;
        for (int i = 0; i < iCount; i++)
        {
            if (pList[i].bUsing == false)
            {
                pList[i].bUsing = true;
                return pList[i].go;
            }
        }
        return null;
    }

    public void UnLoadObjectToPool(List<ObjectPoolData> pList, GameObject go)
    {
        int iCount = pList.Count;
        for (int i = 0; i < iCount; i++)
        {
            if (pList[i].go == go)
            {
                go.SetActive(false);
                
                pList[i].bUsing = false;
                
                break;
            }
        }
    }
}
