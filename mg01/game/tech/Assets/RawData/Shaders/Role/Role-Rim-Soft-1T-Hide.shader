// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:False,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:32725,y:32712,varname:node_4883,prsc:2|emission-7801-OUT;n:type:ShaderForge.SFN_ViewVector,id:9327,x:30783,y:32694,varname:node_9327,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4406,x:30786,y:32902,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4011,x:31076,y:32792,varname:node_4011,prsc:2,dt:4|A-9327-OUT,B-4406-OUT;n:type:ShaderForge.SFN_Lerp,id:7801,x:32431,y:32752,varname:node_7801,prsc:2|A-5440-RGB,B-6216-RGB,T-5370-OUT;n:type:ShaderForge.SFN_Color,id:5440,x:31997,y:32331,ptovrint:False,ptlb:Rim-Color,ptin:_RimColor,varname:_node_5440,prsc:2,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:608,x:30883,y:33052,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:_Edge,prsc:2,min:1,cur:1.5,max:2;n:type:ShaderForge.SFN_Power,id:5370,x:31970,y:33018,varname:node_5370,prsc:2|VAL-414-OUT,EXP-3985-OUT;n:type:ShaderForge.SFN_Slider,id:3985,x:31562,y:33116,ptovrint:False,ptlb:Soft,ptin:_Soft,varname:_Soft,prsc:2,min:0.01,cur:9,max:40;n:type:ShaderForge.SFN_Multiply,id:980,x:31523,y:32904,varname:node_980,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Clamp01,id:414,x:31714,y:32914,varname:node_414,prsc:2|IN-980-OUT;n:type:ShaderForge.SFN_Color,id:6216,x:31948,y:32664,ptovrint:False,ptlb:node_6216,ptin:_node_6216,varname:node_6216,prsc:2,glob:False,c1:0,c2:0,c3:0,c4:1;proporder:5440-608-3985-6216;pass:END;sub:END;*/

Shader "EJoyShader/Role/Role-Rim-Soft-1T-Hide" {
    Properties {
        _RimColor ("Rim-Color", Color) = (1,0,0,1)
        _Edge ("Edge", Range(1, 2)) = 1.5
        _Soft ("Soft", Range(0.01, 40)) = 9
        _node_6216 ("node_6216", Color) = (0,0,0,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _RimColor;
            uniform float _Edge;
            uniform float _Soft;
            uniform float4 _node_6216;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 node_7801 = lerp(_RimColor.rgb,_node_6216.rgb,pow(saturate((0.5*dot(viewDirection,i.normalDir)+0.5*_Edge)),_Soft));
                float3 emissive = node_7801;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
