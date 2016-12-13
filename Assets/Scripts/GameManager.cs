using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	// A container that holds cursor
	public GameObject cursorsContainer;

	// Toggle UI layout. 1 for old UI, 2 for new UI
	public int uiType;

	// To know whether the selector is currently moving or not-**
	public bool isSelectorMoving;

	// To know whose turn it right now, Player == 1 or Enemy == 2
	public int turnID = 0;

	// Stamina limit for enemy units
	public int staminaLimit;

	//to know what is selected right now. 0 means normal action point, 1 means first special points, 2 means second special points, etc.
	public int selectedTypeOfPointsForPlayer;

	// Action Points. Index 0 for Player, Index 1 for Enemy
	public int[] actionPoints;

	// Special Points. Index 0 for Green soldier, Index 1 for Red soldier
	public int[] specialPoints;

	// Soul Points. Index 0 for dead bodies of Stomper, Index 1 for Dead Bodies of 
	public int[] soulPoints;

	// A boolean to know whether soulPoints system is activated or not
	public bool isSoulPointActivated;

	// The order of enemy's unit movement
	public List<GameObject> orderOfEnemyUnits = new List<GameObject>();

	// A boolean to know whether specialPoints system is activated or not
	//public bool isSpecialPointActivated;

	// To track rotation, how far the rotation has been going. 0 is initial position, 1 is clockwise position, -1 is counter-clockwise position
	[HideInInspector] public int rotationTracker;

	// List of rotated units
	[HideInInspector] public List<Unit> rotatedUnits = new List<Unit>();

	// To track turn, how far the turn has been going. 0 is initial position, 1 is clockwise position, -1 is counter-clockwise position
	[HideInInspector] public int turnTracker;

	// Distance of cursor from a selected unit in tiles unit 
	[HideInInspector] public int distanceCursorToSelectedUnit; 

	// List of moving points to move
	[HideInInspector] public List<Dot> movementTracker = new List<Dot>();

	// To track how many action points that an attack can have
	[HideInInspector] public List<Unit> attackTracker = new List<Unit>();

	// To know how many action points that an attack can have
	[HideInInspector] public int actionPointNeededToAttack;

	// List of dots where player can resurrect
	[HideInInspector] public List<Dot> resurrectTracker = new List<Dot>();
	// List of gameobject that needs to be resurrected
	[HideInInspector] public List<GameObject> resurrectedGameObject = new List<GameObject>();

	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
		//StartCoroutine(SwitchTurn(2));
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// to switch turn to player's or enemy's turn. 1 for player, 2 for enemy
	public IEnumerator SwitchTurn (int targetTurnID)
	{
		//check if this is literally a switching turn event or not
		if (turnID == targetTurnID)
		{
			yield break;
		}

		//turn off the input for a while
		InputManager.instance.isInputActive = false;

		//hide selector
		HideSelector(cursorsContainer.transform.GetChild(0).gameObject);

		//new turn
		turnID = targetTurnID;

		//if switching to player's turn
		if (targetTurnID == 1)
		{
			//start to animate the background to red
			HOTween.To
			(
				Camera.main, 
				5f, 
				new TweenParms()
			        .Prop
					(
						"backgroundColor", 
						new Color(0f, 0f, 0.25f, 0f),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);

			//animate 
			yield return StartCoroutine(UIManager.instance.ShowTurnBumperAnimation(1));

			#region RESET ROTATION POINT
			UnitManager.instance.ResetRotationPoints();
			#endregion

			#region UPDATE STAMINA LIMIT
			//update stamina limit 
			if (staminaLimit == 3)
			{
				//go back to 1
				staminaLimit = 1;
			}
			else
			{
				staminaLimit += 1;
			}

			//refresh action points on UI
			UIManager.instance.RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI();	
			#endregion
		}
		else
		//if switching to enemy's turn
		if (targetTurnID == 2)
		{
			//start to animate the background to red
			HOTween.To
			(
				Camera.main, 
				5f, 
				new TweenParms()
			        .Prop
					(
						"backgroundColor", 
						new Color(0.25f, 0f, 0f, 0f),
						false	
					) 
			        .Ease(EaseType.EaseOutBack)
					.Delay(0f)
			);

			//animate 
			yield return StartCoroutine(UIManager.instance.ShowTurnBumperAnimation(2));
		}

		//find the target camera unit
		Unit targetCameraUnit = new Unit();
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			if (targetTurnID == 1)
			{
				//if this unit is a player
				if (UnitManager.instance.units[i].unitID >= 0 &&  UnitManager.instance.units[i].unitID < 100)
				{
					targetCameraUnit = UnitManager.instance.units[i];

					break;
				}
			}
			else
			if (targetTurnID == 2)
			{
				//if this unit is an enemy
				if (UnitManager.instance.units[i].unitID >= 100 &&  UnitManager.instance.units[i].unitID < 200)
				{
					targetCameraUnit = UnitManager.instance.units[i];

					break;
				}
			}
		}

		//find the new camera position and rotation
		Vector3 newCameraPosition = Vector3.zero;
		Vector3 newCameraRotation = Vector3.zero;

		//if switching to player's turn
		if (targetTurnID == 1)
		{
			newCameraPosition = new Vector3(targetCameraUnit.associatedGameObject.transform.position.x, 0.45f, targetCameraUnit.associatedGameObject.transform.position.z - 2.5027f);
			newCameraRotation = new Vector3(45f, 0f, 0f);
		}
		else
		//if switching to enemy's turn
		if (targetTurnID == 2)
		{
			newCameraPosition = new Vector3(targetCameraUnit.associatedGameObject.transform.position.x, 0.45f, targetCameraUnit.associatedGameObject.transform.position.z + 2.5027f);
			newCameraRotation = new Vector3(45f, 180f, 0f);
		}

		//focus to a specific dimension gate
		yield return StartCoroutine(FocusCameraToAPosition(newCameraPosition, newCameraRotation));

		//hide player's unit when going to enemy's turn
		if (targetTurnID == 2)
		{
			//change cursor rotation
			cursorsContainer.transform.GetChild(0).gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);

			UnitManager.instance.HidePlayerUnitsInEnemysTurn();
		}
		else
		if (targetTurnID == 1)
		{
			//change cursor rotation
			cursorsContainer.transform.GetChild(0).gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);

			UnitManager.instance.ShowAllPlayerUnits();
		}

		yield return new WaitForSeconds(0.25f);

		//if going to enemy's turn
		if (targetTurnID == 2)
		{
			//give bonus action point to enemy depends on how many stamina limit we have right now
			yield return StartCoroutine(GameManager.instance.GiveBonusActionPoints(staminaLimit, targetTurnID));

			//update stamina of each of enemy's unit
			UnitManager.instance.UpdateEnemyUnitsStamina();
		}

		//show selector on a target unit
		ShowSelectorOnAPosition(cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[targetCameraUnit.associatedDots[0]]);

		//if going to enemy's turn
		if (targetTurnID == 2)
		{
			newCameraPosition = new Vector3(cursorsContainer.transform.GetChild(0).gameObject.transform.position.x, 0.45f, cursorsContainer.transform.GetChild(0).gameObject.transform.position.z + 2.5027f);
		}
		else
		//if going to player's turn
		if (targetTurnID == 1)
		{
			newCameraPosition = new Vector3(cursorsContainer.transform.GetChild(0).gameObject.transform.position.x, 0.45f, cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - 2.5027f);
		}

		//focus to a specific cursor
		yield return StartCoroutine(FocusCameraToAPosition(newCameraPosition, Camera.main.transform.eulerAngles));

		//activate the input again
		InputManager.instance.isInputActive = true;
	}

	//give bonus action point(s) to either player or enemy. 1 for player, 2 for enemy
	public IEnumerator GiveBonusActionPoints (int bonusActionPointsGiven, int idWhoReceive)
	{
		//check if Dimension Gate unit still exists on the battle field or not
		List<int> dimensionGateUnitIDs = UnitManager.instance.SearchUnitIDByUnitTypeID(102);
		
		GameObject actionPointInUnit = null;

		//if give to player
		if (idWhoReceive == 1)
		{
			
		}
		else
		//if give to enemy
		if (idWhoReceive == 2)
		{
			//if Dimension Gate still exist
			if (dimensionGateUnitIDs.Count > 0)
			{
				//get the gameobjec of action points
				actionPointInUnit = UnitManager.instance.enemyUnitsTemplate[0].transform.GetChild(7).gameObject;

				for (int h = 0; h < dimensionGateUnitIDs.Count; h++)
				{
					Unit dimensionGateUnit = UnitManager.instance.units[dimensionGateUnitIDs[h]];
		
					Vector3 newCameraPosition = new Vector3(dimensionGateUnit.associatedGameObject.transform.position.x, 0.45f, dimensionGateUnit.associatedGameObject.transform.position.z + 2.5027f);
					Vector3 newCameraRotation = new Vector3(45f, 180f, 0f);

					//focus to a specific dimension gate
					yield return StartCoroutine(FocusCameraToAPosition(newCameraPosition, newCameraRotation));

					//create action points container
					GameObject actionPointsContainer = new GameObject();
					actionPointsContainer.name = "ActionPointsContainer";

					for (int i = 0; i < bonusActionPointsGiven; i++)
					{
						GameObject instantiatedActionPoint = Instantiate(actionPointInUnit);
						instantiatedActionPoint.transform.position = new Vector3(dimensionGateUnit.associatedGameObject.transform.position.x, actionPointInUnit.transform.position.y, dimensionGateUnit.associatedGameObject.transform.position.z);
						instantiatedActionPoint.transform.parent = actionPointsContainer.transform;
						instantiatedActionPoint.GetComponent<MeshRenderer>().material.color = Color.red;
		
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
	
					//add action points
					GameManager.instance.actionPoints[idWhoReceive-1] += bonusActionPointsGiven;

					//refresh action points on UI
					UIManager.instance.RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI();

					//destroy action points container
					Destroy(actionPointsContainer);
				}
			}
			else
			{

			}
		}			
	}

	// to know if a selector is movable or not
	public bool IsSelectorMovable (GameObject selector)
	{
		if (selector.activeSelf)
		{
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "R")
			{
				return false;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "M")
			{
				return true;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "A")
			{
				return true;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "T")
			{
				return false;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "E")
			{
				return true;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "")
			{
				return true;
			}
		}

		return false;
	}

	// get selector status. -1 is hidden, 0 is normal mode, 1 == M is move status, 2 == R is rotate status, 3 == A is attack status, 4 == T is turn status
	public int GetSelectorStatus (GameObject selector)
	{
		if (selector.activeSelf)
		{
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "R")
			{
				return 2;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "M")
			{
				return 1;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "A")
			{
				return 3;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "T")
			{
				return 4;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "E")
			{
				return 5;
			}
			else
			if (selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text == "")
			{
				return 0;
			}
		}

		return -1;
	}

	// set selector to a specific status. 0 is normal status, 1 == M is move status, 2 == R is rotate status, 3 == A is attack status, 4 == T is turn status, 5 == E is resurrection status
	public void SetSelectorStatus (GameObject selector, int statusID)
	{
		selector.transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		if (statusID == 0)
		{
			selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "";
			selector.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "";
			selector.transform.GetChild(2).gameObject.GetComponent<TextMesh>().text = "";
			selector.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "";
		}
		else
		if (statusID == 1)
		{
			selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "M";
			selector.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "" + distanceCursorToSelectedUnit;
			selector.transform.GetChild(2).gameObject.GetComponent<TextMesh>().text = "";
			selector.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "";
		}
		else
		if (statusID == 2)
		{
			selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "R";
			selector.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "0";
			selector.transform.GetChild(2).gameObject.GetComponent<TextMesh>().text = "LB";
			selector.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "RB";
		}
		else
		if (statusID == 3)
		{
			//Debug.Log("Set to attack!");

			selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "A";
			selector.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "0";
			selector.transform.GetChild(2).gameObject.GetComponent<TextMesh>().text = "";
			selector.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "";
		}
		else
		if (statusID == 4)
		{
			selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "T";
			selector.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "0";
			selector.transform.GetChild(2).gameObject.GetComponent<TextMesh>().text = "LB";
			selector.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "RB";
		}
		else
		if (statusID == 5)
		{
			selector.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "E";
			selector.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "";
			selector.transform.GetChild(2).gameObject.GetComponent<TextMesh>().text = "";
			selector.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = "";
		}
	}

	// show a specific selector on a certain position / dot
	public void ShowSelectorOnAPosition (GameObject selector, Dot dotToShow)
	{
		selector.transform.position = new Vector3(dotToShow.associatedGameObject.transform.position.x, selector.transform.position.y, dotToShow.associatedGameObject.transform.position.z);
		selector.transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		selector.SetActive(true);
	}

	// hide a specific selector
	public void HideSelector (GameObject selector)
	{
		selector.SetActive(false);
	}

	// move selector to a certain Dot relative to the current Dot. 
	//0 is top left dot, 1 is top right dot, 2 is right dot, 3 is bottom right dot, 4 is bottom left dot, 5 is left dot
	public void MoveSelector (int direction)
	{
		GameObject cursorToMove = cursorsContainer.transform.GetChild(0).gameObject;

		//Debug.Log("Move selector! : " + GetSelectorStatus(cursorToMove));

		if (!IsSelectorMovable(cursorToMove))
		{
			return;
		}

		if (uiType == 1)
		{
			//deactivate input
			InputManager.instance.isInputActive = false;
	
			Dot dotBelowCursor = TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToMove.transform.position)];
	
			//if there's a Dot there
			if (dotBelowCursor.neighboringDots[direction] > -1)
			{
				//Debug.Log("There's a Dot. Move to Top Right!");
	
				//assign a new cursor position
				HOTween.To
				(
					cursorToMove.transform, 
					0.25f, 
					new TweenParms()
				        .Prop
						(
							"position", 
							new Vector3
							(
								TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x,
								cursorToMove.transform.position.y,
								TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z
							),
							false	
						) 
				        .Ease(EaseType.EaseOutBack)
						.Delay(0f)
						.OnComplete(OnCompleteMovingSelector)
				);
			}
			else
			{
				//reactivate input
				InputManager.instance.isInputActive = true;
			}
		}
		else
		if (uiType == 2)
		{
			//deactivate input
			InputManager.instance.isInputActive = false;

			//if in normal mode or movement mode
			if (GetSelectorStatus(cursorToMove) == 0 || GetSelectorStatus(cursorToMove) == 1)
			{
				Dot dotBelowCursor = TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToMove.transform.position)];
		
				//if there's a Dot there
				if (dotBelowCursor.neighboringDots[direction] > -1)
				{
					//if in movement mode
					if (GetSelectorStatus(cursorToMove) == 1)
					{
						int unitIDOnTargetDot = UnitManager.instance.SearchUnitIDByPosition(TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position);

						int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();

						//if there's a Unit on the target dot that is not the selected Unit
						if (unitIDOnTargetDot > -1 && unitIDOnTargetDot != selectedUnitID)
						{
							//reactivate input
							InputManager.instance.isInputActive = true;
	
							return;
						}

						Dot targetDot = TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]];
								
						//if selecting normal action point
						if (selectedTypeOfPointsForPlayer == 0)
						{
							if (movementTracker.Count > 0)
							{
								//if it's not the prev dots
								if (TileDotManager.instance.dots.IndexOf(targetDot) != TileDotManager.instance.dots.IndexOf(movementTracker[movementTracker.Count-2]))
								{
									//Debug.Log("Used action points: " + ((actionPoints[turnID-1])-(movementTracker.Count-1)));

									//check if there's enough action points
									if ((actionPoints[turnID-1])-(movementTracker.Count-1) == 0)
									{
										Debug.Log("Press");

										//reactivate input
										InputManager.instance.isInputActive = true;
										return;
									}
								}
							}
						}
						//otherwise, special action point
						else
						{
							if (movementTracker.Count > 0)
							{
								//if it's not the prev dots
								if (TileDotManager.instance.dots.IndexOf(targetDot) != TileDotManager.instance.dots.IndexOf(movementTracker[movementTracker.Count-2]))
								{
									//Debug.Log("Used action points: " + ((actionPoints[turnID-1])-(movementTracker.Count-1)));

									//check if there's enough action points
									if ((specialPoints[selectedTypeOfPointsForPlayer-1])-(movementTracker.Count-1) == 0)
									{	
										//reactivate input
										InputManager.instance.isInputActive = true;
										return;
									}
								}
							}
						}
					}

					//if it's going to the z positive or x negative direction
					if (direction == 0)
					{
						if (turnID == 1)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= 3.5f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to Z positive and X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= 3.5f)
							{
								Debug.Log("Follow camera to Z positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
						else
						if (turnID == 2)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= -2.1f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to Z positive and X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= -2.1f)
							{
								Debug.Log("Follow camera to Z positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
					}
					else
					//if it's going to the z positive or x positive direction
					if (direction == 1)
					{
						if (turnID == 1)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= 3.5f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to Z positive and X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= 3.5f)
							{
								Debug.Log("Follow camera to Z positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
						else
						if (turnID == 2)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= -2.1f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to Z positive and X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z >= -2.1f)
							{
								Debug.Log("Follow camera to Z positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
					}
					else
					//if it's going to the z negative  or x positive direction
					if (direction == 3)
					{
						if (turnID == 1)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= 2.1f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to Z negative and X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= 2.1f)
							{
								Debug.Log("Follow camera to Z negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
						else
						if (turnID == 2)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= -3.5f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to Z negative and X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= -3.5f)
							{
								Debug.Log("Follow camera to Z negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
							{
								Debug.Log("Follow camera to X positive");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
					}
					else
					//if it's going to the z negative  or x negative direction
					if (direction == 4)
					{
						if (turnID == 1)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= 2.1f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to Z negative and X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= 2.1f)
							{
								Debug.Log("Follow camera to Z negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
						else
						if (turnID == 2)
						{
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= -3.5f && cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to Z negative and X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.z - Camera.main.transform.position.z <= -3.5f)
							{
								Debug.Log("Follow camera to Z negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												0f,
												0f,
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z-dotBelowCursor.associatedGameObject.transform.position.z
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
							else
							if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
							{
								Debug.Log("Follow camera to X negative");
					
								//move camera together with the cursor
								HOTween.To
								(
									Camera.main.transform, 
									0.25f, 
									new TweenParms()
								        .Prop
										(
											"position", 
											new Vector3
											(
												TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
												0f,
												0f
											),
											true	
										) 
								        .Ease(EaseType.Linear)
										.Delay(0f)
								);
							}
						}
					}
					else
					//if it's going to the x positive direction
					if (direction == 2)
					{
						if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x >= 1.75f)
						{
							Debug.Log("Follow camera to X positive");
				
							//move camera together with the cursor
							HOTween.To
							(
								Camera.main.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
											0f,
											0f
										),
										true	
									) 
							        .Ease(EaseType.Linear)
									.Delay(0f)
							);
						}
					}
					else
					//if it's going to the x negative direction
					if (direction == 5)
					{
						if (cursorsContainer.transform.GetChild(0).gameObject.transform.position.x - Camera.main.transform.position.x <= -1.75f)
						{
							Debug.Log("Follow camera to X negative");
				
							//move camera together with the cursor
							HOTween.To
							(
								Camera.main.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x-dotBelowCursor.associatedGameObject.transform.position.x,
											0f,
											0f
										),
										true	
									) 
							        .Ease(EaseType.Linear)
									.Delay(0f)
							);
						}
					}

					//Debug.Log("There's a Dot. Move to Top Right!");
		
					//assign a new cursor position
					HOTween.To
					(
						cursorToMove.transform, 
						0.25f, 
						new TweenParms()
					        .Prop
							(
								"position", 
								new Vector3
								(
									TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.x,
									cursorToMove.transform.position.y,
									TileDotManager.instance.dots[dotBelowCursor.neighboringDots[direction]].associatedGameObject.transform.position.z
								),
								false	
							) 
					        .Ease(EaseType.EaseOutBack)
							.Delay(0f)
							.OnComplete(OnCompleteMovingSelector)
					);
				}
				else
				{
					//reactivate input
					InputManager.instance.isInputActive = true;
				}
			}
			else
			//if in attack mode
			if (GetSelectorStatus(cursorToMove) == 3)
			{
				//Debug.Log("Move selector in attack mode!");

				//if go to the top left, left, or bottom left
				if (direction == 0 || direction == 5 || direction == 4)
				{
					for (int i = 0; i < attackTracker.Count; i++)
					{
						int idxInAttackTracker = GameManager.instance.attackTracker.IndexOf(UnitManager.instance.units[UnitManager.instance.SearchUnitIDByPosition(cursorToMove.transform.position)]);

						//check the current cursor is in what index of possible attacked units
						if (idxInAttackTracker == 0)
						{
							//go to the unit on the last index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.attackTracker[GameManager.instance.attackTracker.Count-1].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.attackTracker[GameManager.instance.attackTracker.Count-1].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);
						}
						else
						{
							//go to the unit on the prev index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.attackTracker[idxInAttackTracker-1].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.attackTracker[idxInAttackTracker-1].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);
						}
					}
				}
				else
				//if go to the top right, right, or bottom right
				if (direction == 1 || direction == 2 || direction == 3)
				{
					for (int i = 0; i < attackTracker.Count; i++)
					{
						int idxInAttackTracker = GameManager.instance.attackTracker.IndexOf(UnitManager.instance.units[UnitManager.instance.SearchUnitIDByPosition(cursorToMove.transform.position)]);

						//check the current cursor is in what index of possible attacked units
						if (idxInAttackTracker == GameManager.instance.attackTracker.Count-1)
						{
							//go to the unit on the first index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.attackTracker[0].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.attackTracker[0].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);
						}
						else
						{
							//go to the unit on the prev index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.attackTracker[idxInAttackTracker+1].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.attackTracker[idxInAttackTracker+1].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);
						}
					}
				}
			}
			else
			//if in resurrection mode
			if (GetSelectorStatus(cursorToMove) == 5)
			{
				//Debug.Log("Move selector in resurrection mode!");

				//if go to the top left, left, or bottom left
				if (direction == 0 || direction == 5 || direction == 4)
				{
					//for (int i = 0; i < resurrectTracker.Count; i++)
					//{
						int idxInResurrectTracker = GameManager.instance.resurrectTracker.IndexOf(TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToMove.transform.position)]);

						if (idxInResurrectTracker == 0)
						{
							//go to the dot on the last index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.resurrectTracker[GameManager.instance.resurrectTracker.Count-1].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.resurrectTracker[GameManager.instance.resurrectTracker.Count-1].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);

							//tween the camera, determine to which unit the camera should focus on
							List<int> resurrectionUnits = UnitManager.instance.SearchUnitIDByResurrectionEffect();
							Unit targetUnit = new Unit();				

							for (int h = 0; h < resurrectionUnits.Count; h++)
							{
								for (int j = 0; j < TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots.Count; j++)
								{
									if (TileDotManager.instance.dots[TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots[j]] == GameManager.instance.resurrectTracker[GameManager.instance.resurrectTracker.Count-1])
									{
										targetUnit = UnitManager.instance.units[resurrectionUnits[h]];

										break;
									}
								}
							}

							Vector3 newCameraPosition = new Vector3(targetUnit.associatedGameObject.transform.position.x, 0.45f, targetUnit.associatedGameObject.transform.position.z - 2.5027f);
							Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);
				
							//rotate and move the camera to the target unit
							StartCoroutine(FocusCameraToAPosition(newCameraPosition, newCameraRotation, 0.25f));
						}
						else
						{
							//go to the dot on the prev index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.resurrectTracker[idxInResurrectTracker-1].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.resurrectTracker[idxInResurrectTracker-1].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);

							//tween the camera, determine to which unit the camera should focus on
							List<int> resurrectionUnits = UnitManager.instance.SearchUnitIDByResurrectionEffect();
							Unit targetUnit = new Unit();				

							for (int h = 0; h < resurrectionUnits.Count; h++)
							{
								for (int j = 0; j < TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots.Count; j++)
								{
									if (TileDotManager.instance.dots[TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots[j]] == GameManager.instance.resurrectTracker[idxInResurrectTracker-1])
									{
										targetUnit = UnitManager.instance.units[resurrectionUnits[h]];

										break;
									}
								}
							}

							Vector3 newCameraPosition = new Vector3(targetUnit.associatedGameObject.transform.position.x, 0.45f, targetUnit.associatedGameObject.transform.position.z - 2.5027f);
							Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);
				
							//rotate and move the camera to the target unit
							StartCoroutine(FocusCameraToAPosition(newCameraPosition, newCameraRotation, 0.25f));
						}
					//}
				}
				else
				//if go to the top right, right, or bottom right
				if (direction == 1 || direction == 2 || direction == 3)
				{
					//for (int i = 0; i < resurrectTracker.Count; i++)
					//{
						int idxInResurrectTracker = GameManager.instance.resurrectTracker.IndexOf(TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToMove.transform.position)]);

						if (idxInResurrectTracker == GameManager.instance.resurrectTracker.Count-1)
						{
							//go to the dot on the first index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.resurrectTracker[0].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.resurrectTracker[0].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);

							//tween the camera, determine to which unit the camera should focus on
							List<int> resurrectionUnits = UnitManager.instance.SearchUnitIDByResurrectionEffect();
							Unit targetUnit = new Unit();				

							for (int h = 0; h < resurrectionUnits.Count; h++)
							{
								for (int j = 0; j < TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots.Count; j++)
								{
									if (TileDotManager.instance.dots[TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots[j]] == GameManager.instance.resurrectTracker[0])
									{
										targetUnit = UnitManager.instance.units[resurrectionUnits[h]];

										break;
									}
								}
							}

							Vector3 newCameraPosition = new Vector3(targetUnit.associatedGameObject.transform.position.x, 0.45f, targetUnit.associatedGameObject.transform.position.z - 2.5027f);
							Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);
				
							//rotate and move the camera to the target unit
							StartCoroutine(FocusCameraToAPosition(newCameraPosition, newCameraRotation, 0.25f));
						}
						else
						{
							Debug.Log("Rewew");

							//go to the dot on the prev index
							HOTween.To
							(
								cursorToMove.transform, 
								0.25f, 
								new TweenParms()
							        .Prop
									(
										"position", 
										new Vector3
										(
											GameManager.instance.resurrectTracker[idxInResurrectTracker+1].associatedGameObject.transform.position.x,
											cursorToMove.transform.position.y,
											GameManager.instance.resurrectTracker[idxInResurrectTracker+1].associatedGameObject.transform.position.z
										),
										false	
									) 
							        .Ease(EaseType.EaseOutBack)
									.Delay(0f)
									.OnComplete(OnCompleteMovingSelector)
							);

							//tween the camera, determine to which unit the camera should focus on
							List<int> resurrectionUnits = UnitManager.instance.SearchUnitIDByResurrectionEffect();
							Unit targetUnit = new Unit();				

							for (int h = 0; h < resurrectionUnits.Count; h++)
							{
								for (int j = 0; j < TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots.Count; j++)
								{
									if (TileDotManager.instance.dots[TileDotManager.instance.dots[UnitManager.instance.units[resurrectionUnits[h]].associatedDots[0]].neighboringDots[j]] == GameManager.instance.resurrectTracker[idxInResurrectTracker+1])
									{
										targetUnit = UnitManager.instance.units[resurrectionUnits[h]];

										break;
									}
								}
							}

							Vector3 newCameraPosition = new Vector3(targetUnit.associatedGameObject.transform.position.x, 0.45f, targetUnit.associatedGameObject.transform.position.z - 2.5027f);
							Vector3 newCameraRotation = new Vector3(45f, 0f, 0f);
				
							//rotate and move the camera to the target unit
							StartCoroutine(FocusCameraToAPosition(newCameraPosition, newCameraRotation, 0.25f));
						}
					//}
				}
			}
		}
	}

	public void OnCompleteMovingSelector ()
	{
		InputManager.instance.isInputActive = true;

		if (GameManager.instance.uiType == 2)
		{
			GameObject cursorToMove = cursorsContainer.transform.GetChild(0).gameObject;

			//if in movement mode
			if (GetSelectorStatus(cursorToMove) == 1)
			{
				int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();

				if (selectedUnitID > -1)
				{
					Unit selectedUnit;

					selectedUnit = UnitManager.instance.units[selectedUnitID];

					int currentDotID = TileDotManager.instance.SearchDotIDByPosition(cursorToMove.transform.position);

					Dot currentDot = TileDotManager.instance.dots[currentDotID];
	
					//if this is the first time creating movement lines
					if (movementTracker.Count == 0)
					{
						distanceCursorToSelectedUnit += 1;
		
						//show move text on this cursor
						UIManager.instance.ShowMoveActionPointTextOnCursor(cursorToMove, "" + distanceCursorToSelectedUnit);
	
						movementTracker.Add(TileDotManager.instance.dots[selectedUnit.associatedDots[0]]);
						movementTracker.Add(currentDot);
	
						//create movement line
						UIManager.instance.CreateMovementLine(selectedUnit, TileDotManager.instance.dots[selectedUnit.associatedDots[0]], currentDot);
					}
					else
					{
						//Debug.Log("Current Dot ID: " + TileDotManager.instance.dots.IndexOf(currentDot));
						//Debug.Log("Last Dot ID in Movement Tracker: " + TileDotManager.instance.dots.IndexOf(movementTracker[movementTracker.Count-1]));

						if (TileDotManager.instance.dots.IndexOf(currentDot) != TileDotManager.instance.dots.IndexOf(movementTracker[movementTracker.Count-2]))
						{
							//Debug.Log("Create Movement Line!");

							distanceCursorToSelectedUnit += 1;
	
							//show move text on this cursor
							UIManager.instance.ShowMoveActionPointTextOnCursor(cursorToMove, "" + distanceCursorToSelectedUnit);
	
							movementTracker.Add(currentDot);
	
							//create movement line
							UIManager.instance.CreateMovementLine(selectedUnit, movementTracker[movementTracker.Count-2], currentDot);
						}
						else
						{
							//Debug.Log("Remove line!");
	
							distanceCursorToSelectedUnit -= 1;
		
							//show move text on this cursor
							UIManager.instance.ShowMoveActionPointTextOnCursor(cursorToMove, "" + distanceCursorToSelectedUnit);
		
							movementTracker.RemoveAt(movementTracker.Count-1);

							if (movementTracker.Count <= 1)
							{
								movementTracker.Clear();
							}

							//remove line
							GameObject linesContainer = GameObject.Find("MovementLines");
							Destroy(linesContainer.transform.GetChild(linesContainer.transform.childCount-1).gameObject);
						}
					}
				}

				/*for (int i = 0; i < crossfiringPlayerUnits.Count; i++)
				{
					//create objects in the middle of crossfire and crossfiring units
					GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
	
					Vector3 location = crossfiringPlayerUnits[i].associatedGameObject.transform.position + crossfiredEnemyUnits[i].associatedGameObject.transform.position;
	
					line.name = "Line";
					line.transform.position = new Vector3(location.x/2f, -3f, location.z/2f);
					line.transform.localScale = new Vector3(0.1f, 0.1f, 0.5f);
	
					//add mesh renderer to it and change the color
					//line.AddComponent<MeshRenderer>();
					line.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color"));
					line.GetComponent<MeshRenderer>().material.color = Color.magenta;
	
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
				}*/
			}
			else
			//if in attack mode
			if (GetSelectorStatus(cursorToMove) == 3)
			{
				int selectedUnitID = UnitManager.instance.SearchUnitIDByPosition(cursorToMove.transform.position);

				if (selectedUnitID > -1)
				{
					Unit selectedUnit;
					selectedUnit = UnitManager.instance.units[selectedUnitID];

					//if player's turn
					if (turnID == 1)
					{
						if (selectedUnit.unitID >= 100 && selectedUnit.unitID < 200)
						{
							//StartCoroutine(UIManager.instance.HidePlayerStatusWindow());

							//show enemy status window
							StartCoroutine(UIManager.instance.ShowEnemyStatusWindow(selectedUnit));
						}
					}
					else
					//if enemy's turn
					if (turnID == 2)
					{
						//if player unit
						if (selectedUnit.unitID >= 0 && selectedUnit.unitID < 100)
						{
							//StartCoroutine(UIManager.instance.HideEnemyStatusWindow());

							//show player status window
							StartCoroutine(UIManager.instance.ShowPlayerStatusWindow(selectedUnit));
						}
					}
				}
			}
			//otherwise
			else
			//if in normal mode
			if (GetSelectorStatus(cursorToMove) == 0)
			{
				int selectedUnitID = UnitManager.instance.SearchUnitIDByPosition(cursorToMove.transform.position);

				if (selectedUnitID > -1)
				{
					Unit selectedUnit;
					selectedUnit = UnitManager.instance.units[selectedUnitID];

					//if player's turn
					if (turnID == 1)
					{
						//if player unit
						if (selectedUnit.unitID >= 0 && selectedUnit.unitID < 100)
						{
							StartCoroutine(UIManager.instance.HideEnemyStatusWindow());

							//show player status window
							StartCoroutine(UIManager.instance.ShowPlayerStatusWindow(selectedUnit));
						}
						else
						if (selectedUnit.unitID >= 100 && selectedUnit.unitID < 200)
						{
							StartCoroutine(UIManager.instance.HidePlayerStatusWindow());

							//show enemy status window
							StartCoroutine(UIManager.instance.ShowEnemyStatusWindow(selectedUnit));
						}
					}
					else
					//if enemy's turn
					if (turnID == 2)
					{
						//if player unit
						if (selectedUnit.unitID >= 0 && selectedUnit.unitID < 100)
						{
							StartCoroutine(UIManager.instance.HideEnemyStatusWindow());

							//show player status window
							StartCoroutine(UIManager.instance.ShowPlayerStatusWindow(selectedUnit));
						}
						else
						if (selectedUnit.unitID >= 100 && selectedUnit.unitID < 200)
						{
							StartCoroutine(UIManager.instance.HidePlayerStatusWindow());

							//show enemy status window
							StartCoroutine(UIManager.instance.ShowEnemyStatusWindow(selectedUnit));
						}
					}
				}
				else
				{
					//hide everything
					StartCoroutine(UIManager.instance.HidePlayerStatusWindow());
					StartCoroutine(UIManager.instance.HideEnemyStatusWindow());
				}
			}
		}
		else
		if (GameManager.instance.uiType == 1)
		{

		}
	}

	// to know whether it is currently in rotation mode or not
	public bool IsRotatingTiles ()
	{
		if (IsSelectorMovable(cursorsContainer.transform.GetChild(0).gameObject))
		{
			return false;
		}

		int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();

		if (selectedUnitID > -1)
		{
			//get tiles around a cursor
			List<int> tilesAround = TileDotManager.instance.SearchTileIDsAroundAPosition(UnitManager.instance.units[selectedUnitID].associatedGameObject.transform.position);

			for (int i = 0; i < tilesAround.Count; i++)
			{
				//if there is a Tile around a Dot
				if (tilesAround[i] > -1)
				{
					if (TileDotManager.instance.tiles[tilesAround[i]].associatedGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color.b >= 0.5f)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	// to know whether it is currently in turn mode or not
	public bool IsTurningBody ()
	{
		if (IsSelectorMovable(cursorsContainer.transform.GetChild(0).gameObject))
		{
			return false;
		}

		if (GetSelectorStatus(cursorsContainer.transform.GetChild(0).gameObject) == 4)
		{
			return true;
		}
	
		return false;
	}

	// to check whether the enemy can still be able to turn or not. 1 is clockwise, 2 is counter clockwise
	public bool IsAbleToTurn (int direction)
	{
		//clockwise
		if (direction == 1)
		{
			if (uiType == 2)
			{
				if (turnTracker == 3)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}
		//counter clockwise
		else
		if (direction == 2)
		{
			if (uiType == 2)
			{
				if (turnTracker == -3)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		return false;
	}

	// to check whether the player can still be able to rotate or not. 1 is clockwise, 2 is counter clockwise
	public bool IsAbleToRotate (int direction)
	{
		//clockwise
		if (direction == 1)
		{
			if (uiType == 1)
			{
				if (rotationTracker == 1)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			if (uiType == 2)
			{
				if (rotationTracker == 1)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}
		//counter clockwise
		else
		if (direction == 2)
		{
			if (uiType == 1)
			{
				if (rotationTracker == -1)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			if (uiType == 2)
			{
				if (rotationTracker == -1)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		return false;
	}

	public IEnumerator FocusCameraToAPosition (Vector3 posToFocus, Vector3 eulerToFocus)
	{
		//rotate and move the camera to the target unit
		HOTween.To
		(
			Camera.main.transform, 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"position", 
					posToFocus,
					false	
				) 
		        .Ease(EaseType.EaseOutQuad)
				.Delay(0f)
		);
		
		HOTween.To
		(
			Camera.main.transform, 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"eulerAngles", 
					eulerToFocus,
					false	
				) 
		        .Ease(EaseType.EaseOutQuad)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.5f);
	}

	public IEnumerator FocusCameraToAPosition (Vector3 posToFocus, Vector3 eulerToFocus, float duration)
	{
		//rotate and move the camera to the target unit
		HOTween.To
		(
			Camera.main.transform, 
			duration, 
			new TweenParms()
		        .Prop
				(
					"position", 
					posToFocus,
					false	
				) 
		        .Ease(EaseType.EaseOutQuad)
				.Delay(0f)
		);
		
		HOTween.To
		(
			Camera.main.transform, 
			duration, 
			new TweenParms()
		        .Prop
				(
					"eulerAngles", 
					eulerToFocus,
					false	
				) 
		        .Ease(EaseType.EaseOutQuad)
				.Delay(0f)
		);

		yield return new WaitForSeconds(duration);
	}
}
