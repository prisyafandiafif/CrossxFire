using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;

public class InputManager : MonoBehaviour 
{
	public static InputManager instance;

	// To know whether right now the game is accepting input or not
	public bool isInputActive;

	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isInputActive)
		{
			//Debug.Log("Left Joystick Player 1 X Axis: " + (Input.GetAxis("HorizontalPlayer")*100f) + " && Left Joystick Player 1 Y Axis: " + (Input.GetAxis("VerticalPlayer")*100f));

			/*Debug.Log("D-Pad Joystick Player 1 X Axis: " + Input.GetAxis("HorizontalDPadPlayer") + " && D-Pad Joystick Player 1 Y Axis: " + Input.GetAxis("VerticalDPadPlayer"));

			Debug.Log("Triggers Joystick Player 1: " + Input.GetAxis("Triggers"));

			Debug.Log("Right Bumper Joystick Player 1: " + Input.GetKey(KeyCode.Joystick1Button5));

			Debug.Log("Left Bumper Joystick Player 1: " + Input.GetKey(KeyCode.Joystick1Button4));*/

			CheckControllersInput();
		}
	}

	// confirm rotation
	public IEnumerator ConfirmRotation (Unit selectedUnit)
	{
		//hide highlight AFTER confirm
		StartCoroutine(TileDotManager.instance.HideHighlightOnTilesAroundAUnit(selectedUnit));

		//if crossfire
		if (TileDotManager.instance.crossfiringPlayerUnits.Count > 0 && GameManager.instance.rotationTracker != 0)
		{
			StartCoroutine(UnitManager.instance.ExecuteCrossfire(selectedUnit));
		}
		else
		{
			if (GameManager.instance.rotationTracker != 0)
			{
				StartCoroutine(UnitManager.instance.SpendRotationPoints(selectedUnit, Mathf.Abs(GameManager.instance.rotationTracker)));

				#region OVERLAP LOGIC
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
				}

				if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)	
				{
					//give special action points
					yield return StartCoroutine(UnitManager.instance.GiveSpecialActionPoints(Mathf.Abs(GameManager.instance.rotationTracker), GameManager.instance.selectedTypeOfPointsForPlayer-1, selectedUnit));
				}
				else
				{
					yield return StartCoroutine(UnitManager.instance.GiveNormalActionPoints(Mathf.Abs(GameManager.instance.rotationTracker), selectedUnit));
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
						yield return StartCoroutine(UnitManager.instance.KillAUnit(overlappedPlayerUnit[i]));
					}
				}
				#endregion
			}

			//deselect unit
			UnitManager.instance.DeselectUnit(selectedUnit);

			//reset to zero
			GameManager.instance.rotationTracker = 0;
	
			//if selecting special points and it's player's turns
			if (GameManager.instance.selectedTypeOfPointsForPlayer > 0 && GameManager.instance.turnID == 1)
			{
				//if there's something to resurrect
				if (GameManager.instance.resurrectedGameObject.Count > 0)
				{
					yield return StartCoroutine(UIManager.instance.ShowResurrectionBumperAnimation()); 

					List<int> unitIDForResurrection = UnitManager.instance.SearchUnitIDByResurrectionEffect();

					Unit unitToFocus = new Unit();

					for (int i = 0; i < unitIDForResurrection.Count; i++)
					{
						//find to which unit the camera needs to focus
						if (TileDotManager.instance.SearchDotsToResurrectAroundAUnit(UnitManager.instance.units[unitIDForResurrection[i]]).Count > 0)
						{
							unitToFocus = UnitManager.instance.units[unitIDForResurrection[i]];

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
				else
				{
					//call reset for rotation function
					TileDotManager.instance.ResetForRotation();

					if (GameManager.instance.uiType == 2)
					{
						//set selector to normal status again
						GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
					}
		
					//show selector
					GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[selectedUnit.associatedDots[0]]);
				}
			}
			else
			{
				//call reset for rotation function
				TileDotManager.instance.ResetForRotation();

				if (GameManager.instance.uiType == 2)
				{
					//set selector to normal status again
					GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
				}
	
				//show selector
				GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[selectedUnit.associatedDots[0]]);
			}

			//put the selected Unit to idle
			//StartCoroutine(UnitManager.instance.PutAUnitToIdle(selectedUnit));
		}
	}

	// confirm turn
	public IEnumerator ConfirmTurn (Unit selectedUnit)
	{
		//yield return new WaitForSeconds(0f);		

		if (GameManager.instance.uiType == 2)
		{
			//set selector to normal status again
			GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
		}

		GameManager.instance.HideSelector(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject);

		if (GameManager.instance.turnTracker != 0)
		{
			//StartCoroutine(UnitManager.instance.SpendRotationPoints(selectedUnit, Mathf.Abs(GameManager.instance.rotationTracker)));

			yield return StartCoroutine(UnitManager.instance.GiveNormalActionPoints(Mathf.Abs(1), selectedUnit));
		}

		//check if there are enemies in attack range
		List<int> attackedToBeUnitOrBuilding = new List<int>();
		attackedToBeUnitOrBuilding = UnitManager.instance.SearchUnitIDsByAttackRangeOfAUnit(selectedUnit);
	
		if (attackedToBeUnitOrBuilding.Count > 0 && GameManager.instance.turnTracker != 0)
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
			UIManager.instance.ShowActionOptionsWindowOnAUnit(selectedUnit, possibleActions);

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
			//deselect unit
			UnitManager.instance.DeselectUnit(selectedUnit);

			//show selector
			GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[selectedUnit.associatedDots[0]]);
		}

		//reset to zero
		GameManager.instance.turnTracker = 0;
	}

	// check every joystick and button input
	public void CheckControllersInput ()
	{
		/*for (int i = 0; i < Input.GetJoystickNames().Length; i++)
		{
			Debug.Log(Input.GetJoystickNames()[i]);
		}*/

		//not checking the controller input when there is a canvas world activated
		if (UIManager.instance.IsThereACanvasWorldActive())
		{
			//Debug.Log("Canvas active!");

			return;
		}

		//if A button is selected
		if (Input.GetKeyDown(KeyCode.Joystick1Button0))
		{
			Debug.Log("A button is pressed");
			
			//if in rotation mode
			if (GameManager.instance.IsRotatingTiles())
			{
				int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();

				//if there's a Unit that is being selected
				if (selectedUnitID > -1)
				{
					Unit selectedUnit = UnitManager.instance.units[selectedUnitID];

					//confirm rotation
					StartCoroutine(ConfirmRotation(selectedUnit));
				}
				//otherwise
				else
				{

				}
			}
			else
			//if in turn mode
			if (GameManager.instance.IsTurningBody())
			{
				int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();

				//if there's a Unit that is being selected
				if (selectedUnitID > -1)
				{
					Unit selectedUnit = UnitManager.instance.units[selectedUnitID];

					//confirm turn
					StartCoroutine(ConfirmTurn(selectedUnit));
				}
				//otherwise
				else
				{

				}
			}
			else
			{
				//if (GameManager.instance.uiType == 1)
				//{
				//if in normal mode
				//get the cursor
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;
		
				//if selector is on a unit
				if (UnitManager.instance.IsUnitExistAtAPosition(cursorToCheck.transform.position))
				{
					int unitIDAtThisPos = UnitManager.instance.SearchUnitIDByPosition(cursorToCheck.transform.position);
			
					Unit unitAtThisPos = UnitManager.instance.units[unitIDAtThisPos];
		
					if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(unitAtThisPos))
					{
						//if Unit is showing attack text
						if (UnitManager.instance.IsUnitShowingAttackText(unitAtThisPos))
						{
							//Debug.Log("Attack this Unit!");
			
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								//attack a unit using the selected unit
								StartCoroutine(UnitManager.instance.AttackAUnitWithAUnit(selectedUnit, unitAtThisPos));
							}
							//otherwise
							else
							{
			
							}
						}
						//if Unit is showing rotate text
						else
						if (UnitManager.instance.IsUnitShowingRotateText(unitAtThisPos))
						{
							//Debug.Log("Rotate around this Unit!");
			
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								if (GameManager.instance.uiType == 2)
								{
									UIManager.instance.HideActionOptionsWindow();
								}
	
								//show highlight
								StartCoroutine(TileDotManager.instance.ShowHighlightOnTilesAroundAUnit(selectedUnit));
							}
							//otherwise
							else
							{
			
							}
						}
						//if Unit is showing stay text
						else
						if (UnitManager.instance.IsUnitShowingStayText(unitAtThisPos))
						{
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								//put the selected Unit to idle
								StartCoroutine(UnitManager.instance.PutAUnitToIdle(selectedUnit));
							}
							//otherwise
							else
							{
			
							}
						}
						else
						//if it's in Attack Mode
						if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 3)
						{
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								//attack a unit using the selected unit
								StartCoroutine(UnitManager.instance.AttackAUnitWithAUnit(selectedUnit, unitAtThisPos));
							}
							//otherwise
							else
							{
			
							}
						}
						else
						//if it's in Rotation Mode
						if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 1)
						{
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								//cancel movement mode
								//put the moved unit to idle
								UnitManager.instance.DeselectUnit(selectedUnit);

								//set selector status to normal
								GameManager.instance.SetSelectorStatus(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, 0);
	
								//put and show the cursor on the moved unit
								GameManager.instance.ShowSelectorOnAPosition(GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject, TileDotManager.instance.dots[selectedUnit.associatedDots[0]]);	

							}
							//otherwise
							else
							{
			
							}
						}
						else
						//if it's in Movement mode
						if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 3) //move buitton on action options window
						{

						}
						//if Unit is not showing Attack or Rotate or Stay Text or not in Attack Mode or not in Rotation Mode
						else 
						{
							//if there's already a Unit that has been selected
							if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 2)
							{
								
							}
							else
							{
								//if it's not selected yet
								if (!unitAtThisPos.isSelected)
								{
									//if this is player's turn or enemy's turn, select the correct unit
									if ((GameManager.instance.turnID == 1 && unitAtThisPos.unitID >= 0 && unitAtThisPos.unitID < 100) || (GameManager.instance.turnID == 2 && unitAtThisPos.unitID >= 100 && unitAtThisPos.unitID < 200))
									{
										//put the selected Unit to action
										StartCoroutine(UnitManager.instance.PutAUnitToAction(unitAtThisPos));
									}
								}
							}
						}
					}
					//if a unit is showing action options window
					else
					{
						if (UnitManager.instance.IsUnitHighlightingRotateButton(unitAtThisPos)) //rotate buitton on action options window
						{
							//Debug.Log("Rotate around this Unit!");
			
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								if (GameManager.instance.uiType == 2)
								{
									UIManager.instance.HideActionOptionsWindow();

									//set selector to rotate status
									GameManager.instance.SetSelectorStatus(cursorToCheck, 2);
								}
	
								//show highlight
								StartCoroutine(TileDotManager.instance.ShowHighlightOnTilesAroundAUnit(selectedUnit));
							}
							//otherwise
							else
							{
			
							}
						}
						else
						if (UnitManager.instance.IsUnitHighlightingTurnButton(unitAtThisPos)) //turn buitton on action options window
						{
							//Debug.Log("Turn this Unit!");
			
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								if (GameManager.instance.uiType == 2)
								{
									UIManager.instance.HideActionOptionsWindow();

									//set selector to turn status
									GameManager.instance.SetSelectorStatus(cursorToCheck, 4);
								}
	
								//show highlight
								//StartCoroutine(TileDotManager.instance.ShowHighlightOnTilesAroundAUnit(selectedUnit));
							}
							//otherwise
							else
							{
			
							}
						}
						else
						if (UnitManager.instance.IsUnitHighlightingMoveButton(unitAtThisPos)) //move buitton on action options window
						{
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								if (GameManager.instance.uiType == 2)
								{
									UIManager.instance.HideActionOptionsWindow();

									//if it's enemy units
									if (selectedUnit.unitID >= 100 && selectedUnit.unitID < 200)
									{
										//hide selector
										GameManager.instance.HideSelector(cursorToCheck);

										Dot dotBelow = TileDotManager.instance.dots[selectedUnit.associatedDots[0]];

										GameManager.instance.movementTracker.Add(dotBelow);
										GameManager.instance.movementTracker.Add(TileDotManager.instance.dots[dotBelow.neighboringDots[selectedUnit.direction]]);

										UIManager.instance.CreateMovementLine(selectedUnit, TileDotManager.instance.dots[selectedUnit.associatedDots[0]], TileDotManager.instance.dots[dotBelow.neighboringDots[selectedUnit.direction]]);

										StartCoroutine(UnitManager.instance.MoveAUnitToDotBasedOnCreatedPath(selectedUnit));
									}
									else
									{									
										//set selector to move status
										GameManager.instance.SetSelectorStatus(cursorToCheck, 1);
									}
								}
							}
							//otherwise
							else
							{
			
							}
						}
						else
						if (UnitManager.instance.IsUnitHighlightingAttackButton(unitAtThisPos)) //attack buitton on action options window
						{
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								if (GameManager.instance.uiType == 2)
								{
									UIManager.instance.HideActionOptionsWindow();

									//if this is a Shooter
									if (selectedUnit.unitID == 101)
									{
										//hide action options window
										UIManager.instance.HideActionOptionsWindow();

										//find a list of unit or building that can be attacked	
										List<int> attackedToBeUnitOrBuilding = new List<int>();
										attackedToBeUnitOrBuilding = UnitManager.instance.SearchUnitIDsByAttackRangeOfAUnit(selectedUnit);
										
										for (int i = 0; i < attackedToBeUnitOrBuilding.Count; i++)
										{
											//attack a unit using the selected unit
											StartCoroutine(UnitManager.instance.AttackAUnitWithAUnit(selectedUnit, UnitManager.instance.units[attackedToBeUnitOrBuilding[i]]));
										}
									}							
									else
									{
										//set selector to attack status
										GameManager.instance.SetSelectorStatus(cursorToCheck, 3);
	
										//find a list of unit or building that can be attacked	
										List<int> attackedToBeUnitOrBuilding = new List<int>();
										attackedToBeUnitOrBuilding = UnitManager.instance.SearchUnitIDsByAttackRangeOfAUnit(selectedUnit);
										for (int i = 0; i < attackedToBeUnitOrBuilding.Count; i++)
										{
											GameManager.instance.attackTracker.Add(UnitManager.instance.units[attackedToBeUnitOrBuilding[i]]);
										}								
	
										//put attack selector on the first unit that can be attacked
										GameManager.instance.ShowSelectorOnAPosition(cursorToCheck, TileDotManager.instance.dots[UnitManager.instance.units[attackedToBeUnitOrBuilding[0]].associatedDots[0]]);
									
										//show attack text on this cursor
										UIManager.instance.ShowAttackActionPointTextOnCursor(cursorToCheck, "" + GameManager.instance.actionPointNeededToAttack);
									}
								}
							}
							//otherwise
							else
							{
			
							}
						}
						else
						if (UnitManager.instance.IsUnitHighlightingExitButton(unitAtThisPos)) //ext buitton on action options window
						{
							int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
							//if there's a Unit that is being selected
							if (selectedUnitID > -1)
							{
								Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
								if (GameManager.instance.uiType == 2)
								{
									UIManager.instance.HideActionOptionsWindow();

									//set selector to normal status
									GameManager.instance.SetSelectorStatus(cursorToCheck, 0);
						
									StartCoroutine(UnitManager.instance.PutAUnitToIdle(selectedUnit));
								}
							}
							//otherwise
							else
							{
			
							}
						}
					}
				}
				//otherwise if there's not a Unit on that selector
				else
				{
					//if selector is on a Dot
					if (TileDotManager.instance.IsDotExistAtAPosition(cursorToCheck.transform.position))
					{
						Dot dotAtThisPos = TileDotManager.instance.SearchDotsByPosition(cursorToCheck.transform.position);
		
						if (GameManager.instance.uiType == 1)
						{
							//if the Dot is showing Move Here text 
							if (TileDotManager.instance.IsDotShowingMoveText(dotAtThisPos))
							{
								int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
								//if there's a Unit that is being selected
								if (selectedUnitID > -1)
								{
									Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
				
									//move the selected Unit to this Dot
									StartCoroutine(UnitManager.instance.MoveAUnitToANeighborDot(selectedUnit, dotAtThisPos));
								}
								//otherwise
								else
								{
			
								}
							}
							//otherwise
							else
							{
			
							}
						}
						else
						if (GameManager.instance.uiType == 2)
						{
							//if in movement mode
							if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 1)
							{
								int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
								//if there's a Unit that is being selected
								if (selectedUnitID > -1)
								{
									Unit selectedUnit = UnitManager.instance.units[selectedUnitID];

									//Debug.Log("Move a Unit to a Dot!");

									if (int.Parse(cursorToCheck.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text) > 0)
									{
										StartCoroutine(UnitManager.instance.MoveAUnitToDotBasedOnCreatedPath(selectedUnit));
									}
								}
								else
								{

								}
							}
							else
							//if in resurrection mode
							if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 5)
							{
								//Debug.Log("Create unit on this dot!");

								StartCoroutine(UnitManager.instance.ResurrectAUnitToADot(dotAtThisPos));
							}
							else
							//if in normal mode
							if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 0)
							{
								//if action options are not showed on dot
								if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotAtThisPos))
								{
									List<string> possibleActions = new List<string>();
	
									possibleActions.Add("END");
									possibleActions.Add("EXIT");
									
									//show ui for action options
									UIManager.instance.ShowActionOptionsWindowOnADot(dotAtThisPos, possibleActions);
								}
								else //if showed
								{
									if (TileDotManager.instance.IsDotHighlightingExitButton(dotAtThisPos)) //exit buitton on action options window
									{
										int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
									
										//if there's a Unit that is being selected
										if (selectedUnitID > -1)
										{
											
										}
										//otherwise
										else
										{
											if (GameManager.instance.uiType == 2)
											{
												UIManager.instance.HideActionOptionsWindow();
				
												//set selector to normal status
												GameManager.instance.SetSelectorStatus(cursorToCheck, 0);
												
												StartCoroutine(TileDotManager.instance.PutADotToIdle(dotAtThisPos));
											}
										}
									}
									else
									if (TileDotManager.instance.IsDotHighlightingEndButton(dotAtThisPos)) //end button on action options window
									{
										int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
									
										//if there's a Unit that is being selected
										if (selectedUnitID > -1)
										{
											
										}
										//otherwise
										else
										{
											if (GameManager.instance.uiType == 2)
											{
												UIManager.instance.HideActionOptionsWindow();
				
												//set selector to normal status
												GameManager.instance.SetSelectorStatus(cursorToCheck, 0);
												
												//switch to enemy's / player's turn
												if (GameManager.instance.turnID == 1)
												{
													StartCoroutine(GameManager.instance.SwitchTurn(2));
												}	
												else
												if (GameManager.instance.turnID == 2)
												{
													StartCoroutine(GameManager.instance.SwitchTurn(1));
												}	
											}
										}
									}								
								}	
							}
						}
					}
					//otherwise
					else
					{
		
					}
				}
				//}
			}
		}

#region HIGHLIGHT MOVEMENT IN ACTION OPTIONS WINDOW CONTROL
		//if up joystick is pressed 
		if (Input.GetAxis("VerticalPlayer") >= 0.1f)
		{
			Debug.Log("up!");

			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];

				//if action options are not showed on unit
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					
				}
				//otherwise
				else
				{
					StartCoroutine(UIManager.instance.MoveHighlightInActionOptionsWindow(1, selectedUnit.associatedGameObject.transform.GetChild(8).gameObject));
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						
					}
					//otherwise
					else
					{
						StartCoroutine(UIManager.instance.MoveHighlightInActionOptionsWindow(1, dotBelowCursor.associatedGameObject.transform.GetChild(2).gameObject));
					}
				}
			}
		}
		else
		//if down joystick is pressed 
		if (Input.GetAxis("VerticalPlayer") <= -0.1f)
		{
			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
				//if action options are not showed 
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					
				}
				//otherwise
				else
				{
					StartCoroutine(UIManager.instance.MoveHighlightInActionOptionsWindow(2, selectedUnit.associatedGameObject.transform.GetChild(8).gameObject));
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						
					}
					//otherwise
					else
					{
						StartCoroutine(UIManager.instance.MoveHighlightInActionOptionsWindow(2, dotBelowCursor.associatedGameObject.transform.GetChild(2).gameObject));
					}
				}
			}
		}
#endregion
		
#region SELECTOR MOVEMENT CONTROL
		//if move to top left
		//if ((Input.GetAxis("HorizontalPlayer") <= -0.1f && Input.GetAxis("VerticalPlayer") >= 0.1f) || (Input.GetAxis("Triggers") == 1f))
		if ((Input.GetAxis("HorizontalPlayer") <= -0.1f && Input.GetAxis("VerticalPlayer") >= 0.1f) || (Input.GetAxis("RightHorizontalPlayer") <= -0.1f && Input.GetAxis("VerticalPlayer") >= 0.1f))
		{
			//Debug.Log("Left Joystick Player 1 X Axis: " + Input.GetAxis("HorizontalPlayer") + " && Left Joystick Player 1 Y Axis: " + Input.GetAxis("VerticalPlayer"));
	
			int directionForSelector = 0;

			//if enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				directionForSelector = 3;
			}

			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
				//if action options are not showed 
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					GameManager.instance.MoveSelector(directionForSelector);
				}
				else
				{
					//do nothing
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						GameManager.instance.MoveSelector(directionForSelector);
					}
					//otherwise
					else
					{
						
					}
				}
			}
		}
		else
		//if move to top right
		//if ((Input.GetAxis("HorizontalPlayer") >= 0.1f && Input.GetAxis("VerticalPlayer") >= 0.1f) || (Input.GetAxis("Triggers") == -1f))
		if ((Input.GetAxis("HorizontalPlayer") >= 0.1f && Input.GetAxis("VerticalPlayer") >= 0.1f) || (Input.GetAxis("RightHorizontalPlayer") >= 0.1f && Input.GetAxis("VerticalPlayer") >= 0.1f))
		{
			int directionForSelector = 1;

			//if enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				directionForSelector = 4;
			}

			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
				//if action options are not showed 
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					GameManager.instance.MoveSelector(directionForSelector);
				}
				else
				{
					//do nothing
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						GameManager.instance.MoveSelector(directionForSelector);
					}
					//otherwise
					else
					{
						
					}
				}
			}
		}
		else
		//if move to right
		if (Input.GetAxis("HorizontalPlayer") >= 0.1f && Input.GetAxis("VerticalPlayer") <= 0.1f && Input.GetAxis("VerticalPlayer") >= -0.1f && Input.GetAxis("Triggers") == 0f)
		{
			int directionForSelector = 2;

			//if enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				directionForSelector = 5;
			}

			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
				//if action options are not showed 
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					GameManager.instance.MoveSelector(directionForSelector);
				}
				else
				{
					//do nothing
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						GameManager.instance.MoveSelector(directionForSelector);
					}
					//otherwise
					else
					{
						
					}
				}
			}
		}
		else
		//if move to bottom right
		//if (Input.GetAxis("HorizontalPlayer") >= 0.1f && Input.GetAxis("VerticalPlayer") <= -0.1f || (Input.GetKeyDown(KeyCode.Joystick1Button5))) 
		if ((Input.GetAxis("HorizontalPlayer") >= 0.1f && Input.GetAxis("VerticalPlayer") <= -0.1f) || (Input.GetAxis("RightHorizontalPlayer") >= 0.1f && Input.GetAxis("VerticalPlayer") <= -0.1f))
		{
			int directionForSelector = 3;

			//if enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				directionForSelector = 0;
			}

			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
				//if action options are not showed 
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					GameManager.instance.MoveSelector(directionForSelector);
				}
				else
				{
					//do nothing
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						GameManager.instance.MoveSelector(directionForSelector);
					}
					//otherwise
					else
					{
						
					}
				}
			}
		}
		else
		//if move to bottom left
		//if (Input.GetAxis("HorizontalPlayer") <= -0.1f && Input.GetAxis("VerticalPlayer") <= -0.1f || (Input.GetKeyDown(KeyCode.Joystick1Button4)))
		if ((Input.GetAxis("HorizontalPlayer") <= -0.1f && Input.GetAxis("VerticalPlayer") <= -0.1f) || (Input.GetAxis("RightHorizontalPlayer") <= -0.1f && Input.GetAxis("VerticalPlayer") <= -0.1f))
		{
			//Debug.Log("Left Joystick Player 1 X Axis: " + Input.GetAxis("HorizontalPlayer") + " && Left Joystick Player 1 Y Axis: " + Input.GetAxis("VerticalPlayer"));
	
			int directionForSelector = 4;

			//if enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				directionForSelector = 1;
			}

			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
				//if action options are not showed 
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					GameManager.instance.MoveSelector(directionForSelector);
				}
				else
				{
					//do nothing
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						GameManager.instance.MoveSelector(directionForSelector);
					}
					//otherwise
					else
					{
						
					}
				}
			}
		}
		else
		//if move to left
		if (Input.GetAxis("HorizontalPlayer") <= -0.1f && Input.GetAxis("VerticalPlayer") <= 0.1f && Input.GetAxis("VerticalPlayer") >= -0.1f && Input.GetAxis("Triggers") == 0f)
		{
			//Debug.Log("Left Joystick Player 1 X Axis: " + Input.GetAxis("HorizontalPlayer") + " && Left Joystick Player 1 Y Axis: " + Input.GetAxis("VerticalPlayer"));
	
			int directionForSelector = 5;

			//if enemy's turn
			if (GameManager.instance.turnID == 2)
			{
				directionForSelector = 2;
			}

			int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
							
			//if there's a Unit that is being selected
			if (selectedUnitID > -1)
			{
				Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
				//if action options are not showed 
				if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
				{
					//Debug.Log("Left");

					GameManager.instance.MoveSelector(directionForSelector);
				}
				else
				{
					//do nothing
				}
			}
			else
			{
				GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

				int dotIDBelowCursor = TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position);

				//if there's a Dot below the cursor
				if (dotIDBelowCursor > -1)		
				{
					Dot dotBelowCursor = TileDotManager.instance.dots[dotIDBelowCursor];

					//if action options are not showed on dot
					if (!TileDotManager.instance.IsDotShowingActionOptionsWindow(dotBelowCursor))
					{
						GameManager.instance.MoveSelector(directionForSelector);
					}
					//otherwise
					else
					{
						
					}
				}
			}
		}
#endregion

#region TURN CONTROL
		//when LB button is pressed 
		if (Input.GetKeyDown(KeyCode.Joystick1Button4))
		{
			//if not in turn mode mode
			if (!GameManager.instance.IsTurningBody())
			{
				//return;

				//Debug.Log("Turn Right Bumper");
			}	
			else
			{
				//if valid for turn. 1 for clockwise, 2 for counter clockwise
				if (!GameManager.instance.IsAbleToTurn(2))
				{
					//return;
				}
				else
				{
					int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
					//if there's a Unit that is being selected
					if (selectedUnitID > -1)
					{
						Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
			
						StartCoroutine(UnitManager.instance.TurnUnitAround(selectedUnit, 2));
					}
					//otherwise
					else
					{
		
					}
				}
			}
		}

		//when RB button is pressed 
		if (Input.GetKeyDown(KeyCode.Joystick1Button5))
		{
			//Debug.Log("Color of tile: " + TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position)].associatedGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color.r);

			//Dot dotBelow = TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position)];

			//if not in turn mode
			if (!GameManager.instance.IsTurningBody())
			{
				//return;
			}
			else
			{
				//if valid for turn. 1 for clockwise, 2 for counter clockwise
				if (!GameManager.instance.IsAbleToTurn(1))
				{
					//return;
				}
				else
				{
					int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
					//if there's a Unit that is being selected
					if (selectedUnitID > -1)
					{
						Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
			
						StartCoroutine(UnitManager.instance.TurnUnitAround(selectedUnit, 1));
					}
					//otherwise
					else
					{
		
					}
				}
			}
		}
#endregion

#region ROTATION AND SPECIAL ACTION POINT HIGHLIGHT CONTROL
		//when LB button is pressed 
		if (Input.GetKeyDown(KeyCode.Joystick1Button4))
		{
			//if not in rotate mode
			if (!GameManager.instance.IsRotatingTiles())
			{
				//if player's turn
				if (GameManager.instance.turnID == 1)
				{
					GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

					//if in normal mode
					if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 0)
					{
						int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
						//if there's a Unit that is being selected
						if (selectedUnitID > -1)
						{
							Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
							//if not showing action option window
							if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
							{
								if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
								{
									GameManager.instance.selectedTypeOfPointsForPlayer = GameManager.instance.specialPoints.Length;
								}
								else
								{
									GameManager.instance.selectedTypeOfPointsForPlayer -= 1;
								}
			
								UIManager.instance.RefreshSelectedActionPointsUI();
							}
							//if showing action option window
							else
							{
								List<int> attackedToBeUnitOrBuilding = UnitManager.instance.SearchUnitIDsByAttackRangeOfAUnit(selectedUnit);

								if (attackedToBeUnitOrBuilding.Count > 0)
								{
									if (GameManager.instance.actionPointNeededToAttack > 0)
									{
										UIManager.instance.HideActionOptionsWindow();
			
										//set selector to normal status
										GameManager.instance.SetSelectorStatus(cursorToCheck, 0);
									
										if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
										{
											GameManager.instance.selectedTypeOfPointsForPlayer = GameManager.instance.specialPoints.Length;
										}
										else
										{
											GameManager.instance.selectedTypeOfPointsForPlayer -= 1;
										}
					
										UIManager.instance.RefreshSelectedActionPointsUI();
			
										StartCoroutine(UnitManager.instance.PutAUnitToAction(selectedUnit));
									}
									else
									{

									}
								}
								else
								{
									UIManager.instance.HideActionOptionsWindow();
			
									//set selector to normal status
									GameManager.instance.SetSelectorStatus(cursorToCheck, 0);
									
									if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
									{
										GameManager.instance.selectedTypeOfPointsForPlayer = GameManager.instance.specialPoints.Length;
									}
									else
									{
										GameManager.instance.selectedTypeOfPointsForPlayer -= 1;
									}
					
									UIManager.instance.RefreshSelectedActionPointsUI();
			
									StartCoroutine(UnitManager.instance.PutAUnitToAction(selectedUnit));
								}
							}
						}
						//if no unit is selected
						else
						{
							if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
							{
								GameManager.instance.selectedTypeOfPointsForPlayer = GameManager.instance.specialPoints.Length;
							}
							else
							{
								GameManager.instance.selectedTypeOfPointsForPlayer -= 1;
							}
			
							UIManager.instance.RefreshSelectedActionPointsUI();
						}
					}
				}
			}
			else
			{
				//if valid for rotation. 1 for clockwise, 2 for counter clockwise
				if (!GameManager.instance.IsAbleToRotate(2))
				{
					//return;
				}
				else
				{
					int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
					//if there's a Unit that is being selected
					if (selectedUnitID > -1)
					{
						//Debug.Log("Rotate Test!");
		
						Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
			
						StartCoroutine(TileDotManager.instance.RotateTilesAroundAUnit(selectedUnit, 2));
					}
					//otherwise
					else
					{
		
					}
				}
			}
		}

		//when RB button is pressed 
		if (Input.GetKeyDown(KeyCode.Joystick1Button5))
		{
			//Debug.Log("Color of tile: " + TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position)].associatedGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color.r);

			//Dot dotBelow = TileDotManager.instance.dots[TileDotManager.instance.SearchDotIDByPosition(cursorToCheck.transform.position)];

			//if not in rotate mode
			if (!GameManager.instance.IsRotatingTiles())
			{
				//if player's turn
				if (GameManager.instance.turnID == 1)
				{
					GameObject cursorToCheck = GameManager.instance.cursorsContainer.transform.GetChild(0).gameObject;

					//if in normal mode
					if (GameManager.instance.GetSelectorStatus(cursorToCheck) == 0)
					{
						int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
						//if there's a Unit that is being selected
						if (selectedUnitID > -1)
						{
							Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
	
							//if not showing action option window
							if (!UnitManager.instance.IsUnitShowingActionOptionsWindow(selectedUnit))
							{
								if (GameManager.instance.selectedTypeOfPointsForPlayer == GameManager.instance.specialPoints.Length)
								{
									GameManager.instance.selectedTypeOfPointsForPlayer = 0;
								}
								else
								{
									GameManager.instance.selectedTypeOfPointsForPlayer += 1;
								}
			
								UIManager.instance.RefreshSelectedActionPointsUI();
							}
							//if showing action option window
							else
							{
								List<int> attackedToBeUnitOrBuilding = UnitManager.instance.SearchUnitIDsByAttackRangeOfAUnit(selectedUnit);

								if (attackedToBeUnitOrBuilding.Count > 0)
								{
									if (GameManager.instance.actionPointNeededToAttack > 0)
									{
										UIManager.instance.HideActionOptionsWindow();
		
										//set selector to normal status
										GameManager.instance.SetSelectorStatus(cursorToCheck, 0);
									
										if (GameManager.instance.selectedTypeOfPointsForPlayer == GameManager.instance.specialPoints.Length)
										{
											GameManager.instance.selectedTypeOfPointsForPlayer = 0;
										}
										else
										{
											GameManager.instance.selectedTypeOfPointsForPlayer += 1;
										}
					
										UIManager.instance.RefreshSelectedActionPointsUI();
			
										StartCoroutine(UnitManager.instance.PutAUnitToAction(selectedUnit));
									}
									else
									{

									}
								}
								else
								{
									UIManager.instance.HideActionOptionsWindow();
		
									//set selector to normal status
									GameManager.instance.SetSelectorStatus(cursorToCheck, 0);
								
									if (GameManager.instance.selectedTypeOfPointsForPlayer == GameManager.instance.specialPoints.Length)
									{
										GameManager.instance.selectedTypeOfPointsForPlayer = 0;
									}
									else
									{
										GameManager.instance.selectedTypeOfPointsForPlayer += 1;
									}
				
									UIManager.instance.RefreshSelectedActionPointsUI();
		
									StartCoroutine(UnitManager.instance.PutAUnitToAction(selectedUnit));
								}
							}
						}
						//if no unit is selected
						else
						{
							if (GameManager.instance.selectedTypeOfPointsForPlayer == GameManager.instance.specialPoints.Length)
							{
								GameManager.instance.selectedTypeOfPointsForPlayer = 0;
							}
							else
							{
								GameManager.instance.selectedTypeOfPointsForPlayer += 1;
							}
			
							UIManager.instance.RefreshSelectedActionPointsUI();
						}
					}
				}
			}
			else
			{
				//if valid for rotation. 1 for clockwise, 2 for counter clockwise
				if (!GameManager.instance.IsAbleToRotate(1))
				{
					//return;
				}
				else
				{
					int selectedUnitID = UnitManager.instance.SearchUnitIDBySelectedUnit();
								
					//if there's a Unit that is being selected
					if (selectedUnitID > -1)
					{
						//Debug.Log("Rotate Test!");
		
						Unit selectedUnit = UnitManager.instance.units[selectedUnitID];
			
						StartCoroutine(TileDotManager.instance.RotateTilesAroundAUnit(selectedUnit, 1));
					}
					//otherwise
					else
					{
		
					}
				}
			}
		}
#endregion
	}
}
