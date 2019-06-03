using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	public Text scoreText;
	public Text gameOverText;
	public Text pauseText;
	public Text quitText;

	private int score;
	private bool begin;
	private bool isPaused;
	private bool quitQuestion;
	private bool playAgainQuestion;
	private PlayerController Player;
	private BallController Ball;
	private int lives;
	private int numberOfTiles;
	private GameObject[] Tiles;
	private GameObject[] Lives;

	void Start ()
	{
		Tiles = GameObject.FindGameObjectsWithTag ("Tile");
		numberOfTiles = Tiles.Length;

		Lives = new GameObject[3];
		for( int i = 0; i < 3; i++ )
			Lives[i] = GameObject.FindWithTag ( "Life (" + (i+1).ToString() + ")" );
		
		Player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		Ball = GameObject.FindWithTag ("Ball").GetComponent<BallController> ();

		ResetGameVariables ();
	}

	void ResetGameVariables()
	{
		begin = true;
		isPaused = false;
		pauseText.text = "";
		quitText.text = "";
		playAgainQuestion = false;
		quitQuestion = false;
		score = 0;
		SetScoreText ();
		gameOverText.text = "";
		lives = 3;
	}

	void Update()
	{
		if ( playAgainQuestion )
		{
			if ( Input.GetButtonDown ("Y") )
			{
				Player.InitialPosition ();
				Ball.InitialPosition ();

				foreach (GameObject Tile in Tiles) 
					Tile.SetActive (true);

				foreach (GameObject Life in Lives)
					Life.SetActive (true);
				
				ResetGameVariables ();

				return;
			}

			if (Input.GetButtonDown ("N"))
			{
				Application.Quit ();
			}

			return;
		}

		if ( !quitQuestion && Input.GetButtonDown ("Quit") )
		{
			Ball.Pause ();
			isPaused = true;
			pauseText.text = "";
			quitText.text = "Do You really want to quit?\nY / N";
			quitQuestion = true;
			return;
		}

		if (quitQuestion && Input.GetButtonDown ("N"))
		{
			quitText.text = "";
			pauseText.text = "Pause";
			quitQuestion = false;
			return;
		}

		if (quitQuestion && Input.GetButtonDown ("Y"))
		{
			Application.Quit ();
		}

		if ( Input.GetButtonDown ("Pause") && GamePaused()  )
		{
			Ball.UnPause ();
			pauseText.text = "";
			isPaused = false;
			return;
		}

		if ( !GameActive () )
			return;

		if ( Input.GetButtonDown ("Pause") && !GamePaused()  )
		{
			Ball.Pause ();
			pauseText.text = "Pause";
			isPaused = true;
			return;
		}

		if ( Input.GetButtonDown ("Pause") && GamePaused()  )
		{
			Ball.UnPause ();
			pauseText.text = "";
			isPaused = false;
			return;
		}
	}

	void SetScoreText()
	{
		scoreText.text = "Score: " + score.ToString ();
	}

	public void IncrementScore()
	{
		score++;
		SetScoreText ();

		if (score == numberOfTiles)
		{
			lives = 0;
			gameOverText.text = "Congratulations";
			NewGame ();
		}
	}

	public void Death ()
	{
		Ball.InitialPosition();
		Player.InitialPosition();
		SetBegin (true);

		if ( !GameActive() )
			return;

		if ( GameActive () )
		{
			string lifeTag = "Life (" + lives.ToString () + ")";
			GameObject Life = GameObject.FindWithTag (lifeTag);
			Life.SetActive (false);
			lives--;
		}

		if ( !GameActive() )
		{
			gameOverText.text = "Game Over";
			NewGame ();
		}
	}

	void NewGame()
	{
		quitText.text = "Do You want to play again?\nY / N";
		playAgainQuestion = true;
	}

	public bool GameActive()
	{
		return lives > 0;
	}

	public bool GamePaused()
	{
		return isPaused;
	}

	public bool Begin()
	{
		return begin;
	}

	public void SetBegin( bool newBegin )
	{
		begin = newBegin;
	}
}
