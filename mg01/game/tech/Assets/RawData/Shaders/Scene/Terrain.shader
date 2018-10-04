// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:True,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.07136679,fgcg:0.6357055,fgcb:0.9705882,fgca:1,fgde:4,fgrn:42,fgrf:3000,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:33944,y:32188,varname:node_1,prsc:2|diff-3431-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:2,x:33453,y:32160,varname:node_2,prsc:2,chbt:1|M-7-RGB,R-33-RGB,G-29-RGB,B-31-RGB,BTM-3-RGB;n:type:ShaderForge.SFN_Tex2d,id:3,x:33083,y:32604,ptovrint:False,ptlb:A,ptin:_A,varname:_A,prsc:2,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7,x:33455,y:31803,ptovrint:False,ptlb:MaskRGBA,ptin:_MaskRGBA,varname:_MaskRGBA,prsc:2,tex:56113815d2ad04b419463950a7cfa49a,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:29,x:33084,y:32164,ptovrint:False,ptlb:G,ptin:_G,varname:_G,prsc:2,tex:76ad9478a8e45e04a8a9dade41ef0b35,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:31,x:33086,y:32377,ptovrint:False,ptlb:B,ptin:_B,varname:_B,prsc:2,tex:372583e92ba3e094d975806be5534b2e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:33,x:33086,y:31914,ptovrint:False,ptlb:R,ptin:_R,varname:_R,prsc:2,tex:d786b42597d136f4d8d027f5f96e3a43,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3431,x:33706,y:32315,varname:node_3431,prsc:2|A-2-OUT,B-8077-RGB;n:type:ShaderForge.SFN_Color,id:8077,x:33431,y:32396,ptovrint:False,ptlb:Main-Color,ptin:_MainColor,varname:_MainColor,prsc:2,glob:False,c1:0.8308824,c2:0.8308824,c3:0.8308824,c4:1;proporder:7-33-29-31-3-8077;pass:END;sub:END;*/

Shader "EJoyShader/Scene/Terrain-4Texture" {
    Properties {
        _MaskRGBA ("MaskRGBA", 2D) = "white" {}
        _R ("R", 2D) = "white" {}
        _G ("G", 2D) = "white" {}
        _B ("B", 2D) = "white" {}
        _A ("A", 2D) = "bump" {}
        _MainColor ("Main-Color", Color) = (0.8308824,0.8308824,0.8308824,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 2000
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _A; uniform float4 _A_ST;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform sampler2D _G; uniform float4 _G_ST;
            uniform sampler2D _B; uniform float4 _B_ST;
            uniform sampler2D _R; uniform float4 _R_ST;
            uniform float4 _MainColor;
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
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
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
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// GI Data:
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
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float4 _MaskRGBA_var = tex2D(_MaskRGBA,TRANSFORM_TEX(i.uv0, _MaskRGBA));
                float4 _A_var = tex2D(_A,TRANSFORM_TEX(i.uv0, _A));
                float4 _R_var = tex2D(_R,TRANSFORM_TEX(i.uv0, _R));
                float4 _G_var = tex2D(_G,TRANSFORM_TEX(i.uv0, _G));
                float4 _B_var = tex2D(_B,TRANSFORM_TEX(i.uv0, _B));
                float3 diffuseColor = ((lerp( lerp( lerp( _A_var.rgb, _R_var.rgb, _MaskRGBA_var.rgb.r ), _G_var.rgb, _MaskRGBA_var.rgb.g ), _B_var.rgb, _MaskRGBA_var.rgb.b ))*_MainColor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
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
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _A; uniform float4 _A_ST;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform sampler2D _G; uniform float4 _G_ST;
            uniform sampler2D _B; uniform float4 _B_ST;
            uniform sampler2D _R; uniform float4 _R_ST;
            uniform float4 _MainColor;
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
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _MaskRGBA_var = tex2D(_MaskRGBA,TRANSFORM_TEX(i.uv0, _MaskRGBA));
                float4 _A_var = tex2D(_A,TRANSFORM_TEX(i.uv0, _A));
                float4 _R_var = tex2D(_R,TRANSFORM_TEX(i.uv0, _R));
                float4 _G_var = tex2D(_G,TRANSFORM_TEX(i.uv0, _G));
                float4 _B_var = tex2D(_B,TRANSFORM_TEX(i.uv0, _B));
                float3 diffuseColor = ((lerp( lerp( lerp( _A_var.rgb, _R_var.rgb, _MaskRGBA_var.rgb.r ), _G_var.rgb, _MaskRGBA_var.rgb.g ), _B_var.rgb, _MaskRGBA_var.rgb.b ))*_MainColor.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
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
            uniform sampler2D _A; uniform float4 _A_ST;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform sampler2D _G; uniform float4 _G_ST;
            uniform sampler2D _B; uniform float4 _B_ST;
            uniform sampler2D _R; uniform float4 _R_ST;
            uniform float4 _MainColor;
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
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float4 _MaskRGBA_var = tex2D(_MaskRGBA,TRANSFORM_TEX(i.uv0, _MaskRGBA));
                float4 _A_var = tex2D(_A,TRANSFORM_TEX(i.uv0, _A));
                float4 _R_var = tex2D(_R,TRANSFORM_TEX(i.uv0, _R));
                float4 _G_var = tex2D(_G,TRANSFORM_TEX(i.uv0, _G));
                float4 _B_var = tex2D(_B,TRANSFORM_TEX(i.uv0, _B));
                float3 diffColor = ((lerp( lerp( lerp( _A_var.rgb, _R_var.rgb, _MaskRGBA_var.rgb.r ), _G_var.rgb, _MaskRGBA_var.rgb.g ), _B_var.rgb, _MaskRGBA_var.rgb.b ))*_MainColor.rgb);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "VertexLit"
    CustomEditor "ShaderForgeMaterialInspector"
}
