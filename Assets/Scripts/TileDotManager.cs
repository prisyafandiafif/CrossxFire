using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;

[System.Serializable]
// A class to hold information of each dot between tiles
public class Dot	 
{
	//associated GameObject of dots in the gameplay
	public GameObject associatedGameObject;

	//neighbour dots of this dot
	public List<int> neighboringDots = new List<int>();
}

[System.Serializable]
// A class to hold information of each tile
public class Tile	 
{
	//tile ID. 0 is Normal Tile, 1 is //TODO add description of tileID in the comment
	public int tileID; 

	//associated GameObject of tile in the gameplay
	public GameObject associatedGameObject;

	//associated dots ID at this tile
	public List<int> associatedDots = new List<int>();
}

public class TileDotManager : MonoBehaviour 
{
	public static TileDotManager instance;

	// A container of all Tiles
	public GameObject tilesContainer;

	// A container of all Dots
	public GameObject dotsContainer;

	// An array to hold the Dot class
	public List<Dot> dots = new List<Dot>();

	// An array to hold the Tile class
	public List<Tile> tiles = new List<Tile>();

	//list of enem units that will be attacked in crossfire
	[HideInInspector] public List<Unit> crossfiredEnemyUnits = new List<Unit>();
	//list of player units that will attack in crossfire
	[HideInInspector] public List<Unit> crossfiringPlayerUnits = new List<Unit>();


	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
		Debug.Log("Time before total execution: " + Time.realtimeSinceStartup);

		//put tiles into list
		GenerateTiles();

		Debug.Log("Time before Generate Dots: " + Time.realtimeSinceStartup);

		//generated dots in the gameplay and put them into list
		GenerateDots();

		Debug.Log("Time after Generate Dots: " + Time.realtimeSinceStartup);

		//put associated dots of a Tile
		for (int i = 0; i < tiles.Count; i++)
		{
			AssignDotsToATile(tiles[i]);
		}

		//assign neighbouring dots to each dot
		for (int i = 0; i < dots.Count; i++)
		{
			AssignDotWithNeighbourDots(dots[i]);
		}

		Debug.Log("Time after total execution: " + Time.realtimeSinceStartup);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// put tiles into Tiles list (heavy computation process)
	public void GenerateTiles ()
	{
		for (int i = 0; i < tilesContainer.transform.childCount; i++)
		{
			GameObject tile = tilesContainer.transform.GetChild(i).gameObject;

			//put it inside Tile class
			Tile toPutTile = new Tile();

			toPutTile.associatedGameObject = tile;
	
			//TODO put more if if there are more tile variations
			//if normal tile
			if (toPutTile.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.name == "black (Instance)")
			{
				toPutTile.tileID = 0;
			}		

			tiles.Add(toPutTile);
		}
	}

	// generate dots and put it into Dots list (heavy computation process)
	public void GenerateDots ()
	{
		for (int i = 1; i < dotsContainer.transform.childCount; i++)
		{
			dotsContainer.transform.GetChild(i).gameObject.SetActive(false);
		}

		dots.Clear();

		for (int i = 0; i < tiles.Count; i++)
		{
			GameObject tile = tiles[i].associatedGameObject;

			//if Tile is active
			if (tile.activeSelf)
			{
				float xPosToCheck = 0f;
				float yPosToCheck = dotsContainer.transform.GetChild(0).gameObject.transform.position.y;
				float zPosToCheck = 0f;

				//if the tile is facing down
				if (tile.transform.localRotation.eulerAngles.y > 29f && tile.transform.localRotation.eulerAngles.y < 31f)
				{
					//top left dot
					xPosToCheck = tile.transform.position.x - 0.3005f;
					zPosToCheck = tile.transform.position.z + 0.1706f;

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
					xPosToCheck = tile.transform.position.x + 0.3005f;
					zPosToCheck = tile.transform.position.z + 0.1706f;

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
					zPosToCheck = tile.transform.position.z - 0.3328f;
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
				{
					//bottom left dot
					xPosToCheck = tile.transform.position.x - 0.3005f;
					zPosToCheck = tile.transform.position.z - 0.1706f;
	
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
					xPosToCheck = tile.transform.position.x + 0.3005f;
					zPosToCheck = tile.transform.position.z - 0.1706f;

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
					zPosToCheck = tile.transform.position.z + 0.3328f;

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

	// check whether a Dot exists or not at a certain position
	public bool IsDotExistAtAPosition (Vector3 dotToCheck)
	{
		for (int i = 0; i < dots.Count; i++)
		{
			if (dots[i].associatedGameObject.activeSelf &&
				dotToCheck.x > dots[i].associatedGameObject.transform.position.x - 0.1f &&
				dotToCheck.x < dots[i].associatedGameObject.transform.position.x + 0.1f &&
				dotToCheck.z > dots[i].associatedGameObject.transform.position.z - 0.1f &&
				dotToCheck.z < dots[i].associatedGameObject.transform.position.z + 0.1f)
			{
				return true;
			}
		}

		/*for (int i = 0; i < dotsContainer.transform.childCount; i++)
		{
			if (dotsContainer.transform.GetChild(i).gameObject.activeSelf &&
				dotToCheck.x > dotsContainer.transform.GetChild(i).gameObject.transform.position.x - 0.1f &&
				dotToCheck.x < dotsContainer.transform.GetChild(i).gameObject.transform.position.x + 0.1f &&
				dotToCheck.z > dotsContainer.transform.GetChild(i).gameObject.transform.position.z - 0.1f &&
				dotToCheck.z < dotsContainer.transform.GetChild(i).gameObject.transform.position.z + 0.1f)
			{
				return true;
			}
		}*/

		return false;
	}

	// create a Dot at a position
	public void CreateDotAtAPosition (Vector3 pos)
	{
		GameObject createdDot = Instantiate (dotsContainer.transform.GetChild(0).gameObject);

		createdDot.SetActive(true);

		//put the created dot to dots
		createdDot.transform.parent = dotsContainer.transform;
		
		//position it
		createdDot.transform.position = pos;

		//name it
		createdDot.name = dotsContainer.transform.GetChild(0).gameObject.name;
	
		//put it inside Dot class
		Dot toPutDot = new Dot();
		
		toPutDot.associatedGameObject = createdDot;
		
		dots.Add(toPutDot);
	}

	// return a Dot class with desired position
	public Dot SearchDotsByPosition (Vector3 searchPos)
	{
		Dot searchResultDot = new Dot();

		for (int i = 0; i < dots.Count; i++)
		{
			if (searchPos.x > dots[i].associatedGameObject.transform.position.x - 0.1f &&
				searchPos.x < dots[i].associatedGameObject.transform.position.x + 0.1f &&
				searchPos.z > dots[i].associatedGameObject.transform.position.z - 0.1f &&
				searchPos.z < dots[i].associatedGameObject.transform.position.z + 0.1f)
			{
				searchResultDot = dots[i];

				return searchResultDot;
			}
		}

		//searchResultDot = null;

		return searchResultDot;
	}

	// return a Dot ID with desired position
	public int SearchDotIDByPosition (Vector3 searchPos)
	{
		int searchResultDotID = -1;

		for (int i = 0; i < dots.Count; i++)
		{
			if (searchPos.x > dots[i].associatedGameObject.transform.position.x - 0.15f &&
				searchPos.x < dots[i].associatedGameObject.transform.position.x + 0.15f &&
				searchPos.z > dots[i].associatedGameObject.transform.position.z - 0.15f &&
				searchPos.z < dots[i].associatedGameObject.transform.position.z + 0.15f)
			{
				searchResultDotID = i;

				return searchResultDotID;
			}
		}

		//searchResultDot = null;

		return searchResultDotID;
	}

	// return Dot IDs where player can resurrect unit //TODO consider about leader too when we have leader
	public List<Dot> SearchDotsToResurrectAroundAUnit (Unit unitForResurection)
	{
		List<Dot> dotsResurrection = new List<Dot>();

		Dot dotBelowAUnit = dots[unitForResurection.associatedDots[0]];

		//find through the neighbour dots
		for (int j = 0; j < dotBelowAUnit.neighboringDots.Count; j++)
		{
			//if a neighbour exist
			if (dotBelowAUnit.neighboringDots[j] > -1)
			{
				Dot neighbourDot = dots[dotBelowAUnit.neighboringDots[j]];
	
				int unitIDOnTopOfNeighbourDot = UnitManager.instance.SearchUnitIDByPosition(neighbourDot.associatedGameObject.transform.position);
	
				//if there isn't a unit on top of the neigbour dot
				if (unitIDOnTopOfNeighbourDot == -1)
				{
					dotsResurrection.Add(neighbourDot);
				}
			}
		}

		return dotsResurrection;
	}

	// find Dots around a Tile
	public void AssignDotsToATile (Tile tileToAssign)
	{
		//clear it first
		tileToAssign.associatedDots.Clear();

		float xPosToCheck = 0f;
		float yPosToCheck = dotsContainer.transform.GetChild(0).gameObject.transform.position.y;
		float zPosToCheck = 0f;

		//if the tile is facing down
		if (tileToAssign.associatedGameObject.transform.localRotation.eulerAngles.y > 29f && 
			tileToAssign.associatedGameObject.transform.localRotation.eulerAngles.y < 31f)
		{
			//find the top left dot
			xPosToCheck = tileToAssign.associatedGameObject.transform.position.x - 0.3005f;
			zPosToCheck = tileToAssign.associatedGameObject.transform.position.z + 0.1706f;
			
			//put the associated dot to the Tile
			tileToAssign.associatedDots.Add(SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));	

			//find the top right dot
			xPosToCheck = tileToAssign.associatedGameObject.transform.position.x + 0.3005f;
			zPosToCheck = tileToAssign.associatedGameObject.transform.position.z + 0.1706f;

			//put the associated dot to the Tile
			tileToAssign.associatedDots.Add(SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));	
		
			//find the bottom dot
			xPosToCheck = tileToAssign.associatedGameObject.transform.position.x + 0f;
			zPosToCheck = tileToAssign.associatedGameObject.transform.position.z - 0.3328f;

			//put the associated dot to the Tile
			tileToAssign.associatedDots.Add(SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));	
		}
		//otherwise (facing up)
		else
		{
			//find the bottom left dot
			xPosToCheck = tileToAssign.associatedGameObject.transform.position.x - 0.3005f;
			zPosToCheck = tileToAssign.associatedGameObject.transform.position.z - 0.1706f;
			
			//put the associated dot to the Tile
			tileToAssign.associatedDots.Add(SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));	

			//find the bottom right dot
			xPosToCheck = tileToAssign.associatedGameObject.transform.position.x + 0.3005f;
			zPosToCheck = tileToAssign.associatedGameObject.transform.position.z - 0.1706f;

			//put the associated dot to the Tile
			tileToAssign.associatedDots.Add(SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));	
		
			//find the top dot
			xPosToCheck = tileToAssign.associatedGameObject.transform.position.x + 0f;
			zPosToCheck = tileToAssign.associatedGameObject.transform.position.z + 0.3328f;

			//put the associated dot to the Tile
			tileToAssign.associatedDots.Add(SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));	
		}
	}
	
	// find neighbouring Dots around a Dot
	public void AssignDotWithNeighbourDots (Dot dotToAssign)
	{
		//clear it first
		dotToAssign.neighboringDots.Clear();

		int neighbourDotID = -1;

		float xPosToCheck = 0f;
		float yPosToCheck = dotsContainer.transform.GetChild(0).gameObject.transform.position.y;
		float zPosToCheck = 0f;

		//find on top left
		xPosToCheck = dotToAssign.associatedGameObject.transform.position.x - 0.3005f;
		zPosToCheck = dotToAssign.associatedGameObject.transform.position.z + 0.5034f;

		neighbourDotID = SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

		//if we found something there
		if (neighbourDotID > -1)
		{
			//check if the found Dot is at the same tile with the origin Dot
			if (AreDotsOnTheSameTile(dotToAssign, dots[neighbourDotID]))
			{
				//add this Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
			//otherwise
			else
			{
				neighbourDotID = -1;

				//add empty Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
		}
		else
		{
			//add empty Dot to the list
			dotToAssign.neighboringDots.Add(neighbourDotID);
		}

		//find on top right
		xPosToCheck = dotToAssign.associatedGameObject.transform.position.x + 0.3005f;
		zPosToCheck = dotToAssign.associatedGameObject.transform.position.z + 0.5034f;

		neighbourDotID = SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

		//if we found something there
		if (neighbourDotID > -1)
		{
			//check if the found Dot is at the same tile with the origin Dot
			if (AreDotsOnTheSameTile(dotToAssign, dots[neighbourDotID]))
			{
				//add this Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
			//otherwise
			else
			{
				neighbourDotID = -1;

				//add empty Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
		}
		else
		{
			//add empty Dot to the list
			dotToAssign.neighboringDots.Add(neighbourDotID);
		}

		//find on right
		xPosToCheck = dotToAssign.associatedGameObject.transform.position.x + 0.601f;
		zPosToCheck = dotToAssign.associatedGameObject.transform.position.z + 0f;

		neighbourDotID = SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

		//if we found something there
		if (neighbourDotID > -1)
		{
			//check if the found Dot is at the same tile with the origin Dot
			if (AreDotsOnTheSameTile(dotToAssign, dots[neighbourDotID]))
			{
				//add this Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
			//otherwise
			else
			{
				neighbourDotID = -1;

				//add empty Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
		}
		else
		{
			//add empty Dot to the list
			dotToAssign.neighboringDots.Add(neighbourDotID);
		}

		//find on bottom right
		xPosToCheck = dotToAssign.associatedGameObject.transform.position.x + 0.3005f;
		zPosToCheck = dotToAssign.associatedGameObject.transform.position.z - 0.5034f;

		neighbourDotID = SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

		//if we found something there
		if (neighbourDotID > -1)
		{
			//check if the found Dot is at the same tile with the origin Dot
			if (AreDotsOnTheSameTile(dotToAssign, dots[neighbourDotID]))
			{
				//add this Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
			//otherwise
			else
			{
				neighbourDotID = -1;

				//add empty Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
		}
		else
		{
			//add empty Dot to the list
			dotToAssign.neighboringDots.Add(neighbourDotID);
		}

		//find on bottom left
		xPosToCheck = dotToAssign.associatedGameObject.transform.position.x - 0.3005f;
		zPosToCheck = dotToAssign.associatedGameObject.transform.position.z - 0.5034f;

		neighbourDotID = SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

		//if we found something there
		if (neighbourDotID > -1)
		{
			//check if the found Dot is at the same tile with the origin Dot
			if (AreDotsOnTheSameTile(dotToAssign, dots[neighbourDotID]))
			{
				//add this Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
			//otherwise
			else
			{
				neighbourDotID = -1;

				//add empty Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
		}
		else
		{
			//add empty Dot to the list
			dotToAssign.neighboringDots.Add(neighbourDotID);
		}

		//find on left
		xPosToCheck = dotToAssign.associatedGameObject.transform.position.x - 0.601f;
		zPosToCheck = dotToAssign.associatedGameObject.transform.position.z + 0f;

		neighbourDotID = SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck));

		//if we found something there
		if (neighbourDotID > -1)
		{
			//check if the found Dot is at the same tile with the origin Dot
			if (AreDotsOnTheSameTile(dotToAssign, dots[neighbourDotID]))
			{
				//add this Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
			//otherwise
			else
			{
				neighbourDotID = -1;

				//add empty Dot ID to the list
				dotToAssign.neighboringDots.Add(neighbourDotID);
			}
		}
		else
		{
			//add empty Dot to the list
			dotToAssign.neighboringDots.Add(neighbourDotID);
		}
	}

	// check whether 2 Dots are on the same Tile or not
	public bool AreDotsOnTheSameTile (Dot dot1ToCheck, Dot dot2ToCheck)
	{
		for (int i = 0; i < tiles.Count; i++)
		{
			if (tiles[i].associatedDots.Contains(dots.IndexOf(dot1ToCheck)) && tiles[i].associatedDots.Contains(dots.IndexOf(dot2ToCheck)))
			{
				return true;
			}
		}

		return false;
	}

	// return a List of Dot IDs that are within move range of another unit //TODO add conditional based on what unit is this
	public List<int> SearchDotIDsByMoveRangeOfAUnit (Unit unitToCheck)
	{
		List<int> dotsWithinMoveRange = new List<int>();

		int dotIDBelowUnit = SearchDotIDByPosition(unitToCheck.associatedGameObject.transform.position);
		Dot dotBelowUnit = dots[dotIDBelowUnit];

		//Debug.Log("Dot ID below Unit: " + dotIDBelowUnit);

		//if Green Soldier or Red Soldier //TODO for Stomper and Shooter Monster
		if (unitToCheck.unitID == 0 || unitToCheck.unitID == 1)
		{
			for (int i = 0; i < dotBelowUnit.neighboringDots.Count; i++)
			{
				//if the neighboring dot exists 
				if (dotBelowUnit.neighboringDots[i] > -1)
				{
					//Debug.Log("Selected Unit ID: " + UnitManager.instance.SearchUnitIDByPosition(dots[dotBelowUnit.neighboringDots[i]].associatedGameObject.transform.position));
	
					int searchResultUnitID = UnitManager.instance.SearchUnitIDByPosition(dots[dotBelowUnit.neighboringDots[i]].associatedGameObject.transform.position);
	
					//if there is no Unit on top of it
					if (searchResultUnitID == -1)
					{
						dotsWithinMoveRange.Add(dotBelowUnit.neighboringDots[i]);
					}
				}
			}
		}

		return dotsWithinMoveRange;
	}

	// to know whether the Dot is showing move text or not
	public bool IsDotShowingMoveText (Dot dotToCheck)
	{
		if (GameManager.instance.uiType == 1)
		{
			if (dotToCheck.associatedGameObject.transform.GetChild(0).gameObject.activeSelf && dotToCheck.associatedGameObject.transform.GetChild(1).gameObject.activeSelf)
			{
				return true;
			}
		}
	
		return false;
	}

	// return an array of Tile ID around a position
	public List<int> SearchTileIDsAroundAPosition (Vector3 searchPos)
	{
		//find a dot on that position first
		int dotID = SearchDotIDByPosition(searchPos);
		Dot dot = dots[dotID];

		List<int> prevSearchResultTileIDs = new List<int>();

		for (int i = 0; i < tiles.Count; i++)
		{
			if (tiles[i].associatedDots.Contains(dotID))
			{
				prevSearchResultTileIDs.Add(i);
			}
		}

		List<int> nextSearchResultTileIDs = new List<int>();

		//find the top left tile
		//if we did not found one
		if (dot.neighboringDots[0] == -1 || dot.neighboringDots[5] == -1)
		{
			nextSearchResultTileIDs.Add(-1);
		}
		//otherwise
		else
		{
			for (int i = 0; i < prevSearchResultTileIDs.Count; i++)
			{
				if (tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[0]) && tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[5]))
				{
					nextSearchResultTileIDs.Add(prevSearchResultTileIDs[i]);

					break;
				}

				if (i == prevSearchResultTileIDs.Count - 1)
				{
					nextSearchResultTileIDs.Add(-1);
				}
			}
		}

		//find the top tile
		//if we did not found one
		if (dot.neighboringDots[0] == -1 || dot.neighboringDots[1] == -1)
		{
			nextSearchResultTileIDs.Add(-1);
		}
		//otherwise
		else
		{
			for (int i = 0; i < prevSearchResultTileIDs.Count; i++)
			{
				if (tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[0]) && tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[1]))
				{
					nextSearchResultTileIDs.Add(prevSearchResultTileIDs[i]);

					break;
				}

				if (i == prevSearchResultTileIDs.Count - 1)
				{
					nextSearchResultTileIDs.Add(-1);
				}
			}
		}

		//find the top right tile
		//if we did not found one
		if (dot.neighboringDots[1] == -1 || dot.neighboringDots[2] == -1)
		{
			nextSearchResultTileIDs.Add(-1);
		}
		//otherwise
		else
		{
			for (int i = 0; i < prevSearchResultTileIDs.Count; i++)
			{
				if (tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[1]) && tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[2]))
				{
					nextSearchResultTileIDs.Add(prevSearchResultTileIDs[i]);

					break;
				}

				if (i == prevSearchResultTileIDs.Count - 1)
				{
					nextSearchResultTileIDs.Add(-1);
				}
			}
		}

		//find the bottom right tile
		//if we did not find one
		if (dot.neighboringDots[2] == -1 || dot.neighboringDots[3] == -1)
		{
			nextSearchResultTileIDs.Add(-1);
		}
		//otherwise
		else
		{
			for (int i = 0; i < prevSearchResultTileIDs.Count; i++)
			{
				if (tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[2]) && tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[3]))
				{
					nextSearchResultTileIDs.Add(prevSearchResultTileIDs[i]);

					break;
				}

				if (i == prevSearchResultTileIDs.Count - 1)
				{
					nextSearchResultTileIDs.Add(-1);
				}
			}
		}

		//find the bottom tile
		//if we did not find one
		if (dot.neighboringDots[3] == -1 || dot.neighboringDots[4] == -1)
		{
			nextSearchResultTileIDs.Add(-1);
		}
		//otherwise
		else
		{
			for (int i = 0; i < prevSearchResultTileIDs.Count; i++)
			{
				if (tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[3]) && tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[4]))
				{
					nextSearchResultTileIDs.Add(prevSearchResultTileIDs[i]);

					break;
				}

				if (i == prevSearchResultTileIDs.Count - 1)
				{
					nextSearchResultTileIDs.Add(-1);
				}
			}
		}

		//find the bottom left tile
		//if we did not find one
		if (dot.neighboringDots[4] == -1 || dot.neighboringDots[5] == -1)
		{
			nextSearchResultTileIDs.Add(-1);
		}
		//otherwise
		else
		{
			for (int i = 0; i < prevSearchResultTileIDs.Count; i++)
			{
				if (tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[4]) && tiles[prevSearchResultTileIDs[i]].associatedDots.Contains(dot.neighboringDots[5]))
				{
					nextSearchResultTileIDs.Add(prevSearchResultTileIDs[i]);

					break;
				}
			
				if (i == prevSearchResultTileIDs.Count - 1)
				{
					nextSearchResultTileIDs.Add(-1);
				}
			}
		}

		//searchResultDot = null;

		return nextSearchResultTileIDs;
	}

	// to show highlight on tiles around a Unit
	public IEnumerator ShowHighlightOnTilesAroundAUnit (Unit unitToRotate)
	{
		List<int> tileIDsAround = new List<int>();

		tileIDsAround = SearchTileIDsAroundAPosition(unitToRotate.associatedGameObject.transform.position);

		for (int i = 0; i < tileIDsAround.Count; i++)
		{
			if (tileIDsAround[i] > -1)
			{
				tiles[tileIDsAround[i]].associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
			}
		}

		List<int> dotIDsToRotate = new List<int>();

		dotIDsToRotate = dots[unitToRotate.associatedDots[0]].neighboringDots;

		List<int> unitIDsNotToRotateToFloat = new List<int>();

		//search through all rotated dots whether there's a Unit on top of them or not
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			for (int j = 0; j < dotIDsToRotate.Count; j++)
			{
				//if there's a unit on top of a rotated Dot and it's not rotatable and it's player's unit
				if (UnitManager.instance.units[i].associatedDots[0] == dotIDsToRotate[j] && !UnitManager.instance.units[i].isRotatable && UnitManager.instance.units[i].unitID >= 0 &&UnitManager.instance.units[i].unitID < 100)
				{
					//put it to the list
					unitIDsNotToRotateToFloat.Add(i);
				}
			}
		}

		//hide cursor
		if (GameManager.instance.uiType == 1)
		{
			GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);
		}

		//hide all move text
		UIManager.instance.HideAllMoveText();
	
		//hide all rotate text
		UIManager.instance.HideAllRotateText();

		//hide all attack text
		UIManager.instance.HideAllAttackText();

		//hide all stay text
		UIManager.instance.HideAllStayText();

		//deactivate input
		InputManager.instance.isInputActive = false;

		//find which player's unit is not rotatable, and put the model up
		for (int i = 0; i < unitIDsNotToRotateToFloat.Count; i++)
		{
			GameObject unitModel = UnitManager.instance.units[unitIDsNotToRotateToFloat[i]].associatedGameObject;

			HOTween.To
			(
				unitModel.transform, 
				0.5f, 
				new TweenParms()
			        .Prop
					(
						"position", 
						new Vector3
						(
							0f,
							0.1f,
							0f
						),
						true	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}

		yield return new WaitForSeconds(0.5f);

		//reactivate input
		InputManager.instance.isInputActive = true;
	}

	// to hide highlight on tiles around a Unit
	public IEnumerator HideHighlightOnTilesAroundAUnit (Unit unitToRotate)
	{
		List<int> tileIDsAround = new List<int>();

		tileIDsAround = SearchTileIDsAroundAPosition(unitToRotate.associatedGameObject.transform.position);

		for (int i = 0; i < tileIDsAround.Count; i++)
		{
			if (tileIDsAround[i] > -1)
			{
				tiles[tileIDsAround[i]].associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color(0f, 0.5f, 0f, 1f);
			}
		}

		List<int> dotIDsToRotate = new List<int>();

		dotIDsToRotate = dots[unitToRotate.associatedDots[0]].neighboringDots;

		List<int> unitIDsNotRotatableWillFloat = new List<int>();

		//search through all rotated dots whether there's a Unit on top of them or not
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			for (int j = 0; j < dotIDsToRotate.Count; j++)
			{
				//if there's a unit on top of a rotated Dot and it's not rotatable and it's player's unit
				if (UnitManager.instance.units[i].associatedDots[0] == dotIDsToRotate[j] && !UnitManager.instance.units[i].isRotatable && UnitManager.instance.units[i].unitID >= 0 &&UnitManager.instance.units[i].unitID < 100)
				{
					//put it to the list
					unitIDsNotRotatableWillFloat.Add(i);
				}
			}
		}

		//hide cursor
		GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);

		//hide all move text
		UIManager.instance.HideAllMoveText();
	
		//hide all rotate text
		UIManager.instance.HideAllRotateText();

		//hide all attack text
		UIManager.instance.HideAllAttackText();

		//hide all stay text
		UIManager.instance.HideAllStayText();

		//deactivate input
		InputManager.instance.isInputActive = false;

		//find which player's unit is not rotatable, and put the model up
		for (int i = 0; i < unitIDsNotRotatableWillFloat.Count; i++)
		{
			GameObject unitModel = UnitManager.instance.units[unitIDsNotRotatableWillFloat[i]].associatedGameObject;

			HOTween.To
			(
				unitModel.transform, 
				0.5f, 
				new TweenParms()
			        .Prop
					(
						"position", 
						new Vector3
						(
							0f,
							-0.1f,
							0f
						),
						true	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}

		yield return new WaitForSeconds(0.5f);

		//reactivate input
		InputManager.instance.isInputActive = true;
	}
	
	/*//return a relative position of a position relative to another position. 0 is top left, 1 is top right, 2 is right, 3 is bottom right, 4 is bottom left, 5 is left
	public int RelativePositionToAPosition (Vector3 anchorPosition, Vector3 positionToCheck)
	{
		if 

		return -1; 
	}*/

	// rotate tiles using a Unit as a pivot, 1 for clockwise, 2 for counter clockwise
	public IEnumerator RotateTilesAroundAUnit (Unit unitWhoRotates, int direction)
	{
		ResetForRotation();

		Dot dotWhoRotates = dots[unitWhoRotates.associatedDots[0]];

		List<int> tileIDsToRotate = new List<int>();

		tileIDsToRotate = SearchTileIDsAroundAPosition(unitWhoRotates.associatedGameObject.transform.position);
		
		//Debug.Log("Tile IDs around total: " + tileIDsToRotate.Count);

		/*for (int i = 0; i < tileIDsToRotate.Count; i++)
		{
			Debug.Log("Tile ID around: " + tileIDsToRotate[i]);
		}	*/

		List<int> dotIDsToRotate = new List<int>();

		dotIDsToRotate = dots[unitWhoRotates.associatedDots[0]].neighboringDots;

		List<int> unitIDsToRotate = new List<int>();

		//search through all rotated dots whether there's a Unit on top of them or not
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			for (int j = 0; j < dotIDsToRotate.Count; j++)
			{
				//if there's a unit on top of a rotated Dot and it's rotatable
				if (UnitManager.instance.units[i].associatedDots[0] == dotIDsToRotate[j] && UnitManager.instance.units[i].isRotatable)
				{
					//put it to the list
					unitIDsToRotate.Add(i);
				}
			}
		}

		//save to game manager 
		for (int i = 0; i < unitIDsToRotate.Count; i++)
		{
			GameManager.instance.rotatedUnits.Add(UnitManager.instance.units[unitIDsToRotate[i]]);
		}

		//Debug.Log("Tile ID 0 around: " + tileIDsAround[0]);
		//Debug.Log("Tile ID 1 around: " + tileIDsAround[1]);

		//if clockwise
		if (direction == 1)
		{
			if (GameManager.instance.uiType == 1)
			{
				//change the value of rotationTracker
				if (GameManager.instance.rotationTracker < 1)
				{
					GameManager.instance.rotationTracker += 1;
				}
			}
			else
			if (GameManager.instance.uiType == 2)
			{
				//change the value of rotationTracker
				if (GameManager.instance.rotationTracker < 1)
				{
					GameManager.instance.rotationTracker += 1;
				}
		
				//animate the rotation point icon
				GameObject playerStatusWindow = UIManager.instance.uiCanvas.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
				GameObject rotationPointIcon = playerStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(unitWhoRotates.rotationPoint-1).gameObject;

				if (GameManager.instance.rotationTracker != 0)
				{
					rotationPointIcon.GetComponent<Animator>().Play("Blink");
				}
				else
				{
					rotationPointIcon.GetComponent<Animator>().Play("Idle");
				}
			}

			//Debug.Log("Rotate Clockwise");
	
			float yTargetRotation = unitWhoRotates.associatedGameObject.transform.localEulerAngles.y + 60f;

			if (yTargetRotation > 360f)
			{
				yTargetRotation = yTargetRotation - 360f;
			}

			if (yTargetRotation > 180f)
			{
				yTargetRotation = -(360f - yTargetRotation);
			}

			//Debug.Log("Y Target Rotation: " + yTargetRotation);


			
			InputManager.instance.isInputActive = false;

			List<Vector3> tileIDsToRotateLocalPosition = new List<Vector3>();
			List<Vector3> tileIDsToRotateLocalEulerangles = new List<Vector3>();
			List<Vector3> unitIDsToRotatePosition = new List<Vector3>();

			//save each position and eulerangles of tiles that are rotated to a variable
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tileIDsToRotateLocalPosition.Add(tiles[tileIDsToRotate[i]].associatedGameObject.transform.localPosition);
					tileIDsToRotateLocalEulerangles.Add(tiles[tileIDsToRotate[i]].associatedGameObject.transform.localEulerAngles);
				}
				else
				{
					tileIDsToRotateLocalPosition.Add(Vector3.zero);
					tileIDsToRotateLocalEulerangles.Add(Vector3.zero);
				}
			}

			//save each position of units that are rotated to a variable
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				unitIDsToRotatePosition.Add(UnitManager.instance.units[unitIDsToRotate[i]].associatedGameObject.transform.position);
			}

			//put all rotated tiles as rotating unit's children
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.parent = unitWhoRotates.associatedGameObject.transform;
				}
			}

			//put all rotated units as rotating unit's children
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				unit.associatedGameObject.transform.parent = unitWhoRotates.associatedGameObject.transform;
			}

			//tween the rotation of unit who rotates
			HOTween.To
			(
				unitWhoRotates.associatedGameObject.transform, 
				0.5f, 
				new TweenParms()
			        .Prop
					(
						"localEulerAngles", 
						new Vector3
						(
							0f,
							60f,
							0f
						),
						true
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);

			//tween the units so the rotation is the same
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				HOTween.To
				(
					unit.associatedGameObject.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"eulerAngles", 
							new Vector3
							(
								0f,
								unit.associatedGameObject.transform.eulerAngles.y,
								0f
							),
							false
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
				);
			}

			//wait for sometime
			yield return new WaitForSeconds(0.5f);

			//put all rotated tiles back to tiles container
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.parent = tilesContainer.transform;
				}
			}

			//put all rotated units back to units container
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				unit.associatedGameObject.transform.parent = UnitManager.instance.unitsContainer.transform;
			}

			//reset each localposition and localeulerangles of tiles that has been rotated
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.localPosition = tileIDsToRotateLocalPosition[i];
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.localEulerAngles = tileIDsToRotateLocalEulerangles[i];
				}
			}

			//reset each position of units that has been rotated
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				unit.associatedGameObject.transform.position = unitIDsToRotatePosition[i];
			}

			//InputManager.instance.isInputActive = true;



			unitWhoRotates.associatedGameObject.transform.localEulerAngles = new Vector3(0f, yTargetRotation, 0f);

			//if top left tile exist
			if (tileIDsToRotate[0] > -1)
			{
				//move and rotate top left tile to top tile
				tiles[tileIDsToRotate[0]].associatedGameObject.transform.localPosition += new Vector3(0.3005f ,0f , 0.1621f);
				tiles[tileIDsToRotate[0]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
			}

			//if top tile exist
			if (tileIDsToRotate[1] > -1)
			{
				//move and rotate top tile to top right tile
				tiles[tileIDsToRotate[1]].associatedGameObject.transform.localPosition += new Vector3(0.3005f ,0f , -0.1621f);
				tiles[tileIDsToRotate[1]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			}

			//if top right tile exist
			if (tileIDsToRotate[2] > -1)
			{
				//move and rotate top right tile to bottom right tile
				tiles[tileIDsToRotate[2]].associatedGameObject.transform.localPosition += new Vector3(0f , 0f, -0.3413f);
				tiles[tileIDsToRotate[2]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
			}

			//if bottom right tile exist
			if (tileIDsToRotate[3] > -1)
			{
				//move and rotate bottom right tile to bottom tile
				tiles[tileIDsToRotate[3]].associatedGameObject.transform.localPosition += new Vector3(-0.3005f , 0f, -0.1621f);
				tiles[tileIDsToRotate[3]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			}

			//if bottom  tile exist
			if (tileIDsToRotate[4] > -1)
			{
				//move and rotate bottom tile to bottom left tile
				tiles[tileIDsToRotate[4]].associatedGameObject.transform.localPosition += new Vector3(-0.3005f , 0f, 0.1621f);
				tiles[tileIDsToRotate[4]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
			}

			//if bottom left tile exist
			if (tileIDsToRotate[5] > -1)
			{
				//move and rotate bottom left tile to top left tile
				tiles[tileIDsToRotate[5]].associatedGameObject.transform.localPosition += new Vector3(0f , 0f, 0.3413f);
				tiles[tileIDsToRotate[5]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			}

			//rotate all rotateable units around
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				//if rotateable unit is on the top left of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 0)
				{
					unit.associatedGameObject.transform.position += new Vector3(0.601f, 0f, 0f);
				}
				else
				//if rotateable unit is on the top right  of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 1)
				{
					unit.associatedGameObject.transform.position += new Vector3(0.3005f, 0f, -0.5033f);
				}
				else
				//if rotateable unit is on the right of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 2)
				{
					unit.associatedGameObject.transform.position += new Vector3(-0.3005f, 0f, -0.5033f);
				}
				else
				//if rotateable unit is on the bottom right of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 3)
				{
					unit.associatedGameObject.transform.position += new Vector3(-0.601f, 0f, 0f);
				}
				else
				//if rotateable unit is on the bottom left  of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 4)
				{
					unit.associatedGameObject.transform.position += new Vector3(-0.3005f, 0f, 0.5033f);
				}
				else
				//if rotateable unit is on the left  of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 5)
				{
					unit.associatedGameObject.transform.position += new Vector3(0.3005f, 0f, 0.5033f);
				}
			}
		}
		else
		//if counter clockwise
		if (direction == 2)
		{
			if (GameManager.instance.uiType == 1)
			{
				//change the value of rotationTracker
				if (GameManager.instance.rotationTracker > -1)
				{
					GameManager.instance.rotationTracker -= 1;
				}
			}
			else
			if (GameManager.instance.uiType == 2)
			{
				//Debug.Log("Yppp");

				//change the value of rotationTracker
				if (GameManager.instance.rotationTracker > -1)
				{
					GameManager.instance.rotationTracker -= 1;
				}

				//animate the rotation point icon
				GameObject playerStatusWindow = UIManager.instance.uiCanvas.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
				GameObject rotationPointIcon = playerStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(unitWhoRotates.rotationPoint-1).gameObject;

				if (GameManager.instance.rotationTracker != 0)
				{
					rotationPointIcon.GetComponent<Animator>().Play("Blink");
				}
				else
				{
					rotationPointIcon.GetComponent<Animator>().Play("Idle");
				}
			}

			//Debug.Log("Rotate Counter Clockwise");
	
			float yTargetRotation = unitWhoRotates.associatedGameObject.transform.localEulerAngles.y - 60f;

			if (yTargetRotation > 360f)
			{
				yTargetRotation = yTargetRotation - 360f;
			}

			if (yTargetRotation > 180f)
			{
				yTargetRotation = -(360f - yTargetRotation);
			}

			//Debug.Log("Y Target Rotation: " + yTargetRotation);



			InputManager.instance.isInputActive = false;

			List<Vector3> tileIDsToRotateLocalPosition = new List<Vector3>();
			List<Vector3> tileIDsToRotateLocalEulerangles = new List<Vector3>();
			List<Vector3> unitIDsToRotatePosition = new List<Vector3>();

			//save each position and eulerangles of tiles that are rotated to a variable
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tileIDsToRotateLocalPosition.Add(tiles[tileIDsToRotate[i]].associatedGameObject.transform.localPosition);
					tileIDsToRotateLocalEulerangles.Add(tiles[tileIDsToRotate[i]].associatedGameObject.transform.localEulerAngles);
				}
				else
				{
					tileIDsToRotateLocalPosition.Add(Vector3.zero);
					tileIDsToRotateLocalEulerangles.Add(Vector3.zero);
				}
			}

			//save each position of units that are rotated to a variable
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				unitIDsToRotatePosition.Add(UnitManager.instance.units[unitIDsToRotate[i]].associatedGameObject.transform.position);
			}

			//put all rotated tiles as rotating unit's children
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.parent = unitWhoRotates.associatedGameObject.transform;
				}
			}

			//put all rotated units as rotating unit's children
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				unit.associatedGameObject.transform.parent = unitWhoRotates.associatedGameObject.transform;
			}

			//tween the rotation of unit who rotates
			HOTween.To
			(
				unitWhoRotates.associatedGameObject.transform, 
				0.5f, 
				new TweenParms()
			        .Prop
					(
						"localEulerAngles", 
						new Vector3
						(
							0f,
							-60f,
							0f
						),
						true
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);

			//tween the units so the rotation is the same
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				HOTween.To
				(
					unit.associatedGameObject.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"eulerAngles", 
							new Vector3
							(
								0f,
								unit.associatedGameObject.transform.eulerAngles.y,
								0f
							),
							false
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
				);
			}

			//wait for sometime
			yield return new WaitForSeconds(0.5f);

			//put all rotated tiles back to tiles container
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.parent = tilesContainer.transform;
				}
			}

			//put all rotated units back to units container
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				unit.associatedGameObject.transform.parent = UnitManager.instance.unitsContainer.transform;
			}

			//reset each localposition and localeulerangles of tiles that has been rotated
			for (int i = 0; i < tileIDsToRotate.Count; i++)
			{
				if (tileIDsToRotate[i] > -1)
				{
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.localPosition = tileIDsToRotateLocalPosition[i];
					tiles[tileIDsToRotate[i]].associatedGameObject.transform.localEulerAngles = tileIDsToRotateLocalEulerangles[i];
				}
			}

			//reset each position of units that has been rotated
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				unit.associatedGameObject.transform.position = unitIDsToRotatePosition[i];
			}



			unitWhoRotates.associatedGameObject.transform.localEulerAngles = new Vector3(0f, yTargetRotation, 0f);

			//if top left tile exist
			if (tileIDsToRotate[0] > -1)
			{
				//move and rotate top left tile to bottom left tile
				tiles[tileIDsToRotate[0]].associatedGameObject.transform.localPosition += new Vector3(0f , 0f, -0.3413f);
				tiles[tileIDsToRotate[0]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
			}

			//if top tile exist
			if (tileIDsToRotate[1] > -1)
			{
				//move and rotate top tile to top left tile
				tiles[tileIDsToRotate[1]].associatedGameObject.transform.localPosition += new Vector3(-0.3005f, 0f , -0.1621f);
				tiles[tileIDsToRotate[1]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			}

			//if top right tile exist
			if (tileIDsToRotate[2] > -1)
			{
				//move and rotate top right tile to top tile
				tiles[tileIDsToRotate[2]].associatedGameObject.transform.localPosition += new Vector3(-0.3005f , 0f, 0.1621f);
				tiles[tileIDsToRotate[2]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
			}

			//if bottom right tile exist
			if (tileIDsToRotate[3] > -1)
			{
				//move and rotate bottom right tile to top right tile
				tiles[tileIDsToRotate[3]].associatedGameObject.transform.localPosition += new Vector3(0f , 0f, 0.3413f);
				tiles[tileIDsToRotate[3]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			}

			//if bottom tile exist
			if (tileIDsToRotate[4] > -1)
			{
				//move and rotate bottom tile to bottom right tile
				tiles[tileIDsToRotate[4]].associatedGameObject.transform.localPosition += new Vector3(0.3005f , 0f, 0.1621f);
				tiles[tileIDsToRotate[4]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
			}

			//if bottom left tile exist
			if (tileIDsToRotate[5] > -1)
			{
				//move and rotate bottom left tile to bottom tile
				tiles[tileIDsToRotate[5]].associatedGameObject.transform.localPosition += new Vector3(0.3005f , 0f, -0.1621f);
				tiles[tileIDsToRotate[5]].associatedGameObject.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
			}

			//rotate all rotateable units around
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				Unit unit = UnitManager.instance.units[unitIDsToRotate[i]];

				//if rotateable unit is on the top left of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 0)
				{
					unit.associatedGameObject.transform.position += new Vector3(-0.3005f, 0f, -0.5033f);
				}
				else
				//if rotateable unit is on the top right  of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 1)
				{
					unit.associatedGameObject.transform.position += new Vector3(-0.601f, 0f, 0f);
				}
				else
				//if rotateable unit is on the right of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 2)
				{
					unit.associatedGameObject.transform.position += new Vector3(-0.3005f, 0f, 0.5033f);
				}
				else
				//if rotateable unit is on the bottom right of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 3)
				{
					unit.associatedGameObject.transform.position += new Vector3(0.3005f, 0f, 0.5033f);
				}
				else
				//if rotateable unit is on the bottom left  of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 4)
				{
					unit.associatedGameObject.transform.position += new Vector3(0.601f, 0f, 0f);
				}
				else
				//if rotateable unit is on the left  of a Unit who rotates
				if (dotWhoRotates.neighboringDots.IndexOf(unit.associatedDots[0]) == 5)
				{
					unit.associatedGameObject.transform.position += new Vector3(0.3005f, 0f, -0.5033f);
				}
			}
		}


		//hide highlight AFTER confirm
		//HideHighlightOnTilesAroundAUnit(unitWhoRotates);

		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			int dotIDBelow = SearchDotIDByPosition(UnitManager.instance.units[i].associatedGameObject.transform.position);
			
			if (dotIDBelow == -1)
			{
				//kill the unit TODO remove from unit list, delete gameobject from hierarchy
			}
		}

		GenerateDots();
		
		//put associated dots of a Tile
		for (int i = 0; i < tiles.Count; i++)
		{
			AssignDotsToATile(tiles[i]);
		}
	
		//assign neighbouring dots to each dot
		for (int i = 0; i < dots.Count; i++)
		{
			AssignDotWithNeighbourDots(dots[i]);
		}

		//check dots if still has at least one neighbour. if not, destroy it
		for (int i = 0; i < dots.Count; i++)
		{
			for (int j = 0; j < dots[i].neighboringDots.Count; j++)
			{
				if (dots[i].neighboringDots[j] > -1)
				{
					break;
				}

				if (j == 5)
				{
					Destroy(dots[i].associatedGameObject);
					dots.Remove(dots[i]);
				}
			}
		}

		//assign direction and dots to a Unit
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			UnitManager.instance.AssignDirectionToAUnit(UnitManager.instance.units[i]);
			UnitManager.instance.AssignDotsToAUnit(UnitManager.instance.units[i]);
		}

		if (GameManager.instance.uiType == 2)
		{
			//assign text to a cursor
			UIManager.instance.ShowRotationActionPointTextOnCursor(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, "" + Mathf.Abs(GameManager.instance.rotationTracker));
		}

		if (GameManager.instance.rotationTracker != 0)
		{
			#region DETERMINE CROSSFIRE
			for (int i = 0; i < unitIDsToRotate.Count; i++)
			{
				//if this is an enemy and not a Dimension Gate
				if (UnitManager.instance.units[unitIDsToRotate[i]].unitID >= 100 && UnitManager.instance.units[unitIDsToRotate[i]].unitID < 200 && UnitManager.instance.units[unitIDsToRotate[i]].unitID != 102)
				{
					Unit enemyUnit = UnitManager.instance.units[unitIDsToRotate[i]];
					List<Unit> playerUnit = new List<Unit>();
		
					//Debug.Log("Enemy Unit that is rotated: " + unitIDsToRotate[i]);
	
					for (int j = 0; j < UnitManager.instance.units.Count; j++)
					{
						if (UnitManager.instance.IsAUnitWithinAnotherUnitAttackRange(enemyUnit, UnitManager.instance.units[j]))
						{
							//Debug.Log("Enemy Unit with ID: " + UnitManager.instance.units.IndexOf(enemyUnit) + " is within attack range of Player Unit with ID: " + j);
		
							if (!UnitManager.instance.IsPlayerUnitOverlappedWithEnemyUnit(UnitManager.instance.units[j]))
							{
								playerUnit.Add(UnitManager.instance.units[j]);
							}
						}
					}
	
					if (playerUnit.Count > 1)
					{
						for (int j = 0; j < playerUnit.Count; j++)
						{
							//Debug.Log("Player Unit with ID " + UnitManager.instance.units.IndexOf(playerUnit[j]) + " will crossfire Enemy Unit with ID " + UnitManager.instance.units.IndexOf(enemyUnit));
	
							//if this is not the player
							//if (playerUnit[j] != unitWhoRotates)
							//{
								/*HOTween.To
								(
									playerUnit[j].associatedGameObject.transform, 
									0.5f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0.1f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.EaseOutBack)
										.Delay(0f)
								);*/

							Material yellowMaterial = Instantiate(playerUnit[j].associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material);
							yellowMaterial.color = Color.yellow;
							//yellowMaterial.name = "Yellow";

							playerUnit[j].associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = yellowMaterial;
							//}

							crossfiredEnemyUnits.Add(enemyUnit);
							crossfiringPlayerUnits.Add(playerUnit[j]);
						}
					}
				}
			}
			#endregion
		}

		//if crossfire
		if (crossfiringPlayerUnits.Count > 0 && GameManager.instance.rotationTracker != 0)
		{
			//create a container for lines
			GameObject lineParent = new GameObject();
			lineParent.name = "AttackLines";

			for (int i = 0; i < crossfiringPlayerUnits.Count; i++)
			{
				//create objects in the middle of crossfire and crossfiring units
				GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);

				Vector3 location = crossfiringPlayerUnits[i].associatedGameObject.transform.position + crossfiredEnemyUnits[i].associatedGameObject.transform.position;

				line.name = "Line";
				
				//if Red Soldier
				if (crossfiringPlayerUnits[i].unitID == 1)
				{
					line.transform.position = new Vector3(location.x/2f, -3f, location.z/2f);
					line.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				else
				{
					line.transform.position = new Vector3(location.x/2f, -3f, location.z/2f);
					line.transform.localScale = new Vector3(0.1f, 0.1f, 0.5f);
				}

				//add mesh renderer to it and change the color
				//line.AddComponent<MeshRenderer>();
				line.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color"));
				line.GetComponent<MeshRenderer>().material.color = Color.yellow; 

				int directionToCrossfiredUnits = UnitManager.instance.GetDirectionFromAUnitToAnotherUnit(crossfiringPlayerUnits[i], crossfiredEnemyUnits[i]);

				//if on top left or bottom right
				if (directionToCrossfiredUnits == 0 || directionToCrossfiredUnits == 3)
				{
					line.transform.localEulerAngles = new Vector3(0f, -30f, 0f);
				}
				else
				//if on top right or bottom left
				if (directionToCrossfiredUnits == 1 || directionToCrossfiredUnits == 4)
				{
					line.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
				}
				else
				//if on right or left
				if (directionToCrossfiredUnits == 2 || directionToCrossfiredUnits == 5)
				{
					line.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
				}

				//put this as a child of Lines container
				line.transform.parent = lineParent.transform;
			}
		}
		//otherwise
		else
		{
		
		}

		yield return new WaitForSeconds(0.5f);

		for (int i = 1; i < dotsContainer.transform.childCount; i++)
		{
			if (!dotsContainer.transform.GetChild(i).gameObject.activeSelf)
			{
				Destroy(dotsContainer.transform.GetChild(i).gameObject);
			}
		}

		InputManager.instance.isInputActive = true;
	}

	// to know whether action options window is showed or not
	public bool IsDotShowingActionOptionsWindow (Dot dotToCheck)
	{
		if (dotToCheck.associatedGameObject.transform.GetChild(2).gameObject.activeSelf)
		{
			return true;
		}

		return false;
	}

	// to know whether a player is highlighting exit button in action options window or not while selecting a dot
	public bool IsDotHighlightingExitButton (Dot dotToCheck)
	{
		if (GameManager.instance.uiType == 2)
		{
			GameObject actionOptionsWindow = dotToCheck.associatedGameObject.transform.GetChild(2).gameObject;
		
			TextMesh[] textMeshes = actionOptionsWindow.transform.GetComponentsInChildren<TextMesh>();
		
			for (int i = 0; i < textMeshes.Length; i++)
			{
				if (textMeshes[i].text == "EXIT" && textMeshes[i].gameObject.activeSelf)
				{
					if (textMeshes[i].gameObject.transform.childCount > 0)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	// to know whether a player is highlighting end turn button in action options window or not while selecting a dot
	public bool IsDotHighlightingEndButton (Dot dotToCheck)
	{
		if (GameManager.instance.uiType == 2)
		{
			GameObject actionOptionsWindow = dotToCheck.associatedGameObject.transform.GetChild(2).gameObject;
		
			TextMesh[] textMeshes = actionOptionsWindow.transform.GetComponentsInChildren<TextMesh>();
		
			for (int i = 0; i < textMeshes.Length; i++)
			{
				if (textMeshes[i].text == "END" && textMeshes[i].gameObject.activeSelf)
				{
					if (textMeshes[i].gameObject.transform.childCount > 0)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	public void ResetForRotation ()
	{
		//Debug.Log("Reset for rotation!");

		//reset height y position of each crossfiring units
		/*for (int i = 0; i < crossfiringPlayerUnits.Count; i++)
		{
			crossfiringPlayerUnits[i].associatedGameObject.transform.position = new Vector3(crossfiringPlayerUnits[i].associatedGameObject.transform.position.x, crossfiringPlayerUnits[i].initYPosition, crossfiringPlayerUnits[i].associatedGameObject.transform.position.z);
		}*/

		//reset crossfiring units color
		for (int i = 0; i < crossfiringPlayerUnits.Count; i++)
		{
			//Debug.Log("Reset material!");

			crossfiringPlayerUnits[i].associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = crossfiringPlayerUnits[i].initModelMaterial;
		}

		//reset bak crossfired and crossfiring units
		crossfiredEnemyUnits = new List<Unit>();
		crossfiringPlayerUnits = new List<Unit>();

		GameManager.instance.rotatedUnits.Clear();

		//hide Lines container
		GameObject lines = GameObject.Find("AttackLines");
		
		if (lines != null)
		{
			lines.SetActive(false);
		}
	}

	// put a Dot to idle mode
	public IEnumerator PutADotToIdle (Dot dotToPut)
	{
		//deactivate input for a while
		InputManager.instance.isInputActive = false;

		//hide cursor
		GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);

		//hide all move text
		UIManager.instance.HideAllMoveText();
		
		//hide all rotate text
		UIManager.instance.HideAllRotateText();
		
		//hide all attack text
		UIManager.instance.HideAllAttackText();
		
		//hide all stay text
		UIManager.instance.HideAllStayText();
	
		//reset everything
		GameManager.instance.rotationTracker = 0;
		GameManager.instance.distanceCursorToSelectedUnit = 0;
		GameManager.instance.movementTracker.Clear();
		GameManager.instance.attackTracker.Clear();
		GameManager.instance.actionPointNeededToAttack = 0;
		TileDotManager.instance.crossfiredEnemyUnits.Clear();
		TileDotManager.instance.crossfiringPlayerUnits.Clear();

		yield return new WaitForSeconds(0.15f);

		//reactivate input 
		InputManager.instance.isInputActive = true;

		//show cursor on the selected dot
		GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, dotToPut);
	}
}
