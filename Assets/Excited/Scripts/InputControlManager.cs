using UnityEngine;
using System.Collections;

public class InputControlManager : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		//set the value in the UI
		//InterfaceManager.instance.snapTolerance.text = "" + GameObject.Find("Dot").GetComponent<BoxCollider>().size.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// Set snap tolerance to each dot
	/*public void SetSnapTolerance ()
	{
		for (int i = 0; i < GameObject.Find("Dots").transform.childCount; i++)
		{
			GameObject.Find("Dots").transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size = 
			new Vector3
			(
				float.Parse(InterfaceManager.instance.snapTolerance.text), 
				GameObject.Find("Dots").transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size.y, 
				float.Parse(InterfaceManager.instance.snapTolerance.text)
			);
		}
	}*/
}
