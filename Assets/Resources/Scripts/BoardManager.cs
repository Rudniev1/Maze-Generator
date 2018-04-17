using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.IO;

public class BoardManager : MonoBehaviour {

    public int size;
    public int obstaclesCount = 0;

    public GameObject start, meta;
    public GameObject floorTile;
    public GameObject[] obstacleTiles;

    public Collider2D heCollide,toCollide;

    Transform boardHolder;
    Transform wholeBoard;
    protected List<Vector3> gridPositions = new List<Vector3>();
    protected List<Vector3> newGridPositions = new List<Vector3>();
    protected List<string> obstacleName = new List<string>();

    protected Vector3[,] gridPositions1;
    Vector3 posStart, posExit;
    string fileName = "tempBoardUniqueTempNeverKnownName.data";
    string filePath = "C://users/" + Environment.UserName + "/documents/Maze Generator/GameData/";

    int randomIndex;
    Vector3 randomPosition1;

    // Use this for initialization
    void Start()
    {
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        size = PlayerPrefs.GetInt("size");
        gridPositions1 = new Vector3[size,size];
        obstaclesCount = PlayerPrefs.GetInt("obstacles");
        wholeBoard = new GameObject("WholeBoard").transform;
        BoardSetup();
        InitialiseList();
        layoutObjectRandom(obstacleTiles, obstaclesCount);
        layoutObjectRandom(new GameObject[] { start }, 1);
        layoutObjectRandom(new GameObject[] { meta }, 1);
        saveToFile();

    }

    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < size; x++)
        {
            for (int y = 1; y < size; y++)
            {
                newGridPositions.Add(new Vector3(x, y, 0f));
                gridPositions1[x, y] = new Vector3(x, y, 0f);
            }
        }    
    }

    void BoardSetup()
    {
      //  boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < size+2; x++)
        {
            for (int y = 0; y < size+2; y++)
            {
                if (x == 0 || x == size + 1 || y == 0 || y == size + 1)
                {
                    GameObject instance = Instantiate(obstacleTiles[0], new Vector3 (x,y,0f),Quaternion.identity) as GameObject;
                    instance.transform.SetParent(wholeBoard);               
                }              
                GameObject instanceFloor = Instantiate(floorTile, new Vector3(x, y, 0f), Quaternion.identity);
                instanceFloor.transform.SetParent(wholeBoard);
                
            }
        }
    }

    void layoutObjectRandom(GameObject[] tileChoice, int obstacles)
    {
        for (int i = 0; i < obstacles; i++)
        {
          
             randomIndex = Random.Range(0, newGridPositions.Count);
             randomPosition1 = newGridPositions[randomIndex];
              
             int indexForInstantiate = Random.Range(0, tileChoice.Length);

             GenerateObject("Obstacle 1x1", tileChoice[indexForInstantiate]);
             GenerateObject("Obstacle 1x2", tileChoice[indexForInstantiate]);
             GenerateObject("Obstacle 2x1", tileChoice[indexForInstantiate]);
             GenerateObject("Obstacle 2x2", tileChoice[indexForInstantiate]);
             GenerateObject(start.name, tileChoice[indexForInstantiate]);
             GenerateObject(meta.name, tileChoice[indexForInstantiate]);
 
             newGridPositions.RemoveAt(randomIndex);
        }
    }

    void GenerateObject(string name,GameObject @object)
    {
        if (@object.name == name)
        {
            heCollide = @object.GetComponent<Collider2D>();
            do
            {
                randomIndex = Random.Range(0, newGridPositions.Count);
                randomPosition1 = newGridPositions[randomIndex];
            }
            while (heCollide.IsTouching(toCollide));
                GameObject instance = Instantiate(@object, randomPosition1, Quaternion.identity);
            instance.transform.SetParent(wholeBoard);
            gridPositions.Add(randomPosition1);
            obstacleName.Add(@object.name);
        }
    }

    void saveToFile()
    {
        File.WriteAllText(filePath + fileName, size.ToString());
      for (int i=0;i < gridPositions.Count;i++)
        File.AppendAllText(filePath + fileName,";" + obstacleName[i] + "," + gridPositions[i].x.ToString() + "," + gridPositions[i].y.ToString());

    }
}
