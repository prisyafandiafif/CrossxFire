using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;

public class UIManager : MonoBehaviour 
{
	public static UIManager instance;

	// Canvas for action menu
	public GameObject actionMenuCanvas;

	// Canvas for UI
	public GameObject uiCanvas;

	private Tweener playerStatusWindowTweener;
	private Tweener enemyStatusWindowTweener;

	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
		//show action points on UI
		RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	//refreseh which action point that is selected 
	public void RefreshSelectedActionPointsUI ()
	{
		GameObject normalActionPointBase = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject; 
		GameObject specialActionPointBaseContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject; 

		if (GameManager.instance.selectedTypeOfPointsForPlayer == 0)
		{
			//reset everything
			normalActionPointBase.GetComponent<RectTransform>().anchoredPosition = new Vector2(6f, normalActionPointBase.GetComponent<RectTransform>().anchoredPosition.y);
			normalActionPointBase.GetComponent<Outline>().effectDistance = new Vector2(6f, -6);
			normalActionPointBase.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

			for (int i = 0; i < GameManager.instance.specialPoints.Length; i++)
			{
				GameObject specialActionPointBase = specialActionPointBaseContainer.transform.GetChild(i).gameObject;

				specialActionPointBase.GetComponent<RectTransform>().anchoredPosition = new Vector2(130f + (94f*i), specialActionPointBase.GetComponent<RectTransform>().anchoredPosition.y);
				specialActionPointBase.GetComponent<Outline>().effectDistance = new Vector2(6f, -6);
				specialActionPointBase.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			}
		}
		else
		{
			normalActionPointBase.GetComponent<RectTransform>().anchoredPosition = new Vector2(6f, normalActionPointBase.GetComponent<RectTransform>().anchoredPosition.y);
			normalActionPointBase.GetComponent<Outline>().effectDistance = new Vector2(9f, -9);
			normalActionPointBase.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);

			for (int i = 0; i < GameManager.instance.specialPoints.Length; i++)
			{
				GameObject specialActionPointBase = specialActionPointBaseContainer.transform.GetChild(i).gameObject;

				if (i == GameManager.instance.selectedTypeOfPointsForPlayer - 1)
				{
					specialActionPointBase.GetComponent<RectTransform>().anchoredPosition = new Vector2(130f + (94f*i) - 30f, specialActionPointBase.GetComponent<RectTransform>().anchoredPosition.y);
					specialActionPointBase.GetComponent<Outline>().effectDistance = new Vector2(4f, -4);
					specialActionPointBase.GetComponent<RectTransform>().localScale = new Vector3(1.43f, 1.43f, 1.43f);
				}
				else
				{
					if (i < GameManager.instance.selectedTypeOfPointsForPlayer - 1)
					{ 
						specialActionPointBase.GetComponent<RectTransform>().anchoredPosition = new Vector2(130f + (94f*i) - 30f, specialActionPointBase.GetComponent<RectTransform>().anchoredPosition.y);
					}
					else
					if (i > GameManager.instance.selectedTypeOfPointsForPlayer - 1)
					{ 
						specialActionPointBase.GetComponent<RectTransform>().anchoredPosition = new Vector2(130f + (94f*i), specialActionPointBase.GetComponent<RectTransform>().anchoredPosition.y);
					}
					specialActionPointBase.GetComponent<Outline>().effectDistance = new Vector2(6f, -6);
					specialActionPointBase.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
	}
	
	// show/refresh action points on UI //TODO add more logics if there are more enemy units or player units
	public void RefreshActionPointsAndSpecialPointsAndSouldPointsAndStaminaPointsUI ()
	{
		GameObject playerNormalActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
		GameObject enemyNormalActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;

		GameObject playerSpecialPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
		
		GameObject enemySoulPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;

		GameObject enemyStaminaPointsContainer = uiCanvas.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;

		//change text
		playerNormalActionPointsContainer.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + GameManager.instance.actionPoints[0];
		enemyNormalActionPointsContainer.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + GameManager.instance.actionPoints[1];

		playerSpecialPointsContainer.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + GameManager.instance.specialPoints[0]; //thrower
		playerSpecialPointsContainer.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + GameManager.instance.specialPoints[1]; //shocktrooper
	
		enemySoulPointsContainer.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + GameManager.instance.soulPoints[0]; //shooter
		enemySoulPointsContainer.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + GameManager.instance.soulPoints[1]; //stomper

		enemyStaminaPointsContainer.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + GameManager.instance.staminaLimit;
	}

	// hide all action points from UI
	public void HideAllActionPointsUI ()
	{
		GameObject playerNormalActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
		GameObject enemyNormalActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;

		GameObject playerSpecialActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
		GameObject enemySpecialActionPointsContainer = uiCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;

		playerNormalActionPointsContainer.transform.GetChild(0).gameObject.SetActive(false);
		enemyNormalActionPointsContainer.transform.GetChild(0).gameObject.SetActive(false);

		for (int i = 0; i < playerSpecialActionPointsContainer.transform.childCount; i++)
		{
			playerSpecialActionPointsContainer.transform.GetChild(i).gameObject.SetActive(false);
		}
		
		for (int i = 0; i < enemySpecialActionPointsContainer.transform.childCount; i++)
		{
			enemySpecialActionPointsContainer.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	// show a canvas world
	public void ShowACanvasWorld (GameObject canvasObject)
	{
		canvasObject.SetActive(true);
	}

	// hide a canvas world
	public void HideACanvasWorld (GameObject canvasObject)
	{
		canvasObject.SetActive(false);
	}

	// position canvas world on a certain point
	public void PositionCanvasWorldOnWorldCoordinate (GameObject canvasObject, Vector3 newPos)
	{
		canvasObject.transform.position = newPos + new Vector3(0f, 1f, 0f);
	}

	// set a UI button as selected
	public void SelectAUIButton (Button button)
	{
		button.Select();
	}

	// to check whether there is a canvas world that is active
	public bool IsThereACanvasWorldActive ()
	{
		for (int i = 0; i < actionMenuCanvas.transform.childCount; i++)
		{
			//if one of the children is active
			if (actionMenuCanvas.transform.GetChild(i).gameObject.activeSelf)
			{
				return true;
			}
		}

		return false;
	}

	// hide player status window
	public IEnumerator HidePlayerStatusWindow ()
	{
		GameObject playerStatusWindow = uiCanvas.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
	
		if (playerStatusWindowTweener != null)
		{
			playerStatusWindowTweener.Kill();
		}

		playerStatusWindowTweener = HOTween.To
		(
			playerStatusWindow.GetComponent<RectTransform>(), 
			0.25f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						-994f,
						6f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutExpo)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.25f);
	}

	// show player status window
	public IEnumerator ShowPlayerStatusWindow (Unit unitToShow)
	{
		if (!unitToShow.associatedGameObject.activeSelf)
		{
			yield break;
		}

		GameObject playerStatusWindow = uiCanvas.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;

		//put name text
		playerStatusWindow.transform.GetChild(1).gameObject.GetComponent<Text>().text = unitToShow.unitName;
		
		//put rotation point
		for (int i = 0; i < playerStatusWindow.transform.GetChild(3).gameObject.transform.childCount; i++)
		{
			playerStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(i).gameObject.SetActive(false);
		}
		for (int i = 0; i < unitToShow.rotationPoint; i++)
		{
			playerStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(i).gameObject.SetActive(true);
			playerStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(i).gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
			playerStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(i).gameObject.GetComponent<Animator>().Play("Idle");
		}

		//put player icon image //TODO
	
		//put player icon text
		playerStatusWindow.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = unitToShow.unitName[0].ToString();

		//put player equipped item //TODO

		if (playerStatusWindowTweener != null)
		{
			playerStatusWindowTweener.Kill();
		}

		playerStatusWindowTweener = HOTween.To
		(
			playerStatusWindow.GetComponent<RectTransform>(), 
			0.25f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						6f,
						6f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutExpo)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.25f);
	}

	// hide enemy status window
	public IEnumerator HideEnemyStatusWindow ()
	{
		GameObject enemyStatusWindow = uiCanvas.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
	
		if (enemyStatusWindowTweener != null)
		{
			enemyStatusWindowTweener.Kill();
		}

		enemyStatusWindowTweener = HOTween.To
		(
			enemyStatusWindow.GetComponent<RectTransform>(), 
			0.25f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						994f,
						6f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutExpo)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.25f);
	}

	// show player status window
	public IEnumerator ShowEnemyStatusWindow (Unit unitToShow)
	{
		if (!unitToShow.associatedGameObject.activeSelf)
		{
			yield break;
		}

		GameObject enemyStatusWindow = uiCanvas.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;

		//put name text
		enemyStatusWindow.transform.GetChild(1).gameObject.GetComponent<Text>().text = unitToShow.unitName;
		
		//put health point
		enemyStatusWindow.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = unitToShow.healthPoint + "/" + unitToShow.fullHealthPoint;

		//put enemy icon image //TODO
	
		//put enemy icon text
		enemyStatusWindow.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = unitToShow.unitName[0].ToString();

		//put enemy equipped item //TODO

		if (enemyStatusWindowTweener != null)
		{
			enemyStatusWindowTweener.Kill();
		}

		if (enemyStatusWindowTweener != null)
		{
			enemyStatusWindowTweener.Kill();
		}

		enemyStatusWindowTweener = HOTween.To
		(
			enemyStatusWindow.GetComponent<RectTransform>(), 
			0.25f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						-6f,
						6f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutExpo)
				.Delay(0f)
		);

		yield return new WaitForSeconds(0.25f);
	}

	// to show animation of other text on a Unit
	public IEnumerator ShowOtherText (Unit unitToShow, string text, Color color)
	{
		//unitToShow.associatedGameObject.transform.GetChild(5).eulerAngles = Vector3.zero;	

		unitToShow.associatedGameObject.transform.GetChild(5).eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		unitToShow.associatedGameObject.transform.GetChild(5).gameObject.SetActive(true);

		unitToShow.associatedGameObject.transform.GetChild(5).gameObject.GetComponent<TextMesh>().text = text;

		unitToShow.associatedGameObject.transform.GetChild(5).gameObject.GetComponent<TextMesh>().color = color;
	
		yield return new WaitForSeconds(1f);

		unitToShow.associatedGameObject.transform.GetChild(5).gameObject.SetActive(false);

		unitToShow.associatedGameObject.transform.GetChild(5).gameObject.GetComponent<TextMesh>().text = "";

		unitToShow.associatedGameObject.transform.GetChild(5).gameObject.GetComponent<TextMesh>().color = Color.white;
	}

	// to show rotate text on a Unit
	public void ShowRotateText (Unit unitToShow)
	{
		unitToShow.associatedGameObject.transform.GetChild(2).eulerAngles = Vector3.zero;	

		unitToShow.associatedGameObject.transform.GetChild(2).gameObject.SetActive(true);
	}

	// to show attack text on a Unit
	public void ShowAttackText (Unit unitToShow)
	{
		unitToShow.associatedGameObject.transform.GetChild(3).eulerAngles = Vector3.zero;	

		unitToShow.associatedGameObject.transform.GetChild(3).gameObject.SetActive(true);
	}

	// to show attack text on a Unit with text
	public void ShowAttackText (Unit unitToShow, string textToShow)
	{
		unitToShow.associatedGameObject.transform.GetChild(3).eulerAngles = Vector3.zero;	

		unitToShow.associatedGameObject.transform.GetChild(3).gameObject.SetActive(true);

		unitToShow.associatedGameObject.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text = textToShow;
	}

	// to show stay text on a Unit
	public void ShowStayText (Unit unitToShow)
	{
		unitToShow.associatedGameObject.transform.GetChild(6).eulerAngles = Vector3.zero;	

		unitToShow.associatedGameObject.transform.GetChild(6).gameObject.SetActive(true);
	}
	
	// to show move text on a Unit
	public void ShowMoveText (Dot dotToShow)
	{
		dotToShow.associatedGameObject.transform.GetChild(0).gameObject.SetActive(true);
		dotToShow.associatedGameObject.transform.GetChild(1).gameObject.SetActive(true);
	}

	// to show action points that are needed to move text on a cursor
	public void ShowMoveActionPointTextOnCursor (GameObject cursor, string textToShow)
	{
		cursor.transform.GetChild(1).eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		//cursor.transform.GetChild(1).eulerAngles = Vector3.zero;	

		cursor.transform.GetChild(1).gameObject.SetActive(true);

		cursor.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = textToShow;
	}

	// to show action points that are needed to attack text on a crsor
	public void ShowAttackActionPointTextOnCursor (GameObject cursor, string textToShow)
	{
		cursor.transform.GetChild(1).eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		//cursor.transform.GetChild(1).eulerAngles = Vector3.zero;	

		cursor.transform.GetChild(1).gameObject.SetActive(true);

		cursor.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = textToShow;
	}

	// to show action points that are needed to rotation text on a crsor
	public void ShowRotationActionPointTextOnCursor (GameObject cursor, string textToShow)
	{
		cursor.transform.GetChild(1).eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		//cursor.transform.GetChild(1).eulerAngles = Vector3.zero;	

		cursor.transform.GetChild(1).gameObject.SetActive(true);

		cursor.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = textToShow;
	}

	// to show action points that are needed to turn text on a crsor
	public void ShowTurnActionPointTextOnCursor (GameObject cursor, string textToShow)
	{
		cursor.transform.GetChild(1).eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		//cursor.transform.GetChild(1).eulerAngles = Vector3.zero;	

		cursor.transform.GetChild(1).gameObject.SetActive(true);

		cursor.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = textToShow;
	}
	
	// to hide all text on a cursor
	public void HideAllTextOnCursor (GameObject cursor)
	{
		cursor.transform.GetChild(1).eulerAngles = Vector3.zero;	

		cursor.transform.GetChild(1).gameObject.SetActive(true);

		cursor.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "";
		cursor.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "";
	}

	// to show action option window on a Dot
	public void ShowActionOptionsWindowOnADot (Dot dotToShowOption, List<string> textForButtons)
	{
		GameObject actionOptions = dotToShowOption.associatedGameObject.transform.GetChild(2).gameObject;

		actionOptions.transform.eulerAngles = Vector3.zero;	

		actionOptions.SetActive(true);

		//activate the base
		actionOptions.transform.FindChild("Base_" + textForButtons.Count + "_Option").gameObject.SetActive(true);

		//show buttons
		for (int i = 0; i < textForButtons.Count; i++)
		{
			actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.SetActive(true);

			actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.GetComponent<TextMesh>().text = textForButtons[i];

			if (i > 0)
			{
				actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.GetComponent<TextMesh>().color = Color.yellow;
			}
			else
			{
				actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.GetComponent<TextMesh>().color = Color.black;
			}
		}

		//change the rotation based on camera rotation
		actionOptions.transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);

		//show highlight and put int to button 1 parent
		actionOptions.transform.FindChild("Highlight").gameObject.SetActive(true);
		actionOptions.transform.FindChild("Highlight").gameObject.transform.position = new Vector3(actionOptions.transform.FindChild("Highlight").gameObject.transform.position.x, actionOptions.transform.FindChild("Button_1").gameObject.transform.position.y, actionOptions.transform.FindChild("Highlight").gameObject.transform.position.z);
		actionOptions.transform.FindChild("Highlight").gameObject.transform.parent = actionOptions.transform.FindChild("Button_1").gameObject.transform;
	}


	// to show action option window on a unit
	public void ShowActionOptionsWindowOnAUnit (Unit unitToShowOption, List<string> textForButtons)
	{
		GameObject actionOptions = unitToShowOption.associatedGameObject.transform.GetChild(8).gameObject;

		actionOptions.transform.eulerAngles = Vector3.zero;	

		actionOptions.SetActive(true);

		//Debug.Log("Base " + textForButtons.Count);

		//activate the base
		actionOptions.transform.FindChild("Base_" + textForButtons.Count + "_Option").gameObject.SetActive(true);

		//show buttons
		for (int i = 0; i < textForButtons.Count; i++)
		{
			actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.SetActive(true);

			actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.GetComponent<TextMesh>().text = textForButtons[i];

			if (i > 0)
			{
				actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.GetComponent<TextMesh>().color = Color.yellow;
			}
			else
			{
				actionOptions.transform.FindChild("Button_" + (i+1)).gameObject.GetComponent<TextMesh>().color = Color.black;
			}
		}

		//change the rotation based on camera rotation
		actionOptions.transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);
	
		//show highlight and put int to button 1 parent
		actionOptions.transform.FindChild("Highlight").gameObject.SetActive(true);
		actionOptions.transform.FindChild("Highlight").gameObject.transform.position = new Vector3(actionOptions.transform.FindChild("Highlight").gameObject.transform.position.x, actionOptions.transform.FindChild("Button_1").gameObject.transform.position.y, actionOptions.transform.FindChild("Highlight").gameObject.transform.position.z);
		actionOptions.transform.FindChild("Highlight").gameObject.transform.parent = actionOptions.transform.FindChild("Button_1").gameObject.transform;
	}

	// to hide action options window
	public void HideActionOptionsWindow ()
	{
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			GameObject unit = UnitManager.instance.units[i].associatedGameObject;

			GameObject actionOptions = unit.transform.GetChild(8).gameObject;
	
			actionOptions.SetActive(false);

			//deactivate all children
			for (int j = 0; j < actionOptions.transform.childCount; j++)
			{
				actionOptions.transform.GetChild(j).gameObject.SetActive(false);

				//unparent hightlight
				if (actionOptions.transform.GetChild(j).gameObject.transform.childCount > 0)
				{
					actionOptions.transform.GetChild(j).gameObject.transform.GetChild(0).gameObject.transform.parent = actionOptions.transform;
				}
			}
		}

		for (int i = 0; i < TileDotManager.instance.dots.Count; i++)
		{
			GameObject dot = TileDotManager.instance.dots[i].associatedGameObject;

			GameObject actionOptions = dot.transform.GetChild(2).gameObject;
	
			actionOptions.SetActive(false);

			//deactivate all children
			for (int j = 0; j < actionOptions.transform.childCount; j++)
			{
				actionOptions.transform.GetChild(j).gameObject.SetActive(false);

				//unparent hightlight
				if (actionOptions.transform.GetChild(j).gameObject.transform.childCount > 0)
				{
					actionOptions.transform.GetChild(j).gameObject.transform.GetChild(0).gameObject.transform.parent = actionOptions.transform;
				}
			}
		}
	}

	// to hide move text on all dots
	public void HideAllMoveText ()
	{
		for (int i = 0; i < TileDotManager.instance.dots.Count; i++)
		{
			Dot relatedDot = TileDotManager.instance.dots[i];

			relatedDot.associatedGameObject.transform.GetChild(0).gameObject.SetActive(false);
			relatedDot.associatedGameObject.transform.GetChild(1).gameObject.SetActive(false);
		}
	}

	// to hide rotate text on all Units
	public void HideAllRotateText ()
	{
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			Unit relatedUnit = UnitManager.instance.units[i];

			relatedUnit.associatedGameObject.transform.GetChild(2).gameObject.SetActive(false);
		}
	}

	// to hide attack text on all Units
	public void HideAllAttackText ()
	{
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			Unit relatedUnit = UnitManager.instance.units[i];

			relatedUnit.associatedGameObject.transform.GetChild(3).gameObject.SetActive(false);
		}
	}

	// to hide stay text on all Units
	public void HideAllStayText ()
	{
		for (int i = 0; i < UnitManager.instance.units.Count; i++)
		{
			Unit relatedUnit = UnitManager.instance.units[i];

			relatedUnit.associatedGameObject.transform.GetChild(6).gameObject.SetActive(false);
		}
	}

	// move highlight. 1 move up, 2 move down
	public IEnumerator MoveHighlightInActionOptionsWindow (int direction, GameObject actionOptionsWindow)
	{
		InputManager.instance.isInputActive = false;

		int maxButtonForActionOptionsWindow = 4; //CAN BE CHANGED

		int currentSelectedButtonID = 0;
		int highestActiveButtonID = 1;

		GameObject highlight = null;

		for (int i = 0; i < maxButtonForActionOptionsWindow; i++)
		{
			//make all button text to yellow
			actionOptionsWindow.transform.FindChild("Button_" + (i+1)).gameObject.GetComponent<TextMesh>().color = Color.yellow;

			if (actionOptionsWindow.transform.FindChild("Button_" + (i+1)).gameObject.transform.childCount > 0)
			{
				currentSelectedButtonID = i+1;
				highlight = actionOptionsWindow.transform.FindChild("Button_" + (i+1)).gameObject.transform.GetChild(0).gameObject;
			}

			if (!actionOptionsWindow.transform.FindChild("Button_" + (i+1)).gameObject.activeSelf)
			{
				Debug.Log("Highest active button ID: " + (i));

				highestActiveButtonID = i;

				break;
			}

			if (i == maxButtonForActionOptionsWindow-1)
			{
				highestActiveButtonID = i+1;
			}
		}

		if (direction == 1)
		{
			if (currentSelectedButtonID == maxButtonForActionOptionsWindow)
			{
				highlight.transform.parent = actionOptionsWindow.transform.FindChild("Button_1").gameObject.transform;

				actionOptionsWindow.transform.FindChild("Button_1").gameObject.GetComponent<TextMesh>().color = Color.black;
			}
			else
			{
				if (actionOptionsWindow.transform.FindChild("Button_" + (currentSelectedButtonID+1)).gameObject.activeSelf)
				{
					highlight.transform.parent = actionOptionsWindow.transform.FindChild("Button_" + (currentSelectedButtonID+1)).gameObject.transform;

					actionOptionsWindow.transform.FindChild("Button_" + (currentSelectedButtonID+1)).gameObject.GetComponent<TextMesh>().color = Color.black;
				}
				else
				{
					highlight.transform.parent = actionOptionsWindow.transform.FindChild("Button_1").gameObject.transform;

					actionOptionsWindow.transform.FindChild("Button_1").gameObject.GetComponent<TextMesh>().color = Color.black;
				}
			}

			highlight.transform.localPosition = new Vector3(highlight.transform.localPosition.x, 0f, highlight.transform.localPosition.z);
		}
		else
		{
			if (currentSelectedButtonID == 1)
			{
				highlight.transform.parent = actionOptionsWindow.transform.FindChild("Button_" + highestActiveButtonID).gameObject.transform;

				actionOptionsWindow.transform.FindChild("Button_" + highestActiveButtonID).gameObject.GetComponent<TextMesh>().color = Color.black;
			}
			else
			{
				if (actionOptionsWindow.transform.FindChild("Button_" + (currentSelectedButtonID-1)).gameObject.activeSelf)
				{
					highlight.transform.parent = actionOptionsWindow.transform.FindChild("Button_" + (currentSelectedButtonID-1)).gameObject.transform;

					actionOptionsWindow.transform.FindChild("Button_" + (currentSelectedButtonID-1)).gameObject.GetComponent<TextMesh>().color = Color.black;
				}
				else
				{
					highlight.transform.parent = actionOptionsWindow.transform.FindChild("Button_" + highestActiveButtonID).gameObject.transform;

					actionOptionsWindow.transform.FindChild("Button_" + highestActiveButtonID).gameObject.GetComponent<TextMesh>().color = Color.black;
				}
			}

			highlight.transform.localPosition = new Vector3(highlight.transform.localPosition.x, 0f, highlight.transform.localPosition.z);
		}

		yield return new WaitForSeconds(0.15f);
		
		InputManager.instance.isInputActive = true;
	}

	// show turn bumper animation. 1 is player, 2 is enemy
	public IEnumerator ShowTurnBumperAnimation (int turnTargetID)
	{
		GameObject bumperUI = uiCanvas.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
		Text bumperText = bumperUI.transform.GetChild(0).gameObject.GetComponent<Text>();

		bumperUI.SetActive(true);

		//if going to player's turn
		if (turnTargetID == 1)
		{
			bumperUI.GetComponent<Image>().color = Color.blue;
			bumperText.text = "PLAYER'S TURN";
		}
		else
		//if going to enemy's turn
		if (turnTargetID == 2)
		{
			bumperUI.GetComponent<Image>().color = Color.red;
			bumperText.text = "ENEMY'S TURN";
		}

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						0f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		//yield return new WaitForSeconds(1f);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						2000f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseInBack)
				.Delay(1f)
		);

		yield return new WaitForSeconds(1.5f);
	
		bumperUI.SetActive(false);	
		bumperUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
	}

	// show resurrection bumper animation
	public IEnumerator ShowResurrectionBumperAnimation ()
	{
		GameObject bumperUI = uiCanvas.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject;

		bumperUI.SetActive(true);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						0f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		//yield return new WaitForSeconds(1f);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						2000f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseInBack)
				.Delay(1f)
		);

		yield return new WaitForSeconds(1.5f);
	
		bumperUI.SetActive(false);	
		bumperUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
	}

	// show soul theft bumper animation
	public IEnumerator ShowSoulTheftBumperAnimation ()
	{
		GameObject bumperUI = uiCanvas.transform.GetChild(7).gameObject.transform.GetChild(0).gameObject;

		bumperUI.SetActive(true);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						0f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		//yield return new WaitForSeconds(1f);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						2000f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseInBack)
				.Delay(1f)
		);

		yield return new WaitForSeconds(1.5f);
	
		bumperUI.SetActive(false);	
		bumperUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
	}

	// show crossfire bumper animation
	public IEnumerator ShowCrossfireBumperAnimation ()
	{
		GameObject bumperUI = uiCanvas.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;

		bumperUI.SetActive(true);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						0f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		//yield return new WaitForSeconds(1f);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						2000f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseInBack)
				.Delay(1f)
		);

		yield return new WaitForSeconds(1.5f);
	
		bumperUI.SetActive(false);	
		bumperUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
	}

	// show suicide bumper animation
	public IEnumerator ShowSuicideBumperAnimation ()
	{
		GameObject bumperUI = uiCanvas.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject;

		bumperUI.SetActive(true);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						0f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseOutBack)
				.Delay(0f)
		);

		//yield return new WaitForSeconds(1f);

		HOTween.To
		(
			bumperUI.GetComponent<RectTransform>(), 
			0.5f, 
			new TweenParms()
		        .Prop
				(
					"anchoredPosition", 
					new Vector2
					(
						2000f,
						0f
					),
					false	
				) 
		        .Ease(EaseType.EaseInBack)
				.Delay(1f)
		);

		yield return new WaitForSeconds(1.5f);
	
		bumperUI.SetActive(false);	
		bumperUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
	}

	public void CreateMovementLine (Unit movingUnit, Dot startDot, Dot endDot)
	{
		//Debug.Log("Create Movement Line");

		//create objects in the middle of crossfire and crossfiring units
		GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
	
		Vector3 location = startDot.associatedGameObject.transform.position + endDot.associatedGameObject.transform.position;
	
		line.name = "Line";
		line.transform.position = new Vector3(location.x/2f, -3f, location.z/2f);
		line.transform.localScale = new Vector3(0.1f, 0.1f, 0.5f);
	
		//add mesh renderer to it and change the color
		//line.AddComponent<MeshRenderer>();
		line.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color"));
		line.GetComponent<MeshRenderer>().material.color = Color.yellow; //player's color
	
		int directionToEndDot = -1;

		for (int i = 0; i < startDot.neighboringDots.Count; i++)
		{
			if (startDot.neighboringDots[i] == TileDotManager.instance.dots.IndexOf(endDot))
			{
				directionToEndDot = i;
			}
		}

		//wrong dot is assigned
		if (directionToEndDot == -1)
		{
			Destroy(line);

			return;
		}
	
		//if on top left or bottom right
		if (directionToEndDot == 0 || directionToEndDot == 3)
		{
			line.transform.localEulerAngles = new Vector3(0f, -30f, 0f);
		}
		else
		//if on top right or bottom left
		if (directionToEndDot == 1 || directionToEndDot == 4)
		{
			line.transform.localEulerAngles = new Vector3(0f, 30f, 0f);
		}
		else
		//if on right or left
		if (directionToEndDot == 2 || directionToEndDot == 5)
		{
			line.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
		}
	
		//if container already exists
		if (GameObject.Find("MovementLines"))
		{
			//put this as a child of Lines container
			line.transform.parent = GameObject.Find("MovementLines").transform;
		}
		else
		{
			//create a container for lines
			GameObject lineParent = new GameObject();
			lineParent.name = "MovementLines";

			//put this as a child of Lines container
			line.transform.parent = lineParent.transform;
		}
	}
}
