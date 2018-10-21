using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	private static bool MusicManExists;

	public AudioSource[] tracks;

	public int currentTrack;

	void Start()
	{
		if(!MusicManExists)
		{
			MusicManExists = true;
			DontDestroyOnLoad(transform.gameObject);
		}

		else
		{
			Destroy(gameObject);
		}
	}

	void Update()
	{
		if(!tracks[currentTrack].isPlaying)
		{
			tracks[currentTrack].Play();
		}
	}
}
