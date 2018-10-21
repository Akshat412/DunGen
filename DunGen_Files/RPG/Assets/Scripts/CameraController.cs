using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	public GameObject followTarget;

	private Vector3 targetPos;

	public float moveSpeed;

	private static bool cameraExists;

	void Start () 
	{
		if(!cameraExists)
		{
			cameraExists = true;

			DontDestroyOnLoad(transform.gameObject);
		}

		else
		{
			Destroy(gameObject);
		}
	}

	void FixedUpdate () 
	{
		targetPos = new Vector3 (followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, targetPos, moveSpeed * Time.deltaTime);
	}
}
