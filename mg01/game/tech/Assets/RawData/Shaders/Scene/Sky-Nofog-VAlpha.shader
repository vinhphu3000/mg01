// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:0,wrdp:False,dith:0,ufog:False,aust:False,igpj:True,qofs:1,qpre:1,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.8308824,fgcg:0.2871431,fgcb:0.2871431,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:7020,x:33035,y:32701,varname:node_7020,prsc:2|emission-336-OUT;n:type:ShaderForge.SFN_Tex2d,id:6513,x:32388,y:32638,ptovrint:False,ptlb:BaseMap,ptin:_BaseMap,varname:_BaseMap,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:336,x:32682,y:32850,varname:node_336,prsc:2|A-6513-RGB,B-1057-RGB,C-1057-A;n:type:ShaderForge.SFN_VertexColor,id:1057,x:32391,y:33101,varname:node_1057,prsc:2;proporder:6513;pass:END;sub:END;*/

Shader "EJoyShader/Scene/Sky-Nofog-VAlpha" {
    Properties {
        _BaseMap ("BaseMap", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Geometry+1"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Less
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BaseMap; uniform float4 _BaseMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _BaseMap_var = tex2D(_BaseMap,TRANSFORM_TEX(i.uv0, _BaseMap));
                float3 emissive = (_BaseMap_var.rgb*i.vertexColor.rgb*i.vertexColor.a);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
