using UnityEngine;
using System.Collections;

public class ObjectEmitter : MonoBehaviour {
	
	public Transform emitObject;
	public float 
		emission = 3f,
		interval = 5f;
	
	public Vector3 radius;
	
	public bool emit = true;
	
	// Use this for initialization
	void Start () {
		InvokeRepeating("Emit", 0, interval);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Emit(){
		if(!emit)
			return;
		
		for(int i = 0; i < emission; i ++)
			Instantiate(emitObject, emitPos, emitObject.rotation);
	}
	Vector3 emitPos{
		get{
			Vector3 v3 = Random.insideUnitSphere;
			v3.x *= radius.x;
			v3.y *= radius.y;
			v3.z *= radius.z;
		
			v3 += transform.position;
			return v3;
		}
	}
}
