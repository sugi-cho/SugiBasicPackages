using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour {
	public BoidGroup boidGroup;
	
	Boid nearBoid;
	BoidTarget target;
	Vector3 position{
		get{
			return rigidbody.position;
		}
		set{
			rigidbody.position = value;
		}
	}
	Vector3 velocity{
		get{
			return rigidbody.velocity;
		}
		set{
			rigidbody.velocity = value;
		}
	}
	float safeArea{
		get{
			return boidGroup.safeArea;
		}
	}
	float warnArea{
		get{
			return boidGroup.warnArea;
		}
	}
	
	// Use this for initialization
	void Start () {
		if(boidGroup == null){
			Destroy(gameObject);
			return;
		}
		else
			boidGroup.boids.Add(this);
		
		velocity = Random.insideUnitSphere * 10f;
		InvokeRepeating("GetNearBoid", Random.value * boidGroup.searchFrec, boidGroup.searchFrec);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(nearBoid != null){
			float d = Vector3.Distance(position, nearBoid.position);
			if(d == 0)
				return;
			Vector3 v = nearBoid.velocity;
			Vector3 p = nearBoid.position;
			
			velocity += (v - velocity)/d/60f;
			velocity += (nearBoid.position - position) * Mathf.Min(d,boidGroup.warnArea)/100f;
			if(d < safeArea)
				velocity -= (nearBoid.position - position).normalized*(safeArea - d)/d;
		}
		
		if(target != null)
			velocity += Vector3.ClampMagnitude(target.transform.position - position, boidGroup.warnArea) / 10f;
		
		foreach(BoidEnemy be in boidGroup.enemys){
			float d = Vector3.Distance(position, be.transform.position);
			if(d < warnArea){
				Vector3 normalized = Vector3.Lerp(velocity.normalized, -(be.position - position).normalized, 0.5f).normalized;
				velocity += be.strength * (warnArea - d) * Mathf.Min((be.position - position).magnitude, boidGroup.warnArea) * normalized / 10f;
			}
		}
		
		velocity += Vector3.ClampMagnitude(boidGroup.center - position, boidGroup.warnArea) / 100f;
		position = Vector3.ClampMagnitude(position,1000f);
		//velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude,0,10f);
	}
	
	void GetNearBoid(){
		float m = Mathf.Infinity;
		nearBoid = null;
		foreach(Boid b in boidGroup.boids){
			float sm = (b.position - position).sqrMagnitude;
			if(sm != 0 && sm < m){
				m = sm;
				nearBoid = b;
			}
		}
		
		m = Mathf.Infinity;
		target = null;
		foreach(BoidTarget bt in boidGroup.targets){
			float sm = (bt.transform.position - position).sqrMagnitude/bt.strength;
			if(sm < m){
				m = sm;
				target = bt;
			}
		}
	}
	
	void OnDestroy(){
		if(boidGroup != null)
			boidGroup.boids.Remove(this);
	}
}
