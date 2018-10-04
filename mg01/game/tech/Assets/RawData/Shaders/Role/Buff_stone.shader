// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:False,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:32725,y:32712,varname:node_4883,prsc:2|emission-1056-OUT;n:type:ShaderForge.SFN_Tex2d,id:1114,x:30935,y:32220,ptovrint:False,ptlb:Bass-RGB,ptin:_MainTex,varname:_MainTex,prsc:2,tex:c22cfa2b2a5942f49a2636642a213f65,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ViewVector,id:9327,x:30055,y:32662,varname:node_9327,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4406,x:30058,y:32870,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4011,x:30348,y:32760,varname:node_4011,prsc:2,dt:4|A-9327-OUT,B-4406-OUT;n:type:ShaderForge.SFN_Lerp,id:7801,x:31913,y:32750,varname:node_7801,prsc:2|A-9613-OUT,B-6440-OUT,T-5370-OUT;n:type:ShaderForge.SFN_Color,id:5440,x:31538,y:32131,ptovrint:False,ptlb:Rim-Color,ptin:_RimColor,varname:_node_5440,prsc:2,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:608,x:30365,y:33050,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:_Edge,prsc:2,min:1,cur:1.5,max:2;n:type:ShaderForge.SFN_Power,id:5370,x:31452,y:33016,varname:node_5370,prsc:2|VAL-414-OUT,EXP-3985-OUT;n:type:ShaderForge.SFN_Slider,id:3985,x:31044,y:33114,ptovrint:False,ptlb:Soft,ptin:_Soft,varname:_Soft,prsc:2,min:0.01,cur:9,max:40;n:type:ShaderForge.SFN_Multiply,id:980,x:31005,y:32902,varname:node_980,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Clamp01,id:414,x:31196,y:32912,varname:node_414,prsc:2|IN-980-OUT;n:type:ShaderForge.SFN_Add,id:9344,x:31539,y:32368,varname:node_9344,prsc:2|A-1114-RGB,B-8039-RGB;n:type:ShaderForge.SFN_Tex2d,id:8039,x:30925,y:32505,ptovrint:False,ptlb:DarkMap（RGB）,ptin:_DarkMapRGB,varname:node_8039,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Desaturate,id:1056,x:32312,y:32783,varname:node_1056,prsc:2|COL-7801-OUT,DES-5855-OUT;n:type:ShaderForge.SFN_Slider,id:5855,x:31885,y:32943,ptovrint:False,ptlb:Gray,ptin:_Gray,varname:node_5855,prsc:2,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_Lerp,id:6440,x:31450,y:32735,varname:node_6440,prsc:2|A-1114-RGB,B-8039-R,T-5959-OUT;n:type:ShaderForge.SFN_Slider,id:5959,x:31039,y:32826,ptovrint:False,ptlb:Lerp,ptin:_Lerp,varname:node_5959,prsc:2,min:0,cur:0,max:0.75;n:type:ShaderForge.SFN_Multiply,id:9613,x:31793,y:32184,varname:node_9613,prsc:2|A-5440-RGB,B-9344-OUT;proporder:1114-5440-608-3985-8039-5855-5959;pass:END;sub:END;*/

Shader "A2/Buff_Stone" {
    Properties {
        _MainTex ("Bass-RGB", 2D) = "white" {}
        _RimColor ("Rim-Color", Color) = (1,0,0,1)
        _Edge ("Edge", Range(1, 2)) = 1.5
        _Soft ("Soft", Range(0.01, 40)) = 9
        _DarkMapRGB ("DarkMap（RGB）", 2D) = "white" {}
        _Gray ("Gray", Range(0, 1)) = 0.2
        _Lerp ("Lerp", Range(0, 0.75)) = 0
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
            #pragma multi_compile_fog
			#pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _RimColor;
            uniform float _Edge;
            uniform float _Soft;
            uniform sampler2D _DarkMapRGB; uniform float4 _DarkMapRGB_ST;
            uniform float _Gray;
            uniform float _Lerp;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _DarkMapRGB_var = tex2D(_DarkMapRGB,TRANSFORM_TEX(i.uv0, _DarkMapRGB));
                float3 emissive = lerp(lerp((_RimColor.rgb*(_MainTex_var.rgb+_DarkMapRGB_var.rgb)),lerp(_MainTex_var.rgb,float3(_DarkMapRGB_var.r,_DarkMapRGB_var.r,_DarkMapRGB_var.r),_Lerp),pow(saturate((0.5*dot(viewDirection,i.normalDir)+0.5*_Edge)),_Soft)),dot(lerp((_RimColor.rgb*(_MainTex_var.rgb+_DarkMapRGB_var.rgb)),lerp(_MainTex_var.rgb,float3(_DarkMapRGB_var.r,_DarkMapRGB_var.r,_DarkMapRGB_var.r),_Lerp),pow(saturate((0.5*dot(viewDirection,i.normalDir)+0.5*_Edge)),_Soft)),float3(0.3,0.59,0.11)),_Gray);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}
