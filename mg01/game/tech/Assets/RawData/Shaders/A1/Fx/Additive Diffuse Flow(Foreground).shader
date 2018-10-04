Shader "A1/Self-Illumin/Non Ambient/Additive Diffuse Flow(Foreground)" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_EmissionLM ("Emission (Lightmapper)", Float) = 0

		_FlowMap ("FalowMap", 2D) = "white" {}
		_FlowSpeed("Flow Speed",float) = 1.0
		_FlowColor("Flow Color",Color) = (1,1,1,1)
	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _FlowMap;
		float _FlowSpeed;
		fixed3 _FlowColor;
		fixed4 _Color;
		
		half4 _MainTex_ST;
						
		struct v2f {
			half4 pos : SV_POSITION;
			half2 uv_MainTex : TEXCOORD0;
			half2 uv_FlowMap : TEXCOORD1;
		};

		v2f vert(appdata_full v) {
			v2f o;
			
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	
			o.uv_MainTex.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv_FlowMap = v.texcoord1;
					
			return o; 
		}
		
		fixed4 frag( v2f i ) : COLOR {	
			fixed4 tex = tex2D(_MainTex, i.uv_MainTex);
			fixed4 c2 = tex2D(_FlowMap, i.uv_FlowMap + _Time.xx * _FlowSpeed);
			fixed4 c = tex * _Color;
			return fixed4(c.rgb * _Color.a + c2.rgb * tex.a * _FlowColor, 1);
		}
	
	ENDCG
	
	SubShader {
		Tags { "RenderType"="Transparent" "Complexity"="4" "Queue" = "Overlay-1"}
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One One
		
	Pass {
	
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma only_renderers glcore gles gles3 metal d3d9
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG
		 
		}
				
	} 
	FallBack Off
}