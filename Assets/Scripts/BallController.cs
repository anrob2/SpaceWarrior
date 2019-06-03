using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	public float speed;
	public float thrust;

	private Rigidbody2D rb;
	private Vector2 initialPosition;
	private GameData GameData;
	private PlayerController Player;
	private Vector2 pauseVelocity;

	void Start ()
	{
		rb = GetComponent <Rigidbody2D> ();
		initialPosition = rb.position;
		GameData = GameObject.FindWithTag ("Game Data").GetComponent<GameData> ();
		Player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
	}

	void Update ()
	{
		if ( GameData.Begin() && Input.GetButton("Jump") )
		{
			rb.AddForce (thrust * Vector2.up);
			GameData.SetBegin (false);
		}
	}

	void FixedUpdate ()
	{
		if (GameData.GamePaused ())
			return;

		if ( GameData.Begin() )
		{
			Vector2 newPosition = new Vector2 ( Player.Position().x, initialPosition.y );
			rb.MovePosition ( newPosition );
			return;
		}
			
		Vector2 newVelocity = rb.velocity.normalized;

		if (newVelocity == Vector2.zero || (newVelocity.y >= -speed/100 &&  newVelocity.y <= speed/100) )
			rb.AddForce (thrust * Vector2.up);
		
		if (newVelocity == Vector2.up || newVelocity == Vector2.down )
			rb.AddForce (thrust * Vector2.right);
	
		newVelocity *= speed;

		rb.velocity = newVelocity;
	}

	void OnCollisionEnter2D( Collision2D coll )
	{
		if (coll.gameObject.CompareTag ("Tile") && GameData.GameActive() )
		{
			coll.gameObject.SetActive (false);
			GameData.IncrementScore ();
		}

		if ( coll.gameObject.CompareTag ("Floor") )
		{
			GameData.Death ();
		}
	}

	public void InitialPosition()
	{
		rb.MovePosition( initialPosition );
		rb.velocity = Vector2.zero;
	}

	public void Pause()
	{
		pauseVelocity = rb.velocity;
		rb.velocity = Vector2.zero;
	}

	public void UnPause()
	{
		rb.velocity = pauseVelocity;
	}
}
