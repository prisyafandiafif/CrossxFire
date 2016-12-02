using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileSnapper : MonoBehaviour 
{
	public bool moveRight;
	public bool moveLeft;
	public bool moveUp;
	public bool moveDown;
	public bool rotate;

    void Update() 
	{
		this.gameObject.name = "Tile_Container";

		if (Application.isEditor && !Application.isPlaying) 
		{
			if (moveRight)
			{
				moveRight = false;

				this.gameObject.transform.position += new Vector3(0.3005f, 0f, 0f);
			}
			else
			if (moveLeft)
			{
				moveLeft = false;

				this.gameObject.transform.position -= new Vector3(0.3005f, 0f, 0f);
			}
			else
			if (rotate)
			{
				rotate = false;

				if (this.gameObject.transform.eulerAngles.y > 29 && this.gameObject.transform.eulerAngles.y < 31)
				{
					this.gameObject.transform.position -= new Vector3(0f, 0f, 0.1621f);
					this.gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				}
				else
				if (this.gameObject.transform.eulerAngles.y > 89 && this.gameObject.transform.eulerAngles.y < 91)
				{
					this.gameObject.transform.position += new Vector3(0f, 0f, 0.1621f);
					this.gameObject.transform.eulerAngles = new Vector3(0f, 30, 0f);
				}	
			}
			else
			if (moveUp)
			{
				moveUp = false;

				this.gameObject.transform.position += new Vector3(0f, 0f, 0.5034f);
			}
			else
			if (moveDown)
			{
				moveDown = false;

				this.gameObject.transform.position -= new Vector3(0f, 0f, 0.5034f);
			}
		}
    }
}
