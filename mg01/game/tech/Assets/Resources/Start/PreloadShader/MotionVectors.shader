Shader "Hidden/Internal-MotionVectors" {
	SubShader {
		Pass {
			CGPROGRAM
			// compile directives
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma only_renderers glcore gles gles3 metal d3d9 
			#include "UnityCG.cginc"

			struct app_data_ {
	            float4 vertex : POSITION;
	        };
			// no lightmaps:
			struct v2f_surf {
			  float4 pos : SV_POSITION;
			};
			

			// vertex shader
			v2f_surf vert_surf (app_data_ v) {
			  v2f_surf o;
			  UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
			  o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			  return o;
			}

			// fragment shader
			fixed4 frag_surf (v2f_surf IN) : SV_Target {		
				return fixed4(1.0, 1.0, 1.0, 1.0);
			}
			ENDCG

		}
	}
	FallBack "VertexLit"
}