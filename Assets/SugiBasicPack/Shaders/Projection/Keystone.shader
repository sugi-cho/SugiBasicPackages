Shader "Projection/KeyStoneCorrection" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Rect ("Xmin,Ymin,Xmax,Ymax", Vector) = (0,0,1,1)
		_A ("a",float) = 1
		_B ("b",float) = 0
		_C ("c",float) = 0
		_D ("d",float) = 0
		_E ("e",float) = 0
		_F ("f",float) = 1
		_G ("e",float) = 0
		_H ("f",float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert

		sampler2D _MainTex;
		half4 _Rect;
		half _A,_B,_C,_D,_E,_F,_G,_H;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			//half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			half2 uv = IN.uv_MainTex;
			
			if(_Rect.x < uv.x && _Rect.y < uv.y && uv.x < _Rect.z && uv.y < _Rect.w ){
				uv = (uv - half2(_Rect.x,_Rect.y)) / half2(_Rect.z - _Rect.x, _Rect.w - _Rect.y);
				half fx = floor(IN.uv_MainTex );
				half fy = floor(IN.uv_MainTex );
			
				half u =  _A*uv.x + _B*uv.y + _C*uv.x*uv.y + _D;
				half v =  _E*uv.x + _F*uv.y + _G*uv.x*uv.y + _H;
			
				uv = half2(lerp(_Rect.x,_Rect.z,u),lerp(_Rect.y,_Rect.w,v));
				
				if(uv.x < _Rect.x || uv.y < _Rect.y || _Rect.z < uv.x || _Rect.w < uv.y){
					o.Emission = 0;
				}
				else{
					o.Emission = tex2D(_MainTex, uv);
				}
			}
			else{
				o.Emission = tex2D(_MainTex, uv);
			}
			
//			half4 c = tex2D(_MainTex, uv);
//			o.Emission = c.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
