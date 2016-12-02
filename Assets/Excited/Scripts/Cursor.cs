using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour 
{
	public float speed;

	public float freezeDelayAfterSnap;

	private bool isMovable = true;
	
	private float tempTime;

	// Use this for initialization
	void Start () 
	{
		InterfaceManager.instance.cursorSpeedInputField.text = "" + speed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isMovable)
		{
			MoveCursor();
		}
		else
		{
			if (Time.time - tempTime >= freezeDelayAfterSnap)
			{
				isMovable = true;
				tempTime = 0f;
			}
		}
	}

	// Move the cursor based on the joystick movement
	public void MoveCursor ()
	{
		this.gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
	}

	// Set the written cursor speed value
	public void SetCursorSpeed ()
	{
		speed = float.Parse(InterfaceManager.instance.cursorSpeedInputField.text);
	}

	public void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name == "Dot")
		{
			Debug.Log("Snap to: " + other.gameObject.name);

			//snap to dot
			this.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, this.gameObject.transform.position.y, other.gameObject.transform.position.z);
		
			//freeze cursor for a while
			isMovable = false;
			tempTime = Time.time;
		}
	}
}
