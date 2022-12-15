using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum DungeonState { inactive, generatingMain, generatingBranches, completed }

public class Random_room : MonoBehaviour
{
    [SerializeField] GameObject[] startPrefabs;
    [SerializeField] GameObject[] tilesPrefabs;
    [SerializeField] GameObject[] exitPrefabs;
    [SerializeField] GameObject[] blockedPredabs;
    [SerializeField] GameObject[] doorPredabs;
    
    //�ϩж������|(����)
    [Header("Debug options")]
    [SerializeField] bool useBoxColliders;


    [Header("Generation Limits")]
    [Range(0,1.0f)] [SerializeField] float constructionDelay = 0.3f;
    [Range(6,30)] [SerializeField] int mainLength = 7;
    [Range(0, 50)] [SerializeField] int branchLength = 3;
    [Range(0, 25)] [SerializeField] int noBranches = 10;
    [Range(0, 100)] [SerializeField] int doorPercent = 25;

    //�W���ثe�ж�
    [Header("Available at Runtime")]
    public List<Tile> generatedTiles = new List<Tile>();

    //HideInInspector����
    [HideInInspector] public DungeonState dungeonState = DungeonState.inactive;


    List<Connector> availableConnectors = new List<Connector>();
    Transform tilesForm, tilesTo,tileRoot;
    Transform container;

    List<int> FixedRandom = new List<int> { 0, 1, 2, 3 };

    void Start()
    {
        StartCoroutine(DungeonBuild());
    }

    IEnumerator DungeonBuild()
    {
        GameObject goContainer = new GameObject("Main Path");
        container = goContainer.transform;
        container.SetParent(transform);
        tileRoot = roomStart();
        tilesTo = tileRoot;
        dungeonState = DungeonState.generatingMain;
        while(generatedTiles.Count < mainLength )
        {
            yield return new WaitForSeconds(constructionDelay);
            tilesForm = tilesTo;
            if(generatedTiles.Count == mainLength-1)
            {
                //Exit�إߥX�f
                tilesTo = roomExit();
            }
            else if(generatedTiles.Count > mainLength -6)
            {
                tilesTo = tilesFixedRoom();
            }
            else
            {
                tilesTo = tilesRoom();
            }

            ConnectTile();
        }
        //�S���s�����q�D
        foreach (Connector connector in container.GetComponentsInChildren<Connector>())
        {
            if(!connector.isConnected)
            {
                if(availableConnectors.Contains(connector))
                {
                    availableConnectors.Add(connector);
                }
            }
        }
        //branching
        dungeonState = DungeonState.generatingBranches;
        for (int b=0 ; b<noBranches ; b++)
        {
            if(availableConnectors.Count>0)
            {
                goContainer = new GameObject("Branch " + (b + 1));
                container = goContainer.transform;
                container.SetParent(transform);
                int availIndex = Random.Range(0, availableConnectors.Count);
                tileRoot = availableConnectors[availIndex].transform.parent.parent;
                availableConnectors.RemoveAt(availIndex);
                tilesTo = tileRoot;
                for (int i = 0; i < branchLength - 1; i++)
                {
                    yield return new WaitForSeconds(constructionDelay);
                    tilesForm = tilesTo;
                    tilesTo = tilesRoom();
                    ConnectTile();
                }
            }
            else { break; }
        }
        BlockedPassages();
        dungeonState = DungeonState.completed;
        yield return null;
    }
    /// <summary>
    /// �ͦ���ê��(�צ�S�ͦ��ж������f�B)
    /// </summary>
    void BlockedPassages()
    {
       foreach ( Connector connector in transform.GetComponentsInChildren<Connector>())
        {
            if(!connector.isConnected)
            {
                Vector3 pos = connector.transform.position;
                int wallIndex = Random.Range(0, blockedPredabs.Length);
                GameObject goWall = Instantiate(blockedPredabs[wallIndex], pos, connector.transform.rotation, connector.transform) as GameObject;
            }
        }
    }

    /// <summary>
    /// ��Form���to���������k0����180
    /// </summary>
    private void ConnectTile()
    {
        Transform connectForm = GetRandomConnector(tilesForm);
        if (connectForm == null) { return; }
        Transform connectTo = GetRandomConnector(tilesTo);
        if (connectTo == null) { return; }
        connectTo.SetParent(connectForm);
        tilesTo.SetParent(connectTo);
        connectTo.localPosition = Vector3.zero;
        connectTo.localRotation = Quaternion.identity;
        connectTo.Rotate(0.0f, 180.0f, 0.0f);
        tilesTo.SetParent(container);
        connectTo.SetParent(tilesTo.Find("Connectors"));
        generatedTiles.Last().connector = connectForm.GetComponent<Connector>();
    }
    /// <summary>
    /// �H����@�өж��é��}�C
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    Transform GetRandomConnector(Transform tile)
    {
        if (tile == null) { return null; }
        List<Connector> connectorList = tile.GetComponentsInChildren<Connector>().ToList().FindAll(x => x.isConnected == false);
        if(connectorList.Count>0)
        {
            int connectorIndex = Random.Range(0, connectorList.Count);
            connectorList[connectorIndex].isConnected = true;
            return connectorList[connectorIndex].transform;
        }
        return null;
    }
    /// <summary>
    /// �ͦ��Ǫ��ж�
    /// </summary>
    /// <returns></returns>
    Transform tilesRoom()
    {
        int Index = Random.Range(0, tilesPrefabs.Length-1);
        GameObject goTile = Instantiate(tilesPrefabs[Index], Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = tilesPrefabs[Index].name;
        //�[��ͦ�tile
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tilesForm)].tile;
        generatedTiles.Add(new Tile(goTile.transform,origin));
        return goTile.transform;
    }
    /// <summary>
    /// �ͦ��T�w�ж�(��4���ܤ֦U1)
    /// </summary>
    /// <returns></returns>
    Transform tilesFixedRoom()
    {
        
        int Index = Random.Range(0, FixedRandom.Count);      
        int result = FixedRandom[Index];
        GameObject goTile = Instantiate(tilesPrefabs[result], Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = tilesPrefabs[result].name;
        //�[��ͦ�tile
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tilesForm)].tile;
        generatedTiles.Add(new Tile(goTile.transform, origin));
        FixedRandom.RemoveAt(Index);
        return goTile.transform;
    }

    /// <summary>
    /// �ͦ��X�f
    /// </summary>
    /// <returns></returns>
    Transform roomExit()
    {
        int Index = Random.Range(0, exitPrefabs.Length);
        GameObject goTile = Instantiate(exitPrefabs[Index], Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = "ExitRoom";
        //�[��ͦ�tile
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tilesForm)].tile;
        generatedTiles.Add(new Tile(goTile.transform, origin));
        return goTile.transform;
    }
    /// <summary>
    /// �H���a�����}�l�I
    /// </summary>
    /// <returns></returns>
    Transform roomStart()
    {
        int Index = Random.Range(0, startPrefabs.Length);
        GameObject goTile = Instantiate(startPrefabs[Index], Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = "SartRoom";
        //�[��ͦ�tile
        generatedTiles.Add(new Tile(goTile.transform, null));

        return goTile.transform;
    }


}
