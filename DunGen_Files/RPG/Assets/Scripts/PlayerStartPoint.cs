using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour 
{
    //private PlayerController Player;
    public GameObject player;
	public CameraController Camera;
	
	void Start () 
	{
        StartPointInit();
	}
	
	public void StartPointInit() {
        //Player = FindObjectOfType<PlayerController>();
        //player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        //Camera = FindObjectOfType<CameraController>();

        Camera.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.transform.position.z);
    }
}
