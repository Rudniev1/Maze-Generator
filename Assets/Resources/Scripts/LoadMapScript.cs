using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LoadMapScript : MonoBehaviour {

    public NavMeshSurface surfaces;

    public Text files;
    public Text algoritmMovesInfo;
    string[] fileArray;
    string fileName;
    string filePath = "C://users/" + Environment.UserName + "/documents/Maze Generator/GameData/";

    public int size;
    Vector3 startPos;
    public GameObject floorTile;
    public GameObject obstacleTile;
    public GameObject Player;
    private Transform wholeBoard;

    public GameObject PathMap;
    GameObject PathMapTemp;
   public  GameObject target, algorigm1;
    GameObject targetTemp, algoritm1Temp;

    float algoritmMoves = 0;
    AIDestinationSetter destinationTarget;
    bool reachedPath = false;
   
    

    // Use this for initialization
    void Start () {
        wholeBoard = new GameObject("Board").transform;
        fileName = PlayerPrefs.GetString("fileName");
        readSizeFromFile();
        BoardSetup();
        generateObstaclesFromFile();
        Instantiate(Player, startPos, Quaternion.identity);

        PathMapTemp = Instantiate(PathMap);
        
        targetTemp = Instantiate(target, GameObject.Find("Meta(Clone)").transform.position, Quaternion.identity);
        algoritm1Temp = Instantiate(algorigm1,startPos, Quaternion.identity);
        destinationTarget = algoritm1Temp.GetComponent<AIDestinationSetter>();
        destinationTarget.target = targetTemp.transform;
        //Time.fixedDeltaTime
      //  path = algoritm1Temp.GetComponent<AIPath>();
    }
	
	// Update is called once per frame
	void Update () {

        //   pathProcessor = PathMap.GetComponent<PathProcessor>();
        //    path1 = PathMapTemp.GetComponent<Pathfinding.Path>();

        algoritmMoves += Time.deltaTime;

      //  Debug.Log(algoritmMoves);
        if (algoritm1Temp.GetComponent<AILerp>().reachedEndOfPath && reachedPath==false)
        {
            Debug.Log(algoritmMoves);
            PlayerPrefs.SetFloat("StepsToCount", algoritmMoves);
            reachedPath = true;
        }
        
        
    }



    void BoardSetup()
    {
        for (int x = 0; x < size + 2; x++)
        {
            for (int y = 0; y < size + 2; y++)
            {
                if (x == 0 || x == size + 1 || y == 0 || y == size + 1)
                {
                    GameObject instance = Instantiate(obstacleTile, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(wholeBoard);
                }
                GameObject instanceFloor = Instantiate(floorTile, new Vector3(x, y, 0f), Quaternion.identity);
                instanceFloor.transform.SetParent(wholeBoard);

            }
        }
    }

    void readSizeFromFile()
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(filePath + fileName + ".data", Encoding.Default);
               line = theReader.ReadLine();
            string[] entries = line.Split(';');
               size = int.Parse(entries[0]);          
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void generateObstaclesFromFile()
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(filePath + fileName + ".data", Encoding.Default);

            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();
        
                    if (line != null)
                    {
                        string[] entries = line.Split(';');

                        if (entries.Length > 0)
                        {
                            for (int i = 1; i < entries.Length; i++)
                            {

                                string[] entries1 = entries[i].Split(',');
                                generateGameObject(entries1[0], new Vector3(float.Parse(entries1[1]), float.Parse(entries1[2]), 0f));
                            }
                        }

                    }
                }
                while (line != null);
                theReader.Close();
            }
        }

        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }

    void generateGameObject(string name, Vector3 position)
    {
        GameObject instance = Instantiate(Resources.Load("Prefabs/" + name),position,Quaternion.identity) as GameObject;       
        instance.transform.SetParent(wholeBoard);
        if (name == "Start")
        {
            startPos = instance.transform.position;
        }

    }
}
