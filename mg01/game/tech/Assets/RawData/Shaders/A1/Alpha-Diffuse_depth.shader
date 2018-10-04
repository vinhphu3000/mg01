Shader "A1/Transparent/Diffuse_Depth" {
Properties {
	_AlphaColor ("Main Color", Color) = (0.8, 0.8, 0.8, 0.5)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	   	Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
	    Pass {
	    	Zwrite On
	    	ColorMask 0

	        CGPROGRAM

	        #pragma vertex vert
	        #pragma fragment frag
			#pragma only_renderers glcore gles gles3 metal d3d9 

	        float4 vert(float4 v:POSITION) : SV_POSITION {
	            return mul (UNITY_MATRIX_MVP, v);
	        }

	        fixed4 frag() : SV_Target {
	            return fixed4(0.0,0.0,0.0,0.0);
	        }

	        ENDCG
	    }

	    pass 
	    {
	        Blend SrcAlpha OneMinusSrcAlpha
	        ColorMask RGBA
	        ZWrite Off
	        ZTest LEqual

			CGPROGRAM
				#pragma vertex vert
	        	#pragma fragment frag
				#pragma only_renderers glcore gles gles3 metal d3d9 
				
				sampler2D _MainTex;
				fixed4 _AlphaColor;

		        struct appdata {
		            float4 vertex : POSITION;
		            float4 texcoord : TEXCOORD0;
		        };

		        struct v2f {
		            float4 pos : SV_POSITION;
		            float2 uv : TEXCOORD0;
		        };
		        
		        v2f vert (appdata v) {
		            v2f o;
		            o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
		            o.uv = v.texcoord.xy;
		            return o;
		        }

				half4 frag( v2f i ) : SV_Target {
					return tex2D(_MainTex, i.uv) * _AlphaColor;
				}
			ENDCG

	    }
	}
	Fallback off
}
