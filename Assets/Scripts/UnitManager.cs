using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;
using Holoville.HOTween.Core;

[System.Serializable]
// A class to hold information of each unity
public class Unit	 
{
	//unit ID. 0 is Green Soldier, 1 is Red Soldier, 100 is Stomper Monster, 101 is Shooter Monster //TODO add description of unitID in the comment
	public int unitID; 

	//unit name
	public string unitName;

	//unit icon
	public Image unitIcon;

	//equipped item ID
	public int equippedItemID;

	//health point
	public int healthPoint;

	//full health point
	public int fullHealthPoint;

	//damage point
	public int damagePoint;

	//rotation point
	public int rotationPoint;

	//full rotation point
	public int fullRotationPoint;

	//stamina 
	public int stamina;

	//is this unit rotatable by a Soldier or not
	public bool isRotatable = false; 

	//direction. 0 is top left, 1 is top right, 2 is right, 3 is bottom right, 4 is bottom left, 5 is left
	public int direction;

	//to know whether this unit is selected or not (for doing action, etc)
	public bool isSelected;

	//to save init model material of each unit
	public Material initModelMaterial;

	//to save init y position of each unit (height)
	//public float initYPosition;

	//associated GameObject of unit in the gameplay
	public GameObject associatedGameObject;

	//associated dots ID of this unit
	public List<int> associatedDots = new List<int>();
}

public class UnitManager : MonoBehaviour 
{
	public static UnitManager instance;

	// A container of all units
	public GameObject unitsContainer;

	// gameobject of each unit
	public GameObject[] playerUnitsTemplate;

	// gameobject of each enemy units
	public GameObject[] enemyUnitsTemplate;

	// An array to hold the Unit class
	public List<Unit> units = new List<Unit>();

	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
		//put units into list
		GenerateUnits();

		//assign direction and dots to a Unit
		for (int i = 0; i < units.Count; i++)
		{
			AssignDirectionToAUnit(units[i]);
			AssignDotsToAUnit(units[i]);
		}

		//Debug.Log("Distance: " + Vector3.Distance(units[0].associatedGameObject.transform.position, units[2].associatedGameObject.transform.position));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// put units into Units list (heavy computation process)
	public void GenerateUnits ()
	{
		for (int i = 0; i < unitsContainer.transform.childCount; i++)
		{
			GameObject unit = unitsContainer.transform.GetChild(i).gameObject;

			//put it inside Unit class
			Unit toPutUnit = new Unit();

			toPutUnit.associatedGameObject = unit;
	
			bool isUnitAlreadyExist = false;

			//check if unit already exists in the list or not
			for (int h = 0; h < units.Count; h++)
			{
				if (units[h].associatedGameObject == toPutUnit.associatedGameObject)
				{
					isUnitAlreadyExist = true;

					Debug.Log("True");
				}
			}	

			//if not exist yet
			if (!isUnitAlreadyExist)
			{
				//TODO put more if if there are more tile variations
				//if Green Soldier
				if (toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.name == "green (Instance)")
				{
					toPutUnit.unitID = 0;
					toPutUnit.unitName = "Shocktrooper";
					toPutUnit.unitIcon = null; //TODO
					toPutUnit.equippedItemID = 0;
					toPutUnit.isRotatable = false;
					toPutUnit.healthPoint = 1;
					toPutUnit.fullHealthPoint = toPutUnit.healthPoint;
					toPutUnit.damagePoint = 1;
					toPutUnit.rotationPoint = 2;
					toPutUnit.fullRotationPoint = toPutUnit.rotationPoint;
					toPutUnit.stamina = 0;
					//toPutUnit.initYPosition = toPutUnit.associatedGameObject.transform.position.y;
					toPutUnit.initModelMaterial = toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
				}		
				else
				//if Red Soldier
				if (toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.name == "red (Instance)")
				{
					toPutUnit.unitID = 1;
					toPutUnit.unitName = "Thrower";
					toPutUnit.unitIcon = null;
					toPutUnit.equippedItemID = 0;
					toPutUnit.isRotatable = false;
					toPutUnit.healthPoint = 1;
					toPutUnit.fullHealthPoint = toPutUnit.healthPoint;
					toPutUnit.damagePoint = 1;
					toPutUnit.rotationPoint = 2;
					toPutUnit.fullRotationPoint = toPutUnit.rotationPoint;
					toPutUnit.stamina = 0;
					//toPutUnit.initYPosition = toPutUnit.associatedGameObject.transform.position.y;
					toPutUnit.initModelMaterial = toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
				}		
				else
				//if Power Plant
				if (toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.name == "white (Instance)")
				{
					toPutUnit.unitID = 2;
					toPutUnit.unitName = "Power Plant";
					toPutUnit.unitIcon = null; //TODO
					toPutUnit.equippedItemID = 0;
					toPutUnit.isRotatable = false;
					toPutUnit.healthPoint = 1;
					toPutUnit.fullHealthPoint = toPutUnit.healthPoint;
					toPutUnit.damagePoint = 0;
					toPutUnit.rotationPoint = 0;
					toPutUnit.fullRotationPoint = toPutUnit.rotationPoint;
					toPutUnit.stamina = 0;
					//toPutUnit.initYPosition = toPutUnit.associatedGameObject.transform.position.y;
					toPutUnit.initModelMaterial = toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
				}		
				else
				//if Stomper Monster
				if (toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.name == "blue (Instance)")
				{
					toPutUnit.unitID = 100;
					toPutUnit.unitName = "Stomper";
					toPutUnit.unitIcon = null; //TODO
					toPutUnit.equippedItemID = 0;
					toPutUnit.isRotatable = true;
					toPutUnit.healthPoint = 6;
					toPutUnit.fullHealthPoint = toPutUnit.healthPoint;
					toPutUnit.damagePoint = 1;
					toPutUnit.rotationPoint = 0;
					toPutUnit.fullRotationPoint = toPutUnit.rotationPoint;
					toPutUnit.stamina = 0;
					//toPutUnit.initYPosition = toPutUnit.associatedGameObject.transform.position.y;
					toPutUnit.initModelMaterial = toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
				}		
				else
				//if Shooter Monster
				if (toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.name == "grey (Instance)")
				{
					toPutUnit.unitID = 101;
					toPutUnit.unitName = "Lasershooter";
					toPutUnit.unitIcon = null; //TODO
					toPutUnit.equippedItemID = 0;
					toPutUnit.isRotatable = true;
					toPutUnit.healthPoint = 6;
					toPutUnit.fullHealthPoint = toPutUnit.healthPoint;
					toPutUnit.damagePoint = 1;
					toPutUnit.rotationPoint = 0;
					toPutUnit.fullRotationPoint = toPutUnit.rotationPoint;
					toPutUnit.stamina = 0;
					//toPutUnit.initYPosition = toPutUnit.associatedGameObject.transform.position.y;
					toPutUnit.initModelMaterial = toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
				}		
				else
				//if Dimensional Rim
				if (toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.name == "dark_green (Instance)")
				{
					toPutUnit.unitID = 102;
					toPutUnit.unitName = "Dimension G.";
					toPutUnit.unitIcon = null; //TODO
					toPutUnit.equippedItemID = 0;
					toPutUnit.isRotatable = false;
					toPutUnit.healthPoint = 1;
					toPutUnit.fullHealthPoint = toPutUnit.healthPoint;
					toPutUnit.damagePoint = 0;
					toPutUnit.rotationPoint = 0;
					toPutUnit.fullRotationPoint = toPutUnit.rotationPoint;
					toPutUnit.stamina = 0;
					//toPutUnit.initYPosition = toPutUnit.associatedGameObject.transform.position.y;
					toPutUnit.initModelMaterial = toPutUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
				}		
	
				units.Add(toPutUnit);
			}
		}
	}

	// assign direction to a unit
	public void AssignDirectionToAUnit (Unit unitToCheck)
	{
		GameObject unitGameObject = unitToCheck.associatedGameObject;

		//Debug.Log("Wow: " + units[0].associatedGameObject.transform.localRotation.eulerAngles.y);

		//if facing top left
		if (unitGameObject.transform.localRotation.eulerAngles.y > 329f && unitGameObject.transform.localRotation.eulerAngles.y < 331f)
		{
			//Debug.Log("Assign direction 0");

			unitToCheck.direction = 0;
		}
		else
		//if facing top right
		if (unitGameObject.transform.localRotation.eulerAngles.y > 29f && unitGameObject.transform.localRotation.eulerAngles.y < 31f)
		{
			unitToCheck.direction = 1;
		}
		else
		//if facing right
		if (unitGameObject.transform.localRotation.eulerAngles.y > 89f && unitGameObject.transform.localRotation.eulerAngles.y < 91f)
		{
			unitToCheck.direction = 2;
		}
		else
		//if facing bottom right
		if (unitGameObject.transform.localRotation.eulerAngles.y > 149f && unitGameObject.transform.localRotation.eulerAngles.y < 151f)
		{
			unitToCheck.direction = 3;
		}
		else
		//if facing bottom left
		if (unitGameObject.transform.localRotation.eulerAngles.y > 209f && unitGameObject.transform.localRotation.eulerAngles.y < 211f)
		{
			unitToCheck.direction = 4;
		}
		else
		//if facing left
		if (unitGameObject.transform.localRotation.eulerAngles.y > 269f && unitGameObject.transform.localRotation.eulerAngles.y < 271f)
		{
			unitToCheck.direction = 5;
		}
	}
	
	// assign dots to a unit
	public void AssignDotsToAUnit (Unit unitToAssign)
	{
		float xPosToCheck = unitToAssign.associatedGameObject.transform.position.x;
		float yPosToCheck = TileDotManager.instance.dotsContainer.transform.GetChild(0).gameObject.transform.position.y;
		float zPosToCheck = unitToAssign.associatedGameObject.transform.position.z;

		//clear it first
		unitToAssign.associatedDots.Clear();

		//put the associated dot to the Tile
		unitToAssign.associatedDots.Add(TileDotManager.instance.SearchDotIDByPosition(new Vector3(xPosToCheck, yPosToCheck, zPosToCheck)));	
	}

	// update stamina points for enemies depends on how many action points that enemy has right now
	public void UpdateEnemyUnitsStamina ()
	{
		int actionPointsToDistribute = Mathf.CeilToInt((GameManager.instance.actionPoints[1]*1f) / 2f);

		for (int i = 0; i < GameManager.instance.orderOfEnemyUnits.Count; i++)
		{
			for (int j = 0; j < units.Count; j++)
			{
				if (units[j].associatedGameObject == GameManager.instance.orderOfEnemyUnits[i])
				{
					if (actionPointsToDistribute - GameManager.instance.staminaLimit >= 0)
					{
						units[j].stamina = GameManager.instance.staminaLimit;
					}
					else
					{
						units[j].stamina = actionPointsToDistribute;
					}
		
					actionPointsToDistribute -= units[j].stamina;
				}
			}
		}
	}

	// check whether a Unit exists or not at a certain position
	public bool IsUnitExistAtAPosition (Vector3 unitToCheck)
	{
		for (int i = 0; i < units.Count; i++)
		{
			if (units[i].associatedGameObject.activeSelf &&
				unitToCheck.x > units[i].associatedGameObject.transform.position.x - 0.1f &&
				unitToCheck.x < units[i].associatedGameObject.transform.position.x + 0.1f &&
				unitToCheck.z > units[i].associatedGameObject.transform.position.z - 0.1f &&
				unitToCheck.z < units[i].associatedGameObject.transform.position.z + 0.1f)
			{
				return true;
			}
		}

		return false;
	}

	// return a Unit ID of Player with desired position
	public int SearchPlayerUnitIDByPosition (Vector3 searchPos)
	{
		int searchResultUnitID = -1;

		for (int i = 0; i < units.Count; i++)
		{
			if (searchPos.x > units[i].associatedGameObject.transform.position.x - 0.1f &&
				searchPos.x < units[i].associatedGameObject.transform.position.x + 0.1f &&
				searchPos.z > units[i].associatedGameObject.transform.position.z - 0.1f &&
				searchPos.z < units[i].associatedGameObject.transform.position.z + 0.1f &&
				units[i].unitID >= 0 &&
				units[i].unitID < 100)
			{
				searchResultUnitID = i;

				return searchResultUnitID;
			}
		}

		//searchResultUnitID = null;

		return searchResultUnitID;
	}

	//return a Unit ID of a specific unit type
	public List<int> SearchUnitIDByUnitTypeID (int unitTypeID)
	{
		List<int> unitsWithTypeID = new List<int>();

		for (int i = 0; i < units.Count; i++)
		{
			if (units[i].unitID == unitTypeID)
			{
				unitsWithTypeID.Add(i);
			}
		}

		return unitsWithTypeID;
	}

	//return a Unit ID with resurrection effect
	public List<int> SearchUnitIDByResurrectionEffect ()
	{
		List<int> unitsWithResurrectionEffect = new List<int>();

		for (int i = 0; i < units.Count; i++)
		{
			//if this is a Power Plant or a Leader
			if (units[i].unitID == 2 || units[i].unitID == 3)
			{
				unitsWithResurrectionEffect.Add(i);
			}
		}

		return unitsWithResurrectionEffect;
	}
	
	// return a Unit ID of Enemy with desired position
	public int SearchUnitIDOfEnemyByPosition (Vector3 searchPos)
	{
		int searchResultUnitID = -1;

		for (int i = 0; i < units.Count; i++)
		{
			if (searchPos.x > units[i].associatedGameObject.transform.position.x - 0.1f &&
				searchPos.x < units[i].associatedGameObject.transform.position.x + 0.1f &&
				searchPos.z > units[i].associatedGameObject.transform.position.z - 0.1f &&
				searchPos.z < units[i].associatedGameObject.transform.position.z + 0.1f &&
				units[i].unitID >= 100 &&
				units[i].unitID < 200)
			{
				searchResultUnitID = i;

				return searchResultUnitID;
			}
		}

		//searchResultUnitID = null;

		return searchResultUnitID;
	}

	// return a Unit ID with desired position
	public int SearchUnitIDByPosition (Vector3 searchPos)
	{
		int searchResultUnitID = -1;

		for (int i = 0; i < units.Count; i++)
		{
			if (searchPos.x > units[i].associatedGameObject.transform.position.x - 0.1f &&
				searchPos.x < units[i].associatedGameObject.transform.position.x + 0.1f &&
				searchPos.z > units[i].associatedGameObject.transform.position.z - 0.1f &&
				searchPos.z < units[i].associatedGameObject.transform.position.z + 0.1f)
			{
				searchResultUnitID = i;

				return searchResultUnitID;
			}
		}

		//searchResultUnitID = null;

		return searchResultUnitID;
	}

	// return all Unit IDs with desired position
	public List<int> SearchUnitIDsByPosition (Vector3 searchPos)
	{
		List<int> searchResultUnitIDs = new List<int>();

		for (int i = 0; i < units.Count; i++)
		{
			if (searchPos.x > units[i].associatedGameObject.transform.position.x - 0.1f &&
				searchPos.x < units[i].associatedGameObject.transform.position.x + 0.1f &&
				searchPos.z > units[i].associatedGameObject.transform.position.z - 0.1f &&
				searchPos.z < units[i].associatedGameObject.transform.position.z + 0.1f)
			{
				searchResultUnitIDs.Add(i);
			}
		}

		//searchResultUnitID = null;

		return searchResultUnitIDs;
	}

	// return a List of Unit IDs that are within attack range of another unit //TODO add conditional based on what unit is this
	public List<int> SearchUnitIDsByAttackRangeOfAUnit (Unit unitToCheck)
	{
		List<int> unitsWithinAttackRange = new List<int>();

		int dotIDBelowUnit = TileDotManager.instance.SearchDotIDByPosition(unitToCheck.associatedGameObject.transform.position);
		Dot dotBelowUnit = TileDotManager.instance.dots[dotIDBelowUnit];

		//if Green Soldier
		if (unitToCheck.unitID == 0)
		{
			for (int i = 0; i < dotBelowUnit.neighboringDots.Count; i++)
			{
				//if the neighbor exists
				if (dotBelowUnit.neighboringDots[i] > -1)
				{
					Dot neighboringDot = TileDotManager.instance.dots[dotBelowUnit.neighboringDots[i]];
	
					List<int> searchResultUnitIDs = SearchUnitIDsByPosition(neighboringDot.associatedGameObject.transform.position);
	
					//Debug.Log("Other Units within attack range: " + searchResultUnitID);
		
					//if there's a unit on the neighbor dot
					if (searchResultUnitIDs.Count > 0)
					{
						for (int j = 0; j < searchResultUnitIDs.Count; j++)
						{
							//if there's an Enemy Unit within the attack range and it's not a Dimensional Rim
							if (units[searchResultUnitIDs[j]].unitID >= 100 && units[searchResultUnitIDs[j]].unitID < 200 && units[searchResultUnitIDs[j]].unitID != 102)
							{
								unitsWithinAttackRange.Add(searchResultUnitIDs[j]);
							}
						}
					}
				}
			}
		}
		else
		//if Red Soldier
		if (unitToCheck.unitID == 1)
		{
			for (int i = 0; i < dotBelowUnit.neighboringDots.Count; i++)
			{
				//if the neighbor exists
				if (dotBelowUnit.neighboringDots[i] > -1)
				{
					Dot neighboringDot = TileDotManager.instance.dots[dotBelowUnit.neighboringDots[i]];
	
					if (neighboringDot.neighboringDots[i] > -1)
					{
						Dot neighboringNeighborDot = TileDotManager.instance.dots[neighboringDot.neighboringDots[i]];

						List<int> searchResultUnitIDs = SearchUnitIDsByPosition(neighboringNeighborDot.associatedGameObject.transform.position);
	
						//Debug.Log("Other Units within attack range: " + searchResultUnitID);
			
						//if there's a unit on the neighbor dot
						if (searchResultUnitIDs.Count > 0)
						{
							for (int j = 0; j < searchResultUnitIDs.Count; j++)
							{
								//if there's an Enemy Unit within the attack range and it's not a Dimensional Rim
								if (units[searchResultUnitIDs[j]].unitID >= 100 && units[searchResultUnitIDs[j]].unitID < 200 && units[searchResultUnitIDs[j]].unitID != 102)
								{
									unitsWithinAttackRange.Add(searchResultUnitIDs[j]);
								}
							}
						}
					}
				}
			}
		}
		else
		//if Stomper
		if (unitToCheck.unitID == 100)
		{
			
		}
		else
		//if Shooter
		if (unitToCheck.unitID == 101)
		{
			//if the neighbor dot in the way the face sees exists
			/*if (dotBelowUnit.neighboringDots[unitToCheck.direction] > -1)
			{
				int searchResultUnitID = SearchUnitIDByPosition(TileDotManager.instance.dots[dotBelowUnit.neighboringDots[unitToCheck.direction]].associatedGameObject.transform.position);
			
				if (searchResultUnitID > -1)
				{
					//if there's a Player Unit within the attack range
					if (units[searchResultUnitID].unitID >= 0 && units[searchResultUnitID].unitID < 100)
					{
							unitsWithinAttackRange.Add(searchResultUnitID);
					}
				}
			}*/

			//while dot exists
			while (dotBelowUnit.neighboringDots[unitToCheck.direction] > -1)
			{
				//if the neighbor dot in the way the unit sees exists
				if (dotBelowUnit.neighboringDots[unitToCheck.direction] > -1)
				{
					List<int> searchResultUnitIDs = SearchUnitIDsByPosition(TileDotManager.instance.dots[dotBelowUnit.neighboringDots[unitToCheck.direction]].associatedGameObject.transform.position);

					if (searchResultUnitIDs.Count > 0)
					{
						for (int j = 0; j < searchResultUnitIDs.Count; j++)
						{
							//if there's a Player Unit within the attack range and it's not a Power Plant
							if (units[searchResultUnitIDs[j]].unitID >= 0 && units[searchResultUnitIDs[j]].unitID < 100 && units[searchResultUnitIDs[j]].unitID != 2 && units[searchResultUnitIDs[j]].associatedGameObject.activeSelf)
							{
								unitsWithinAttackRange.Add(searchResultUnitIDs[j]);
	
								//break;
							}
						}
					}

					dotBelowUnit = TileDotManager.instance.dots[dotBelowUnit.neighboringDots[unitToCheck.direction]];
				}
			}
		}

		/*for (int i = 0; i < unitsWithinAttackRange.Count; i++)
		{
			Debug.Log("Units that are in attack range of " + units.IndexOf(unitToCheck) + ": " + unitsWithinAttackRange[i]);
		}*/

		//Debug.Log("Position of Unit 7: " + units[7].associatedGameObject.transform.position);

		/*for (int i = 0; i < units.Count; i++)
		{
			Debug.Log("Unit that is being checked: " + units.IndexOf(unitToCheck));
		}*/

		return unitsWithinAttackRange;
	}

	// is a Unit within a Unit's attack range
	public bool IsAUnitWithinAnotherUnitAttackRange (Unit unitAttacked, Unit unitAttacking)
	{
		List<int> unitsWithinAttackRange = SearchUnitIDsByAttackRangeOfAUnit(unitAttacking);

		/*for (int i = 0; i < unitsWithinAttackRange.Count; i++)
		{
			Debug.Log("Units that are in attack range of " + units.IndexOf(unitAttacking) + ": " + unitsWithinAttackRange[i]);
		}*/

		if (unitsWithinAttackRange.Contains(units.IndexOf(unitAttacked)))
		{
			return true;
		}

		return false;
	}

	// return a selected Unit
	public int SearchUnitIDBySelectedUnit ()
	{
		int searchResultUnitID = -1;

		for (int i = 0; i < units.Count; i++)
		{
			if (units[i].isSelected)
			{
				searchResultUnitID = i;

				return searchResultUnitID;
			}
		}

		return searchResultUnitID;
	}

	// attack a Unit with a Unit 
	public IEnumerator AttackAUnitWithAUnit (Unit attackingUnit, Unit attackedUnit)
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

		string[] attackArray = attackedUnit.associatedGameObject.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text.Split('\n');

		//Debug.Log("Attack text 2: " + attackArray[2]);

		if (GameManager.instance.uiType == 1)
		{
			if (attackArray[2] == "(0 AP)")
			{
				//do nothing
			}
			else
			if (attackArray[2] == "(1 AP)")
			{
				yield return StartCoroutine(GiveNormalActionPoints(1, attackingUnit));
			}
		}
		else
		if (GameManager.instance.uiType == 2)
		{
			if (GameManager.instance.actionPointNeededToAttack > 0)
			{
				if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)	
				{
					//give special action points
					yield return StartCoroutine(GiveSpecialActionPoints(GameManager.instance.actionPointNeededToAttack, GameManager.instance.selectedTypeOfPointsForPlayer-1, attackingUnit));
				}
				else
				{	
					yield return StartCoroutine(GiveNormalActionPoints(GameManager.instance.actionPointNeededToAttack, attackingUnit));
				}		
			}

			//if player's turn
			if (GameManager.instance.turnID == 1)
			{
				if (attackedUnit.unitID >= 100 && attackedUnit.unitID < 200)
				{
					//StartCoroutine(UIManager.instance.HidePlayerStatusWindow());

					//show enemy status window
					StartCoroutine(UIManager.instance.ShowEnemyStatusWindow(attackedUnit));
				}
			}
			else
			//if enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				//if player unit
				if (attackedUnit.unitID >= 0 && attackedUnit.unitID < 100)
				{
					//StartCoroutine(UIManager.instance.HideEnemyStatusWindow());

					//show player status window
					StartCoroutine(UIManager.instance.ShowPlayerStatusWindow(attackedUnit));
				}
			}
		}

		//if not a crossfire attack
		if (TileDotManager.instance.crossfiredEnemyUnits.Count == 0)
		{
			//turn the attacking unit to the attacked unit
			yield return StartCoroutine(TurnAUnitToFaceADot(attackingUnit, TileDotManager.instance.dots[attackedUnit.associatedDots[0]]));
		}
		//if a crossfire attack
		else
		{
			//turn the attacking unit to the attacked unit
			StartCoroutine(TurnAUnitToFaceADot(attackingUnit, TileDotManager.instance.dots[attackedUnit.associatedDots[0]]));

			yield return new WaitForSeconds(0.5f);
		}		

		//if not a crossfire attack
		if (TileDotManager.instance.crossfiredEnemyUnits.Count == 0)
		{
			//put up the attacked unit a little bit
			attackedUnit.associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
		}

		//show bullet 
		GameObject bullet = Instantiate(attackingUnit.associatedGameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject);
		bullet.transform.position = attackingUnit.associatedGameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.position;
		bullet.transform.eulerAngles = attackingUnit.associatedGameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.eulerAngles;
		Vector3 initBulletPos = bullet.transform.position;		

		//reposition the bullet
		bullet.transform.position = new Vector3((attackedUnit.associatedGameObject.transform.position.x + attackingUnit.associatedGameObject.transform.position.x)/2f, bullet.transform.position.y, (attackedUnit.associatedGameObject.transform.position.z + attackingUnit.associatedGameObject.transform.position.z)/2f);

		//change bullet color
		if (attackedUnit.unitID >= 100 && attackedUnit.unitID < 200) //if enemy unit
		{
			bullet.GetComponent<MeshRenderer>().material.color = Color.yellow;
		}
		else
		if (attackedUnit.unitID >= 0 && attackedUnit.unitID < 100) //if player unit
		{
			bullet.GetComponent<MeshRenderer>().material.color = Color.yellow;
		}

		//if Red Soldier
		if (attackingUnit.unitID == 1)
		{
			//put the bullet to attacking unit position and scale it to 1
			bullet.transform.position = attackingUnit.associatedGameObject.transform.position;
			bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);	
			bullet.GetComponent<Rigidbody>().isKinematic = false;

			//throw bullet
			Vector3 powerDirection = new Vector3
			(
				100f * (attackedUnit.associatedGameObject.transform.position.x - attackingUnit.associatedGameObject.transform.position.x), 
				125f, 
				100f * (attackedUnit.associatedGameObject.transform.position.z - attackingUnit.associatedGameObject.transform.position.z)
			);
			bullet.GetComponent<Rigidbody>(). AddForce(powerDirection);

			yield return new WaitForSeconds(0.5f);

			bullet.transform.position = initBulletPos;
			bullet.transform.localScale = Vector3.zero;
			bullet.GetComponent<Rigidbody>().isKinematic = true;
		}
		//otherwise
		else
		{
			HOTween.To
			(
				bullet.transform, 
				0.5f, 
				new TweenParms()
			        .Prop
					(
						"localScale", 
						new Vector3
						(
							0.1f,
							0.1f,
							Vector3.Distance(attackedUnit.associatedGameObject.transform.position, attackingUnit.associatedGameObject.transform.position)
						),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);

			yield return new WaitForSeconds(0.5f);
		}

		//Material initAttackedMaterial = attackedUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material;
		Material bulletMaterial = bullet.GetComponent<MeshRenderer>().material;

		//change the attacked Unit color to bullet color
		for (int i = 0; i < attackedUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.childCount; i++)
		{
			attackedUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = bulletMaterial;
		}

		//yield return new WaitForSeconds(0.5f);

		//if not a crossfire attack
		if (TileDotManager.instance.crossfiredEnemyUnits.Count == 0)
		{
			//turn the attacked unit to the attacking unit
			yield return StartCoroutine(TurnAUnitToFaceADot(attackedUnit, TileDotManager.instance.dots[attackingUnit.associatedDots[0]]));
		}

		yield return new WaitForSeconds(0.5f);

		//if not a crossfire attack
		if (TileDotManager.instance.crossfiredEnemyUnits.Count == 0)
		{
			//reduce HP
			attackedUnit.healthPoint -= attackingUnit.damagePoint;

			//show damage -number animation
			yield return StartCoroutine(UIManager.instance.ShowOtherText(attackedUnit, "-" + attackingUnit.damagePoint + " HP", Color.red));
		}	
		//if a crossfire 
		else
		{
			//int lastIndexForDifferentUnit = 0;
			int totalDamage = 0; //to store the total damage of crossfire attack

			//total damage of crossfire attack
			for (int i = 0; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
			{
				//if unit is the same with attacked unit
				if (TileDotManager.instance.crossfiredEnemyUnits[i] == attackedUnit)
				{
					totalDamage += TileDotManager.instance.crossfiringPlayerUnits[i].damagePoint;			

					/*if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
					{
						totalDamage += TileDotManager.instance.crossfiringPlayerUnits[i].damagePoint * (i+1-lastIndexForDifferentUnit);
	
						//save the last index so we know how many action points that we need to give if the crossfired unit is dead
						lastIndexForDifferentUnit = i+1;	
					}
					else
					if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
					{
						totalDamage += TileDotManager.instance.crossfiringPlayerUnits[i].damagePoint * (i+1-lastIndexForDifferentUnit);
	
						//save the last index so we know how many action points that we need to give if the crossfired unit is dead
						lastIndexForDifferentUnit = i+1;
					}*/
				}
			}

			//reduce HP
			attackedUnit.healthPoint -= attackingUnit.damagePoint;

			//show damage -number animation
			yield return StartCoroutine(UIManager.instance.ShowOtherText(attackedUnit, "-" + totalDamage + " HP", Color.red));
		}

		//scale back bullet
		HOTween.To
		(
			bullet.transform, 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"localScale", 
					new Vector3
					(
						0f,
						0f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.5f);

		//reset position of bullet
		bullet.transform.position = initBulletPos;
		bullet.GetComponent<Rigidbody>().isKinematic = true;

		//change back attacked Unit color
		for (int i = 0; i < attackedUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.childCount; i++)
		{
			attackedUnit.associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = attackedUnit.initModelMaterial;
		}

		//attackedUnit.associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = initAttackedMaterial;

		//reactivate input 
		InputManager.instance.isInputActive = true;

		//if not a crossfire attack
		if (TileDotManager.instance.crossfiredEnemyUnits.Count == 0)
		{
			//assign direction and dots to a Unit
			for (int i = 0; i < units.Count; i++)
			{
				AssignDirectionToAUnit(units[i]);
				AssignDotsToAUnit(units[i]);
			}

			//check if the attacked unit runs out of health
			if (attackedUnit.healthPoint <= 0)
			{
				//dead
				Debug.Log("Attacked unit is dead!");
			
				//if this is an enemy unit that is being attacked
				if (attackedUnit.unitID >= 100 && attackedUnit.unitID < 200)
				{
					//move the object up a little bit
					attackedUnit.associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
				
					StartCoroutine(GiveNormalActionPoints(1, attackedUnit));
	
					//kill this unit
					yield return StartCoroutine(KillAUnit(attackedUnit));
				}
				else
				//if this is a player unit that is being attacked
				if (attackedUnit.unitID >= 0 && attackedUnit.unitID < 100)
				{
					//move the object up a little bit
					attackedUnit.associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);

					//soul theft
					if (GameManager.instance.soulPoints[attackingUnit.unitID-100] > 0)
					{
						yield return StartCoroutine(UIManager.instance.ShowSoulTheftBumperAnimation());

						StartCoroutine(GiveNormalActionPoints(GameManager.instance.soulPoints[attackingUnit.unitID-100], attackedUnit));
					}

					//kill this unit
					yield return StartCoroutine(KillAUnit(attackedUnit));
				}
			}
		}

		if (GameManager.instance.uiType == 2)
		{
			//cleare attack tracker list
			GameManager.instance.attackTracker.Clear();

			//reset action point need to zero
			GameManager.instance.actionPointNeededToAttack = 0;

			//reset back cursor status to zero
			GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
		}

		//if not a crossfire attack
		if (TileDotManager.instance.crossfiredEnemyUnits.Count == 0)
		{
			//if selecting special points and player's turn
			if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)
			{
				//if there's something to resurrect
				if (GameManager.instance.resurrectedGameObject.Count > 0)
				{

				}
			}
			else
			{
				//show cursor on the attacking Unit
				GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[attackingUnit.associatedDots[0]]);
			}
		}

		//if not a crossfire attack
		if (TileDotManager.instance.crossfiredEnemyUnits.Count == 0)
		{
			//put the attacked Unit down 
			if (attackedUnit.unitID >= 100 && attackedUnit.unitID < 200) //if enemy unit
			{
				attackedUnit.associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
			}
			else
			if (attackedUnit.unitID >= 0 && attackedUnit.unitID < 100) //if player unit
			{
				attackedUnit.associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
			}
		}

		//if player's turn
		if (GameManager.instance.turnID == 1)
		{
			//if enemy unit that is being attacked
			if (attackedUnit.unitID >= 100 && attackedUnit.unitID < 200)
			{
				StartCoroutine(UIManager.instance.HideEnemyStatusWindow());

				//show enemy status window
				//StartCoroutine(UIManager.instance.ShowEnemyStatusWindow(attackedUnit));

				//if selecting special points 
				if (GameManager.instance.selectedTypeOfPointsForPlayer > 0)
				{
					//if there's something to resurrect
					if (GameManager.instance.resurrectedGameObject.Count > 0)
					{
						yield return StartCoroutine(UIManager.instance.ShowResurrectionBumperAnimation()); 
	
						List<int> unitIDForResurrection = SearchUnitIDByResurrectionEffect();
	
						Unit unitToFocus = new Unit();

						for (int i = 0; i < unitIDForResurrection.Count; i++)
						{
							//find to which unit the camera needs to focus
							if (TileDotManager.instance.SearchDotsToResurrectAroundAUnit(units[unitIDForResurrection[i]]).Count > 0)
							{
								unitToFocus = units[unitIDForResurrection[i]];
	
								break;
							}
						}
	
						Vector3 newCameraPosition = new Vector3(unitToFocus.associatedGameObject.transform.position.x, 0.45f, unitToFocus.associatedGameObject.transform.position.z - 2.5027f);
						Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);
		
						//rotate and move the camera to the target unit
						yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition, newCameraRotation));
	
						//set selector status to resurrection
						GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 5);
	
						GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, GameManager.instance.resurrectTracker[0]);	
					}
					/*//if there's no gameobject to resurrect
					else
					{
						//set selector status to normal
						GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
				
						//put and show the cursor on the moved unit
						GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToMove.associatedDots[0]]);	
					}*/
				}
			}
		}
		else
		//if enemy's turn
		if (GameManager.instance.turnID == 2)
		{
			//if player unit that is being attacked
			if (attackedUnit.unitID >= 0 && attackedUnit.unitID < 100)
			{
				StartCoroutine(UIManager.instance.HidePlayerStatusWindow());

				//show player status window
				//StartCoroutine(UIManager.instance.ShowPlayerStatusWindow(attackedUnit));
			}
		}

		//destroy bullet
		Destroy(bullet);

		//put the Unit to idle: put him down, also make it not selected
		DeselectUnit(attackingUnit);
	}

	// put a Unit to action mode
	public IEnumerator PutAUnitToAction (Unit unitToPut)
	{
		//deactivate input for a while
		InputManager.instance.isInputActive = false;

		//hide cursor
		//GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);

		//select the Unit
		SelectUnit(unitToPut);

		//reset to 0
		GameManager.instance.distanceCursorToSelectedUnit = 0;

		//reset movement list
		GameManager.instance.movementTracker.Clear();

		//reset rotation tracker
		GameManager.instance.rotationTracker = 0;

		//reset attack tracker
		GameManager.instance.attackTracker.Clear();

		//reset how many action points needed to attack
		GameManager.instance.actionPointNeededToAttack = 0;

		//if it's player's turn
		if (GameManager.instance.turnID == 1)
		{
			if (GameManager.instance.uiType == 1)
			{
				List<int> rotatedToBeUnitOrBuilding = new List<int>();
				rotatedToBeUnitOrBuilding.Add(units.IndexOf(unitToPut));
			
				for (int i = 0; i < rotatedToBeUnitOrBuilding.Count; i++)
				{
					//show rotate text on a Unit that is being selected
					UIManager.instance.ShowRotateText(units[rotatedToBeUnitOrBuilding[i]]);
				}	
			
				List<int> attackedToBeUnitOrBuilding = new List<int>();
				attackedToBeUnitOrBuilding = SearchUnitIDsByAttackRangeOfAUnit(unitToPut);
			
				for (int i = 0; i < attackedToBeUnitOrBuilding.Count; i++)
				{
					//show attack text on units that can be attacked by the selected unit
					UIManager.instance.ShowAttackText(units[attackedToBeUnitOrBuilding[i]], "ATTACK\nTHIS UNIT\n(1 AP)");
				}
		
				List<int> moveToDot = new List<int>();
				moveToDot = TileDotManager.instance.SearchDotIDsByMoveRangeOfAUnit(unitToPut);
			
				for (int i = 0; i < moveToDot.Count; i++)
				{
					//show move text on dots that can be attacked by the selected unit
					UIManager.instance.ShowMoveText(TileDotManager.instance.dots[moveToDot[i]]);
				}
			}		
			else
			if (GameManager.instance.uiType == 2)
			{
				List<string> possibleActions = new List<string>();

				//if this is not a power plant
				if (unitToPut.unitID != 2)
				{
					//if rotation points are still enough to spend
					if (unitToPut.rotationPoint > 0)
					{
						//if normal action point is selected
						if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
						{
							//check the normal action point is still enough or not
							if (GameManager.instance.actionPoints[GameManager.instance.turnID-1] > 0)
							{
								possibleActions.Add("ROTATE");
							}
							//if not enough
							else
							{
								//do nothing
							}
						}
						else
						//if special action point is selected
						{
							//Debug.Log("Putting to action while special action point is selected");

							//check the special action point is still enough or not
							if (GameManager.instance.specialPoints[GameManager.instance.selectedTypeOfPointsForPlayer-1] > 0)
							{
								possibleActions.Add("ROTATE");
							}
							//if not enough
							else
							{
								//do nothing
							}
						}
					}
					else
					{
						//do nothing
					}

					//if normal action point is selected
					if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
					{
						//check the normal action point is still enough or not
						if (GameManager.instance.actionPoints[GameManager.instance.turnID-1] > 0)
						{
							possibleActions.Add("MOVE");
						}
					}		
					else
					//if special action point is selected
					{
						//Debug.Log("Putting to action while special action point is selected");

						//check the special action point is still enough or not
						if (GameManager.instance.specialPoints[GameManager.instance.selectedTypeOfPointsForPlayer-1] > 0)
						{
							possibleActions.Add("MOVE");
						}
						//if not enough
						else
						{
							//do nothing
						}
					}			

					List<int> attackedToBeUnitOrBuilding = new List<int>();
					attackedToBeUnitOrBuilding = instance.SearchUnitIDsByAttackRangeOfAUnit(unitToPut);
						
					if (attackedToBeUnitOrBuilding.Count > 0)
					{
						GameManager.instance.actionPointNeededToAttack = 1;	
					}

					//if normal action point is selected
					if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
					{
						//check the normal action point is still enough or not
						if (GameManager.instance.actionPoints[GameManager.instance.turnID-1] > 0)
						{
							if (GameManager.instance.actionPointNeededToAttack > 0)
							{
								possibleActions.Add("ATTACK");
							}
						}
					}		
					else
					//if special action point is selected
					{
						//Debug.Log("Putting to action while special action point is selected");

						//check the special action point is still enough or not
						if (GameManager.instance.specialPoints[GameManager.instance.selectedTypeOfPointsForPlayer-1] > 0)
						{
							if (GameManager.instance.actionPointNeededToAttack > 0)
							{
								possibleActions.Add("ATTACK");
							}
						}
						//if not enough
						else
						{
							//do nothing
						}
					}		
				}

				possibleActions.Add("EXIT");

				//show ui for action options
				UIManager.instance.ShowActionOptionsWindowOnAUnit(unitToPut, possibleActions);

				//put and show the cursor on the unit
				GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToPut.associatedDots[0]]);	
			}	
		}
		else
		//if it's enemy's turn 
		if (GameManager.instance.turnID == 2)
		{
			if (GameManager.instance.uiType == 2)
			{
				//Debug.Log("Put enemy unit to action!");

				List<string> possibleActions = new List<string>();

				possibleActions.Add("TURN");

				int dotIDInFrontOfUnit = TileDotManager.instance.dots[unitToPut.associatedDots[0]].neighboringDots[unitToPut.direction];
		
				//if there's a dot in the front of the enemy
				if (dotIDInFrontOfUnit > -1)
				{
					int unitIDToCheck = SearchUnitIDByPosition(TileDotManager.instance.dots[dotIDInFrontOfUnit].associatedGameObject.transform.position);

					if (unitIDToCheck > - 1)
					{
						//check if there's not an enemy's unit in the front of enemy
						if (units[unitIDToCheck].unitID >= 100 && units[unitIDToCheck].unitID < 200)
						{
							
						}
						else
						{
							possibleActions.Add("MOVE");
						}
					}
					else
					{
						possibleActions.Add("MOVE");
					}

					//check if there's a unit in the front of the enemy
					/*if (SearchUnitIDByPosition(TileDotManager.instance.dots[dotIDInFrontOfUnit].associatedGameObject.transform.position) > -1)
					{
						//do nothing
					}	
					else
					{
						possibleActions.Add("MOVE");
					}*/
				}			

				List<int> attackedToBeUnitOrBuilding = new List<int>();
				attackedToBeUnitOrBuilding = SearchUnitIDsByAttackRangeOfAUnit(unitToPut);
			
				if (attackedToBeUnitOrBuilding.Count > 0)
				{
					GameManager.instance.actionPointNeededToAttack = 1;
	
					possibleActions.Add("ATTACK");
				}

				possibleActions.Add("EXIT");

				//show ui for action options
				UIManager.instance.ShowActionOptionsWindowOnAUnit(unitToPut, possibleActions);

				//put and show the cursor on the unit
				GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToPut.associatedDots[0]]);	
			}
		}

		yield return new WaitForSeconds(0.15f);

		//reactivate input 
		InputManager.instance.isInputActive = true;
	}

	// put a Unit to idle mode
	public IEnumerator PutAUnitToIdle (Unit unitToPut)
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

		//assign direction and dots to a Unit
		for (int i = 0; i < units.Count; i++)
		{
			AssignDirectionToAUnit(units[i]);
			AssignDotsToAUnit(units[i]);
		}
	
		//show cursor on the selected Unit
		GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToPut.associatedDots[0]]);
		
		//put the Unit to idle: put him down, also make it not selected
		DeselectUnit(unitToPut);

		//if selecting special points and it's player's turns
		if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)
		{
			//if there's something to resurrect
			if (GameManager.instance.resurrectedGameObject.Count > 0)
			{
				yield return StartCoroutine(UIManager.instance.ShowResurrectionBumperAnimation()); 
		
				List<int> unitIDForResurrection = SearchUnitIDByResurrectionEffect();
		
				Unit unitToFocus = new Unit();

				for (int i = 0; i < unitIDForResurrection.Count; i++)
				{
					//find to which unit the camera needs to focus
					if (TileDotManager.instance.SearchDotsToResurrectAroundAUnit(units[unitIDForResurrection[i]]).Count > 0)
					{
						unitToFocus = units[unitIDForResurrection[i]];

						break;
					}
				}

				Vector3 newCameraPosition = new Vector3(unitToFocus.associatedGameObject.transform.position.x, 0.45f, unitToFocus.associatedGameObject.transform.position.z - 2.5027f);
				Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);
		
				//rotate and move the camera to the target unit
				yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition, newCameraRotation));
		
				//set selector status to resurrection
				GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 5);
		
				GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, GameManager.instance.resurrectTracker[0]);	
			}
		}
	}

	// move a Unit to a selected Dot pos
	public IEnumerator MoveAUnitToANeighborDot (Unit unitToMove, Dot dotTargetPos)
	{
		//deactivate input for a while
		InputManager.instance.isInputActive = false;

		if (GameManager.instance.uiType == 1)
		{
			//hide cursor
			GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);
		}
		else
		if (GameManager.instance.uiType == 2)
		{
			//do nothing
		}

		//hide all move text
		UIManager.instance.HideAllMoveText();
	
		//hide all rotate text
		UIManager.instance.HideAllRotateText();

		//hide all attack text
		UIManager.instance.HideAllAttackText();

		//hide all stay text
		UIManager.instance.HideAllStayText();

		if (GameManager.instance.uiType == 1)
		{
			string[] moveArray = unitToMove.associatedGameObject.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text.Split('\n');
	
			//Debug.Log("Attack text 2: " + attackArray[2]);
	
			if (moveArray[2] == "(0 AP)")
			{
				//do nothing
			}
			else
			if (moveArray[2] == "(1 AP)")
			{
				yield return StartCoroutine(GiveNormalActionPoints(1, unitToMove));
			}
		}
		else
		if (GameManager.instance.uiType == 2) 
		{
			//do nothing
		}

		//turn the Unit to the Dot target
		yield return StartCoroutine(TurnAUnitToFaceADot(unitToMove, dotTargetPos));

		//tween there 
		HOTween.To
		(
			unitToMove.associatedGameObject.transform, 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"position", 
					new Vector3
					(
						dotTargetPos.associatedGameObject.transform.position.x,
						unitToMove.associatedGameObject.transform.position.y,
						dotTargetPos.associatedGameObject.transform.position.z
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.5f);

		//reactivate input 
		InputManager.instance.isInputActive = true;

		//assign direction and dots to a Unit
		for (int i = 0; i < units.Count; i++)
		{
			AssignDirectionToAUnit(units[i]);
			AssignDotsToAUnit(units[i]);
		}

		if (GameManager.instance.uiType == 1)
		{
			//check if there are enemies in attack range
			List<int> attackedToBeUnitOrBuilding = new List<int>();
			attackedToBeUnitOrBuilding = SearchUnitIDsByAttackRangeOfAUnit(unitToMove);
	
			if (attackedToBeUnitOrBuilding.Count > 0)
			{
				//Debug.Log("Attack after moving!");
	
				//put the cursor on the first enemy that is in attack
				GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[units[attackedToBeUnitOrBuilding[0]].associatedDots[0]]);
	
				for (int i = 0; i < attackedToBeUnitOrBuilding.Count; i++)
				{
					//show attack text on units that can be attacked by the selected unit
					UIManager.instance.ShowAttackText(units[attackedToBeUnitOrBuilding[i]], "ATTACK\nTHIS UNIT\n(0 AP)");
				}
	
				//show stay text on selected Unit
				UIManager.instance.ShowStayText(unitToMove);
			}
			else
			//if not 
			{
				//put the cursor on the selected unit that is in attack
				GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToMove.associatedDots[0]]);
	
				//show stay text on selected Unit
				UIManager.instance.ShowStayText(unitToMove);
	
				//show cursor on the selected Unit
				//GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToMove.associatedDots[0]]);
	
				//put the Unit to idle: put him down, also make it not selected
				//DeselectUnit(unitToMove);
			}
		}
		else
		if (GameManager.instance.uiType == 2) //TODO
		{

		}
	}

	//spend a certain amount of rotation points
	public IEnumerator SpendRotationPoints (Unit unitToSpend, int rotationPointsToSpend)
	{
		//Debug.Log("Spend rotation points");
	
		//reduce rotation point
		unitToSpend.rotationPoint -= rotationPointsToSpend;

		//animate rotation point to reduce
		GameObject playerStatusWindow = UIManager.instance.uiCanvas.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
		GameObject rotationPointIcon = playerStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(unitToSpend.rotationPoint).gameObject;
		HOTween.To
		(
			rotationPointIcon.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"localScale", 
					new Vector3
					(
						0f,
						0,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		//put the animation back to idle
		rotationPointIcon.GetComponent<Animator>().Play("Idle");

		yield return new WaitForSeconds(0.5f);
	}

	//give action point(s) from a Unit to another Unit
	public IEnumerator GiveNormalActionPoints (int actionPointsGiven, Unit unitWhoGive)
	{
		GameObject actionPointInUnit = unitWhoGive.associatedGameObject.transform.GetChild(7).gameObject;
		
		//create action points container
		GameObject actionPointsContainer = new GameObject();
		actionPointsContainer.name = "ActionPointsContainer";

		//if Player that does an action
		if (unitWhoGive.unitID >= 0 && unitWhoGive.unitID < 100)
		{
			if (GameManager.instance.actionPoints[0] - actionPointsGiven < 0)
			{
				actionPointsGiven = GameManager.instance.actionPoints[0];
			}

			GameManager.instance.actionPoints[0] -= actionPointsGiven;
			GameManager.instance.actionPoints[1] += actionPointsGiven;

			for (int i = 0; i < actionPointsGiven; i++)
			{
				GameObject instantiatedActionPoint = Instantiate(actionPointInUnit);
				instantiatedActionPoint.transform.position = actionPointInUnit.transform.position;
				instantiatedActionPoint.transform.parent = actionPointsContainer.transform;

				HOTween.To
				(
					instantiatedActionPoint.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								0f,
								0.65f + (i * 0.2f),
								0f
							),
							true	
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
				);
	
				if (GameManager.instance.turnID == 1)
				{
					HOTween.To
					(
						instantiatedActionPoint.transform, 
						0.5f, 
						new TweenParms()
					        .Prop
							(
								"position", 
								new Vector3
								(
									Camera.main.transform.position.x + 2.2f,
									Camera.main.transform.position.y - 0.76f,
									Camera.main.transform.position.z + 2.32f
									/*2.2f,
									-0.31f,
									0.15f*/
								),
								false	
							) 
					        .Ease(EaseType.Linear)
							.Delay(0.5f)
					);
				}
				else
				if (GameManager.instance.turnID == 2)
				{
					HOTween.To
					(
						instantiatedActionPoint.transform, 
						0.5f, 
						new TweenParms()
					        .Prop
							(
								"position", 
								new Vector3
								(
									Camera.main.transform.position.x - 2.2f,
									Camera.main.transform.position.y - 0.76f,
									Camera.main.transform.position.z - 2.32f
									/*2.2f,
									-0.31f,
									0.15f*/
								),
								false	
							) 
					        .Ease(EaseType.Linear)
							.Delay(0.5f)
					);
				}
			
				HOTween.To
				(
					instantiatedActionPoint.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"eulerAngles", 
							new Vector3
							(
								0f,
								-30f,
								0f
								/*2.2f,
								-0.31f,
								0.15f*/
							),
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0.5f)
				);
			}

			yield return new WaitForSeconds(1f);

			//destroy action points container
			Destroy(actionPointsContainer);

			//actionPointInUnit.SetActive(false);
			//actionPointInUnit.transform.localPosition = Vector3.zero;
			//actionPointInUnit.transform.localEulerAngles = Vector3.zero;
		}
		//if Enemy that does an action 
		else
		if (unitWhoGive.unitID >= 100 && unitWhoGive.unitID < 200)
		{
			if (GameManager.instance.actionPoints[1] - actionPointsGiven < 0)
			{
				actionPointsGiven = GameManager.instance.actionPoints[1];
			}

			GameManager.instance.actionPoints[0] += actionPointsGiven;
			GameManager.instance.actionPoints[1] -= actionPointsGiven;

			for (int i = 0; i < actionPointsGiven; i++)
			{
				GameObject instantiatedActionPoint = Instantiate(actionPointInUnit);
				instantiatedActionPoint.transform.position = actionPointInUnit.transform.position;
				instantiatedActionPoint.transform.parent = actionPointsContainer.transform;

				HOTween.To
				(
					instantiatedActionPoint.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								0f,
								0.65f + (i * 0.2f),
								0f
							),
							true	
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
				);
	
				if (GameManager.instance.turnID == 1)
				{
					HOTween.To
					(
						instantiatedActionPoint.transform, 
						0.5f, 
						new TweenParms()
					        .Prop
							(
								"position", 
								new Vector3
								(
									Camera.main.transform.position.x - 2.2f,
									Camera.main.transform.position.y - 0.76f,
									Camera.main.transform.position.z + 2.32f
									/*2.2f,
									-0.31f,
									0.15f*/
								),
								false	
							) 
					        .Ease(EaseType.Linear)
							.Delay(0.5f)
					);
				}
				else
				if (GameManager.instance.turnID == 2)
				{
					HOTween.To
					(
						instantiatedActionPoint.transform, 
						0.5f, 
						new TweenParms()
					        .Prop
							(
								"position", 
								new Vector3
								(
									Camera.main.transform.position.x + 2.2f,
									Camera.main.transform.position.y - 0.76f,
									Camera.main.transform.position.z - 2.32f
									/*2.2f,
									-0.31f,
									0.15f*/
								),
								false	
							) 
					        .Ease(EaseType.Linear)
							.Delay(0.5f)
					);
				}
			
				HOTween.To
				(
					instantiatedActionPoint.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"eulerAngles", 
							new Vector3
							(
								0f,
								30f,
								0f
								/*2.2f,
								-0.31f,
								0.15f*/
							),
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0.5f)
				);
			}

			yield return new WaitForSeconds(1f);

			//destroy action points container
			Destroy(actionPointsContainer);

			//actionPointInUnit.SetActive(false);
			//actionPointInUnit.transform.localPosition = Vector3.zero;
			//actionPointInUnit.transform.localEulerAngles = Vector3.zero;
		}

		//refresh action points on UI
		UIManager.instance.RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI();	

		//yield return new WaitForSeconds(3.5f);

		/*GameObject playerActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
		GameObject enemyActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

		//if the one who gives is Player
		if (isFromPlayer)
		{
			List<GameObject> mostRightPlayerActionPoint = new List<GameObject>();

			int currentAmountOfActiveAP = 0;

			//find the most right action point of Player that is currently active
			for (int i = 0; i < playerActionPointsContainer.transform.childCount; i++)
			{
				//if found an active one
				if (playerActionPointsContainer.transform.GetChild(playerActionPointsContainer.transform.childCount-1-i).gameObject.activeSelf)
				{
					mostRightPlayerActionPoint.Add(playerActionPointsContainer.transform.GetChild(playerActionPointsContainer.transform.childCount-1-i).gameObject);
				
					currentAmountOfActiveAP += 1;

					if (currentAmountOfActiveAP == actionPointsGiven)
					{
						break;
					}
				}
			}
	
			GameObject mostLeftEnemyActionPoint = null;
	
			//find the most left action point of Enemy that is currently active
			for (int i = 0; i < enemyActionPointsContainer.transform.childCount; i++)
			{
				//if found an active one
				if (enemyActionPointsContainer.transform.GetChild(i).gameObject.activeSelf)
				{
					mostLeftEnemyActionPoint = enemyActionPointsContainer.transform.GetChild(i).gameObject;
	
					break;
				}
			}
	
			//find an x distance between two action points
			float oneXDistance = playerActionPointsContainer.transform.GetChild(1).gameObject.transform.position.x - playerActionPointsContainer.transform.GetChild(0).gameObject.transform.position.x;

			for (int i = 0; i < actionPointsGiven; i++)
			{
				//tween
				HOTween.To
				(
					mostRightPlayerActionPoint[i].transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								mostLeftEnemyActionPoint.transform.position.x - (oneXDistance * i),
								mostLeftEnemyActionPoint.transform.position.y,
								mostLeftEnemyActionPoint.transform.position.z
							),
							false	
						) 
				        .Ease(EaseType.EaseInQuad)
						.Delay(0f)
				);
			}
		
			yield return new WaitForSeconds(0.5f);
	
			//reduce the amount of the giver and increase the amount of the receiver
			GameManager.instance.actionPoints[0] -= actionPointsGiven;
			GameManager.instance.actionPoints[1] += actionPointsGiven;
	
			//refresh action points on UI
			UIManager.instance.RefreshActionPointsUI();	
		}
		//otherwise, if the one who give is the Enemy
		else
		{

		}*/
	}

	// put a Unit to selected status
	public void SelectUnit (Unit unitToSelect)
	{
		if (unitToSelect.isSelected == true)
		{
			return;
		}

		//select the unit
		unitToSelect.isSelected = true;

		//move the object up a little bit
		if (unitToSelect.unitID >= 100 && unitToSelect.unitID < 200) //if enemy units
		{
			unitToSelect.associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
		}
		else
		if (unitToSelect.unitID >= 0 && unitToSelect.unitID < 100) //if player units
		{
			unitToSelect.associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
		}
	}

	// put a Unit to idle
	public void DeselectUnit (Unit unitToPutIdle)
	{
		if (unitToPutIdle.isSelected == false)
		{
			return;
		}

		//deselect the unit
		unitToPutIdle.isSelected = false;

		//move the object down a little bit
		if (unitToPutIdle.unitID >= 100 && unitToPutIdle.unitID < 200) //if enemy units
		{
			unitToPutIdle.associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
		}
		else
		if (unitToPutIdle.unitID >= 0 && unitToPutIdle.unitID < 100) //if player units
		{
			unitToPutIdle.associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
		}
	}

	// turn a Unit to face a direction of A Dot
	public IEnumerator TurnAUnitToFaceADot (Unit unitToTurn, Dot dotToFace)
	{
		//animation duration
		float duration = 0.5f;

		//find a direction of a Dot relative to a Unit
		Vector3 direction = dotToFace.associatedGameObject.transform.position - unitToTurn.associatedGameObject.transform.position;

		//Debug.Log("Direction to dot: " + direction);

		int newDirection = unitToTurn.direction;

		//if dot is on top left
		if (direction.x < 0f && direction.z > 0.25f)
		{
			newDirection = 0;

			if (newDirection == unitToTurn.direction)
			{
				duration = 0f;
			}

			//tween to turn to that direction
			HOTween.To
			(
				unitToTurn.associatedGameObject.transform, 
				duration, 
				new TweenParms()
			        .Prop
					(
						"eulerAngles", 
						new Vector3
						(
							0f,
							-30f,
							0f
						),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}
		else
		//if dot is on top right
		if (direction.x > 0f && direction.z > 0.25f)
		{
			newDirection = 1;

			if (newDirection == unitToTurn.direction)
			{
				duration = 0f;
			}

			//tween to turn to that direction
			HOTween.To
			(
				unitToTurn.associatedGameObject.transform, 
				duration, 
				new TweenParms()
			        .Prop
					(
						"eulerAngles", 
						new Vector3
						(
							0f,
							30f,
							0f
						),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}
		else
		//if dot is on the right
		if (direction.x > 0f && direction.z > -0.25f && direction.z < 0.25f)
		{
			newDirection = 2;

			if (newDirection == unitToTurn.direction)
			{
				duration = 0f;
			}

			//tween to turn to that direction
			HOTween.To
			(
				unitToTurn.associatedGameObject.transform, 
				duration, 
				new TweenParms()
			        .Prop
					(
						"eulerAngles", 
						new Vector3
						(
							0f,
							90f,
							0f
						),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}
		else
		//if dot is on bottom right
		if (direction.x > 0f && direction.z < -0.25f)
		{
			newDirection = 3;

			if (newDirection == unitToTurn.direction)
			{
				duration = 0f;
			}

			//tween to turn to that direction
			HOTween.To
			(
				unitToTurn.associatedGameObject.transform, 
				duration, 
				new TweenParms()
			        .Prop
					(
						"eulerAngles", 
						new Vector3
						(
							0f,
							150f,
							0f
						),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}
		else
		//if dot is on bottom left
		if (direction.x < 0f && direction.z < -0.25f)
		{
			newDirection = 4;

			if (newDirection == unitToTurn.direction)
			{
				duration = 0f;
			}

			//tween to turn to that direction
			HOTween.To
			(
				unitToTurn.associatedGameObject.transform, 
				duration, 
				new TweenParms()
			        .Prop
					(
						"eulerAngles", 
						new Vector3
						(
							0f,
							-150f,
							0f
						),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}
		else
		//if dot is on the left
		if (direction.x < 0f && direction.z > -0.25f && direction.z < 0.25f)
		{
			newDirection = 5;

			if (newDirection == unitToTurn.direction)
			{
				duration = 0f;
			}

			//tween to turn to that direction
			HOTween.To
			(
				unitToTurn.associatedGameObject.transform, 
				duration, 
				new TweenParms()
			        .Prop
					(
						"eulerAngles", 
						new Vector3
						(
							0f,
							-90f,
							0f
						),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);
		}

		yield return new WaitForSeconds(duration);

		//assign a new direction ID to the associated Unit class
		unitToTurn.direction = newDirection;
	}

	//get a direction from a unit to another unit. 0 is top left, 1 is top right, 2 is right, 3 is bottom right, 4 is bottom left, 5 is left, -1 is unknown
	public int GetDirectionFromAUnitToAnotherUnit (Unit unitFrom, Unit unitTo)
	{
		//find a direction of a Dot relative to a Unit
		Vector3 direction = unitTo.associatedGameObject.transform.position - unitFrom.associatedGameObject.transform.position;

		int newDirection = -1;

		//if dot is on top left
		if (direction.x < 0f && direction.z > 0.25f)
		{
			newDirection = 0;
		}
		else
		//if dot is on top right
		if (direction.x > 0f && direction.z > 0.25f)
		{
			newDirection = 1;
		}
		else
		//if dot is on the right
		if (direction.x > 0f && direction.z > -0.25f && direction.z < 0.25f)
		{
			newDirection = 2;
		}
		else
		//if dot is on bottom right
		if (direction.x > 0f && direction.z < -0.25f)
		{
			newDirection = 3;
		}
		else
		//if dot is on bottom left
		if (direction.x < 0f && direction.z < -0.25f)
		{
			newDirection = 4;
		}
		else
		//if dot is on the left
		if (direction.x < 0f && direction.z > -0.25f && direction.z < 0.25f)
		{
			newDirection = 5;
		}

		return newDirection;
	}

	// to know whether the Unit is showing attack text or not
	public bool IsUnitShowingAttackText (Unit unitToCheck)
	{
		if (unitToCheck.associatedGameObject.transform.GetChild(3).gameObject.activeSelf)
		{
			return true;
		}

		return false;
	}

	// to know whether the Unit is showing rotate text or not
	public bool IsUnitShowingRotateText (Unit unitToCheck)
	{
		if (GameManager.instance.uiType == 1)
		{
			if (unitToCheck.associatedGameObject.transform.GetChild(2).gameObject.activeSelf)
			{
				return true;
			}
		}
	
		return false;
	}

	// to know whether a unit is highlighting rotate button in action options window or not
	public bool IsUnitHighlightingRotateButton (Unit unitToCheck)
	{
		if (GameManager.instance.uiType == 2)
		{
			GameObject actionOptionsWindow = unitToCheck.associatedGameObject.transform.GetChild(8).gameObject;

			TextMesh[] textMeshes = actionOptionsWindow.transform.GetComponentsInChildren<TextMesh>();

			for (int i = 0; i < textMeshes.Length; i++)
			{
				if (textMeshes[i].text == "ROTATE" && textMeshes[i].gameObject.activeSelf)
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

	// to know whether a unit is highlighting turn button in action options window or not
	public bool IsUnitHighlightingTurnButton (Unit unitToCheck)
	{
		if (GameManager.instance.uiType == 2)
		{
			GameObject actionOptionsWindow = unitToCheck.associatedGameObject.transform.GetChild(8).gameObject;

			TextMesh[] textMeshes = actionOptionsWindow.transform.GetComponentsInChildren<TextMesh>();

			for (int i = 0; i < textMeshes.Length; i++)
			{
				if (textMeshes[i].text == "TURN" && textMeshes[i].gameObject.activeSelf)
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

	// to know whether a player is highlighting move button in action options window or not while selecting a unit
	public bool IsUnitHighlightingMoveButton (Unit unitToCheck)
	{
		if (GameManager.instance.uiType == 2)
		{
			GameObject actionOptionsWindow = unitToCheck.associatedGameObject.transform.GetChild(8).gameObject;
		
			TextMesh[] textMeshes = actionOptionsWindow.transform.GetComponentsInChildren<TextMesh>();
		
			for (int i = 0; i < textMeshes.Length; i++)
			{
				if (textMeshes[i].text == "MOVE" && textMeshes[i].gameObject.activeSelf)
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

	// to know whether a player is highlighting attack button in action options window or not while selecting a unit
	public bool IsUnitHighlightingAttackButton (Unit unitToCheck)
	{
		if (GameManager.instance.uiType == 2)
		{
			GameObject actionOptionsWindow = unitToCheck.associatedGameObject.transform.GetChild(8).gameObject;
		
			TextMesh[] textMeshes = actionOptionsWindow.transform.GetComponentsInChildren<TextMesh>();
		
			for (int i = 0; i < textMeshes.Length; i++)
			{
				if (textMeshes[i].text == "ATTACK" && textMeshes[i].gameObject.activeSelf)
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

	// to know whether a player is highlighting exit button in action options window or not while selecting a unit
	public bool IsUnitHighlightingExitButton (Unit unitToCheck)
	{
		if (GameManager.instance.uiType == 2)
		{
			GameObject actionOptionsWindow = unitToCheck.associatedGameObject.transform.GetChild(8).gameObject;
		
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

	// to know whether the Unit is showing stay text or not
	public bool IsUnitShowingStayText (Unit unitToCheck)
	{
		if (unitToCheck.associatedGameObject.transform.GetChild(6).gameObject.activeSelf)
		{
			return true;
		}

		return false;
	}

	// to know whether action options window is showed or not
	public bool IsUnitShowingActionOptionsWindow (Unit unitToCheck)
	{
		if (unitToCheck.associatedGameObject.transform.GetChild(8).gameObject.activeSelf)
		{
			return true;
		}

		return false;
	}

	// to know whether a player unit is overlapped with enemy unit or not
	public bool IsPlayerUnitOverlappedWithEnemyUnit (Unit unitPlayerToCheck)
	{
		for (int i = 0; i < units.Count; i++)
		{
			//if enemy's unit
			if (units[i].unitID >= 100 && units[i].unitID < 200)
			{
				Unit unitEnemyToCheck = units[i];
		
				if 
				(
					unitPlayerToCheck.associatedGameObject.transform.position.x > unitEnemyToCheck.associatedGameObject.transform.position.x - 0.1f &&
					unitPlayerToCheck.associatedGameObject.transform.position.x < unitEnemyToCheck.associatedGameObject.transform.position.x + 0.1f &&
					unitPlayerToCheck.associatedGameObject.transform.position.z > unitEnemyToCheck.associatedGameObject.transform.position.z - 0.1f &&
					unitPlayerToCheck.associatedGameObject.transform.position.z < unitEnemyToCheck.associatedGameObject.transform.position.z + 0.1f
				)
				{
					return true;
				}
			}
		}

		return false;
	}

	// execute a crossfire! 
	public IEnumerator ExecuteCrossfire (Unit selectedUnit)
	{
		Debug.Log("Crossfire triggerred! Amount of player units that will attack: " + TileDotManager.instance.crossfiringPlayerUnits.Count + " and amount of enemy units that will be attacked: " + TileDotManager.instance.crossfiredEnemyUnits.Count);

		InputManager.instance.isInputActive = false;

		//yield return new WaitForSeconds(1.5f);

		bool isContinue = false;

		StartCoroutine(SpendRotationPoints(selectedUnit, Mathf.Abs(GameManager.instance.rotationTracker)));

		List<Unit> overlappedEnemyUnit = new List<Unit>();
		List<Unit> overlappedPlayerUnit = new List<Unit>();

		//check in every enemy units if there's one or more enemy unit that are overlapped
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			Unit unitToCheck = UnitManager.instance.units[i];

			//Debug.Log("Index: " + i);

			//if enemy's unit
			if (unitToCheck.unitID >= 100 && unitToCheck.unitID < 200)
			{
				int unitIDOfPlayerToCheck = UnitManager.instance.SearchPlayerUnitIDByPosition(unitToCheck.associatedGameObject.transform.position);
		
				//if there's a player unit at the same position of enemy's unit
				if (unitIDOfPlayerToCheck > -1)
				{
					Unit unitOfPlayerToCheck = UnitManager.instance.units[unitIDOfPlayerToCheck];
		
					overlappedPlayerUnit.Add(unitOfPlayerToCheck);
					overlappedEnemyUnit.Add(unitToCheck);
				}
			}
		}

		//if there's a unit that's overlapped
		if (overlappedEnemyUnit.Count > 0)
		{
			yield return StartCoroutine(UIManager.instance.ShowSuicideBumperAnimation());

			if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)	
			{
				//give special action points
				yield return StartCoroutine(UnitManager.instance.GiveSpecialActionPoints(1, GameManager.instance.selectedTypeOfPointsForPlayer-1, selectedUnit));
			}
			else
			{
				yield return StartCoroutine(GiveNormalActionPoints(1, selectedUnit));
			}

			//kill overlapped unit
			for (int i = 0; i < overlappedPlayerUnit.Count; i++)
			{
				//bring player's unit up
				HOTween.To
				(
					overlappedPlayerUnit[i].associatedGameObject.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								0f,
								0.65f,
								0f
							),
							true	
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
				);
			
				yield return new WaitForSeconds(0.5f);
			
				//reduce HP
				overlappedPlayerUnit[i].healthPoint -= overlappedPlayerUnit[i].damagePoint;
			
				//show damage -number animation
				yield return StartCoroutine(UIManager.instance.ShowOtherText(overlappedPlayerUnit[i], "-" + overlappedEnemyUnit[i].damagePoint + " HP", Color.red));
			
				//check if the stomped unit runs out of health
				if (overlappedPlayerUnit[i].healthPoint <= 0)
				{
					//dead
					Debug.Log("Overlapped unit is dead!");
				
					//steal some action points based on how many soul points of the killing type enemy's unit has
					if (GameManager.instance.soulPoints[overlappedEnemyUnit[i].unitID-100] > 0)
					{
						//soul theft
						yield return StartCoroutine(UIManager.instance.ShowSoulTheftBumperAnimation());

						StartCoroutine(UnitManager.instance.GiveNormalActionPoints(GameManager.instance.soulPoints[overlappedEnemyUnit[i].unitID-100], overlappedPlayerUnit[i]));
					}
					
					//kill this unit
					yield return StartCoroutine(KillAUnit(overlappedPlayerUnit[i]));
				}
			}


			//if selecting special points and it's player's turns
			if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)
			{
				//if there's something to resurrect
				if (GameManager.instance.resurrectedGameObject.Count > 0)
				{
					//set true
					isContinue = true;

					yield return StartCoroutine(UIManager.instance.ShowResurrectionBumperAnimation()); 

					List<int> unitIDForResurrection = UnitManager.instance.SearchUnitIDByResurrectionEffect();

					Unit unitToFocus = new Unit();

					for (int i = 0; i < unitIDForResurrection.Count; i++)
					{
						//find to which unit the camera needs to focus
						if (TileDotManager.instance.SearchDotsToResurrectAroundAUnit(units[unitIDForResurrection[i]]).Count > 0)
						{
							unitToFocus = units[unitIDForResurrection[i]];

							break;
						}
					}

					Vector3 newCameraPosition = new Vector3(unitToFocus.associatedGameObject.transform.position.x, 0.45f, unitToFocus.associatedGameObject.transform.position.z - 2.5027f);
					Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);

					//rotate and move the camera to the target unit
					yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition, newCameraRotation));

					//deselect unit
					DeselectUnit(selectedUnit);

					//set selector status to resurrection
					GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 5);

					GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, GameManager.instance.resurrectTracker[0]);	
				}
				else
				{
					//set false
					isContinue = false;

					yield return StartCoroutine(UIManager.instance.ShowCrossfireBumperAnimation());
				}
			}
			else
			{
				//set false
				isContinue = false;

				yield return StartCoroutine(UIManager.instance.ShowCrossfireBumperAnimation());
			}				


		}
		//if not, then crossfire
		else
		{
			if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)	
			{
				//give special action points
				yield return StartCoroutine(UnitManager.instance.GiveSpecialActionPoints(1, GameManager.instance.selectedTypeOfPointsForPlayer-1, selectedUnit));
			}
			else
			{
				yield return StartCoroutine(GiveNormalActionPoints(1, selectedUnit));
			}

			//if selecting special points and it's player's turns
			if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)
			{
				//if there's something to resurrect
				if (GameManager.instance.resurrectedGameObject.Count > 0)
				{
					//set true
					isContinue = true;

					yield return StartCoroutine(UIManager.instance.ShowResurrectionBumperAnimation()); 

					List<int> unitIDForResurrection = UnitManager.instance.SearchUnitIDByResurrectionEffect();

					Unit unitToFocus = new Unit();

					for (int i = 0; i < unitIDForResurrection.Count; i++)
					{
						//find to which unit the camera needs to focus
						if (TileDotManager.instance.SearchDotsToResurrectAroundAUnit(units[unitIDForResurrection[i]]).Count > 0)
						{
							unitToFocus = units[unitIDForResurrection[i]];

							break;
						}
					}

					Vector3 newCameraPosition = new Vector3(unitToFocus.associatedGameObject.transform.position.x, 0.45f, unitToFocus.associatedGameObject.transform.position.z - 2.5027f);
					Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);

					//rotate and move the camera to the target unit
					yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition, newCameraRotation));

					//deselect unit
					DeselectUnit(selectedUnit);

					//set selector status to resurrection
					GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 5);

					GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, GameManager.instance.resurrectTracker[0]);	
				}
				else
				{
					//set false
					isContinue = false;

					yield return StartCoroutine(UIManager.instance.ShowCrossfireBumperAnimation());
				}
			}
			else
			{
				//set false
				isContinue = false;

				yield return StartCoroutine(UIManager.instance.ShowCrossfireBumperAnimation());
			}	


			/*yield return StartCoroutine(UIManager.instance.ShowCrossfireBumperAnimation());

			yield return StartCoroutine(GiveNormalActionPoints(1, selectedUnit));*/
		}

		if (!isContinue)
		{
			//deselect unit
			DeselectUnit(selectedUnit);
	
			//reset attack status that are stored in Game Manager
			GameManager.instance.attackTracker.Clear();
			GameManager.instance.actionPointNeededToAttack = 0;
			
			//change the color of crossfiring units back 
			for (int i = 0; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
			{
				TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = TileDotManager.instance.crossfiringPlayerUnits[i].initModelMaterial;
			}
	
			//get lines container
			GameObject lineContainer = GameObject.Find("AttackLines");
	
			int lastIndexForDifferentUnit = 0;
		
	
			//hide selector SSSSSS
			GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);
	
			//find number of enemy unique elements
			List<Unit> crossfiredPlayerUnitsDistinct = TileDotManager.instance.crossfiredEnemyUnits.Distinct().ToList();
	
			for (int h = 0; h < crossfiredPlayerUnitsDistinct.Count; h++)
			{
				for (int i = lastIndexForDifferentUnit; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
				{
					if (GameManager.instance.uiType == 1)
					{
						//make it so the attack does not consume AP
						TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "ATTACK\nTHIS UNIT\n(0 AP)";
					}
			
					if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiringPlayerUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject)
					{
						//move the object up a little bit
						TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
					}
					else
					if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
					{
						//move the object up a little bit
						TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
					}
		
					//attack a unit using the selected unit
					StartCoroutine(AttackAUnitWithAUnit(TileDotManager.instance.crossfiringPlayerUnits[i], TileDotManager.instance.crossfiredEnemyUnits[i]));
		
					//hide the corresponding line to show crossfire
					lineContainer.transform.GetChild(i).gameObject.SetActive(false);
	
					//if the next crossfired unit is not the same unit
					if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1)
					{
						if (TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
						{
							break;
						}
					}
				}
		
				yield return new WaitForSeconds(0.5f);
		
				for (int i = lastIndexForDifferentUnit; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
				{
					if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
					{
						//put up the attacked unit a little bit
						TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
					}
					else
					if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
					{
						//move the object up a little bit
						TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
					}
	
					//if the next crossfired unit is not the same unit
					if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1)
					{
						if (TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
						{
							break;
						}
					}
				}
		
				yield return new WaitForSeconds(3f);
		
				//assign direction and dots to a Unit
				for (int i = 0; i < units.Count; i++)
				{
					AssignDirectionToAUnit(units[i]);
					AssignDotsToAUnit(units[i]);
				}
		
				for (int i = lastIndexForDifferentUnit; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
				{
					TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
				
					if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
					{
						//variable to store the closest distance
						float closestDistance = 0f;
						int unitIDWithClosestDistance = 0;
		
						//turn the attacked unit to the closest attacking unit			
						for (int j = lastIndexForDifferentUnit; j < i+1; j++)
						{
							if (j > lastIndexForDifferentUnit)
							{
								float currentDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
		
								if (currentDistance <= closestDistance)
								{
									closestDistance = currentDistance;
									unitIDWithClosestDistance = j;
								}
							}
							else
							{
								closestDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
								unitIDWithClosestDistance = j;
							}					
						}
		
						yield return StartCoroutine(TurnAUnitToFaceADot(TileDotManager.instance.crossfiredEnemyUnits[i], TileDotManager.instance.dots[TileDotManager.instance.crossfiringPlayerUnits[unitIDWithClosestDistance].associatedDots[0]]));
		
						Debug.Log("Check Crossfired Unit HP!");
				
						//check if the current crossfired unit is dead due to crossfire
						if (TileDotManager.instance.crossfiredEnemyUnits[i].healthPoint <= 0)
						{
							//dead
							Debug.Log("Prev crossfired unit is dead!");
				
							//move the object up a little bit
							TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
				
							//change the material 
							//TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = null;
				
							//yield return new WaitForSeconds(0.25f);
				
							StartCoroutine(GiveNormalActionPoints(i-lastIndexForDifferentUnit+1, TileDotManager.instance.crossfiredEnemyUnits[i]));
	
							//kill this crossfired unit
							yield return StartCoroutine(KillAUnit(TileDotManager.instance.crossfiredEnemyUnits[i]));
						}
						else
						{
							//put the unique crossfired unit down
							TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
						}
				
						//save the last index so we know how many action points that we need to give if the crossfired unit is dead
						lastIndexForDifferentUnit = i+1;		
					}
					else
					if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
					{
						//variable to store the closest distance
						float closestDistance = 0f;
						int unitIDWithClosestDistance = 0;
		
						//turn the attacked unit to the closest attacking unit			
						for (int j = lastIndexForDifferentUnit; j < i+1; j++)
						{
							if (j > lastIndexForDifferentUnit)
							{
								float currentDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
		
								if (currentDistance <= closestDistance)
								{
									closestDistance = currentDistance;
									unitIDWithClosestDistance = j;
								}
							}
							else
							{
								closestDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
								unitIDWithClosestDistance = j;
							}					
						}
		
						yield return StartCoroutine(TurnAUnitToFaceADot(TileDotManager.instance.crossfiredEnemyUnits[i], TileDotManager.instance.dots[TileDotManager.instance.crossfiringPlayerUnits[unitIDWithClosestDistance].associatedDots[0]]));
		
						Debug.Log("Check Crossfired Unit HP!");
			
						//check if the prev crossfired unit is dead due to crossfire
						if (TileDotManager.instance.crossfiredEnemyUnits[i].healthPoint <= 0)
						{
							//dead
							Debug.Log("Prev crossfired unit is dead!");
			
							//move the object up a little bit
							TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
			
							//change the material 
							//TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = null;
			
							//yield return new WaitForSeconds(0.25f);
			
							StartCoroutine(GiveNormalActionPoints(i-lastIndexForDifferentUnit+1, TileDotManager.instance.crossfiredEnemyUnits[i]));
	
							//kill this crossfired unit
							yield return StartCoroutine(KillAUnit(TileDotManager.instance.crossfiredEnemyUnits[i]));
						}
						else
						{
							//put the unique crossfired unit down
							TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
						}
			
						//save the last index so we know how many action points that we need to give if the crossfired unit is dead
						lastIndexForDifferentUnit = i+1;
					}
	
					//if the next crossfired unit is not the same unit
					if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1)
					{
						if (TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
						{
							break;
						}
					}
				}
			}

			//Debug.Log("End Crossfire!");

			//reset to zero
			GameManager.instance.rotationTracker = 0;
		
			//show selector on the prev selected unit
			GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[selectedUnit.associatedDots[0]]);
		
			//call reset for rotation function
			TileDotManager.instance.ResetForRotation();
		}
		/*for (int i = 0; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
		{
			//Debug.Log("Crossfiring player unit ID: " + UnitManager.instance.units.IndexOf(TileDotManager.instance.crossfiringPlayerUnits[i]) + " will attack enemy unit ID: " + UnitManager.instance.units.IndexOf(TileDotManager.instance.crossfiredEnemyUnits[i]));
		
			if (GameManager.instance.uiType == 1)
			{
				//make it so the attack does not consume AP
				TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "ATTACK\nTHIS UNIT\n(0 AP)";
			}

			//move the object up a little bit
			TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
		
			//attack a unit using the selected unit
			yield return StartCoroutine(AttackAUnitWithAUnit(TileDotManager.instance.crossfiringPlayerUnits[i], TileDotManager.instance.crossfiredEnemyUnits[i]));

			//hide the corresponding line to show crossfire
			lineContainer.transform.GetChild(i).gameObject.SetActive(false);

			//move the object down a little bit
			TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
			
			//hide selector
			GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);

			//if the next crossfired unit is different than the current one
			if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
			{
				Debug.Log("Check Crossfired Unit HP!");

				//check if the current crossfired unit is dead due to crossfire
				if (TileDotManager.instance.crossfiredEnemyUnits[i].healthPoint <= 0)
				{
					//dead
					Debug.Log("Prev crossfired unit is dead!");

					//move the object up a little bit
					TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);

					//change the material 
					//TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = null;

					//yield return new WaitForSeconds(0.25f);

					//kill this crossfired unit
					yield return StartCoroutine(KillAUnit(TileDotManager.instance.crossfiredEnemyUnits[i]));

					yield return StartCoroutine(GiveActionPoints(i-lastIndexForDifferentUnit+1, TileDotManager.instance.crossfiredEnemyUnits[i]));
				}

				//save the last index so we know how many action points that we need to give if the crossfired unit is dead
				lastIndexForDifferentUnit = i+1;
			}
			else
			if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
			{
				Debug.Log("Check Crossfired Unit HP!");

				//check if the prev crossfired unit is dead due to crossfire
				if (TileDotManager.instance.crossfiredEnemyUnits[i].healthPoint <= 0)
				{
					//dead
					Debug.Log("Prev crossfired unit is dead!");

					//move the object up a little bit
					TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);

					//change the material 
					//TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = null;

					//yield return new WaitForSeconds(0.25f);

					//kill this crossfired unit
					yield return StartCoroutine(KillAUnit(TileDotManager.instance.crossfiredEnemyUnits[i]));

					yield return StartCoroutine(GiveActionPoints(i-lastIndexForDifferentUnit+1, TileDotManager.instance.crossfiredEnemyUnits[i]));
				}

				//save the last index so we know how many action points that we need to give if the crossfired unit is dead
				lastIndexForDifferentUnit = i+1;
			}
		}*/
	}

	// kill a unit
	public IEnumerator KillAUnit (Unit unitToKill)
	{
		//yield return new WaitForSeconds(0.5f);

		//show killed
		StartCoroutine(UIManager.instance.ShowOtherText(unitToKill, "KILLED!", Color.red));

		//if this is enemy's unit
		if (unitToKill.unitID >= 100 && unitToKill.unitID < 200)
		{
			HOTween.To
			(
				unitToKill.associatedGameObject.transform.GetChild(0).gameObject.transform, 
				0.9f, 
				new TweenParms()
			        .Prop
					(
						"localScale", 
						unitToKill.associatedGameObject.transform.GetChild(0).gameObject.transform.localScale*0.83f,
						false	
					) 
			        .Ease(EaseType.EaseInBack)
					.Delay(0)
			);
	
			HOTween.To
			(
				unitToKill.associatedGameObject.transform.GetChild(1).gameObject.transform, 
				0.9f, 
				new TweenParms()
			        .Prop
					(
						"localScale", 
						unitToKill.associatedGameObject.transform.GetChild(1).gameObject.transform.localScale*0.83f,
						false	
					) 
			        .Ease(EaseType.EaseInBack)
					.Delay(0)
			);
	
			HOTween.To
			(
				unitToKill.associatedGameObject.transform.GetChild(7).gameObject.transform, 
				0.9f, 
				new TweenParms()
			        .Prop
					(
						"localScale", 
						unitToKill.associatedGameObject.transform.GetChild(7).gameObject.transform.localScale*0.83f,
						false	
					) 
			        .Ease(EaseType.EaseInBack)
					.Delay(0)
			);
	
			yield return new WaitForSeconds(1f);
	
			//if in player's turn
			if (GameManager.instance.turnID == 1)
			{
				HOTween.To
				(
					unitToKill.associatedGameObject.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								Camera.main.transform.position.x + (2.52f - (0.3f * (unitToKill.unitID-100))),
								Camera.main.transform.position.y - 1f,
								Camera.main.transform.position.z + 3.21f
								/*2.2f,
								-0.31f,
								0.15f*/
							),
							false	
						) 
				        .Ease(EaseType.Linear)
						.Delay(0f)
				);
			}
			else
			//if in enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				HOTween.To
				(
					unitToKill.associatedGameObject.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								Camera.main.transform.position.x - (2.52f - (0.3f * (unitToKill.unitID-100))),
								Camera.main.transform.position.y - 1f,
								Camera.main.transform.position.z - 3.21f
								/*2.2f,
								-0.31f,
								0.15f*/
							),
							false	
						) 
				        .Ease(EaseType.Linear)
						.Delay(0f)
				);
			}
			
			HOTween.To
			(
				unitToKill.associatedGameObject.transform, 
				0.5f, 
				new TweenParms()
			        .Prop
					(
						"eulerAngles", 
						new Vector3
						(
							26f,
							61.88f,
							0f
						),
						false	
					) 
			        .Ease(EaseType.EaseInBack)
					.Delay(0f)
			);

			yield return new WaitForSeconds(0.5f);

			//if the attacked unit is enemy's unit
			GameManager.instance.soulPoints[unitToKill.unitID-100] += 1;

			//refresh action points on UI
			UIManager.instance.RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI();	

			//remove the killed enemy from the order list
			for (int i = 0; i < GameManager.instance.orderOfEnemyUnits.Count; i++)
			{
				//Debug.Log("Check");

				if (GameManager.instance.orderOfEnemyUnits[i] == unitToKill.associatedGameObject)
				{
					GameManager.instance.orderOfEnemyUnits.RemoveAt(i);
				}
			}

			//remove from hierarchy and Unit list
			Destroy (unitToKill.associatedGameObject);
			units.Remove(unitToKill);
		}
		else
		//if this is player's unit
		if (unitToKill.unitID >= 0 && unitToKill.unitID < 100)
		{
			//yield return new WaitForSeconds(0.5f);

			if (unitToKill.unitID != 2)
			{
				HOTween.To
				(
					unitToKill.associatedGameObject.transform.GetChild(0).gameObject.transform, 
					0.9f, 
					new TweenParms()
				        .Prop
						(
							"localScale", 
							unitToKill.associatedGameObject.transform.GetChild(0).gameObject.transform.localScale*0.83f,
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0)
				);
	
				HOTween.To
				(
					unitToKill.associatedGameObject.transform.GetChild(1).gameObject.transform, 
					0.9f, 
					new TweenParms()
				        .Prop
						(
							"localScale", 
							unitToKill.associatedGameObject.transform.GetChild(1).gameObject.transform.localScale*0.83f,
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0)
				);
		
				HOTween.To
				(
					unitToKill.associatedGameObject.transform.GetChild(7).gameObject.transform, 
					0.9f, 
					new TweenParms()
				        .Prop
						(
							"localScale", 
							unitToKill.associatedGameObject.transform.GetChild(7).gameObject.transform.localScale*0.83f,
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0)
				);
		
				yield return new WaitForSeconds(1f);
	
				if (GameManager.instance.turnID == 1)
				{
					HOTween.To
					(
						unitToKill.associatedGameObject.transform, 
						0.5f, 
						new TweenParms()
					        .Prop
							(
								"position", 
								new Vector3
								(
									Camera.main.transform.position.x - (2.52f - (0.3f * unitToKill.unitID)),
									Camera.main.transform.position.y - 1f,
									Camera.main.transform.position.z + 3.21f
									/*2.2f,
									-0.31f,
									0.15f*/
								),
								false	
							) 
					        .Ease(EaseType.Linear)
							.Delay(0f)
					);
				}
				else
				if (GameManager.instance.turnID == 2)
				{
					HOTween.To
					(
						unitToKill.associatedGameObject.transform, 
						0.5f, 
						new TweenParms()
					        .Prop
							(
								"position", 
								new Vector3
								(
									Camera.main.transform.position.x + (2.52f - (0.3f * unitToKill.unitID)),
									Camera.main.transform.position.y - 1f,
									Camera.main.transform.position.z - 3.21f
									/*2.2f,
									-0.31f,
									0.15f*/
								),
								false	
							) 
					        .Ease(EaseType.Linear)
							.Delay(0f)
					);
				}
				
				HOTween.To
				(
					unitToKill.associatedGameObject.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"eulerAngles", 
							new Vector3
							(
								26f,
								61.88f,
								0f
							),
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0f)
				);

				yield return new WaitForSeconds(0.5f);

				//if the attacked unit is player's unit
				GameManager.instance.specialPoints[unitToKill.unitID] += 1;
			}
			//if it's player's base
			else
			{
				HOTween.To
				(
					unitToKill.associatedGameObject.transform.GetChild(0).gameObject.transform, 
					0.9f, 
					new TweenParms()
				        .Prop
						(
							"localScale", 
							unitToKill.associatedGameObject.transform.GetChild(0).gameObject.transform.localScale*0f,
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0)
				);
	
				HOTween.To
				(
					unitToKill.associatedGameObject.transform.GetChild(1).gameObject.transform, 
					0.9f, 
					new TweenParms()
				        .Prop
						(
							"localScale", 
							unitToKill.associatedGameObject.transform.GetChild(1).gameObject.transform.localScale*0f,
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0)
				);
		
				HOTween.To
				(
					unitToKill.associatedGameObject.transform.GetChild(7).gameObject.transform, 
					0.9f, 
					new TweenParms()
				        .Prop
						(
							"localScale", 
							unitToKill.associatedGameObject.transform.GetChild(7).gameObject.transform.localScale*0f,
							false	
						) 
				        .Ease(EaseType.EaseInBack)
						.Delay(0)
				);
		
				yield return new WaitForSeconds(1f);
			}

			//refresh action points on UI
			UIManager.instance.RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI();	

			//remove from hierarchy and Unit list
			Destroy (unitToKill.associatedGameObject);
			units.Remove(unitToKill);
		}
	}

	public IEnumerator ResurrectAUnitToADot (Dot dotToResurrect)
	{
		InputManager.instance.isInputActive = false;

		GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);

		HOTween.To
		(
			GameManager.instance.resurrectedGameObject[0].transform, 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"position", 
					new Vector3
					(
						dotToResurrect.associatedGameObject.transform.position.x,
						-3f,
						dotToResurrect.associatedGameObject.transform.position.z
					),
					false	
				) 
		        .Ease(EaseType.Linear)
				.Delay(0f)
		);

		HOTween.To
		(
			GameManager.instance.resurrectedGameObject[0].transform, 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"eulerAngles", 
					new Vector3
					(
						0f,
						30f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.Linear)
				.Delay(0f)
		);

		HOTween.To
		(
			GameManager.instance.resurrectedGameObject[0].transform, 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"localScale", 
					new Vector3
					(
						1f,
						1f,
						1f
					),
					false	
				) 
		        .Ease(EaseType.Linear)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.5f);

		//make it a child of units container
		GameManager.instance.resurrectedGameObject[0].transform.parent = unitsContainer.transform;

		//units.Clear();

		//put units into list
		GenerateUnits();

		//assign direction and dots to a Unit
		for (int i = 0; i < units.Count; i++)
		{
			AssignDirectionToAUnit(units[i]);
			AssignDotsToAUnit(units[i]);
		}

		//remove the first one
		GameManager.instance.resurrectedGameObject.RemoveAt(0);

		InputManager.instance.isInputActive = true;

		//Debug.Log("Resurrected gameobject count: " + GameManager.instance.resurrectedGameObject.Count);

		//if there's no more gameobject to resurrect
		if (GameManager.instance.resurrectedGameObject.Count == 0)
		{
			//reset 
			GameManager.instance.resurrectTracker.Clear();
			GameManager.instance.resurrectedGameObject.Clear();
	
			//reset back cursor status to zero
			GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
		
			if (GameManager.instance.rotatedUnits.Count > 0)
			{
				#region DETERMINE CROSSFIRE
				TileDotManager.instance.crossfiredEnemyUnits.Clear();
				TileDotManager.instance.crossfiringPlayerUnits.Clear();
	
				for (int i = 0; i < GameManager.instance.rotatedUnits.Count; i++)
				{
					//Debug.Log("Start determining crossfire!");
	
					//if this is an enemy and not a Dimension Gate
					if (GameManager.instance.rotatedUnits[i].unitID >= 100 && GameManager.instance.rotatedUnits[i].unitID < 200 && GameManager.instance.rotatedUnits[i].unitID != 102)
					{
						Unit enemyUnit = new Unit();

						for (int h = 0; h < units.Count; h++)
						{
							if (units[h].associatedGameObject == GameManager.instance.rotatedUnits[i].associatedGameObject)
							{
								enemyUnit = units[h];
							}
						}

						//Debug.Log("Check crossfire this is an enemy: " + GameManager.instance.rotatedUnits[i].unitName + " with ID: " + units.IndexOf(enemyUnit) +  " at " + GameManager.instance.rotatedUnits[i].associatedGameObject.transform.position);

						List<Unit> playerUnit = new List<Unit>();
			
						for (int j = 0; j < units.Count; j++)
						{
							//try
							//List<int> searchResult = SearchUnitIDsByAttackRangeOfAUnit(units[units.Count-1]);
							//Debug.Log("Crossfiring unit has this in range: " + searchResult[0]);

							if (IsAUnitWithinAnotherUnitAttackRange(enemyUnit, units[j]))
							{
								//Debug.Log("Check crossfire unit is within another unit attack range");
					
								if (!IsPlayerUnitOverlappedWithEnemyUnit(units[j]))
								{
									//Debug.Log("Check crossfire add player unit!");

									playerUnit.Add(units[j]);
								}
							}
						}
		
						if (playerUnit.Count > 1)
						{
							//Debug.Log("Okay put to variable crossfire!");
	
							for (int j = 0; j < playerUnit.Count; j++)
							{
								Material yellowMaterial = Instantiate(playerUnit[j].associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material);
								yellowMaterial.color = Color.yellow;
								
								playerUnit[j].associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = yellowMaterial;
								
								TileDotManager.instance.crossfiredEnemyUnits.Add(enemyUnit);
								TileDotManager.instance.crossfiringPlayerUnits.Add(playerUnit[j]);
							}
						}
					}
				}
				#endregion
				
				//reset
				GameManager.instance.rotatedUnits.Clear();
	
				//if crossfire
				if (TileDotManager.instance.crossfiringPlayerUnits.Count > 0)
				{
					//Debug.Log("Crossfire after resurrection!");
		
					//hide Lines container
					GameObject lines = GameObject.Find("AttackLines");
	
					if (lines != null)
					{
						lines.SetActive(false);
					}
	
					//create a container for lines
					GameObject lineParent = new GameObject();
					lineParent.name = "AttackLines";
		
					for (int i = 0; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
					{
						//create objects in the middle of crossfire and crossfiring units
						GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
		
						Vector3 location = TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position + TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position;
		
						line.name = "Line";
						
						//if Red Soldier
						if (TileDotManager.instance.crossfiringPlayerUnits[i].unitID == 1)
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
		
						int directionToCrossfiredUnits = GetDirectionFromAUnitToAnotherUnit(TileDotManager.instance.crossfiringPlayerUnits[i], TileDotManager.instance.crossfiredEnemyUnits[i]);
		
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



					yield return StartCoroutine(UIManager.instance.ShowCrossfireBumperAnimation());
			
					
					//reset attack status that are stored in Game Manager
					GameManager.instance.attackTracker.Clear();
					GameManager.instance.actionPointNeededToAttack = 0;
					
					//change the color of crossfiring units back 
					for (int i = 0; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
					{
						TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = TileDotManager.instance.crossfiringPlayerUnits[i].initModelMaterial;
					}
			
					//get lines container
					GameObject lineContainer = GameObject.Find("AttackLines");
			
					int lastIndexForDifferentUnit = 0;
				
			
					//hide selector SSSSSS
					GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);
			
					//find number of enemy unique elements
					List<Unit> crossfiredPlayerUnitsDistinct = TileDotManager.instance.crossfiredEnemyUnits.Distinct().ToList();
			
					for (int h = 0; h < crossfiredPlayerUnitsDistinct.Count; h++)
					{
						Vector3 newCameraPosition = new Vector3(crossfiredPlayerUnitsDistinct[h].associatedGameObject.transform.position.x, 0.45f, crossfiredPlayerUnitsDistinct[h].associatedGameObject.transform.position.z - 2.5027f);
						Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);
	
						//rotate and move the camera to the target unit
						yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition, newCameraRotation));

						for (int i = lastIndexForDifferentUnit; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
						{
							if (GameManager.instance.uiType == 1)
							{
								//make it so the attack does not consume AP
								TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "ATTACK\nTHIS UNIT\n(0 AP)";
							}
					
							if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiringPlayerUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject)
							{
								//move the object up a little bit
								TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
							}
							else
							if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
							{
								//move the object up a little bit
								TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
							}
				
							//attack a unit using the selected unit
							StartCoroutine(AttackAUnitWithAUnit(TileDotManager.instance.crossfiringPlayerUnits[i], TileDotManager.instance.crossfiredEnemyUnits[i]));
				
							//hide the corresponding line to show crossfire
							lineContainer.transform.GetChild(i).gameObject.SetActive(false);
			
							//if the next crossfired unit is not the same unit
							if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1)
							{
								if (TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
								{
									break;
								}
							}
						}
				
						yield return new WaitForSeconds(0.5f);
				
						for (int i = lastIndexForDifferentUnit; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
						{
							if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
							{
								//put up the attacked unit a little bit
								TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
							}
							else
							if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
							{
								//move the object up a little bit
								TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
							}
			
							//if the next crossfired unit is not the same unit
							if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1)
							{
								if (TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
								{
									break;
								}
							}
						}
				
						yield return new WaitForSeconds(3f);
				
						//assign direction and dots to a Unit
						for (int i = 0; i < units.Count; i++)
						{
							AssignDirectionToAUnit(units[i]);
							AssignDotsToAUnit(units[i]);
						}
				
						for (int i = lastIndexForDifferentUnit; i < TileDotManager.instance.crossfiringPlayerUnits.Count; i++)
						{
							TileDotManager.instance.crossfiringPlayerUnits[i].associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
						
							if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1 && TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
							{
								//variable to store the closest distance
								float closestDistance = 0f;
								int unitIDWithClosestDistance = 0;
				
								//turn the attacked unit to the closest attacking unit			
								for (int j = lastIndexForDifferentUnit; j < i+1; j++)
								{
									if (j > lastIndexForDifferentUnit)
									{
										float currentDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
				
										if (currentDistance <= closestDistance)
										{
											closestDistance = currentDistance;
											unitIDWithClosestDistance = j;
										}
									}
									else
									{
										closestDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
										unitIDWithClosestDistance = j;
									}					
								}
				
								yield return StartCoroutine(TurnAUnitToFaceADot(TileDotManager.instance.crossfiredEnemyUnits[i], TileDotManager.instance.dots[TileDotManager.instance.crossfiringPlayerUnits[unitIDWithClosestDistance].associatedDots[0]]));
				
								Debug.Log("Check Crossfired Unit HP!");
						
								//check if the current crossfired unit is dead due to crossfire
								if (TileDotManager.instance.crossfiredEnemyUnits[i].healthPoint <= 0)
								{
									//dead
									Debug.Log("Prev crossfired unit is dead!");
						
									//move the object up a little bit
									TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
						
									StartCoroutine(GiveNormalActionPoints(i-lastIndexForDifferentUnit+1, TileDotManager.instance.crossfiredEnemyUnits[i]));
			
									//kill this crossfired unit
									yield return StartCoroutine(KillAUnit(TileDotManager.instance.crossfiredEnemyUnits[i]));
								}
								else
								{
									//put the unique crossfired unit down
									TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
								}
						
								//save the last index so we know how many action points that we need to give if the crossfired unit is dead
								lastIndexForDifferentUnit = i+1;		
							}
							else
							if (i == TileDotManager.instance.crossfiringPlayerUnits.Count-1)
							{
								//variable to store the closest distance
								float closestDistance = 0f;
								int unitIDWithClosestDistance = 0;
				
								//turn the attacked unit to the closest attacking unit			
								for (int j = lastIndexForDifferentUnit; j < i+1; j++)
								{
									if (j > lastIndexForDifferentUnit)
									{
										float currentDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
				
										if (currentDistance <= closestDistance)
										{
											closestDistance = currentDistance;
											unitIDWithClosestDistance = j;
										}
									}
									else
									{
										closestDistance = Mathf.Abs(Vector3.Magnitude(TileDotManager.instance.crossfiringPlayerUnits[j].associatedGameObject.transform.position - TileDotManager.instance.crossfiredEnemyUnits[j].associatedGameObject.transform.position));
										unitIDWithClosestDistance = j;
									}					
								}
				
								yield return StartCoroutine(TurnAUnitToFaceADot(TileDotManager.instance.crossfiredEnemyUnits[i], TileDotManager.instance.dots[TileDotManager.instance.crossfiringPlayerUnits[unitIDWithClosestDistance].associatedDots[0]]));
				
								Debug.Log("Check Crossfired Unit HP!");
					
								//check if the prev crossfired unit is dead due to crossfire
								if (TileDotManager.instance.crossfiredEnemyUnits[i].healthPoint <= 0)
								{
									//dead
									Debug.Log("Prev crossfired unit is dead!");
					
									//move the object up a little bit
									TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position += new Vector3(0f, 0.1f, 0f);
					
									StartCoroutine(GiveNormalActionPoints(i-lastIndexForDifferentUnit+1, TileDotManager.instance.crossfiredEnemyUnits[i]));
			
									//kill this crossfired unit
									yield return StartCoroutine(KillAUnit(TileDotManager.instance.crossfiredEnemyUnits[i]));
								}
								else
								{
									//put the unique crossfired unit down
									TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject.transform.position -= new Vector3(0f, 0.1f, 0f);
								}
					
								//save the last index so we know how many action points that we need to give if the crossfired unit is dead
								lastIndexForDifferentUnit = i+1;
							}
			
							//if the next crossfired unit is not the same unit
							if (i < TileDotManager.instance.crossfiringPlayerUnits.Count-1)
							{
								if (TileDotManager.instance.crossfiredEnemyUnits[i+1].associatedGameObject != TileDotManager.instance.crossfiredEnemyUnits[i].associatedGameObject)
								{
									break;
								}
							}
						}
					}


				}
	
				//call reset for rotation function
				TileDotManager.instance.ResetForRotation();
			}

			//show cursor on the dot
			GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, dotToResurrect);
		}
		else
		//otherwise, if there's more gameobject to resurrect
		{
			//recalculate tracker
			List<int> resurrectionUnits = SearchUnitIDByResurrectionEffect();

			List<Dot> availableDotsToResurrect = new List<Dot>();

			List<int> resurrectionUnitsWithAvailableSpace = new List<int>();

			for (int i = 0; i < resurrectionUnits.Count; i++)
			{
				List<Dot> tempDots = TileDotManager.instance.SearchDotsToResurrectAroundAUnit(units[resurrectionUnits[i]]);

				if (tempDots.Count > 0)
				{
					resurrectionUnitsWithAvailableSpace.Add(resurrectionUnits[i]);
				}

				for (int j = 0; j < tempDots.Count; j++)
				{
					availableDotsToResurrect.Add(tempDots[j]);
				}
			}

			GameManager.instance.resurrectTracker = availableDotsToResurrect;
			
			Vector3 newCameraPosition = new Vector3(units[resurrectionUnitsWithAvailableSpace[0]].associatedGameObject.transform.position.x, 0.45f, units[resurrectionUnitsWithAvailableSpace[0]].associatedGameObject.transform.position.z - 2.5027f);
			Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);

			//rotate and move the camera to the target unit
			StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition, newCameraRotation));

			//show resurrection selector
			GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 5);

			//show cursor on the first dot of available resurrected dot	
			GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, GameManager.instance.resurrectTracker[0]);
		}
	}

	//create unit 
	public IEnumerator GiveSpecialActionPoints (int amountOfSpecialPoint, int unitIDOfSpentSpecialPoints, Unit unitBelow)
	{
		GameManager.instance.resurrectTracker.Clear();
		GameManager.instance.resurrectedGameObject.Clear();

		//if player's unit
		if (unitIDOfSpentSpecialPoints >= 0 && unitIDOfSpentSpecialPoints < 100)
		{
			//reduce the relatead action points
			GameManager.instance.specialPoints[unitIDOfSpentSpecialPoints] -= amountOfSpecialPoint;

			List<int> resurrectionUnits = SearchUnitIDByResurrectionEffect();

			List<Dot> availableDotsToResurrect = new List<Dot>();

			for (int i = 0; i < resurrectionUnits.Count; i++)
			{
				List<Dot> tempDots = TileDotManager.instance.SearchDotsToResurrectAroundAUnit(units[resurrectionUnits[i]]);

				for (int j = 0; j < tempDots.Count; j++)
				{
					availableDotsToResurrect.Add(tempDots[j]);
				}
			}

			GameObject newCreatedUnit = null;

			for (int i = 0; i < amountOfSpecialPoint; i++)
			{
				newCreatedUnit = Instantiate(playerUnitsTemplate[unitIDOfSpentSpecialPoints]);
		
				newCreatedUnit.transform.position = unitBelow.associatedGameObject.transform.position;
				newCreatedUnit.transform.parent = unitBelow.associatedGameObject.transform;
				newCreatedUnit.transform.eulerAngles = unitBelow.associatedGameObject.transform.eulerAngles;
				newCreatedUnit.name = "Unit_Container";
				newCreatedUnit.SetActive(true);
	
				HOTween.To
				(
					newCreatedUnit.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								0f,
								0.65f + (i * 0.2f),
								0f
							),
							true	
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
				);

				newCreatedUnit.transform.localScale = new Vector3(0.67f, 0.67f, 0.67f);

				GameManager.instance.resurrectedGameObject.Add(newCreatedUnit);
			}
			
			yield return new WaitForSeconds(0.5f);

			//if there's a place to resurrect
			if (availableDotsToResurrect.Count > 0)
			{
				GameManager.instance.resurrectTracker = availableDotsToResurrect;

				//if special points that are being used are more than the avialble spaces
				if (amountOfSpecialPoint > GameManager.instance.resurrectTracker.Count)
				{
					List<GameObject> toDestroy = new List<GameObject>();

					//kill the last
					for (int i = GameManager.instance.resurrectTracker.Count; i < amountOfSpecialPoint; i++)
					{
						//make smaller
						HOTween.To
						(
							GameManager.instance.resurrectedGameObject[i].transform.GetChild(0).gameObject.transform, 
							0.9f, 
							new TweenParms()
						        .Prop
								(
									"localScale", 
									Vector3.zero,
									false	
								) 
						        .Ease(EaseType.EaseInBack)
								.Delay(0)
						);
					
						HOTween.To
						(
							GameManager.instance.resurrectedGameObject[i].transform.GetChild(1).gameObject.transform, 
							0.9f, 
							new TweenParms()
						        .Prop
								(
									"localScale", 
									Vector3.zero,
									false	
								) 
						        .Ease(EaseType.EaseInBack)
								.Delay(0)
						);
						
						HOTween.To
						(
							GameManager.instance.resurrectedGameObject[i].transform.GetChild(7).gameObject.transform, 
							0.9f, 
							new TweenParms()
						        .Prop
								(
									"localScale", 
									Vector3.zero,
									false	
								) 
						        .Ease(EaseType.EaseInBack)
								.Delay(0)
						);

						toDestroy.Add(GameManager.instance.resurrectedGameObject[i]);
					}

					yield return new WaitForSeconds(1f);

					//destroy
					for (int i = 0; i < toDestroy.Count; i++)
					{
						GameManager.instance.resurrectedGameObject.RemoveAt(GameManager.instance.resurrectTracker.Count);

						Destroy(toDestroy[i]);
					}

					//Debug.Log("Count of resurrectedGameObject: " + GameManager.instance.resurrectedGameObject.Count);
				} 
			}
			else
			//otherwise
			{
				for (int i = 0; i < GameManager.instance.resurrectedGameObject.Count; i++)
				{
					//make smaller and destroy	
					HOTween.To
					(
						GameManager.instance.resurrectedGameObject[i].transform.GetChild(0).gameObject.transform, 
						0.9f, 
						new TweenParms()
					        .Prop
							(
								"localScale", 
								Vector3.zero,
								false	
							) 
					        .Ease(EaseType.EaseInBack)
							.Delay(0)
					);
				
					HOTween.To
					(
						GameManager.instance.resurrectedGameObject[i].transform.GetChild(1).gameObject.transform, 
						0.9f, 
						new TweenParms()
					        .Prop
							(
								"localScale", 
								Vector3.zero,
								false	
							) 
					        .Ease(EaseType.EaseInBack)
							.Delay(0)
					);
					
					HOTween.To
					(
						GameManager.instance.resurrectedGameObject[i].transform.GetChild(7).gameObject.transform, 
						0.9f, 
						new TweenParms()
					        .Prop
							(
								"localScale", 
								Vector3.zero,
								false	
							) 
					        .Ease(EaseType.EaseInBack)
							.Delay(0)
					);
				}
				
				yield return new WaitForSeconds(1f);

				for (int i = 0; i < GameManager.instance.resurrectedGameObject.Count; i++)
				{
					Destroy(GameManager.instance.resurrectedGameObject[i]);
				}

				//clear everything
				GameManager.instance.resurrectTracker.Clear();
				GameManager.instance.resurrectedGameObject.Clear();
			}
		}

		yield return new WaitForSeconds(0f);

		//refresh action points on UI
		UIManager.instance.RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI();	
	}

	// for UI type 2. Move a unit to a dot based on the created path
	public IEnumerator MoveAUnitToDotBasedOnCreatedPath (Unit unitToMove)
	{
		//get the container of movement lines
		GameObject movementLinesContainer = GameObject.Find("MovementLines");

		//hide cursor
		GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);

		if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)	
		{
			//give special action points
			yield return StartCoroutine(GiveSpecialActionPoints(GameManager.instance.movementTracker.Count-1, GameManager.instance.selectedTypeOfPointsForPlayer-1, unitToMove));
		}
		else
		{
			//give action points
			yield return StartCoroutine(GiveNormalActionPoints(GameManager.instance.movementTracker.Count-1, unitToMove));
		}

		/*int unitIDToCheck = SearchUnitIDByPosition(unitToMove.associatedGameObject.transform.position);

		//if enemy's unit
		if (unitToMove.unitID >= 100 && unitToMove.unitID < 200)
		{
			
			//if there's a player's unit on the same dot with enemy's
			if (unitIDToCheck >= 0 && unitIDToCheck < 100)
			{
				
			}
		}*/

		for (int i = 1; i < GameManager.instance.movementTracker.Count; i++)
		{
			yield return StartCoroutine(MoveAUnitToANeighborDot(unitToMove, GameManager.instance.movementTracker[i]));

			//hide line
			movementLinesContainer.transform.GetChild(i-1).gameObject.SetActive(false);
		}
		
		if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)	
		{
			//destroy the ones that already there
			for (int i = unitToMove.associatedGameObject.transform.childCount-1; i > (unitToMove.associatedGameObject.transform.childCount-1)-(GameManager.instance.movementTracker.Count-1); i--)
			{
				//if a unit
				if (unitToMove.associatedGameObject.transform.GetChild(i).gameObject.name == "Unit_Container")
				{
					Destroy(unitToMove.associatedGameObject.transform.GetChild(i).gameObject);
				}
			}

			//increase special action point for sometime
			GameManager.instance.specialPoints[GameManager.instance.selectedTypeOfPointsForPlayer-1] += GameManager.instance.movementTracker.Count-1;

			//give special action points
			yield return StartCoroutine(GiveSpecialActionPoints(GameManager.instance.movementTracker.Count-1, GameManager.instance.selectedTypeOfPointsForPlayer-1, unitToMove));
		}

		//if enemy's unit that is being moved
		if (unitToMove.unitID >= 100 && unitToMove.unitID < 200)
		{
			int unitIDOfPlayerToCheck = SearchPlayerUnitIDByPosition(unitToMove.associatedGameObject.transform.position);

			//if there's a player unit at the same position of enemy's unit
			if (unitIDOfPlayerToCheck > -1)
			{
				Unit unitOfPlayerToCheck = units[unitIDOfPlayerToCheck];

				//bring player's unit up
				HOTween.To
				(
					unitOfPlayerToCheck.associatedGameObject.transform, 
					0.5f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								0f,
								0.65f,
								0f
							),
							true	
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
				);

				yield return new WaitForSeconds(0.5f);

				//reduce HP
				unitOfPlayerToCheck.healthPoint -= unitToMove.damagePoint;
	
				//show damage -number animation
				yield return StartCoroutine(UIManager.instance.ShowOtherText(unitOfPlayerToCheck, "-" + unitToMove.damagePoint + " HP", Color.red));

				//check if the stomped unit runs out of health
				if (unitOfPlayerToCheck.healthPoint <= 0)
				{
					//dead
					Debug.Log("Stomped unit is dead!");
				
					//if it's not player's base
					if (unitOfPlayerToCheck.unitID != 2)
					{
						Debug.Log("K");

						//steal some action points based on how many soul points of the killing type enemy's unit has
						if (GameManager.instance.soulPoints[unitToMove.unitID-100] > 0)
						{
							//soul theft
							yield return StartCoroutine(UIManager.instance.ShowSoulTheftBumperAnimation());
			
							StartCoroutine(GiveNormalActionPoints(GameManager.instance.soulPoints[unitToMove.unitID-100], unitOfPlayerToCheck));
						}

						//kill this unit
						yield return StartCoroutine(KillAUnit(unitOfPlayerToCheck));
					}
					else
					{
						//kill this unit
						yield return StartCoroutine(KillAUnit(unitOfPlayerToCheck));
					}
				}
			}

			//find if there are player's unit in its sight range
			HidePlayerUnitsInEnemysTurn();
		}

		//if player's unit
		if (unitToMove.unitID >= 0 && unitToMove.unitID < 100)
		{
			Vector3 newCameraPosition2 = new Vector3(unitToMove.associatedGameObject.transform.position.x, 0.45f, unitToMove.associatedGameObject.transform.position.z - 2.5027f);
			Vector3 newCameraRotation2 = new Vector3(45f, 0f, 0f);
			
			//rotate and move the camera to the target unit
			yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition2, newCameraRotation2));
		}
		else
		//if enemy's unit
		if (unitToMove.unitID >= 100 && unitToMove.unitID < 200)
		{
			Vector3 newCameraPosition2 = new Vector3(unitToMove.associatedGameObject.transform.position.x, 0.45f, unitToMove.associatedGameObject.transform.position.z + 2.5027f);
			Vector3 newCameraRotation2 = new Vector3(45f, 180f, 0f);
			
			//rotate and move the camera to the target unit
			yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition2, newCameraRotation2));
		}

		//check if there are enemies in attack range
		List<int> attackedToBeUnitOrBuilding = new List<int>();
		attackedToBeUnitOrBuilding = SearchUnitIDsByAttackRangeOfAUnit(unitToMove);
	
		if (attackedToBeUnitOrBuilding.Count > 0)
		{
			//Debug.Log("Attack after moving!");
	
			//set selector status to normal
			//GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);

			//list of possible actions
			List<string> possibleActions = new List<string>();

			possibleActions.Add("ATTACK");

			possibleActions.Add("EXIT");

			//assign the amount of action points that are needed to attack
			GameManager.instance.actionPointNeededToAttack = 0;

			//show ui for action options
			UIManager.instance.ShowActionOptionsWindowOnAUnit(unitToMove, possibleActions);

			//put and show the cursor on the moved unit
			//GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToMove.associatedDots[0]]);	

			//put the cursor on the first enemy that is in attack
			/*GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[units[attackedToBeUnitOrBuilding[0]].associatedDots[0]]);
	
			for (int i = 0; i < attackedToBeUnitOrBuilding.Count; i++)
			{
				//show attack text on units that can be attacked by the selected unit
				UIManager.instance.ShowAttackText(units[attackedToBeUnitOrBuilding[i]], "ATTACK\nTHIS UNIT\n(0 AP)");
			}
	
			//show stay text on selected Unit
			UIManager.instance.ShowStayText(unitToMove);*/
		}
		else
		//if not 
		{
			//put the moved unit to idle
			DeselectUnit(unitToMove);

			//if selecting special points and it's player's turns
			if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)
			{
				//if there's something to resurrect
				if (GameManager.instance.resurrectedGameObject.Count > 0)
				{
					//destroy 

					//give special action points
					//yield return StartCoroutine(GiveSpecialActionPoints(GameManager.instance.movementTracker.Count-1, GameManager.instance.selectedTypeOfPointsForPlayer-1, unitToMove));

					yield return StartCoroutine(UIManager.instance.ShowResurrectionBumperAnimation()); 

					List<int> unitIDForResurrection = SearchUnitIDByResurrectionEffect();

					Unit unitToFocus = new Unit();

					for (int i = 0; i < unitIDForResurrection.Count; i++)
					{
						//find to which unit the camera needs to focus
						if (TileDotManager.instance.SearchDotsToResurrectAroundAUnit(units[unitIDForResurrection[i]]).Count > 0)
						{
							unitToFocus = units[unitIDForResurrection[i]];

							break;
						}
					}

					Vector3 newCameraPosition = new Vector3(unitToFocus.associatedGameObject.transform.position.x, 0.45f, unitToFocus.associatedGameObject.transform.position.z - 2.5027f);
					Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);

					//rotate and move the camera to the target unit
					yield return StartCoroutine(GameManager.instance.FocusCameraToAPosition(newCameraPosition, newCameraRotation));

					//set selector status to resurrection
					GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 5);

					GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, GameManager.instance.resurrectTracker[0]);	
				}
				//if there's no gameobject to resurrect
				else
				{
					//set selector status to normal
					GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
			
					//put and show the cursor on the moved unit
					GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToMove.associatedDots[0]]);	
				}
			}
		}

		//possibleActions.Add("ROTATE");
		//possibleActions.Add("MOVE");

		//reset to 0
		GameManager.instance.distanceCursorToSelectedUnit = 0;
		
		//reset movement list
		GameManager.instance.movementTracker.Clear();
		
		//remove movement lines Container
		Destroy(movementLinesContainer);
	
		//if using normal action point
		if (GameManager.instance.selectedTypeOfPointsForPlayer == 0 || GameManager.instance.turnID == 2)
		{
			//set selector status to normal
			GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
	
			//put and show the cursor on the moved unit
			GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[unitToMove.associatedDots[0]]);	
		}
	}

	// turn units, 1 for clockwise, 2 for counter clockwise
	public IEnumerator TurnUnitAround (Unit unitWhoRotates, int direction)
	{
		//if clockwise
		if (direction == 1)
		{
			if (GameManager.instance.uiType == 2)
			{
				//change the value of turnTracker
				if (GameManager.instance.turnTracker < 3)
				{
					GameManager.instance.turnTracker += 1;
				}
			}

			//Debug.Log("Turn Clockwise");
	
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

			//tween the rotation of unit who rotates
			HOTween.To
			(
				unitWhoRotates.associatedGameObject.transform, 
				0.4f, 
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

			//wait for sometime
			yield return new WaitForSeconds(0.5f);

			unitWhoRotates.associatedGameObject.transform.localEulerAngles = new Vector3(0f, yTargetRotation, 0f);
		}
		else
		//if counter clockwise
		if (direction == 2)
		{
			if (GameManager.instance.uiType == 2)
			{
				//change the value of turnTracker
				if (GameManager.instance.turnTracker > -3)
				{
					GameManager.instance.turnTracker -= 1;
				}
			}

			//Debug.Log("Turn Counter Clockwise");
	
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

			//tween the rotation of unit who rotates
			HOTween.To
			(
				unitWhoRotates.associatedGameObject.transform, 
				0.4f, 
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

			//wait for sometime
			yield return new WaitForSeconds(0.5f);

			unitWhoRotates.associatedGameObject.transform.localEulerAngles = new Vector3(0f, yTargetRotation, 0f);
		}

		TileDotManager.instance.GenerateDots();
		
		//put associated dots of a Tile
		for (int i = 0; i < TileDotManager.instance.tiles.Count; i++)
		{
			TileDotManager.instance.AssignDotsToATile(TileDotManager.instance.tiles[i]);
		}
	
		//assign neighbouring dots to each dot
		for (int i = 0; i < TileDotManager.instance.dots.Count; i++)
		{
			TileDotManager.instance.AssignDotWithNeighbourDots(TileDotManager.instance.dots[i]);
		}

		//check dots if still has at least one neighbour. if not, destroy it
		for (int i = 0; i < TileDotManager.instance.dots.Count; i++)
		{
			for (int j = 0; j < TileDotManager.instance.dots[i].neighboringDots.Count; j++)
			{
				if (TileDotManager.instance.dots[i].neighboringDots[j] > -1)
				{
					break;
				}

				if (j == 5)
				{
					Destroy(TileDotManager.instance.dots[i].associatedGameObject);
					TileDotManager.instance.dots.Remove(TileDotManager.instance.dots[i]);
				}
			}
		}

		//assign direction and dots to a Unit
		for (int i = 0; i < units.Count; i++)
		{
			AssignDirectionToAUnit(units[i]);
			AssignDotsToAUnit(units[i]);
		}

		if (GameManager.instance.uiType == 2)
		{
			int valueToShow = Mathf.Abs(GameManager.instance.turnTracker);

			if (valueToShow > 1)
			{
				valueToShow = 1;
			}

			//assign text to a cursor
			UIManager.instance.ShowTurnActionPointTextOnCursor(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, "" + valueToShow);
		}

		//find if there are player's unit in its sight range
		HidePlayerUnitsInEnemysTurn();

		yield return new WaitForSeconds(0.25f);

		for (int i = 1; i < TileDotManager.instance.dotsContainer.transform.childCount; i++)
		{
			if (!TileDotManager.instance.dotsContainer.transform.GetChild(i).gameObject.activeSelf)
			{
				Destroy(TileDotManager.instance.dotsContainer.transform.GetChild(i).gameObject);
			}
		}

		InputManager.instance.isInputActive = true;
	}

	public void ShowAllPlayerUnits ()
	{
		for (int i = 0; i < units.Count; i++)
		{
			//if this is player's unit
			if (units[i].unitID >= 0 && units[i].unitID < 100)
			{
				units[i].associatedGameObject.SetActive(true);
			}
		}
	}

	//return a list of player's unit IDs that are within a sight range of a certain enemies
	public List<int> SearchUnitIDsByEnemysSightRange (Unit unitToCheck)
	{
		List<int> unitIDsInEnemySightRange = new List<int>();

		List<int> dotIDsEnemySightRange = new List<int>();
		Dot dotBelowUnit = TileDotManager.instance.dots[unitToCheck.associatedDots[0]];

		//if this is enemy's unit
		if (unitToCheck.unitID >= 100 && unitToCheck.unitID < 200)
		{
			//CHECK SIGHT RANGE //TODO add more cases for more enemies
			//if shooter
			if (unitToCheck.unitID == 101)
			{
				//sight range is a dot in the front of them
				if (dotBelowUnit.neighboringDots[unitToCheck.direction] > -1)
				{
					dotIDsEnemySightRange.Add(dotBelowUnit.neighboringDots[unitToCheck.direction]);

					//Debug.Log("Player's unit in range!");
				}
			}
			else
			//if stomper
			if (unitToCheck.unitID == 100)
			{
				//sight range is 3 dots in the front of them
				//dot in the front of a unit
				if (dotBelowUnit.neighboringDots[unitToCheck.direction] > -1)
				{
					dotIDsEnemySightRange.Add(dotBelowUnit.neighboringDots[unitToCheck.direction]);
				}
				
				//dot in the front left of a unit
				if (unitToCheck.direction - 1 < 0)
				{
					if (dotBelowUnit.neighboringDots[5] > -1)
					{
						dotIDsEnemySightRange.Add(dotBelowUnit.neighboringDots[5]);
					}
				}
				else
				{
					if (dotBelowUnit.neighboringDots[unitToCheck.direction-1] > -1)
					{
						dotIDsEnemySightRange.Add(dotBelowUnit.neighboringDots[unitToCheck.direction-1]);
					}
				}
	
				//dot in the front right of a unit
				if (unitToCheck.direction + 1 > 5)
				{
					if (dotBelowUnit.neighboringDots[0] > -1)
					{
						dotIDsEnemySightRange.Add(dotBelowUnit.neighboringDots[0]);
					}
				}
				else
				{
					if (dotBelowUnit.neighboringDots[unitToCheck.direction+1] > -1)
					{
						dotIDsEnemySightRange.Add(dotBelowUnit.neighboringDots[unitToCheck.direction+1]);
					}
				}
			}
		}

		for (int i = 0; i < units.Count; i++)
		{
			//if this is player's unit
			if (units[i].unitID >= 0 && units[i].unitID < 100)
			{
				//check if this unit is on player
				if (dotIDsEnemySightRange.Contains(units[i].associatedDots[0]))
				{
					unitIDsInEnemySightRange.Add(i);
				} 
			}
		}

		return unitIDsInEnemySightRange;
	}

	public void HidePlayerUnitsInEnemysTurn ()
	{
		List<int> unitIDsInEnemySightRange = new List<int>();

		//Debug.Log("Hide Player Units in Enemy's Turn!");

		for (int i = 0; i < units.Count; i++)
		{
			//if this is enemy's unit and not a power plant
			if (units[i].unitID >= 100 && units[i].unitID < 200 && units[i].unitID != 2)
			{
				List<int> temp = SearchUnitIDsByEnemysSightRange(units[i]);

				/*if (temp.Count > 0)
				{
					Debug.Log("Player's Unit ID: " + temp[0]);
				}*/

				//unitIDsInEnemySightRange.Concat(temp).ToList();

				for (int j = 0; j < temp.Count; j++)
				{
					unitIDsInEnemySightRange.Add(temp[j]);
				}
			}
		}	

		for (int i = 0; i < units.Count; i++)
		{
			//if this is a power plant
			if (units[i].unitID == 2)
			{
				unitIDsInEnemySightRange.Add(i);
			}
		}

		for (int i = 0; i < units.Count; i++)
		{
			//if this is player's unit
			if (units[i].unitID >= 0 && units[i].unitID < 100)
			{
				units[i].associatedGameObject.SetActive(false);
			}
		}

		//Debug.Log("Player's ID: " + unitIDsInEnemySightRange[0]);
	
		//show player's unit which is within sight range of enemy's unit
		for (int i = 0; i < unitIDsInEnemySightRange.Count; i++)
		{
			units[unitIDsInEnemySightRange[i]].associatedGameObject.SetActive(true);
		}
	}

	// to reset rotation points of all units
	public void ResetRotationPoints ()
	{
		for (int i = 0; i < units.Count; i++)
		{
			units[i].rotationPoint = units[i].fullRotationPoint;
		}
	}

	// check if there is one or more enemy Unit die/s on the gameplay, return li
	/*public List<int> SearchDeadEnemyUnits ()
	{
		
	}*/
}
