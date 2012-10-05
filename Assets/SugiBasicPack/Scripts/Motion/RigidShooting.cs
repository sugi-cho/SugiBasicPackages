using UnityEngine;
using System.Collections;

public class RigidShooting : MonoBehaviour {

	public static Vector3 GetVelocity(Vector3 fromPoint, Vector3 toPoint, float time = 3f){
		Vector3 fromToVec = toPoint - fromPoint;
		Vector3 velocity = fromToVec /time - Physics.gravity * time/2f;
		
		return velocity;
	}
	
	public static Vector3 GetVelocity(Rigidbody myBody, Transform target, float time = 3f){
		float time2 = Mathf.Pow(time,2f)/2f;
		Rigidbody targetBody = target.rigidbody;
		
		Vector3 toPoint = target.position;
		if(targetBody != null){
			toPoint += time * targetBody.velocity;
			
			if(targetBody.useGravity)
				toPoint += time2 * Physics.gravity;
			if(targetBody.constantForce != null)
				toPoint += time2 * targetBody.constantForce.force;
		}
		
		Vector3 fromToVec = toPoint - myBody.position;
		
		Vector3 velocity = (fromToVec/time);
		if(myBody.useGravity)
			velocity -= time/2f * Physics.gravity;
		if(myBody.constantForce != null)
			velocity -= time/2f * myBody.constantForce.force;
		
		return velocity;
	}
	
	public Transform target;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
			rigidbody.velocity = GetVelocity(rigidbody,target,1f);
	}
}
