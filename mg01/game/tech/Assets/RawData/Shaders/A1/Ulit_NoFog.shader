Shader "A1/Unlit/Texture" {
    Properties {
        _BaseMap ("BaseMap", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "Queue"="Geometry+1"
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers glcore gles gles3 metal d3d9 
            uniform sampler2D _BaseMap; uniform float4 _BaseMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
				return tex2D(_BaseMap,TRANSFORM_TEX(i.uv0, _BaseMap));
            }
            ENDCG
        }
    }
}
