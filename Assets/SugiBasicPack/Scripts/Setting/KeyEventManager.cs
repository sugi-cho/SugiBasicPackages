using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class KeyEvent{
	public KeyEvent(string n, KeyCode kc, Handler h){
		name = n;
		code = kc;
		handler = h;
	}
	public void LunchEvent(KeyCode kc){
		if(kc != code)
			return;
		
		handler();
	}
	public void Show(){
		GUILayout.BeginHorizontal();
		GUILayout.Label("KeyCode: " + code.ToString());
		GUILayout.FlexibleSpace();
		GUILayout.Label(name);
		GUILayout.EndHorizontal();
	}
	
	public delegate void Handler();
	public string name;
	KeyCode code;
	Handler handler;
}

public class KeyEventManager : MonoBehaviour {
	/**
	 * static
	 */
	public static KeyEventManager instance{
		get{
			if(_instance == null){
				GameObject go = GameObject.Find("Setting");
				if(go != null)
					_instance = go.GetComponent<KeyEventManager>() != null?
						go.GetComponent<KeyEventManager>() : go.AddComponent<KeyEventManager>();
				else
					_instance = new GameObject("Setting").AddComponent<KeyEventManager>();
			}
			return _instance;
		}
	}
	static KeyEventManager _instance;
	
	public static void AddEvent(string name, KeyCode code, KeyEvent.Handler handler){
		instance.AddKeyEvent(name, code, handler);
	}
	public static bool HasEventOfName(string name){
		return instance.HasEvent(name);
	}
	
	/**
	 * private
	 */
	
	List<KeyEvent> events = new List<KeyEvent>();
	bool showKeyEvents = false;
	int windowID;
	
	void Awake(){
		if(_instance != null)
			Destroy(gameObject);
		else
			_instance = this;
		DontDestroyOnLoad(gameObject);
		
		AddEvent("KeyHelper", KeyCode.H, ToggleHelp);
		windowID = WindowIDManager.ID;
	}
	
	void AddKeyEvent(string name, KeyCode code, KeyEvent.Handler handler){
		KeyEvent ke = new KeyEvent(name, code, handler);
		events.Add(ke);
	}
	
	bool HasEvent(string name){
		bool res = false;
		foreach(KeyEvent ke in events)
			res = res || name == ke.name;
		
		return res;
	}
	
	Rect windowRect = new Rect(20,20,400,600);
	void OnGUI(){
		Event e = Event.current;
		
		if(showKeyEvents)
			windowRect = GUILayout.Window(windowID, windowRect, ShowHelp, "KeyEventList");
		else{
			windowRect.x = e.mousePosition.x;
			windowRect.y = e.mousePosition.y;
		}
		
		if(e.type == EventType.KeyDown){
			foreach(KeyEvent ke in events)
				ke.LunchEvent(e.keyCode);
		}
	}
	
	void ShowHelp(int windowID){
		foreach(KeyEvent ke in events)
			ke.Show();
		GUI.DragWindow();
	}
	
	void ToggleHelp(){
		showKeyEvents = !showKeyEvents;
	}
}
