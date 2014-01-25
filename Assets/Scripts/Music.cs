using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	public AudioClip music;

	// Use this for initialization
	void Start () {
		
	}
	
	// Static singleton property
	public static Music Instance { get; private set; }
	
	void Awake()
	{
		// First we check if there are any other instances conflicting
		if(Instance != null && Instance != this)
		{
			//destroy the new instance
			Destroy( this );
			return;
		}
		// Here we save our singleton instance
		Instance = this;
		
		// Furthermore we make sure that we don't destroy between scenes (this is optional)
		DontDestroyOnLoad(gameObject);

		Init();
	}
	
	void Init () {
		if ( Application.loadedLevelName == "Game" ) {
		    if  (!audio.isPlaying ) {
				audio.Play();
			}
		}
	}
}
