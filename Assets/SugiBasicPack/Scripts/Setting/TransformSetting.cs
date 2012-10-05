using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformSetting : MonoBehaviour {
	static string KeyEventName = "SetTransformSettings";
	
	static List<TransformSetting> TransformSettings = new List<TransformSetting>();
	static void SetSettings(){
		foreach(TransformSetting ts in TransformSettings)
			ts.SetSetting();
	}
	static bool Added{
		get{
			return KeyEventManager.HasEventOfName(KeyEventName);
		}
	}
	
	public KeyCode code = KeyCode.Space;
	public bool 
		local = false,
		setPos,
		setRot,
		setScale;
	string
		position,
		rotation,
		scale;
	
	void Awake(){
		position = name + "-Postion";
		rotation = name + "-Rotation";
		scale = name + "-Scale";
	}
	
	// Use this for initialization
	void Start () {
		GetSetting();
		TransformSettings.Add(this);
		if(!Added)
			KeyEventManager.AddEvent(KeyEventName, code, SetSettings);
	}
	
	// Update is called once per frame
	void Update () {
		if(Setting.Showing)
			GetSetting();
	}
	
	void GetSetting(){
		if(local){
			if(setPos) transform.localPosition = (Vector3)Setting.GetParam(position, paramType.Vector3);
			if(setRot) transform.localRotation = Quaternion.Euler((Vector3)Setting.GetParam(rotation, paramType.Vector3));
		}else{
			if(setPos) transform.position = (Vector3)Setting.GetParam(position, paramType.Vector3);
			if(setRot) transform.rotation = Quaternion.Euler((Vector3)Setting.GetParam(rotation, paramType.Vector3));
		}
		if(setScale) transform.localScale = (Vector3)Setting.GetParam(scale, paramType.Vector3);
	}
	
	void SetSetting(){
		if(local){
			if(setPos) Setting.SetParam(position, transform.localPosition);
			if(setRot) Setting.SetParam(rotation, transform.localRotation.eulerAngles);
		}else{
			if(setPos) Setting.SetParam(position, transform.position);
			if(setRot) Setting.SetParam(rotation, transform.rotation.eulerAngles);
		}
		if(setScale) Setting.SetParam(scale, transform.localScale);
	}
}
