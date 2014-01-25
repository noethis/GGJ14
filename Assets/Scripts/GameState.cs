using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameState : MonoBehaviour {
	//CONSTS

	//UNITY
	public List<PlayerController> players;
	public GUIText messageText;

	//VARS
	[HideInInspector] public PlayerController activePlayer;
	private int playerIndex = 0;

	//STATIC
	public static bool DEBUG_MODE = true;

	// Static singleton property
	public static GameState Instance { get; private set; }

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
	}

	void Start() {

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
	}

	void CycleNextPlayer() {
		playerIndex = (playerIndex + 1) % players.Count;
		activePlayer = players [playerIndex];
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
