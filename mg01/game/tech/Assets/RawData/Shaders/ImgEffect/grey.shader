Shader "A2/grey"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma only_renderers glcore gles gles3 metal d3d9 
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 CurTex = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col = 1 - col;
				float brightness = dot(CurTex.rgb, float3(0.2126, 0.7152, 0.0722));
				float blue = saturate((brightness - 0.7));
				float average = ((0.2126 *CurTex.r + 0.7152 * CurTex.g + 0.0722 * CurTex.b));
				return float4(average + blue, average + blue, average + blue * 3 + 0.05, 1);
			}
			ENDCG
		}
	}
}
