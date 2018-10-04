Shader "Spine/Skeleton"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Clip("ClipRange", Vector) = (-10,-10,10,10)

	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float3 world_pos : TEXCOORD1;
			};
			
			fixed4 _Color;
			uniform float4 _Clip;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;

				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
				#endif

				OUT.color = IN.color * _Color;
				OUT.world_pos = mul(unity_ObjectToWorld, IN.vertex).xyz;

				return OUT;
			}

			sampler2D _MainTex;

			float4 frag(v2f IN) : SV_Target
			{
				float4 color = tex2D(_MainTex, IN.texcoord) * IN.color;

				float alpha = 1;
				alpha *= (IN.world_pos.x >= _Clip.x);
				alpha *= (IN.world_pos.y >= _Clip.y);
				alpha *= (IN.world_pos.x <= _Clip.z);
				alpha *= (IN.world_pos.y <= _Clip.w);
				color.xyzw *= alpha;
				return color;
			}
		ENDCG
		}
	}
}