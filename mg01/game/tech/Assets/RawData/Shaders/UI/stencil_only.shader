Shader "Hidden/UI/Stencil"
{
	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Transparent" }

		Stencil
		{
			Ref 2
			Comp Always
			Pass Replace
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always
		Fog { Mode Off }
		ColorMask 0
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
			};


			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				return fixed4(0.0, 0.0, 0.0, 1.0);
			}
		ENDCG
		}
	}
}
