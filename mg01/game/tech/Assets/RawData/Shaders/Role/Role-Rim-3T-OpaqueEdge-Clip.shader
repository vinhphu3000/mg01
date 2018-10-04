// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:False,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:32725,y:32712,varname:node_4883,prsc:2|emission-9536-OUT,clip-4042-OUT;n:type:ShaderForge.SFN_Tex2d,id:1114,x:31099,y:32341,ptovrint:False,ptlb:Bass(RGBA),ptin:_BassRGBA,varname:_BassRGB,prsc:2,tex:d1c5c311c0cb8fa4fac17e1ca4a70c9f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ViewVector,id:9327,x:30218,y:32480,varname:node_9327,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4406,x:30221,y:32688,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4011,x:30511,y:32578,varname:node_4011,prsc:2,dt:4|A-9327-OUT,B-4406-OUT;n:type:ShaderForge.SFN_Lerp,id:7801,x:31761,y:32547,varname:node_7801,prsc:2|A-9344-OUT,B-1114-RGB,T-5370-OUT;n:type:ShaderForge.SFN_Slider,id:608,x:30356,y:32890,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:_Edge,prsc:2,min:1,cur:1.3,max:2;n:type:ShaderForge.SFN_Power,id:5370,x:31467,y:32748,varname:node_5370,prsc:2|VAL-414-OUT,EXP-3985-OUT;n:type:ShaderForge.SFN_Slider,id:3985,x:30841,y:32978,ptovrint:False,ptlb:Soft,ptin:_Soft,varname:_Soft,prsc:2,min:0.01,cur:9,max:40;n:type:ShaderForge.SFN_Multiply,id:980,x:30788,y:32647,varname:node_980,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Clamp01,id:414,x:31001,y:32619,varname:node_414,prsc:2|IN-980-OUT;n:type:ShaderForge.SFN_Add,id:9344,x:31506,y:32184,varname:node_9344,prsc:2|A-3695-RGB,B-1114-RGB;n:type:ShaderForge.SFN_Add,id:7618,x:32047,y:32695,varname:node_7618,prsc:2|A-7801-OUT,B-6375-RGB;n:type:ShaderForge.SFN_Color,id:6375,x:31768,y:32810,ptovrint:False,ptlb:Light-Color,ptin:_LightColor,varname:node_6375,prsc:2,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3695,x:31105,y:32079,ptovrint:False,ptlb:Edge(RGB),ptin:_EdgeRGB,varname:node_3695,prsc:2,ntxv:0,isnm:False|UVIN-1388-OUT;n:type:ShaderForge.SFN_ScreenPos,id:42,x:30675,y:32154,varname:node_42,prsc:2,sctp:0;n:type:ShaderForge.SFN_TexCoord,id:827,x:30687,y:31979,varname:node_827,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:345,x:31873,y:33338,ptovrint:False,ptlb:Edge-Clip,ptin:_EdgeClip,varname:node_345,prsc:2,min:0,cur:5,max:15;n:type:ShaderForge.SFN_Tex2d,id:3462,x:31589,y:33001,ptovrint:False,ptlb:Mask(RGBA),ptin:_MaskRGBA,varname:node_3462,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4042,x:32307,y:33090,varname:node_4042,prsc:2|A-497-OUT,B-345-OUT,C-4357-A;n:type:ShaderForge.SFN_Add,id:497,x:32054,y:33124,varname:node_497,prsc:2|A-9963-OUT,B-7199-OUT;n:type:ShaderForge.SFN_Slider,id:7199,x:31512,y:33267,ptovrint:False,ptlb:Add-Offest,ptin:_AddOffest,varname:node_7199,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:9963,x:31826,y:33049,varname:node_9963,prsc:2|A-3462-RGB,B-3462-A;n:type:ShaderForge.SFN_SwitchProperty,id:1388,x:30894,y:32083,ptovrint:False,ptlb:Screen,ptin:_Screen,varname:node_1388,prsc:2,on:True|A-827-UVOUT,B-42-UVOUT;n:type:ShaderForge.SFN_VertexColor,id:4357,x:31995,y:32898,varname:node_4357,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9536,x:32304,y:32841,varname:node_9536,prsc:2|A-7618-OUT,B-4357-RGB;proporder:1114-3695-1388-3462-6375-608-3985-345-7199;pass:END;sub:END;*/

Shader "EJoyShader/Role/Role-Rim-3T-OpaqueEdge-Clip" {
    Properties {
        _BassRGBA ("Bass(RGBA)", 2D) = "white" {}
        _EdgeRGB ("Edge(RGB)", 2D) = "white" {}
        [MaterialToggle] _Screen ("Screen", Float ) = 0
        _MaskRGBA ("Mask(RGBA)", 2D) = "white" {}
        _LightColor ("Light-Color", Color) = (0,0,0,1)
        _Edge ("Edge", Range(1, 2)) = 1.3
        _Soft ("Soft", Range(0.01, 40)) = 9
        _EdgeClip ("Edge-Clip", Range(0, 15)) = 5
        _AddOffest ("Add-Offest", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
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
            #pragma exclude_renderers d3d11 glcore d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _BassRGBA; uniform float4 _BassRGBA_ST;
            uniform float _Edge;
            uniform float _Soft;
            uniform float4 _LightColor;
            uniform sampler2D _EdgeRGB; uniform float4 _EdgeRGB_ST;
            uniform float _EdgeClip;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform float _AddOffest;
            uniform fixed _Screen;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
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
                float4 _MaskRGBA_var = tex2D(_MaskRGBA,TRANSFORM_TEX(i.uv0, _MaskRGBA));
                clip((((_MaskRGBA_var.rgb*_MaskRGBA_var.a)+_AddOffest)*_EdgeClip*i.vertexColor.a) - 0.5);
////// Lighting:
////// Emissive:
                float2 _Screen_var = lerp( i.uv0, i.screenPos.rg, _Screen );
                float4 _EdgeRGB_var = tex2D(_EdgeRGB,TRANSFORM_TEX(_Screen_var, _EdgeRGB));
                float4 _BassRGBA_var = tex2D(_BassRGBA,TRANSFORM_TEX(i.uv0, _BassRGBA));
                float3 emissive = ((lerp((_EdgeRGB_var.rgb+_BassRGBA_var.rgb),_BassRGBA_var.rgb,pow(saturate((0.5*dot(viewDirection,i.normalDir)+0.5*_Edge)),_Soft))+_LightColor.rgb)*i.vertexColor.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 glcore d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _EdgeClip;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform float _AddOffest;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _MaskRGBA_var = tex2D(_MaskRGBA,TRANSFORM_TEX(i.uv0, _MaskRGBA));
                clip((((_MaskRGBA_var.rgb*_MaskRGBA_var.a)+_AddOffest)*_EdgeClip*i.vertexColor.a) - 0.5);
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 glcore d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _EdgeClip;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform float _AddOffest;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _MaskRGBA_var = tex2D(_MaskRGBA,TRANSFORM_TEX(i.uv0, _MaskRGBA));
                clip((((_MaskRGBA_var.rgb*_MaskRGBA_var.a)+_AddOffest)*_EdgeClip*i.vertexColor.a) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
