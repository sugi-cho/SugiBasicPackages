using UnityEngine;
using System.Collections;

//[ExecuteInEditMode()]
[RequireComponent(typeof(Camera))]
public class KeystoneSetting : MonoBehaviour {
	public Rect r;
	public Vector2[] p;
	
	public int[] indexOfGUI;
	
	public DraggableGUI controllGUI;
	
	bool editing;
	float a,b,c,d,e,f,g,h;
	Material m;
	// Use this for initialization
	void Start () {
		p[0] = new Vector2(0,0);
		p[1] = new Vector2(0,1);
		p[2] = new Vector2(1,1);
		p[3] = new Vector2(1,0);
		
		camera.clearFlags = CameraClearFlags.Depth;
		m = new Material(Shader.Find("Projection/KeyStoneCorrection"));
		m.name = "KeystoneTransMat";
	}
	
	// Update is called once per frame
	void Update () {
		if(editing && Input.GetMouseButton(0)){
			Vector2 pos = camera.ScreenToViewportPoint(Input.mousePosition);
			Vector2 res = p[0];/*
			for(int i = 1; i < 4; i++){
//				if((p[i] - pos).sqrMagnitude < );
			}*/
			
			
			
		}
		if(Input.GetMouseButtonUp(0)){
			CalicurateVals();
			SetMaterial();
		}
	}
	
	void OnRenderImage(RenderTexture source, Texture destination){
		Graphics.Blit(source, m);
	}
	
	void SetMaterial(){
		m.SetFloat("_A",a);
		m.SetFloat("_B",b);
		m.SetFloat("_C",c);
		m.SetFloat("_D",d);
		m.SetFloat("_E",e);
		m.SetFloat("_F",f);
		m.SetFloat("_G",g);
		m.SetFloat("_H",h);
		
		m.SetVector("_Rect", new Vector4(r.xMin,r.yMin,r.xMax,r.yMax));
	}
	
	void CalicurateVals(){
		for(int i = 0; i < p.Length; i++){
			p[i] = controllGUI.points[indexOfGUI[i]];
			p[i].x = (p[i].x - r.xMin) / (r.xMax - r.xMin);
			p[i].y = (p[i].y - r.yMin) / (r.yMax - r.yMin);
		}
		
		float[,] vals = new float[8,9];
		int count = 0;
		
		SetVal(p[0], 0, ref count, ref vals);
		SetVal(p[0], 0, ref count, ref vals);
		SetVal(p[1], 0f, ref count, ref vals);
		SetVal(p[1], 1f, ref count, ref vals);
		SetVal(p[2], 1f, ref count, ref vals);
		SetVal(p[2], 1f, ref count, ref vals);
		SetVal(p[3], 1f, ref count, ref vals);
		SetVal(p[3], 0f, ref count, ref vals);
		
		MATGJ(7,vals);
		
		a = vals[0,8];
		b = vals[1,8];
		c = vals[2,8];
		d = vals[3,8];
		e = vals[4,8];
		f = vals[5,8];
		g = vals[6,8];
		h = vals[7,8];
	}
	
	void SetVal(Vector2 p, float uv, ref int i, ref float[,] vals){
		switch(i % 2){
		case 0:
			vals[i,0] = p.x; vals[i,1] = p.y; vals[i,2] = p.x*p.y; vals[i,3] = 1f;
			vals[i,4] = 0; vals[i,5] = 0; vals[i,6] = 0; vals[i,7] = 0; vals[i,8] = uv;
			break;
		case 1:
			vals[i,0] = 0; vals[i,1] = 0; vals[i,2] = 0; vals[i,3] = 0;
			vals[i,4] = p.x; vals[i,5] = p.y; vals[i,6] = p.x*p.y; vals[i,7] = 1f;
			vals[i,8] = uv;
			break;
		}
		i++;
	}
	
	void MATGJ(int n,float[,] a){
		//Gauss-Jordan法による連立一次方程式の解法
		//http://www.geocities.jp/damyarou/txt1GJMAT/cs_Form1.txt
		int i,j, k, s;
		float p, d, max, dumy;

		for (k = 0; k <= n; k++)
		{
			max = 0.0f;
			s = k;
			for (j = k; j <= n; j++)
			{
				if (Mathf.Abs(a[j, k]) > max)
				{
					max = Mathf.Abs(a[j, k]);
					s = j;
				}
			}
			for (j = 0; j <= n + 1; j++)
			{
				dumy = a[k, j];
				a[k, j] = a[s, j];
				a[s, j] = dumy;
			}
			p = a[k, k];
			for (j = k; j <= n + 1; j++)
			{
				a[k, j] = a[k, j] / p;
			}
			for (i = 0; i <= n; i++)
			{
				if (i != k)
				{
					d = a[i, k];
					for (j = k; j <= n + 1; j++)
					{
						a[i, j] = a[i, j] - d * a[k, j];
					}
				}
			}
		}
	}
}
