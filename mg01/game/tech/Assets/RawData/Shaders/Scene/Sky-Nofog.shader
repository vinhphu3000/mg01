// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.8308824,fgcg:0.2871431,fgcb:0.2871431,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:7020,x:33035,y:32701,varname:node_7020,prsc:2|emission-336-OUT;n:type:ShaderForge.SFN_Tex2d,id:6513,x:32334,y:32621,ptovrint:False,ptlb:BaseMap(RGB),ptin:_BaseMapRGB,varname:_node_6513,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:336,x:32682,y:32850,varname:node_336,prsc:2|A-6513-RGB,B-1057-RGB;n:type:ShaderForge.SFN_VertexColor,id:1057,x:32362,y:32994,varname:node_1057,prsc:2;proporder:6513;pass:END;sub:END;*/

Shader "EJoyShader/Scene/Sky-Nofog" {
    Properties {
        _BaseMapRGB ("BaseMap(RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BaseMapRGB; uniform float4 _BaseMapRGB_ST;
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
                float4 _BaseMapRGB_var = tex2D(_BaseMapRGB,TRANSFORM_TEX(i.uv0, _BaseMapRGB));
                float3 emissive = (_BaseMapRGB_var.rgb*i.vertexColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
