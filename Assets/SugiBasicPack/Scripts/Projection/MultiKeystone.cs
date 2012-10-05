using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MultiKeystone : MonoBehaviour {
	
	public int numVertical = 1;
	public int numHolizontal = 2;
	
	DraggableGUI gui{
		get{
			DraggableGUI dgui = GetComponent<DraggableGUI>();
			return dgui != null?
				dgui : gameObject.AddComponent<DraggableGUI>();
		}
	}
	
	// Use this for initialization
	void Start () {
		gui.points = new Vector2[numHolizontal * numVertical * 4];
		int count = 0;
		for(int v = 0; v < numVertical; v++){
			for(int h = 0; h < numHolizontal; h++){
				
				GameObject g = new GameObject("Keystone");
				Camera c = g.AddComponent<Camera>();
				KeystoneSetting ks = g.AddComponent<KeystoneSetting>();
				
				g.transform.parent = transform;
				
				c.depth = camera.depth-1;
				c.renderingPath = RenderingPath.Forward;
				
				ks.r = new Rect(h * 1f/numHolizontal, v * 1f/numVertical, 1f/numHolizontal, 1f/numVertical);
				ks.controllGUI = gui;
				
				ks.indexOfGUI = new int[4];
				ks.p = new Vector2[4];
				
				for(int i = 0; i < 4; i++){
					switch(i){
					case 0:
						gui.points[count] = new Vector2(ks.r.xMin, ks.r.yMin);
						break;
					case 1:
						gui.points[count] = new Vector2(ks.r.xMin, ks.r.yMax);
						break;
					case 2:
						gui.points[count] = new Vector2(ks.r.xMax, ks.r.yMax);
						break;
					case 3:
						gui.points[count] = new Vector2(ks.r.xMax, ks.r.yMin);
						break;
					}
					ks.indexOfGUI[i] = count;
					count++;
				}
				
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
