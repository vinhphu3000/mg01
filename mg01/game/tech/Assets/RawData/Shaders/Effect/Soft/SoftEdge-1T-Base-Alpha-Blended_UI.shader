// EjoyUnity手游项目Shader
// A2-Effect-Shader
// 此Shader用于渲染特效半透明材质
// 参数：1，带有Alpha通道的贴图一张 2，主颜色与贴图颜色为乘的关系

Shader "EJoyShader/Effect/Soft/SoftEdge-1T-Base-Alpha-Blended_UI" {
Properties {
	_Alpha ("Alpha", Range(0,1.0)) = 1.0
	_TintColor("Tint Color", Color) = (0.5,0.5,0.5,1)
	_MainTex ("Texture(RGBA)", 2D) = "white" {}
	_Clip("ClipRange", Vector) = (-10,-10,10,10)
}

Category {
	Tags { "Queue"="AlphaTest+100" "RenderType"="TransparentCutout" }
	Cull Off
	Lighting Off
	ZWrite On
	Fog{ Mode Off }
	Blend SrcAlpha OneMinusSrcAlpha

	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma only_renderers glcore gles gles3 metal d3d9
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			uniform float4 _Clip;

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 world_pos : TEXCOORD1;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.world_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			fixed _Alpha;

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 co = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
				float alpha = 1;
				alpha *= (i.world_pos.x >= _Clip.x);
				alpha *= (i.world_pos.y >= _Clip.y);
				alpha *= (i.world_pos.x <= _Clip.z);
				alpha *= (i.world_pos.y <= _Clip.w);
				co.xyz *= alpha;
				co.a = co.a * _Alpha * alpha;
				return co;
			}
			ENDCG
		}
	}
}
}
