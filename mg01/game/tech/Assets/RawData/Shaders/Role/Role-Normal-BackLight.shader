// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:2,limd:2,spmd:0,grmd:1,uamb:False,mssp:True,bkdf:True,rprd:True,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:34126,y:32854,varname:node_4883,prsc:2|normal-1780-RGB,emission-5173-OUT;n:type:ShaderForge.SFN_Tex2d,id:8436,x:31790,y:32177,ptovrint:False,ptlb:Base(RGB),ptin:_BaseRGB,varname:node_8436,prsc:2,tex:7ada42ba72b935b4fbc9399b82947913,ntxv:0,isnm:False;n:type:ShaderForge.SFN_NormalVector,id:3432,x:30866,y:32797,prsc:2,pt:False;n:type:ShaderForge.SFN_Color,id:3389,x:31790,y:32403,ptovrint:False,ptlb:LightColor,ptin:_LightColor,varname:node_3389,prsc:2,glob:False,c1:0.7720588,c2:0.1646302,c3:0.1646302,c4:1;n:type:ShaderForge.SFN_Lerp,id:4515,x:32699,y:32838,varname:node_4515,prsc:2|A-2553-OUT,B-8961-OUT,T-7976-OUT;n:type:ShaderForge.SFN_Color,id:1890,x:31780,y:31977,ptovrint:False,ptlb:DarkColor,ptin:_DarkColor,varname:_MainColor_copy,prsc:2,glob:False,c1:0.7720588,c2:0.1646302,c3:0.1646302,c4:1;n:type:ShaderForge.SFN_Multiply,id:2553,x:32183,y:32113,varname:node_2553,prsc:2|A-8436-RGB,B-1890-RGB;n:type:ShaderForge.SFN_Power,id:2546,x:31907,y:32791,varname:node_2546,prsc:2|VAL-8679-OUT,EXP-3904-OUT;n:type:ShaderForge.SFN_Multiply,id:2458,x:32203,y:32748,cmnt:SpecularColor,varname:node_2458,prsc:2|A-236-B,B-2546-OUT,C-1394-OUT,D-1383-OUT;n:type:ShaderForge.SFN_Add,id:8961,x:32465,y:32809,cmnt:Light,varname:node_8961,prsc:2|A-8647-OUT,B-2458-OUT,C-4893-OUT;n:type:ShaderForge.SFN_Slider,id:3904,x:31236,y:32552,ptovrint:False,ptlb:Specular-Power,ptin:_SpecularPower,varname:node_3904,prsc:2,min:0,cur:34.40505,max:100;n:type:ShaderForge.SFN_Dot,id:8679,x:31468,y:32801,varname:node_8679,prsc:2,dt:4|A-8263-OUT,B-2629-OUT;n:type:ShaderForge.SFN_ViewVector,id:8263,x:31146,y:32660,varname:node_8263,prsc:2;n:type:ShaderForge.SFN_Vector4Property,id:4351,x:32413,y:31816,ptovrint:False,ptlb:BackLightDir,ptin:_BackLightDir,varname:_LightDir_copy,prsc:2,glob:False,v1:-0.18,v2:0.9,v3:5.05,v4:0;n:type:ShaderForge.SFN_Add,id:8647,x:32183,y:32261,varname:node_8647,prsc:2|A-8436-RGB,B-3389-RGB;n:type:ShaderForge.SFN_Slider,id:1383,x:31236,y:32413,ptovrint:False,ptlb:Specular-Multiply,ptin:_SpecularMultiply,varname:node_1383,prsc:2,min:0,cur:10,max:10;n:type:ShaderForge.SFN_Cubemap,id:9495,x:31348,y:31964,ptovrint:False,ptlb:Env-Map,ptin:_EnvMap,varname:node_9495,prsc:2,cube:91ad58223cce30442acb6799c36ac77f,pvfc:0;n:type:ShaderForge.SFN_Multiply,id:4893,x:32183,y:32414,cmnt:Env,varname:node_4893,prsc:2|A-9495-RGB,B-6218-OUT,C-236-R;n:type:ShaderForge.SFN_Slider,id:6218,x:31236,y:32268,ptovrint:False,ptlb:Env-Multiply,ptin:_EnvMultiply,varname:node_6218,prsc:2,min:0,cur:0.5,max:2;n:type:ShaderForge.SFN_Color,id:4042,x:31804,y:32584,ptovrint:False,ptlb:SpecularColor,ptin:_SpecularColor,varname:_Ambient_copy,prsc:2,glob:False,c1:0.7720588,c2:0.1646302,c3:0.1646302,c4:1;n:type:ShaderForge.SFN_Multiply,id:1394,x:32169,y:32576,varname:node_1394,prsc:2|A-4042-RGB,B-8436-RGB;n:type:ShaderForge.SFN_Lerp,id:5173,x:33564,y:32847,varname:node_5173,prsc:2|A-1914-OUT,B-2867-OUT,T-598-OUT;n:type:ShaderForge.SFN_Color,id:8013,x:32673,y:32456,ptovrint:False,ptlb:BackLightColor,ptin:_BackLightColor,varname:node_8013,prsc:2,glob:False,c1:0.5940744,c2:0.9852941,c3:0.9852941,c4:1;n:type:ShaderForge.SFN_Add,id:441,x:32921,y:32605,varname:node_441,prsc:2|A-4515-OUT,B-8013-RGB;n:type:ShaderForge.SFN_Power,id:3640,x:33613,y:32216,varname:node_3640,prsc:2|VAL-1305-OUT,EXP-7297-OUT;n:type:ShaderForge.SFN_Add,id:1305,x:33557,y:31929,varname:node_1305,prsc:2|A-8655-OUT,B-50-OUT;n:type:ShaderForge.SFN_Clamp01,id:570,x:33877,y:32416,varname:node_570,prsc:2|IN-3640-OUT;n:type:ShaderForge.SFN_Blend,id:2867,x:33177,y:32658,varname:node_2867,prsc:2,blmd:14,clmp:True|SRC-8013-RGB,DST-4515-OUT;n:type:ShaderForge.SFN_AmbientLight,id:2984,x:32364,y:33124,varname:node_2984,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4084,x:33034,y:32876,varname:node_4084,prsc:2|A-4515-OUT,B-2984-RGB,C-131-OUT;n:type:ShaderForge.SFN_Tex2d,id:236,x:31790,y:31774,ptovrint:False,ptlb:Mask(RGB),ptin:_MaskRGB,varname:node_236,prsc:2,tex:a5ffb2c8b7398b14888950ec585eb619,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Multiply,id:598,x:33627,y:32610,varname:node_598,prsc:2|A-570-OUT,B-236-G;n:type:ShaderForge.SFN_Dot,id:8655,x:33181,y:32027,varname:node_8655,prsc:2,dt:1|A-8118-OUT,B-2629-OUT;n:type:ShaderForge.SFN_ViewVector,id:6590,x:32627,y:31634,varname:node_6590,prsc:2;n:type:ShaderForge.SFN_Normalize,id:8118,x:32872,y:31929,varname:node_8118,prsc:2|IN-4964-OUT;n:type:ShaderForge.SFN_Normalize,id:9652,x:32627,y:31816,varname:node_9652,prsc:2|IN-4351-XYZ;n:type:ShaderForge.SFN_Cross,id:4964,x:32872,y:31742,varname:node_4964,prsc:2|A-9652-OUT,B-6590-OUT;n:type:ShaderForge.SFN_Slider,id:50,x:33068,y:32258,ptovrint:False,ptlb:BL-Offse,ptin:_BLOffse,varname:node_50,prsc:2,min:-1,cur:0.3,max:1;n:type:ShaderForge.SFN_NormalVector,id:1574,x:32872,y:32168,prsc:2,pt:False;n:type:ShaderForge.SFN_Slider,id:7297,x:33068,y:32399,ptovrint:False,ptlb:BL-Soft,ptin:_BLSoft,varname:_Li_copy,prsc:2,min:0,cur:6,max:60;n:type:ShaderForge.SFN_Multiply,id:2629,x:31173,y:32971,varname:node_2629,prsc:2|A-3432-OUT,B-1780-RGB;n:type:ShaderForge.SFN_Tex2d,id:1780,x:30876,y:33149,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:_NormalMap_copy,prsc:2,tex:e933ffa8bbd82854bb703d732f78702e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:131,x:32563,y:33278,ptovrint:False,ptlb:Light,ptin:_Light,varname:node_131,prsc:2,min:0.5,cur:1,max:3;n:type:ShaderForge.SFN_Power,id:1914,x:33319,y:32847,varname:node_1914,prsc:2|VAL-4084-OUT,EXP-6482-OUT;n:type:ShaderForge.SFN_Slider,id:6482,x:32873,y:33312,ptovrint:False,ptlb:Power,ptin:_Power,varname:_Light_copy,prsc:2,min:0.5,cur:1,max:1.5;n:type:ShaderForge.SFN_Clamp01,id:3200,x:31922,y:32929,varname:node_3200,prsc:2|IN-8679-OUT;n:type:ShaderForge.SFN_Add,id:7976,x:32282,y:32956,varname:node_7976,prsc:2|A-3200-OUT,B-9982-OUT;n:type:ShaderForge.SFN_Slider,id:9982,x:31719,y:33229,ptovrint:False,ptlb:LD-Edge,ptin:_LDEdge,varname:node_9982,prsc:2,min:-0.2,cur:0,max:0.2;proporder:8436-1780-236-9495-6218-3389-1890-4042-3904-1383-8013-4351-50-7297-131-6482-9982;pass:END;sub:END;*/

Shader "EJoyShader/Role/Role-Normal-BackLight" {
    Properties {
        _BaseRGB ("Base(RGB)", 2D) = "white" {}
        _NormalMap ("NormalMap", 2D) = "white" {}
        _MaskRGB ("Mask(RGB)", 2D) = "black" {}
        _EnvMap ("Env-Map", Cube) = "_Skybox" {}
        _EnvMultiply ("Env-Multiply", Range(0, 2)) = 0.5
        _LightColor ("LightColor", Color) = (0.7720588,0.1646302,0.1646302,1)
        _DarkColor ("DarkColor", Color) = (0.7720588,0.1646302,0.1646302,1)
        _SpecularColor ("SpecularColor", Color) = (0.7720588,0.1646302,0.1646302,1)
        _SpecularPower ("Specular-Power", Range(0, 100)) = 34.40505
        _SpecularMultiply ("Specular-Multiply", Range(0, 10)) = 10
        _BackLightColor ("BackLightColor", Color) = (0.5940744,0.9852941,0.9852941,1)
        _BackLightDir ("BackLightDir", Vector) = (-0.18,0.9,5.05,0)
        _BLOffse ("BL-Offse", Range(-1, 1)) = 0.3
        _BLSoft ("BL-Soft", Range(0, 60)) = 6
        _Light ("Light", Range(0.5, 3)) = 1
        _Power ("Power", Range(0.5, 1.5)) = 1
        _LDEdge ("LD-Edge", Range(-0.2, 0.2)) = 0
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BaseRGB; uniform float4 _BaseRGB_ST;
            uniform float4 _LightColor;
            uniform float4 _DarkColor;
            uniform float _SpecularPower;
            uniform float4 _BackLightDir;
            uniform float _SpecularMultiply;
            uniform samplerCUBE _EnvMap;
            uniform float _EnvMultiply;
            uniform float4 _SpecularColor;
            uniform float4 _BackLightColor;
            uniform sampler2D _MaskRGB; uniform float4 _MaskRGB_ST;
            uniform float _BLOffse;
            uniform float _BLSoft;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Light;
            uniform float _Power;
            uniform float _LDEdge;
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
                float4 _NormalMap_var = tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap));
                float3 normalDirection = _NormalMap_var.rgb;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
////// Lighting:
////// Emissive:
                float4 _BaseRGB_var = tex2D(_BaseRGB,TRANSFORM_TEX(i.uv0, _BaseRGB));
                float4 _MaskRGB_var = tex2D(_MaskRGB,TRANSFORM_TEX(i.uv0, _MaskRGB));
                float3 node_2629 = (i.normalDir*_NormalMap_var.rgb);
                float node_8679 = 0.5*dot(viewDirection,node_2629)+0.5;
                float3 node_4515 = lerp((_BaseRGB_var.rgb*_DarkColor.rgb),((_BaseRGB_var.rgb+_LightColor.rgb)+(_MaskRGB_var.b*pow(node_8679,_SpecularPower)*(_SpecularColor.rgb*_BaseRGB_var.rgb)*_SpecularMultiply)+(texCUBE(_EnvMap,viewReflectDirection).rgb*_EnvMultiply*_MaskRGB_var.r)),(saturate(node_8679)+_LDEdge));
                float3 emissive = lerp(pow((node_4515*UNITY_LIGHTMODEL_AMBIENT.rgb*_Light),_Power),saturate(( _BackLightColor.rgb > 0.5 ? (node_4515 + 2.0*_BackLightColor.rgb -1.0) : (node_4515 + 2.0*(_BackLightColor.rgb-0.5)))),(saturate(pow((max(0,dot(normalize(cross(normalize(_BackLightDir.rgb),viewDirection)),node_2629))+_BLOffse),_BLSoft))*_MaskRGB_var.g));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BaseRGB; uniform float4 _BaseRGB_ST;
            uniform float4 _LightColor;
            uniform float4 _DarkColor;
            uniform float _SpecularPower;
            uniform float4 _BackLightDir;
            uniform float _SpecularMultiply;
            uniform samplerCUBE _EnvMap;
            uniform float _EnvMultiply;
            uniform float4 _SpecularColor;
            uniform float4 _BackLightColor;
            uniform sampler2D _MaskRGB; uniform float4 _MaskRGB_ST;
            uniform float _BLOffse;
            uniform float _BLSoft;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Light;
            uniform float _Power;
            uniform float _LDEdge;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 _BaseRGB_var = tex2D(_BaseRGB,TRANSFORM_TEX(i.uv0, _BaseRGB));
                float4 _MaskRGB_var = tex2D(_MaskRGB,TRANSFORM_TEX(i.uv0, _MaskRGB));
                float4 _NormalMap_var = tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap));
                float3 node_2629 = (i.normalDir*_NormalMap_var.rgb);
                float node_8679 = 0.5*dot(viewDirection,node_2629)+0.5;
                float3 node_4515 = lerp((_BaseRGB_var.rgb*_DarkColor.rgb),((_BaseRGB_var.rgb+_LightColor.rgb)+(_MaskRGB_var.b*pow(node_8679,_SpecularPower)*(_SpecularColor.rgb*_BaseRGB_var.rgb)*_SpecularMultiply)+(texCUBE(_EnvMap,viewReflectDirection).rgb*_EnvMultiply*_MaskRGB_var.r)),(saturate(node_8679)+_LDEdge));
                o.Emission = lerp(pow((node_4515*UNITY_LIGHTMODEL_AMBIENT.rgb*_Light),_Power),saturate(( _BackLightColor.rgb > 0.5 ? (node_4515 + 2.0*_BackLightColor.rgb -1.0) : (node_4515 + 2.0*(_BackLightColor.rgb-0.5)))),(saturate(pow((max(0,dot(normalize(cross(normalize(_BackLightDir.rgb),viewDirection)),node_2629))+_BLOffse),_BLSoft))*_MaskRGB_var.g));
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
