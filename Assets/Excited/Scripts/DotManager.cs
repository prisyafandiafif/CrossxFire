using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
// A class to hold information of each dot between tiles
public class DotDot	 
{
	//associated GameObject of dots in the scene
	public GameObject associatedGameObject;

	//neighbour dot ids of this dot
	public List<int> neighboringDotIDs = new List<int>();
}

public class DotManager : MonoBehaviour 
{
	public static DotManager instance;

	// A gameobject that contains all dots
	public GameObject dotsContainer;

	// A sample gameobject of dot that will be created
	public GameObject dotSample;

	// A list to hold the Dot class
	public List<DotDot> dots = new List<DotDot>();

	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
		//put all dots into a list
		GenerateDots();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// Generate dots and put it into Dots list (heavy computation process)
	public void GenerateDots ()
	{		
		//hide all dots first 
		for (int i = 1; i < dotsContainer.transform.childCount; i++)
		{
			dotsContainer.transform.GetChild(i).gameObject.SetActive(false);
		}

		//remove all dots that have been in the list
		dots.Clear();

		//instatiate dots in the battlefield
		for (int i = 0; i < TileManager.instance.tilesContainer.transform.childCount; i++)
		{
			GameObject tile = TileManager.instance.tilesContainer.transform.GetChild(i).gameObject;

			//if Tile is active
			if (tile.activeSelf)
			{
				float xPosToCheck = 0f;
				float yPosToCheck = TileManager.instance.tilesContainer.transform.GetChild(0).gameObject.transform.position.y;
				float zPosToCheck = 0f;

				//if the tile is facing down
				if (tile.transform.localRotation.eulerAngles.y > -1 && tile.transform.localRotation.eulerAngles.y < 1)
				{
					//top left dot
					xPosToCheck = tile.transform.position.x - (TileManager.instance.tileUnitSize * 0.666667f);
					zPosToCheck = tile.transform.position.z + (TileManager.instance.tileUnitSize * 0.5f);

					//check if this kind of dot exist
					if (IsDotExistAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)))
					{
						//do nothing

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(SearchDotsByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));
					}
					//otherwise
					else
					{
						//create dot 
						CreateDotAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));
		
						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(dots[dots.Count-1]);
					}
	
					//top right dot
					xPosToCheck = tile.transform.position.x + (TileManager.instance.tileUnitSize * 0.666667f);
					zPosToCheck = tile.transform.position.z + (TileManager.instance.tileUnitSize * 0.5f);

					//check if this kind of dot exist
					if (IsDotExistAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)))
					{
						//do nothing

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(SearchDotsByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));
					}
					//otherwise
					else
					{
						//create dot 
						CreateDotAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(dots[dots.Count-1]);
					}
	
					//bottom dot
					xPosToCheck = tile.transform.position.x + 0f;
					zPosToCheck = tile.transform.position.z - (TileManager.instance.tileUnitSize * 0.5f);
					//check if this kind of dot exist
					if (IsDotExistAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)))
					{
						//do nothing

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(SearchDotsByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));
					}
					//otherwise
					else
					{
						//create dot 
						CreateDotAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(dots[dots.Count-1]);
					}
				}
				else
				//if it's facing up
				if (tile.transform.localRotation.eulerAngles.y > 179 && tile.transform.localRotation.eulerAngles.y < 181)
				{
					//bottom left dot
					xPosToCheck = tile.transform.position.x - (TileManager.instance.tileUnitSize * 0.666667f);
					zPosToCheck = tile.transform.position.z - (TileManager.instance.tileUnitSize * 0.5f);
	
					//check if this kind of dot exist
					if (IsDotExistAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)))
					{
						//do nothing

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(SearchDotsByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));
					}
					//otherwise
					else
					{
						//create dot 
						CreateDotAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(dots[dots.Count-1]);
					}
	
					//bottom right dot
					xPosToCheck = tile.transform.position.x + (TileManager.instance.tileUnitSize * 0.666667f);
					zPosToCheck = tile.transform.position.z - (TileManager.instance.tileUnitSize * 0.5f);

					//check if this kind of dot exist
					if (IsDotExistAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)))
					{
						//do nothing

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(SearchDotsByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));
					}
					//otherwise
					else
					{
						//create dot 
						CreateDotAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(dots[dots.Count-1]);
					}
	
					//top dot
					xPosToCheck = tile.transform.position.x + 0f;
					zPosToCheck = tile.transform.position.z + (TileManager.instance.tileUnitSize * 0.5f);

					//check if this kind of dot exist
					if (IsDotExistAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)))
					{
						//do nothing

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(SearchDotsByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));
					}
					//otherwise
					else
					{
						//create dot 
						CreateDotAtAPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

						//put the associated dot to the Tile
						//tiles[i].associatedDots.Add(dots[dots.Count-1]);
					}
				}
			}
		}
	}

	// Check whether a Dot exists or not at a certain position
	public bool IsDotExistAtAPosition (Vector3 dotToCheck)
	{
		for (int i = 0; i < dots.Count; i++)
		{
			if (dots[i].associatedGameObject.activeSelf &&
				dotToCheck.x > dots[i].associatedGameObject.transform.position.x - 0.05f &&
				dotToCheck.x < dots[i].associatedGameObject.transform.position.x + 0.05f &&
				dotToCheck.z > dots[i].associatedGameObject.transform.position.z - 0.05f &&
				dotToCheck.z < dots[i].associatedGameObject.transform.position.z + 0.05f)
			{
				return true;
			}
		}

		return false;
	}

	// Create a Dot at a position
	public void CreateDotAtAPosition (Vector3 pos)
	{
		GameObject createdDot = Instantiate (dotSample);

		createdDot.SetActive(true);

		//put the created dot to dots
		createdDot.transform.parent = dotsContainer.transform;
		
		//position it
		createdDot.transform.position = pos;

		//name it
		createdDot.name = dotSample.name;
	
		//put it inside Dot class
		DotDot toPutDot = new DotDot();
		toPutDot.associatedGameObject = createdDot;
		dots.Add(toPutDot);
	}

}
