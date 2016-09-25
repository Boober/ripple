using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

    private bool playerAlive;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        playerAlive = true;
        offset = transform.position - player.transform.position;
	}
	
    void LateUpdate()
    {
        if (playerAlive) //So camera doesn't follow dead/unspawned player.
        {
            transform.position = player.transform.position + offset;
        }
    }
}
