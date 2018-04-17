using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour {

    public NavMeshAgent agent;
    
	// Use this for initialization
	void Start () {
        agent.SetDestination(new Vector3(6, 1, 6));
	}

}
