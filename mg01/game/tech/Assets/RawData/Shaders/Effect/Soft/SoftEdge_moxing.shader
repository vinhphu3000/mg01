// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'


Shader "EJoyShader/Effect/Soft/moxing" {
    Properties {
        _color ("color", Color) = (0.5,0.5,0.5,0.5)
        _fresnel_QD ("fresnel_QD", Float ) = 4
        _node_4788 ("node_4788", Float ) = 5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers glcore gles gles3 metal d3d9
            #pragma target 3.0
            uniform float4 _color;
            uniform float _fresnel_QD;
            uniform float _node_4788;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = ((_color.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),_fresnel_QD))*_node_4788);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,i.vertexColor.a);
                return finalRGBA;
            }
            ENDCG
        }
    }
}
