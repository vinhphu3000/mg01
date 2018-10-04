// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5856044,fgcg:0.4677768,fgcb:0.7573529,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4659,x:32787,y:32620,varname:node_4659,prsc:2|diff-1900-RGB,emission-5885-OUT,clip-8593-OUT;n:type:ShaderForge.SFN_Tex2d,id:1900,x:31798,y:32172,ptovrint:False,ptlb:DiffuseRGBA,ptin:_DiffuseRGBA,varname:_DiffuseRGBA,prsc:2,tex:fb26ac23a47244743baa300d02f48609,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1085,x:31647,y:32749,ptovrint:False,ptlb:GlowRGBA,ptin:_GlowRGBA,varname:_GlowRGBA,prsc:2,tex:f97d4e069d1f6054b9d587777a66ede6,ntxv:0,isnm:False|UVIN-9156-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:9156,x:31260,y:33098,varname:node_9156,prsc:2,uv:1;n:type:ShaderForge.SFN_Tex2d,id:4744,x:31778,y:33070,ptovrint:False,ptlb:Mask-ShapeRGB,ptin:_MaskShapeRGB,varname:_MaskShapeRGB,prsc:2,tex:1d536f2df794df4459de83bd13e9e4d6,ntxv:3,isnm:False|UVIN-1067-OUT;n:type:ShaderForge.SFN_Multiply,id:8593,x:32317,y:33176,varname:node_8593,prsc:2|A-4744-RGB,B-2649-OUT,C-1900-A;n:type:ShaderForge.SFN_Power,id:9912,x:31840,y:32658,varname:node_9912,prsc:2|VAL-1085-RGB,EXP-1713-OUT;n:type:ShaderForge.SFN_Add,id:7180,x:32083,y:32635,varname:node_7180,prsc:2|A-3020-OUT,B-9912-OUT;n:type:ShaderForge.SFN_Slider,id:1713,x:31404,y:32554,ptovrint:False,ptlb:Glow-power,ptin:_Glowpower,varname:_Glowpower,prsc:2,min:0.1,cur:9.401715,max:20;n:type:ShaderForge.SFN_Slider,id:4059,x:31767,y:33581,ptovrint:False,ptlb:Mask-Clip,ptin:_MaskClip,varname:_MaskClip,prsc:2,min:32,cur:32,max:0;n:type:ShaderForge.SFN_Tex2d,id:9260,x:31539,y:33280,ptovrint:False,ptlb:Mask_Noise,ptin:_Mask_Noise,varname:_Mask_Noise,prsc:2,tex:f97d4e069d1f6054b9d587777a66ede6,ntxv:0,isnm:False|UVIN-9156-UVOUT;n:type:ShaderForge.SFN_Lerp,id:5885,x:32562,y:32690,varname:node_5885,prsc:2|A-3020-OUT,B-7180-OUT,T-174-OUT;n:type:ShaderForge.SFN_Multiply,id:2649,x:32110,y:33305,varname:node_2649,prsc:2|A-3109-OUT,B-4059-OUT;n:type:ShaderForge.SFN_Add,id:3109,x:31847,y:33415,varname:node_3109,prsc:2|A-9260-RGB,B-568-OUT;n:type:ShaderForge.SFN_Vector1,id:568,x:31544,y:33492,varname:node_568,prsc:2,v1:0.02;n:type:ShaderForge.SFN_Slider,id:479,x:32024,y:32944,ptovrint:False,ptlb:Glow-Mask-Clip,ptin:_GlowMaskClip,varname:_GlowMaskClip,prsc:2,min:0,cur:1.008757,max:1.05;n:type:ShaderForge.SFN_Step,id:174,x:32380,y:32852,varname:node_174,prsc:2|A-479-OUT,B-1085-A;n:type:ShaderForge.SFN_Color,id:5347,x:31806,y:32432,ptovrint:False,ptlb:Glow-Color,ptin:_GlowColor,varname:_GlowColor,prsc:2,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:3020,x:32045,y:32315,varname:node_3020,prsc:2|A-1900-RGB,B-5347-RGB;n:type:ShaderForge.SFN_Append,id:5319,x:31029,y:32740,varname:node_5319,prsc:2|A-6538-OUT,B-5197-OUT;n:type:ShaderForge.SFN_Add,id:1067,x:31272,y:32791,varname:node_1067,prsc:2|A-5319-OUT,B-9156-UVOUT;n:type:ShaderForge.SFN_Vector1,id:6538,x:30794,y:32689,varname:node_6538,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:5197,x:30660,y:32848,ptovrint:False,ptlb:Offset-Clip,ptin:_OffsetClip,varname:_OffsetClip,prsc:2,min:-0.8,cur:0.7075722,max:1.2;n:type:ShaderForge.SFN_TexCoord,id:5102,x:32097,y:33005,varname:node_5102,prsc:2,uv:0;proporder:1900-1085-5347-1713-479-4744-9260-4059-5197;pass:END;sub:END;*/

Shader "EJoyShader/Mesh/Destroy-4T-UGlow-Mask" {
    Properties {
        _DiffuseRGBA ("DiffuseRGBA", 2D) = "white" {}
        _GlowRGBA ("GlowRGBA", 2D) = "white" {}
        _GlowColor ("Glow-Color", Color) = (0,0,0,1)
        _Glowpower ("Glow-power", Range(0.1, 20)) = 9.401715
        _GlowMaskClip ("Glow-Mask-Clip", Range(0, 1.05)) = 1.008757
        _MaskShapeRGB ("Mask-ShapeRGB", 2D) = "bump" {}
        _Mask_Noise ("Mask_Noise", 2D) = "white" {}
        _MaskClip ("Mask-Clip", Range(32, 0)) = 32
        _OffsetClip ("Offset-Clip", Range(-0.8, 1.2)) = 0.7075722
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
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _DiffuseRGBA; uniform float4 _DiffuseRGBA_ST;
            uniform sampler2D _GlowRGBA; uniform float4 _GlowRGBA_ST;
            uniform sampler2D _MaskShapeRGB; uniform float4 _MaskShapeRGB_ST;
            uniform float _Glowpower;
            uniform float _MaskClip;
            uniform sampler2D _Mask_Noise; uniform float4 _Mask_Noise_ST;
            uniform float _GlowMaskClip;
            uniform float4 _GlowColor;
            uniform float _OffsetClip;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
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
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float2 node_1067 = (float2(0.0,_OffsetClip)+i.uv1);
                float4 _MaskShapeRGB_var = tex2D(_MaskShapeRGB,TRANSFORM_TEX(node_1067, _MaskShapeRGB));
                float4 _Mask_Noise_var = tex2D(_Mask_Noise,TRANSFORM_TEX(i.uv1, _Mask_Noise));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                clip((_MaskShapeRGB_var.rgb*((_Mask_Noise_var.rgb+0.02)*_MaskClip)*_DiffuseRGBA_var.a) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = _DiffuseRGBA_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 node_3020 = (_DiffuseRGBA_var.rgb*_GlowColor.rgb);
                float4 _GlowRGBA_var = tex2D(_GlowRGBA,TRANSFORM_TEX(i.uv1, _GlowRGBA));
                float3 emissive = lerp(node_3020,(node_3020+pow(_GlowRGBA_var.rgb,_Glowpower)),step(_GlowMaskClip,_GlowRGBA_var.a));
/// Final Color:
                float3 finalColor = diffuse + emissive;
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
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _DiffuseRGBA; uniform float4 _DiffuseRGBA_ST;
            uniform sampler2D _GlowRGBA; uniform float4 _GlowRGBA_ST;
            uniform sampler2D _MaskShapeRGB; uniform float4 _MaskShapeRGB_ST;
            uniform float _Glowpower;
            uniform float _MaskClip;
            uniform sampler2D _Mask_Noise; uniform float4 _Mask_Noise_ST;
            uniform float _GlowMaskClip;
            uniform float4 _GlowColor;
            uniform float _OffsetClip;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                LIGHTING_COORDS(4,5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
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
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float2 node_1067 = (float2(0.0,_OffsetClip)+i.uv1);
                float4 _MaskShapeRGB_var = tex2D(_MaskShapeRGB,TRANSFORM_TEX(node_1067, _MaskShapeRGB));
                float4 _Mask_Noise_var = tex2D(_Mask_Noise,TRANSFORM_TEX(i.uv1, _Mask_Noise));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                clip((_MaskShapeRGB_var.rgb*((_Mask_Noise_var.rgb+0.02)*_MaskClip)*_DiffuseRGBA_var.a) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = _DiffuseRGBA_var.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            Cull Off
            
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
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _DiffuseRGBA; uniform float4 _DiffuseRGBA_ST;
            uniform sampler2D _MaskShapeRGB; uniform float4 _MaskShapeRGB_ST;
            uniform float _MaskClip;
            uniform sampler2D _Mask_Noise; uniform float4 _Mask_Noise_ST;
            uniform float _OffsetClip;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
                float2 uv1 : TEXCOORD6;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float2 node_1067 = (float2(0.0,_OffsetClip)+i.uv1);
                float4 _MaskShapeRGB_var = tex2D(_MaskShapeRGB,TRANSFORM_TEX(node_1067, _MaskShapeRGB));
                float4 _Mask_Noise_var = tex2D(_Mask_Noise,TRANSFORM_TEX(i.uv1, _Mask_Noise));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                clip((_MaskShapeRGB_var.rgb*((_Mask_Noise_var.rgb+0.02)*_MaskClip)*_DiffuseRGBA_var.a) - 0.5);
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
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _DiffuseRGBA; uniform float4 _DiffuseRGBA_ST;
            uniform sampler2D _MaskShapeRGB; uniform float4 _MaskShapeRGB_ST;
            uniform float _MaskClip;
            uniform sampler2D _Mask_Noise; uniform float4 _Mask_Noise_ST;
            uniform float _OffsetClip;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float2 node_1067 = (float2(0.0,_OffsetClip)+i.uv1);
                float4 _MaskShapeRGB_var = tex2D(_MaskShapeRGB,TRANSFORM_TEX(node_1067, _MaskShapeRGB));
                float4 _Mask_Noise_var = tex2D(_Mask_Noise,TRANSFORM_TEX(i.uv1, _Mask_Noise));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                clip((_MaskShapeRGB_var.rgb*((_Mask_Noise_var.rgb+0.02)*_MaskClip)*_DiffuseRGBA_var.a) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
