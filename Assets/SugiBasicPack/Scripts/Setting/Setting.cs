using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @Author sugi.cho
/// </summary>
public enum paramType{
	Bool,
	Int,
	Float,
	String,
	Vector3,
}

[System.Serializable]
public class SettingParam{
	public SettingParam(string n, paramType pt){
		name = n;
		type = pt;
		b = false;
		i = 0;
		f = 0;
		s = "";
		v3 = Vector3.zero;
		min = 0;
		max = 1f;
	}
	
	public string name;
	public paramType type;
	public bool b;
	public int i;
	public float f;
	public string s;
	public Vector3 v3;
	public float min,max;
	public object val{
		set{
			switch(type){
			case paramType.Bool:
				b = (bool)value;
				break;
			case paramType.Int:
				i = (int)value;
				break;
			case paramType.Float:
				f = (float)value;
				break;
			case paramType.String:
				s = (string)value;
				break;
			case paramType.Vector3:
				v3 = (Vector3)value;
				break;
			}
		}
		get{
			switch(type){
			case paramType.Bool:
				return b;
			case paramType.Int:
				return i;
			case paramType.Float:
				return f;
			case paramType.String:
				return s;
			case paramType.Vector3:
				return v3;
			}
			return 0;
		}
	}
	
	public void ShowParam(){
		switch(type){
		case paramType.Bool:
			b = GUILayout.Toggle(b,name);
			break;
		case paramType.Int:
			GUILayout.Label(name + ": " + i.ToString());
			i = (int)Mathf.Floor(GUILayout.HorizontalSlider(i,min,max));
			break;
		case paramType.Float:
			GUILayout.Label(name + ": " + f.ToString("f2"));
			f = GUILayout.HorizontalSlider(f,min,max);
			break;
		case paramType.String:
			GUILayout.Label(name + ": ");
			s = GUILayout.TextField(s);
			break;
		case paramType.Vector3:
			GUILayout.Label(name + ": " + v3.ToString());
			GUILayout.BeginHorizontal();
			v3.x = GUILayout.HorizontalSlider(v3.x,v3.x - 0.1f,v3.x + 0.1f);
			v3.y = GUILayout.HorizontalSlider(v3.y,v3.y - 0.1f,v3.y + 0.1f);
			v3.z = GUILayout.HorizontalSlider(v3.z,v3.z - 0.1f,v3.z + 0.1f);
			GUILayout.EndHorizontal();
			break;
		}
	}
	public void SetMinMax(float min, float max){
		this.min = min;
		this.max = max;
	}
	public string ParamString{
		get{
			string valSt = type == paramType.Vector3?
				v3.x + "/" + v3.y + "/" + v3.z : val.ToString();
			return name + "," + type.ToString() + "," + valSt + "," + min.ToString() + "," + max.ToString();
		}
	}
}

public class Setting : MonoBehaviour {
	/**
	 * static
	 */
	public static Setting instance{
		get{
			if(_instance == null){
				GameObject go = GameObject.Find("Setting");
				if(go != null)
					_instance = go.GetComponent<Setting>() != null?
						go.GetComponent<Setting>() : go.AddComponent<Setting>();
				else
					_instance = new GameObject("Setting").AddComponent<Setting>();
			}
			return _instance;
		}
	}
	static Setting _instance;
	
	public static bool Showing{
		get{
			return instance.showParamlist;
		}
	}
	public static void AddParam(string name){
		SettingParam sp = new SettingParam(name, paramType.Bool);
		instance.settingParams.Add(sp);
	}
	public static void RemoveParam(int index){
//		instance.settingParams[index] = null;
		if(index < instance.settingParams.Count && index >= 0){
			instance.settingParams.RemoveAt(index);
		}
	}
	public static object GetParam(string name, paramType type){
		SettingParam sp = instance.GetSettingParam(name, type);
		return sp.val;
	}
	public static SettingParam SetParam(string name, bool b){
		SettingParam sp = instance.GetSettingParam(name, paramType.Bool);
		if(sp.type != paramType.Bool){
			Debug.LogError(sp.name + " is " + sp.type + "!!");
			return sp;
		}
		
		sp.val = b;
		return sp;
	}
	public static SettingParam SetParam(string name, int i){
		SettingParam sp = instance.GetSettingParam(name, paramType.Int);
		if(sp.type != paramType.Int){
			Debug.LogError(sp.name + " is " + sp.type + "!!");
			return sp;
		}
		
		sp.val = i;
		return sp;
	}
	public static SettingParam SetParam(string name, float f){
		SettingParam sp = instance.GetSettingParam(name, paramType.Float);
		if(sp.type != paramType.Float){
			Debug.LogError(sp.name + " is " + sp.type + "!!");
			return sp;
		}
		
		sp.val = f;
		return sp;
	}
	public static SettingParam SetParam(string name, string s){
		SettingParam sp = instance.GetSettingParam(name, paramType.String);
		if(sp.type != paramType.String){
			Debug.LogError(sp.name + " is " + sp.type + "!!");
			return sp;
		}
		
		sp.val = s;
		return sp;
	}
	public static SettingParam SetParam(string name, Vector3 v3){
		SettingParam sp = instance.GetSettingParam(name, paramType.Vector3);
		if(sp.type != paramType.Vector3){
			Debug.Log(sp.name + " is " + sp.type + "!!");
			return sp;
		}
		
		sp.val = v3;
		return sp;
	}
	/**
	 * public
	 */
	
	public string path = "/setting.ini";
	public int paramSize;
	public List<SettingParam> settingParams = new List<SettingParam>(); 
	
	/**
	 * private
	 */
	bool showParamlist = false;
	int windowID;
	
	void Awake(){
		if(_instance != null)
			Destroy(gameObject);
		else
			_instance = this;
		DontDestroyOnLoad(gameObject);
		
		windowID = WindowIDManager.ID;
		LoadParams();
	}
	
	void Start(){
		KeyEventManager.AddEvent("ShowSettingWindow", KeyCode.S, ToggleSettingList);
	}
	void ToggleSettingList(){
		showParamlist = !showParamlist;
	}
	
	Rect windowRect = new Rect(20f,20f,400f,600f);
	void OnGUI(){
		Event e = Event.current;
		
		if(showParamlist)
			windowRect = GUILayout.Window(windowID, windowRect, ShowParamList, "ParamList");
		else{
			windowRect.x = e.mousePosition.x;
			windowRect.y = e.mousePosition.y;
		}
		/*
		if(e.type == EventType.KeyDown){
			if(e.keyCode == KeyCode.S)
				showParamlist = !showParamlist;
		}
		*/
	}
	
	void ShowParamList(int id){
		foreach(SettingParam sp in settingParams)
			sp.ShowParam();
		if(GUILayout.Button("SaveParams"))
			SaveParams();
		GUI.DragWindow();
	}
	
	SettingParam GetSettingParam(string name, paramType type){
		foreach(SettingParam sp in settingParams){
			if(sp.name == name)
				return sp;
		}
		SettingParam param = new SettingParam(name,type);
		settingParams.Add(param);
		//paramSize = settingParams.Count;
		return param;
	}
	
	public void SaveParams(){
		StreamWriter w = File.CreateText(Application.dataPath + path);
		w.WriteLine(settingParams.Count.ToString());
		foreach(SettingParam sp in settingParams)
			w.WriteLine(sp.ParamString);
		w.Close();
	}
	
	public void LoadParams(){
		StreamReader r = new StreamReader(Application.dataPath + path);
		int numParams = int.Parse(r.ReadLine());
		
		for(int i = 0; i < numParams; i++){
			string[] ss = r.ReadLine().Split(',');
			if(ss.Length == 5){
			switch(ss[1]){
				case "Bool":
					SetParam(ss[0],bool.Parse(ss[2]));
					break;
				case "Int":
					SetParam(ss[0],int.Parse(ss[2])).SetMinMax(float.Parse(ss[3]), float.Parse(ss[4]));
					break;
				case "Float":
					SetParam(ss[0],float.Parse(ss[2])).SetMinMax(float.Parse(ss[3]), float.Parse(ss[4]));
					break;
				case "String":
					SetParam(ss[0],ss[2]);
					break;
				case "Vector3":
					string[] vv = ss[2].Split('/');
					Vector3 v3 = new Vector3(float.Parse(vv[0]),float.Parse(vv[1]),float.Parse(vv[2]));
					SetParam(ss[0],v3);
					break;
				}
			}
		}
		r.Close();
		paramSize = settingParams.Count;
	}
	
	void OnDestroy(){
		_instance = null;
	}
	
}