using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidGroup : MonoBehaviour {
	public int id = 0;
	public Boid boid;
	public List<Boid> boids;
	public List<BoidEnemy> enemys;
	public List<BoidTarget> targets;
	public float searchFrec = 1.0f;
	public float safeArea = 1.0f;
	public float warnArea = 10.0f;
	public int defaultBoidsNum = 30;
	public Vector3 center;
	
	// Use this for initialization
	void Start () {
		for(int i = 0; i < defaultBoidsNum; i++){
			Boid b = (Boid)Instantiate(boid,transform.position + Random.insideUnitSphere * 10f, transform.rotation);
			b.boidGroup = this;
			b.transform.parent = transform;
		}
		InvokeRepeating("UpdateCenter", 0.1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void UpdateCenter(){
		center = Vector3.zero;
		foreach(Boid b in boids)
			center += b.transform.position;
		center /= boids.Count;
	}
	
	
}
