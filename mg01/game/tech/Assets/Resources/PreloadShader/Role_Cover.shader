Shader "LP/Role/X-Ray" {
    Properties {
        _CoverColor ("Cover-Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest+11"
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            //Blend SrcAlpha OneMinusSrcAlpha
			Blend SrcAlpha One    
			ZTest Greater
            ZWrite Off
            Stencil {
                Ref 0
                Comp Equal
                Pass Keep 
                Fail keep 
                ZFail keep
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #pragma only_renderers glcore gles gles3 metal d3d9

            uniform float4 _CoverColor;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return _CoverColor;
            }
            ENDCG
        }
    }
    FallBack Off
}
