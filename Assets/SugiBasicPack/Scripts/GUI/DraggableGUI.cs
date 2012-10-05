using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class DraggableGUI : MonoBehaviour {
	public Vector2[] points;
	public bool viewing;
	
	int selectingPoint = -1;
	
	public float width = 0.1f;
	public float height = 0.1f;
	public Texture tex;
	public Material mat;
	
	
	Vector2 GetMousePosition2ViewPort(Vector2 mousePosition){
		return new Vector2(mousePosition.x/Screen.width, (Screen.height - mousePosition.y)/Screen.height);
	}
	int GetNearestPoint(Vector2 pos){
		int res = 0;
		float d = Mathf.Infinity;
		for(int i = 0; i < points.Length; i++){
			float dd = (pos - points[i]).sqrMagnitude;
			if(dd < d){
				d = dd;
				res = i;
			}
		}
		return res;
	}
	
	void Awake(){
	}
	
	// Use this for initialization
	void Start () {
		camera.clearFlags = CameraClearFlags.Depth;
		camera.renderingPath = RenderingPath.Forward;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(!viewing)
			return;
		
		Event e = Event.current;
		
		if(selectingPoint != -1){
			points[selectingPoint] = GetMousePosition2ViewPort(e.mousePosition);
		}
		
		if(e.type == EventType.MouseDown){
			selectingPoint = GetNearestPoint(GetMousePosition2ViewPort(e.mousePosition));
		}
		else if(e.type == EventType.MouseUp){
			selectingPoint = -1;
		}
		
		//Debug.Log(GetMousePosition2ViewPort(e.mousePosition));
	}
	
	void OnPostRender(){
		if(!viewing)
			return;
		
		mat.mainTexture = tex;
		float width2 = width/2f;
		float height2 = width2 * Screen.width/Screen.height *height/width;
		
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho();
		GL.Begin(GL.QUADS);
		GL.Color(Color.red);
		foreach(Vector2 point in points){
			GL.TexCoord(new Vector3(0,0,1));
			GL.Vertex3(point.x - width2, point.y - height2, 0);
			GL.TexCoord(new Vector3(0,1,1));
			GL.Vertex3(point.x - width2, point.y + height2, 0);
			GL.TexCoord(new Vector3(1,1,1));
			GL.Vertex3(point.x + width2, point.y + height2, 0);
			GL.TexCoord(new Vector3(1,0,1));
			GL.Vertex3(point.x + width2, point.y - height2, 0);
		}
		GL.End();
		GL.PopMatrix();
	}
}
