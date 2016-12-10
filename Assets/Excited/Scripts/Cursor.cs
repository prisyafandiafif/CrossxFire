using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour 
{
	public float speed; //the bigger, the faster 

	public float snapTolerance; //the lower, the less sensitive

	public bool isSnap; //to know whether this cursor is being snapped or not

	//public float freezeDelayAfterSnap;

	//public bool isWithinDotArea = false;

	//private Vector3 dotPositionToSnap = new Vector3(-1000f, -1000f, -1000f);

	//private bool isMovable = true;
	
	//private float tempTime;

	private GameObject dotsContainer;

	// Use this for initialization
	void Start () 
	{
		//assign
		dotsContainer = GameObject.Find("Dots");

		//show value to the UI
		//InterfaceManager.instance.cursorSpeedInputField.text = "" + speed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if (isMovable)
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
		}*/

		//if the cursor is not in moving condition
		if (Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0f)
		{
			for (int i = 0; i < dotsContainer.transform.childCount; i++)
			{
				if (Vector3.Distance(this.gameObject.transform.position, dotsContainer.transform.GetChild(i).gameObject.transform.position) <= snapTolerance)
				{
					//snap
					isSnap = true;

					//change the color of the cursor //TODO
					this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
	
					this.gameObject.transform.position = new Vector3(dotsContainer.transform.GetChild(i).gameObject.transform.position.x, this.gameObject.transform.position.y, dotsContainer.transform.GetChild(i).gameObject.transform.position.z);
				}
			}
		}
		else
		{
			MoveCursor();
		}

		//if cursor is within a dot area
		/*if (isWithinDotArea)
		{
			//if the cursor is not in moving condition
			if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
			{
				//Debug.Log("Snap to: " + other.gameObject.name);
	
				//snap to closest dot
				SnapToClosestDot();
			}
			else
			{
				MoveCursor();
			}
		}
		else
		{
			MoveCursor();
		}*/
	}

	// Move the cursor based on the joystick movement
	public void MoveCursor ()
	{
		//unsnap
		isSnap = false;

		//change the color of the cursor //TODO
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;

		this.gameObject.transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
	}

	// Set the written cursor speed value
	/*public void SetCursorSpeed ()
	{
		speed = float.Parse(InterfaceManager.instance.cursorSpeedInputField.text);
	}*/

	//public void OnTriggerEnter (Collider other)
	//{
		//if cursor collides with the Dot
		/*if (other.gameObject.name == "Dot")
		{
			isWithinDotArea = true;

			dotPositionToSnap = other.gameObject.transform.position;

			Debug.Log("Collide with: " + other.gameObject.name);*/

			/*if (Input.GetAxis("Horizontal") < 0.1f && Input.GetAxis("Vertical") < 0.1f)
			{
				
			}
			else
			{
				Debug.Log("Snap to: " + other.gameObject.name);
	
				//snap to dot
				this.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, this.gameObject.transform.position.y, other.gameObject.transform.position.z);
			
				//freeze cursor for a while
				isMovable = false;
				tempTime = Time.time;
			}*/
		//}
	//}

	/*public void OnTriggerExit (Collider other)
	{
		//if cursor exits the Dot
		if (other.gameObject.name == "Dot")
		{
			isWithinDotArea = false;

			dotPositionToSnap = new Vector3(-1000f, -1000f, -1000f);
		}
	}*/
}
