using UnityEngine;
using System.Collections;

public class Transpring : MonoBehaviour {
	
	public Transpring[] transGroups;
	public float posK = 0.1f;
	public float posEase = 0.9f;
	public float scaleK = 0.1f;
	public float scaleEase = 0.9f;
	public float eulerK = 0.1f;
	public float eulerEase = 0.9f;
	public float damp = 0.5f;
	
	public Vector3 firstDeltaPos;
	public Vector3 firstDeltaScale;
	public Vector3 firstDeltaEuler;
	
	public float sleepVel = 0.01f;
	
	Vector3 targetPos;
	Vector3 targetScale;
	Vector3 targetEuler;
	
	Vector3 velPos;
	Vector3 velScale;
	Vector3 velEuler;
	
	bool springable = false;
	
	public void AddForcePos(Vector3 pos){
		velPos += pos;
		springable = true;
	}
	public void AddForceSca(Vector3 sca){
		velScale += sca;
		springable = true;
	}
	public void AddForceEul(Vector3 eul){
		velEuler += eul;
		springable = true;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	public Transpring SetTargetPos(Vector3 pos){
		targetPos = pos;
		return this;
	}
	public Transpring SetTargetRot(Vector3 eul){
		targetEuler = eul;
		return this;
	}
	public Transpring SetTargetScl(Vector3 scl){
		targetScale = scl;
		return this;
	}
	
	// Update is called once per frame
	void Update () {
		if(!springable)
			return;
		
		velPos += (transform.position - targetPos)*posK;
		velScale += (transform.localScale - targetScale)*scaleK;
		velEuler += (transform.rotation.eulerAngles - targetEuler)*eulerK;
		
		foreach(Transpring t in transGroups){
			float d = Vector3.Distance(transform.position, t.transform.position);
			velPos += Mathf.Exp(-damp*d) * t.velPos;
			velScale += Mathf.Exp(-damp*d) * t.velScale;
			velEuler += Mathf.Exp(-damp*d) * t.velEuler;
		}
		
		velPos *= posEase;
		velScale *= scaleEase;
		velEuler *= eulerEase;
		
		transform.position += velPos;
		transform.localScale += velScale;
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + velEuler);
		
		if(velPos.magnitude < sleepVel
			&& velScale.magnitude < sleepVel
			&& velEuler.magnitude < sleepVel)
			springable = false;
		
	}
}
