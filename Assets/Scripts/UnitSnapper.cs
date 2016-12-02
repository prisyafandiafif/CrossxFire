using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UnitSnapper : MonoBehaviour 
{
	public bool moveRight;
	public bool moveLeft;
	public bool moveTopRight;
	public bool moveTopLeft;
	public bool moveBottomRight;
	public bool moveBottomLeft;
	public bool rotate;

    void Update() 
	{
		this.gameObject.name = "Unit_Container";

		if (Application.isEditor && !Application.isPlaying) 
		{
			if (moveRight)
			{
				moveRight = false;

				this.gameObject.transform.position += new Vector3(0.601f, 0f, 0f);
			}
			else
			if (moveLeft)
			{
				moveLeft = false;

				this.gameObject.transform.position -= new Vector3(0.601f, 0f, 0f);
			}
			else
			if (rotate)
			{
				rotate = false;

				/*if (this.gameObject.transform.eulerAngles.y > 29 && this.gameObject.transform.eulerAngles.y < 31)
				{
					this.gameObject.transform.position -= new Vector3(0f, 0f, 0.1621f);
					this.gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				}
				else
				if (this.gameObject.transform.eulerAngles.y > 89 && this.gameObject.transform.eulerAngles.y < 91)
				{
					this.gameObject.transform.position += new Vector3(0f, 0f, 0.1621f);
					this.gameObject.transform.eulerAngles = new Vector3(0f, 30, 0f);
				}	*/
			}
			else
			if (moveBottomRight)
			{
				moveBottomRight = false;

				this.gameObject.transform.position += new Vector3(0.3005f, 0f, -0.5034f);
			}
			else
			if (moveBottomLeft)
			{
				moveBottomLeft = false;

				this.gameObject.transform.position += new Vector3(-0.3005f, 0f, -0.5034f);
			}
			else
			if (moveTopRight)
			{
				moveTopRight = false;

				this.gameObject.transform.position += new Vector3(0.3005f, 0f, 0.5034f);
			}
			else
			if (moveTopLeft)
			{
				moveTopLeft = false;

				this.gameObject.transform.position += new Vector3(-0.3005f, 0f, 0.5034f);
			}
		}
    }
}
