// EjoyUnity������ĿShader 
// A2-Effect-Shader
// ��Shader������Ч���ȵ�����ͼ
// ������1����͸��ͨ������ͼ����Ⱦ��ʽΪ��ɫ���ӣ���ͼ��ɫΪ�޹��ɫΪ�׹����

Shader "EJoyShader/Effect/Soft/SoftEdge-1T-Base-Alpha-Add" {
Properties {
	_Alpha ("Alpha", Range(0,1.0)) = 1.0
	_TintColor ("Tint Color", Color) = (0.1,0.1,0.1,1.0)
	_MainTex ("Base Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			float _Alpha;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				//o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord = v.texcoord;
				return o;
			}

			//sampler2D _CameraDepthTexture;
			
			fixed4 frag (v2f i) : COLOR
			{
				float4 CurTex = tex2D(_MainTex, i.texcoord);
				float average = ((CurTex.r + 2 * CurTex.g + CurTex.b) / 3);
				return 1.0f * _TintColor * float4(average, average, average, CurTex.a);
			}
			ENDCG 
		}
	}	
}
}