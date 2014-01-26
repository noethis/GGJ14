using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameState : MonoBehaviour {
	//CONSTS
	public const int NUM_LEVELS = 4;

	//UNITY
	public List<PlayerController> players;
	public GUIText messageText, promptText;

	//VARS
	[HideInInspector] public PlayerController activePlayer;
	private int playerIndex = 0;

	//STATIC
	public static bool DEBUG_MODE = true;
	[HideInInspector] public static int currLevel = 1;

	// Static singleton property
	public static GameState Instance { get; private set; }

	public AudioClip winClip;

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
//		DontDestroyOnLoad(gameObject);

//		DontDestroyOnLoad(messageText);
		
		Init();
	}

	public void Init() {
		playerIndex = 0;
		activePlayer = players [ playerIndex ];
		if (activePlayer is PlayerSight) {
			(activePlayer as PlayerSight).OnActive ();
		}
	}

	void Start() {

	}

	void NextLevel() {
		currLevel++;
		if (currLevel > NUM_LEVELS) {
			Application.LoadLevel( "Game" );
		}
		Application.LoadLevel( "level0" + currLevel );
		Init();
	}

	public void ResetLevel() 
	{	
		GameState.Instance.LightsOff ();
		Application.LoadLevel (Application.loadedLevelName);
	}

	public void WinLevel() {
		ShowMessage ("YOU WIN!", 5f);
		AudioSource.PlayClipAtPoint( winClip, transform.position, 1.0f );
		Invoke ("NextLevel", 3f);
	}

	public void LoseLevel( string overrideText = "YOU LOSE!" ) {
		ShowMessage (overrideText, 5f);
		Invoke ("ResetLevel", 3f);
	}

	void ExitToMenu() {
		Application.LoadLevel( "Game" );
	}

	void Update() {
		RunCommands();
		if ( DEBUG_MODE ) {
			RunDebugCommands();
		}
	}

	void RunCommands() {
		if (Input.GetKeyDown ("space")) {
			CycleNextPlayer();
		}

		if (Input.GetKeyDown ("e")) {
			if (activePlayer is PlayerSight) {
				(activePlayer as PlayerSight).Action ();
			}
			else if (activePlayer is PlayerSound) {
				(activePlayer as PlayerSound).Action ();
			}
			else if (activePlayer is PlayerTouch) {
				(activePlayer as PlayerTouch).Action ();
			}
		}

		if ( Input.GetKeyDown( "escape" ) || Input.GetKeyDown( "f10" ) ) {
			if ( Application.loadedLevelName == "Game" ) {
				Application.Quit();
			}
			ExitToMenu();
		}
	}

	void RunDebugCommands() {
		if ( Input.GetKeyDown( "t" ) ) {
			if ( Time.timeScale == 1.0f ) {
				Time.timeScale = 10.0f;
			}
			else {
				Time.timeScale = 1.0f;
			}
		}
		if ( Input.GetKeyDown( "y" ) ) {
			if ( Time.timeScale == 1.0f ) {
				Time.timeScale = 0.25f;
			}
			else {
				Time.timeScale = 1.0f;
			}
		}

		if ( Input.GetKeyDown( "n" ) ) {
			NextLevel();
		}
		if ( Input.GetKeyDown( "tab" ) ) {
			ResetLevel();
		}
	}

	void CycleNextPlayer() {
		playerIndex = (playerIndex + 1) % players.Count;
		if (activePlayer is PlayerSight) {
			(activePlayer as PlayerSight).OnInactive ();
		}
		else if (activePlayer is PlayerSound) {
			(activePlayer as PlayerSound).OnInactive ();
		}
		else if (activePlayer is PlayerTouch) {
		(activePlayer as PlayerTouch).OnInactive ();
		}

		activePlayer = players [playerIndex];
		if (activePlayer is PlayerSight) {
			(activePlayer as PlayerSight).OnActive ();
		}
		else if (activePlayer is PlayerSound) {
			(activePlayer as PlayerSound).OnActive ();
		}
		else if (activePlayer is PlayerTouch) {
			(activePlayer as PlayerTouch).OnActive ();
		}
	}

	public void LightsOn() {
		RenderSettings.ambientLight = new Color (1f, 1f, 1f, 1f);
	}

	public void LightsOff() {
		RenderSettings.ambientLight = new Color (0f, 0f, 0f, 1f);
	}







	public void ShowPrompt( string _text, float time = 0.0f ) {
		promptText.enabled = true;
		promptText.text = _text;
		if ( time > 0 ) {
			StartCoroutine( HidePrompt( time ) );
		}
	}
	
	public void HidePrompt() {
		promptText.enabled = false;
	}
	
	IEnumerator HidePrompt( float time = 0.0f ) {
		yield return new WaitForSeconds( time );
		promptText.enabled = false;
	}


	public void ShowMessage( string _text, float time = 0.0f ) {
		messageText.enabled = true;
		messageText.text = _text;
		if ( time > 0 ) {
			StartCoroutine( HideMessage( time ) );
		}
	}

	public void HideMessage() {
		messageText.enabled = false;
	}
	
	IEnumerator HideMessage( float time = 0.0f ) {
		yield return new WaitForSeconds( time );
		messageText.enabled = false;
	}
	
	public void BlinkMessage( string _text, float blinkInterval = 0.5f, float time = 0.0f ) {
		messageText.enabled = true;
		messageText.text = _text;
		StartCoroutine( BlinkMessage_Internal( blinkInterval ) );
		if ( time > 0 ) {
			StartCoroutine( HideMessage( time ) );
		}
	}
	
	private IEnumerator BlinkMessage_Internal( float blinkInterval = 0.5f ) {
		Color prevColor = messageText.color;
		while ( messageText.enabled ) {
			messageText.color = prevColor;
			yield return new WaitForSeconds( blinkInterval );
			messageText.color = Color.clear;
			yield return new WaitForSeconds( blinkInterval );
		}
		messageText.color = prevColor;
	}
}
