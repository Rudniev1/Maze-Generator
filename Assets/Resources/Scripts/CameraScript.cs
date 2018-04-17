using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    Camera cameraObject;
	// Use this for initialization
	void Start () {
        cameraObject = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {  
            cameraObject.transform.position = new Vector3(PlayerPrefs.GetInt("size") / 2, PlayerPrefs.GetInt("size") / 2, -10f);
            cameraObject.orthographicSize = (PlayerPrefs.GetInt("size") / 2) + 1.5f;
        
    }
}
