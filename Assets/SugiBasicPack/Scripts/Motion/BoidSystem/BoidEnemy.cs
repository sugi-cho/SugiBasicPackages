using UnityEngine;
using System.Collections;

public class BoidEnemy : MonoBehaviour {
	
	public int id = 0;
	public float strength = 1f;
	public Vector3 position{
		get{
			return transform.position;
		}
	}
	
	// Use this for initialization
	void Start () {
		foreach(BoidGroup bg in FindObjectsOfType(typeof(BoidGroup))){
			if(bg.id == id)
				bg.enemys.Add(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
