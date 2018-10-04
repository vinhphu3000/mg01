// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:8458,x:33114,y:32725,varname:node_8458,prsc:2|spec-3085-OUT,gloss-9340-OUT,emission-7398-OUT,amspl-4839-RGB;n:type:ShaderForge.SFN_Tex2d,id:8469,x:32343,y:32363,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_8469,prsc:2,tex:8983d70cc645f5a48a50ea71ddec459e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5278,x:31910,y:32839,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_5278,prsc:2,tex:30757793edd8ec34fb1f23f67b46e9fc,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Dot,id:4459,x:31977,y:32381,varname:node_4459,prsc:2,dt:0|A-5134-OUT,B-2725-OUT;n:type:ShaderForge.SFN_NormalVector,id:2725,x:31782,y:32438,prsc:2,pt:False;n:type:ShaderForge.SFN_Cubemap,id:4839,x:32821,y:33005,ptovrint:False,ptlb:Skybox,ptin:_Skybox,varname:node_4839,prsc:2,cube:82578c95f771f7646919e0f00d659ab8,pvfc:0;n:type:ShaderForge.SFN_Desaturate,id:4322,x:32428,y:33234,varname:node_4322,prsc:2|COL-9113-OUT,DES-4565-OUT;n:type:ShaderForge.SFN_Multiply,id:9113,x:32211,y:33098,varname:node_9113,prsc:2|A-7835-OUT,B-1363-OUT;n:type:ShaderForge.SFN_Color,id:8453,x:32409,y:33033,ptovrint:False,ptlb:Re-Color,ptin:_ReColor,varname:node_8453,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:7835,x:31974,y:33052,varname:node_7835,prsc:2|A-8469-B,B-5278-RGB;n:type:ShaderForge.SFN_Multiply,id:9215,x:32645,y:33139,varname:node_9215,prsc:2|A-8453-RGB,B-4322-OUT;n:type:ShaderForge.SFN_Lerp,id:7398,x:32696,y:32936,varname:node_7398,prsc:2|A-5278-RGB,B-9215-OUT,T-8469-B;n:type:ShaderForge.SFN_Multiply,id:2055,x:32691,y:32172,varname:node_2055,prsc:2|A-2991-RGB,B-8469-R,C-9945-OUT;n:type:ShaderForge.SFN_Color,id:2991,x:32341,y:32167,ptovrint:False,ptlb:SpecularColor,ptin:_SpecularColor,varname:node_2991,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:8232,x:32944,y:32283,varname:node_8232,prsc:2|A-8469-G,B-2055-OUT;n:type:ShaderForge.SFN_Multiply,id:3085,x:32884,y:32498,varname:node_3085,prsc:2|A-8232-OUT,B-60-OUT,C-2991-RGB;n:type:ShaderForge.SFN_ViewVector,id:5134,x:31797,y:32272,varname:node_5134,prsc:2;n:type:ShaderForge.SFN_Power,id:4152,x:32140,y:32529,varname:node_4152,prsc:2|VAL-4459-OUT,EXP-3073-OUT;n:type:ShaderForge.SFN_Multiply,id:374,x:32436,y:32583,varname:node_374,prsc:2|A-4152-OUT,B-9014-OUT;n:type:ShaderForge.SFN_Clamp01,id:60,x:32660,y:32582,varname:node_60,prsc:2|IN-374-OUT;n:type:ShaderForge.SFN_Vector1,id:9014,x:32232,y:32652,varname:node_9014,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:9945,x:32425,y:32064,varname:node_9945,prsc:2,v1:0.01;n:type:ShaderForge.SFN_Vector1,id:9340,x:32729,y:32765,varname:node_9340,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Vector1,id:4565,x:32113,y:33426,varname:node_4565,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:1363,x:31975,y:33220,varname:node_1363,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:3073,x:31928,y:32580,varname:node_3073,prsc:2,v1:5;proporder:5278-8469-4839-8453-2991;pass:END;sub:END;*/

Shader "EJoyShader/ReColor/MetalMesh-ReColor-2T(Op)" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Skybox ("Skybox", Cube) = "_Skybox" {}
        _ReColor ("Re-Color", Color) = (1,1,1,1)
        _SpecularColor ("SpecularColor", Color) = (0.5,0.5,0.5,1)
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
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform samplerCUBE _Skybox;
            uniform float4 _ReColor;
            uniform float4 _SpecularColor;
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
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.4;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Skybox_var = texCUBE(_Skybox,viewReflectDirection);
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float3 specularColor = ((_Mask_var.g+(_SpecularColor.rgb*_Mask_var.r*0.01))*saturate((pow(dot(viewDirection,i.normalDir),5.0)*3.0))*_SpecularColor.rgb);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 indirectSpecular = (0 + _Skybox_var.rgb)*specularColor;
                float3 specular = (directSpecular + indirectSpecular);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 emissive = lerp(_Diffuse_var.rgb,(_ReColor.rgb*lerp(((_Mask_var.b*_Diffuse_var.rgb)*2.0),dot(((_Mask_var.b*_Diffuse_var.rgb)*2.0),float3(0.3,0.59,0.11)),1.0)),_Mask_var.b);
/// Final Color:
                float3 finalColor = specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _ReColor;
            uniform float4 _SpecularColor;
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
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.4;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float3 specularColor = ((_Mask_var.g+(_SpecularColor.rgb*_Mask_var.r*0.01))*saturate((pow(dot(viewDirection,i.normalDir),5.0)*3.0))*_SpecularColor.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/// Final Color:
                float3 finalColor = specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
