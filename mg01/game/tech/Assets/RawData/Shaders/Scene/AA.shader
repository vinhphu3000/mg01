// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:True,bkdf:True,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:6029,x:32895,y:32707,varname:node_6029,prsc:2|diff-5650-OUT,alpha-6188-OUT;n:type:ShaderForge.SFN_Multiply,id:5650,x:32459,y:32766,varname:node_5650,prsc:2|A-6736-OUT,B-8614-RGB;n:type:ShaderForge.SFN_Slider,id:6188,x:32078,y:33114,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:node_6188,prsc:2,min:0,cur:0.6230641,max:1;n:type:ShaderForge.SFN_Tex2d,id:4053,x:31869,y:32439,ptovrint:False,ptlb:BaseTexture1,ptin:_BaseTexture1,varname:node_4053,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:8614,x:32172,y:32885,ptovrint:False,ptlb:BaseColor,ptin:_BaseColor,varname:node_8614,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:6736,x:32274,y:32591,varname:node_6736,prsc:2|A-4053-RGB,B-9063-RGB,T-1149-OUT;n:type:ShaderForge.SFN_Tex2d,id:9063,x:31869,y:32667,ptovrint:False,ptlb:BaseTexture2,ptin:_BaseTexture2,varname:_BaseTexture2,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:1149,x:31769,y:32884,ptovrint:False,ptlb:TextLerp,ptin:_TextLerp,varname:node_1149,prsc:2,min:0,cur:0,max:1;proporder:4053-9063-8614-6188-1149;pass:END;sub:END;*/

Shader "EJoyShader/Scene/Test" {
    Properties {
        _BaseTexture1 ("BaseTexture1", 2D) = "white" {}
        _BaseTexture2 ("BaseTexture2", 2D) = "white" {}
        _BaseColor ("BaseColor", Color) = (0.5,0.5,0.5,1)
        _Alpha ("Alpha", Range(0, 1)) = 0.6230641
        _TextLerp ("TextLerp", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform float _Alpha;
            uniform sampler2D _BaseTexture1; uniform float4 _BaseTexture1_ST;
            uniform float4 _BaseColor;
            uniform sampler2D _BaseTexture2; uniform float4 _BaseTexture2_ST;
            uniform float _TextLerp;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                UNITY_FOG_COORDS(7)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD8;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
            #endif
            #ifdef DYNAMICLIGHTMAP_ON
                o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
            #endif
            o.normalDir = UnityObjectToWorldNormal(v.normal);
            o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
            o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
            o.posWorld = mul(unity_ObjectToWorld, v.vertex);
            float3 lightColor = _LightColor0.rgb;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            UNITY_TRANSFER_FOG(o,o.pos);
            return o;
        }
        float4 frag(VertexOutput i) : COLOR {
            i.normalDir = normalize(i.normalDir);
            float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            float3 normalDirection = i.normalDir;
            float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            float3 lightColor = _LightColor0.rgb;
// Lighting:
            float attenuation = 1;
            float3 attenColor = attenuation * _LightColor0.xyz;
/// GI Data:
            UnityLight light;
            #ifdef LIGHTMAP_OFF
                light.color = lightColor;
                light.dir = lightDirection;
                light.ndotl = LambertTerm (normalDirection, light.dir);
            #else
                light.color = half3(0.f, 0.f, 0.f);
                light.ndotl = 0.0f;
                light.dir = half3(0.f, 0.f, 0.f);
            #endif
            UnityGIInput d;
            d.light = light;
            d.worldPos = i.posWorld.xyz;
            d.worldViewDir = viewDirection;
            d.atten = attenuation;
            #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                d.ambient = 0;
                d.lightmapUV = i.ambientOrLightmapUV;
            #else
                d.ambient = i.ambientOrLightmapUV;
            #endif
            UnityGI gi = UnityGlobalIllumination (d, 1, 0, normalDirection);
            lightDirection = gi.light.dir;
            lightColor = gi.light.color;
/// Diffuse:
            float NdotL = max(0.0,dot( normalDirection, lightDirection ));
            float3 directDiffuse = max( 0.0, NdotL) * attenColor;
            float3 indirectDiffuse = float3(0,0,0);
            indirectDiffuse += gi.indirect.diffuse;
            float4 _BaseTexture1_var = tex2D(_BaseTexture1,TRANSFORM_TEX(i.uv0, _BaseTexture1));
            float4 _BaseTexture2_var = tex2D(_BaseTexture2,TRANSFORM_TEX(i.uv0, _BaseTexture2));
            float3 diffuseColor = (lerp(_BaseTexture1_var.rgb,_BaseTexture2_var.rgb,_TextLerp)*_BaseColor.rgb);
            float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
// Final Color:
            float3 finalColor = diffuse;
            fixed4 finalRGBA = fixed4(finalColor,_Alpha);
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
        ZWrite Off
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #define UNITY_PASS_FORWARDADD
        #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
        #include "Lighting.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardBRDF.cginc"
        #pragma multi_compile_fwdadd
        #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
        #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
        #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fog
        #pragma only_renderers glcore gles gles3 metal d3d9 
        #pragma target 3.0
        uniform float _Alpha;
        uniform sampler2D _BaseTexture1; uniform float4 _BaseTexture1_ST;
        uniform float4 _BaseColor;
        uniform sampler2D _BaseTexture2; uniform float4 _BaseTexture2_ST;
        uniform float _TextLerp;
        struct VertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float2 texcoord0 : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            float2 texcoord2 : TEXCOORD2;
        };
        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv0 : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            float2 uv2 : TEXCOORD2;
            float4 posWorld : TEXCOORD3;
            float3 normalDir : TEXCOORD4;
            float3 tangentDir : TEXCOORD5;
            float3 bitangentDir : TEXCOORD6;
            LIGHTING_COORDS(7,8)
        };
        VertexOutput vert (VertexInput v) {
            VertexOutput o = (VertexOutput)0;
            o.uv0 = v.texcoord0;
            o.uv1 = v.texcoord1;
            o.uv2 = v.texcoord2;
            o.normalDir = UnityObjectToWorldNormal(v.normal);
            o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
            o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
            o.posWorld = mul(unity_ObjectToWorld, v.vertex);
            float3 lightColor = _LightColor0.rgb;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            TRANSFER_VERTEX_TO_FRAGMENT(o)
            return o;
        }
        float4 frag(VertexOutput i) : COLOR {
            i.normalDir = normalize(i.normalDir);
            float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            float3 normalDirection = i.normalDir;
            float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
            float3 lightColor = _LightColor0.rgb;
// Lighting:
            float attenuation = LIGHT_ATTENUATION(i);
            float3 attenColor = attenuation * _LightColor0.xyz;
/// Diffuse:
            float NdotL = max(0.0,dot( normalDirection, lightDirection ));
            float3 directDiffuse = max( 0.0, NdotL) * attenColor;
            float4 _BaseTexture1_var = tex2D(_BaseTexture1,TRANSFORM_TEX(i.uv0, _BaseTexture1));
            float4 _BaseTexture2_var = tex2D(_BaseTexture2,TRANSFORM_TEX(i.uv0, _BaseTexture2));
            float3 diffuseColor = (lerp(_BaseTexture1_var.rgb,_BaseTexture2_var.rgb,_TextLerp)*_BaseColor.rgb);
            float3 diffuse = directDiffuse * diffuseColor;
// Final Color:
            float3 finalColor = diffuse;
            return fixed4(finalColor * _Alpha,0);
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
        #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardBRDF.cginc"
        #include "UnityMetaPass.cginc"
        #pragma fragmentoption ARB_precision_hint_fastest
        #pragma multi_compile_shadowcaster
        #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
        #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
        #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fog
        #pragma only_renderers glcore gles gles3 metal d3d9 
        #pragma target 3.0
        uniform sampler2D _BaseTexture1; uniform float4 _BaseTexture1_ST;
        uniform float4 _BaseColor;
        uniform sampler2D _BaseTexture2; uniform float4 _BaseTexture2_ST;
        uniform float _TextLerp;
        struct VertexInput {
            float4 vertex : POSITION;
            float2 texcoord0 : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            float2 texcoord2 : TEXCOORD2;
        };
        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv0 : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            float2 uv2 : TEXCOORD2;
            float4 posWorld : TEXCOORD3;
        };
        VertexOutput vert (VertexInput v) {
            VertexOutput o = (VertexOutput)0;
            o.uv0 = v.texcoord0;
            o.uv1 = v.texcoord1;
            o.uv2 = v.texcoord2;
            o.posWorld = mul(unity_ObjectToWorld, v.vertex);
            o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
            return o;
        }
        float4 frag(VertexOutput i) : SV_Target {
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            UnityMetaInput o;
            UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
            
            o.Emission = 0;
            
            float4 _BaseTexture1_var = tex2D(_BaseTexture1,TRANSFORM_TEX(i.uv0, _BaseTexture1));
            float4 _BaseTexture2_var = tex2D(_BaseTexture2,TRANSFORM_TEX(i.uv0, _BaseTexture2));
            float3 diffColor = (lerp(_BaseTexture1_var.rgb,_BaseTexture2_var.rgb,_TextLerp)*_BaseColor.rgb);
            o.Albedo = diffColor;
            
            return UnityMetaFragment( o );
        }
        ENDCG
    }
}
FallBack "Diffuse"
CustomEditor "ShaderForgeMaterialInspector"
}
