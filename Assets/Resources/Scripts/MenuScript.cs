using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public InputField sizeInput,obstaclesInput;
    public InputField nameFileInputField;
    public GameObject generateMenu, mainMenu, inPreviewMenu, loadMapMenu, inGameMenu;
    public GameObject gameController, loadMapController;
    GameObject gameControllerTemp, loadMapControllerTemp;
    public Text saveFileText;
    public Text filesToShow, fileNameToLoad;

    string[] fileArray;
    string[] fileNames;
    string filePath = "C://users/" + Environment.UserName + "/documents/Maze Generator/GameData/";
    string fileName = "tempBoardUniqueTempNeverKnownName.data";
    bool saved = false;
    float saveFileTextTime = 0;
    GameObject menuObjectParent, menuObjectChild, inGameObject, board, loadedMapObject, playerObject;

    GameObject algoritm1, pathMap, target;

    public void Start()
    {
        loadMapMenu.SetActive(false);
        generateMenu.SetActive(false);
        inPreviewMenu.SetActive(false);
        mainMenu.SetActive(true);
        inGameMenu.SetActive(false);     
    }

    private void Update()
    {
        if(saved)
        {
            saveFileText.text = "Plik Zapisano Pomyślnie";
            saveFileTextTime += Time.deltaTime;
            filesToShow.text = "";
            GenerateNames();
        }

        if(saveFileTextTime > 3)
        {
            saveFileText.text = "";
            saved = false;
            saveFileTextTime = 0;
        }
    }
    public void sizeComput()
    {
        if (int.Parse(sizeInput.text) < 10)
        {
            sizeInput.text = "10";
        }
    }

    public void GenerateNames()
    {
        filesToShow.text = "";
        fileArray = Directory.GetFiles(filePath, "*.data", SearchOption.AllDirectories);
        fileNames = new string[fileArray.Length];

        for (int i = 0; i < fileArray.Length; i++)
        {
            fileNames[i] = Path.GetFileNameWithoutExtension(fileArray[i]);
            filesToShow.text = filesToShow.text + fileNames[i] + "\n";
        }
    }

    public void fromGenerateMenuToPreview()
    {
        generateMenu.SetActive(false);
        PlayerPrefs.SetInt("size", int.Parse(sizeInput.text));
        PlayerPrefs.SetInt("obstacles", int.Parse(obstaclesInput.text));
        gameControllerTemp = Instantiate(gameController);
        inPreviewMenu.SetActive(true);

    }

    public void fromMainMenuToGenerateMenu()
    {     
        mainMenu.SetActive(false);
        generateMenu.SetActive(true);  
    }

    public void fromPreviewToGenerateMenu()
    {
        if (gameControllerTemp != null)
        {
            Destroy(gameControllerTemp);
            Destroy(GameObject.Find("WholeBoard"));
        }
        generateMenu.SetActive(true);
        inPreviewMenu.SetActive(false);
    }

    public void fromGenerateMenuToMainMenu()
    {
        File.Delete(filePath + fileName);
        mainMenu.SetActive(true);
        generateMenu.SetActive(false);

        
    }

    public void fromMainMenuToLoadMapMenu()
    {
        mainMenu.SetActive(false);
        loadMapMenu.SetActive(true);
        GenerateNames();
        
    }

    public void fromLoadMapMenuToMainMenu()
    {
        loadMapMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void fromLoadMapMenuToInGameMenu()
    {
        if (File.Exists(filePath + fileNameToLoad.text + ".data"))
        {
            loadMapMenu.SetActive(false);
            inGameMenu.SetActive(true);
            loadMapControllerTemp = Instantiate(loadMapController);
            PlayerPrefs.SetString("fileName", fileNameToLoad.text);
        }
    }

    public void fromInGameMenuToMainMenu()
    {
        inGameMenu.SetActive(false);
        mainMenu.SetActive(true);
        inGameObject = GameObject.Find("InGameObject");
        loadedMapObject = GameObject.Find("LoadedMap(Clone)");
        board = GameObject.Find("Board");
        playerObject = GameObject.Find("Player(Clone)");
        algoritm1 = GameObject.Find("Algoritm1(Clone)");
        pathMap = GameObject.Find("A_(Clone)");
        target = GameObject.Find("Target(Clone)");
        Destroy(loadMapControllerTemp);
        Destroy(board);
        Destroy(playerObject);
        Destroy(algoritm1);
        Destroy(pathMap);
        Destroy(target);
    }

    public void saveFile()
    {
       if( File.Exists(filePath + fileName) && nameFileInputField.text != "")
        {
            if (File.Exists(filePath + nameFileInputField.text + ".data"))
                File.Delete(filePath + nameFileInputField.text + ".data");
            File.Copy(filePath + fileName, filePath + nameFileInputField.text + ".data");
            
            nameFileInputField.text = "";
            saved = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
