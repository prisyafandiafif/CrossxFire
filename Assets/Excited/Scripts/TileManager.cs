using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
// A class to hold information of each tile
public class TileTile	 
{
	//tile ID. 0 is Normal Tile, 1 is //TODO add description of tileID in the comment
	public int tileID; 

	//associated GameObject of tile in the gameplay
	public GameObject associatedGameObject;

	//associated dots ID at this tile
	public List<int> associatedDots = new List<int>();
}

public class TileManager : MonoBehaviour 
{
	public static TileManager instance;

	// A gameobject that contains all tiles
	public GameObject tilesContainer;

	// Unit size of a tile
	public float tileUnitSize;

	// A list to hold all materials of Tile
	public List<Material> tileMaterials = new List<Material>();

	// A list to hold the Tile class
	public List<TileTile> tiles = new List<TileTile>();

	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
		//put all tiles into list
		GenerateTiles();

		//set material to all tiles
		SetMaterialToTiles();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// Generate tiles and put it into tiles list (heavy computation process)
	public void GenerateTiles ()
	{	
		for (int i = 0; i < tilesContainer.transform.childCount; i++)
		{
			GameObject tile = tilesContainer.transform.GetChild(i).gameObject;

			//put it inside Tile class
			TileTile toPutTile = new TileTile();

			//put the actual gameobject into the variable
			toPutTile.associatedGameObject = tile;
	
			//TODO put more if there are more tile variations
			//if normal tile
			if (toPutTile.associatedGameObject.GetComponent<MeshRenderer>().material.name == "tile1" ||
				toPutTile.associatedGameObject.GetComponent<MeshRenderer>().material.name == "tile2" ||
				toPutTile.associatedGameObject.GetComponent<MeshRenderer>().material.name == "tile3" ||
				toPutTile.associatedGameObject.GetComponent<MeshRenderer>().material.name == "tile4" ||
				toPutTile.associatedGameObject.GetComponent<MeshRenderer>().material.name == "tile5" ||
				toPutTile.associatedGameObject.GetComponent<MeshRenderer>().material.name == "tile6" ||
				toPutTile.associatedGameObject.GetComponent<MeshRenderer>().material.name == "tile7")
			{
				toPutTile.tileID = 0;
			}		

			tiles.Add(toPutTile);
		}
	}

	// Set a random material of each tile 
	public void SetMaterialToTiles ()
	{
		for (int i = 0; i < tilesContainer.transform.childCount; i++)
		{
			GameObject tile = tilesContainer.transform.GetChild(i).gameObject;

			int randomMaterialID = Random.Range(0, 7);

			//Debug.Log("O: " + randomMaterialID);

			tile.GetComponent<MeshRenderer>().material = tileMaterials[randomMaterialID];
		}
	}
}
