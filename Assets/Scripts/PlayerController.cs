using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float maxX;
	public float minX;

	private Rigidbody2D rb;
	private Vector2 initialPosition;
	private GameData GameData;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		initialPosition = gameObject.transform.position;
		GameData = GameObject.FindWithTag ("Game Data").GetComponent<GameData> ();
	}

	void FixedUpdate()
	{
		if (GameData.GamePaused ())
			return;
		
		Vector2 movement = new Vector2 ( speed * Input.GetAxis ("Horizontal") * Time.fixedDeltaTime, 0.0f );
		Vector2 newPosition = rb.position + movement;

		if ( newPosition.x > maxX )
			newPosition.x = maxX;
		
		if ( newPosition.x < minX )
			newPosition.x = minX;

		rb.MovePosition ( newPosition );
	}

	public void InitialPosition()
	{
		rb.MovePosition( initialPosition );
	}

	public Vector2 Position()
	{
		return gameObject.transform.position;
	}
}