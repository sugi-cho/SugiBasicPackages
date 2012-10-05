using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugObject : MonoBehaviour {
	static string KeyEventName = "";
	static List<DebugObject> Objects = new List<DebugObject>();
	public static void ToggleShow(){
		foreach(DebugObject obj in Objects){
			if(obj.renderer != null)
				obj.renderer.enabled = !obj.renderer.enabled;
		}
	}
	
	public string keyEventName = "ToggleDebugObjects";
	
	void Awake(){
		Objects.Add(this);
		KeyEventName = KeyEventName == ""?
			keyEventName:KeyEventName;
		if(!KeyEventManager.HasEventOfName(KeyEventName))
			KeyEventManager.AddEvent(KeyEventName,KeyCode.D,ToggleShow);
	}
	
	// Use this for initialization
	void Start () {
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
