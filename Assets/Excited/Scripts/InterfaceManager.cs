using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterfaceManager : MonoBehaviour 
{
	public static InterfaceManager instance;

	public InputField cursorSpeedInputField;
	public InputField snapTolerance;

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
	
	}
}
