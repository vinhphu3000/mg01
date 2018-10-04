// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:False,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:32725,y:32712,varname:node_4883,prsc:2|emission-7618-OUT;n:type:ShaderForge.SFN_Tex2d,id:1114,x:31474,y:32431,ptovrint:False,ptlb:Bass(RGBA),ptin:_BassRGBA,varname:_BassRGB,prsc:2,tex:d1c5c311c0cb8fa4fac17e1ca4a70c9f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ViewVector,id:9327,x:30874,y:32640,varname:node_9327,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4406,x:30877,y:32848,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4011,x:31167,y:32738,varname:node_4011,prsc:2,dt:4|A-9327-OUT,B-4406-OUT;n:type:ShaderForge.SFN_Lerp,id:7801,x:32227,y:32617,varname:node_7801,prsc:2|A-3695-RGB,B-6880-OUT,T-5370-OUT;n:type:ShaderForge.SFN_Slider,id:608,x:31012,y:33050,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:_Edge,prsc:2,min:1,cur:1.3,max:2;n:type:ShaderForge.SFN_Power,id:5370,x:32005,y:32820,varname:node_5370,prsc:2|VAL-414-OUT,EXP-3985-OUT;n:type:ShaderForge.SFN_Slider,id:3985,x:31485,y:33055,ptovrint:False,ptlb:Soft,ptin:_Soft,varname:_Soft,prsc:2,min:0.01,cur:9,max:40;n:type:ShaderForge.SFN_Multiply,id:980,x:31444,y:32807,varname:node_980,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Clamp01,id:414,x:31657,y:32779,varname:node_414,prsc:2|IN-980-OUT;n:type:ShaderForge.SFN_Add,id:7618,x:32447,y:32814,varname:node_7618,prsc:2|A-7801-OUT,B-6375-RGB;n:type:ShaderForge.SFN_Color,id:6375,x:32168,y:32929,ptovrint:False,ptlb:Light-Color,ptin:_LightColor,varname:node_6375,prsc:2,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3695,x:32166,y:32067,ptovrint:False,ptlb:Edge(RGB),ptin:_EdgeRGB,varname:node_3695,prsc:2,ntxv:0,isnm:False|UVIN-1388-OUT;n:type:ShaderForge.SFN_ScreenPos,id:42,x:31724,y:32045,varname:node_42,prsc:2,sctp:0;n:type:ShaderForge.SFN_TexCoord,id:827,x:31726,y:31783,varname:node_827,prsc:2,uv:0;n:type:ShaderForge.SFN_SwitchProperty,id:1388,x:31982,y:31987,ptovrint:False,ptlb:Screen,ptin:_Screen,varname:node_1388,prsc:2,on:True|A-827-UVOUT,B-42-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:7059,x:31033,y:32356,varname:node_7059,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:9108,x:31578,y:32647,ptovrint:False,ptlb:Dark(RGB),ptin:_DarkRGB,varname:_BassRGBA_copy,prsc:2,tex:d1c5c311c0cb8fa4fac17e1ca4a70c9f,ntxv:0,isnm:False|UVIN-5923-OUT;n:type:ShaderForge.SFN_Add,id:6880,x:31823,y:32554,varname:node_6880,prsc:2|A-6551-OUT,B-9108-RGB;n:type:ShaderForge.SFN_Multiply,id:6551,x:31711,y:32321,varname:node_6551,prsc:2|A-4743-RGB,B-1114-RGB;n:type:ShaderForge.SFN_Color,id:4743,x:31487,y:32233,ptovrint:False,ptlb:BaseColor,ptin:_BaseColor,varname:node_4743,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ScreenPos,id:2347,x:31023,y:32538,varname:node_2347,prsc:2,sctp:0;n:type:ShaderForge.SFN_SwitchProperty,id:5923,x:31230,y:32483,ptovrint:False,ptlb:Screen-Dark,ptin:_ScreenDark,varname:node_5923,prsc:2,on:True|A-7059-UVOUT,B-2347-UVOUT;proporder:1114-4743-9108-5923-3695-1388-6375-608-3985;pass:END;sub:END;*/

Shader "EJoyShader/Role/Role-Rim-3T-OpaqueEdge-BaseMul" {
    Properties {
        _BassRGBA ("Bass(RGBA)", 2D) = "white" {}
        _BaseColor ("BaseColor", Color) = (0.5,0.5,0.5,1)
        _DarkRGB ("Dark(RGB)", 2D) = "white" {}
        [MaterialToggle] _ScreenDark ("Screen-Dark", Float ) = 0
        _EdgeRGB ("Edge(RGB)", 2D) = "white" {}
        [MaterialToggle] _Screen ("Screen", Float ) = 0
        _LightColor ("Light-Color", Color) = (0,0,0,1)
        _Edge ("Edge", Range(1, 2)) = 1.3
        _Soft ("Soft", Range(0.01, 40)) = 9
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
            #pragma exclude_renderers d3d11 glcore d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _BassRGBA; uniform float4 _BassRGBA_ST;
            uniform float _Edge;
            uniform float _Soft;
            uniform float4 _LightColor;
            uniform sampler2D _EdgeRGB; uniform float4 _EdgeRGB_ST;
            uniform fixed _Screen;
            uniform sampler2D _DarkRGB; uniform float4 _DarkRGB_ST;
            uniform float4 _BaseColor;
            uniform fixed _ScreenDark;
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
                float4 screenPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float2 _Screen_var = lerp( i.uv0, i.screenPos.rg, _Screen );
                float4 _EdgeRGB_var = tex2D(_EdgeRGB,TRANSFORM_TEX(_Screen_var, _EdgeRGB));
                float4 _BassRGBA_var = tex2D(_BassRGBA,TRANSFORM_TEX(i.uv0, _BassRGBA));
                float2 _ScreenDark_var = lerp( i.uv0, i.screenPos.rg, _ScreenDark );
                float4 _DarkRGB_var = tex2D(_DarkRGB,TRANSFORM_TEX(_ScreenDark_var, _DarkRGB));
                float3 emissive = (lerp(_EdgeRGB_var.rgb,((_BaseColor.rgb*_BassRGBA_var.rgb)+_DarkRGB_var.rgb),pow(saturate((0.5*dot(viewDirection,i.normalDir)+0.5*_Edge)),_Soft))+_LightColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
