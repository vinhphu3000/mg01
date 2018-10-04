// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5856044,fgcg:0.4677768,fgcb:0.7573529,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4659,x:33077,y:32622,varname:node_4659,prsc:2|diff-1900-RGB,emission-5885-OUT,clip-1793-OUT;n:type:ShaderForge.SFN_Tex2d,id:1900,x:32398,y:32133,ptovrint:False,ptlb:DiffuseRGBA,ptin:_DiffuseRGBA,varname:_DiffuseRGBA,prsc:2,tex:fb26ac23a47244743baa300d02f48609,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1085,x:31783,y:32241,ptovrint:False,ptlb:GlowRGB,ptin:_GlowRGB,varname:_GlowRGB,prsc:2,tex:f97d4e069d1f6054b9d587777a66ede6,ntxv:0,isnm:False|UVIN-1205-OUT;n:type:ShaderForge.SFN_TexCoord,id:9156,x:30883,y:32843,varname:node_9156,prsc:2,uv:1;n:type:ShaderForge.SFN_Slider,id:1713,x:32043,y:32340,ptovrint:False,ptlb:Glow-power,ptin:_Glowpower,varname:_Glowpower,prsc:2,min:0.1,cur:2,max:10;n:type:ShaderForge.SFN_Tex2d,id:9260,x:31467,y:33217,ptovrint:False,ptlb:Noise(R),ptin:_NoiseR,varname:_NoiseR,prsc:2,tex:69b885aba1319784888b2c67c3eb0d4b,ntxv:0,isnm:False|UVIN-8348-OUT;n:type:ShaderForge.SFN_Lerp,id:5885,x:32739,y:32640,varname:node_5885,prsc:2|A-9380-OUT,B-5951-OUT,T-174-OUT;n:type:ShaderForge.SFN_Vector1,id:568,x:31466,y:33510,varname:node_568,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Slider,id:479,x:31961,y:32733,ptovrint:False,ptlb:Glow-Mask-Clip,ptin:_GlowMaskClip,varname:_GlowMaskClip,prsc:2,min:0,cur:0.241623,max:1.05;n:type:ShaderForge.SFN_Step,id:174,x:32415,y:32805,varname:node_174,prsc:2|A-479-OUT,B-90-OUT;n:type:ShaderForge.SFN_Color,id:5347,x:31772,y:32555,ptovrint:False,ptlb:Glow-Color,ptin:_GlowColor,varname:_GlowColor,prsc:2,glob:False,c1:0.9705882,c2:0.9705882,c3:0.9705882,c4:1;n:type:ShaderForge.SFN_Append,id:5319,x:30885,y:32653,varname:node_5319,prsc:2|A-6538-OUT,B-5197-OUT;n:type:ShaderForge.SFN_Add,id:1067,x:31097,y:32740,varname:node_1067,prsc:2|A-5319-OUT,B-9156-UVOUT;n:type:ShaderForge.SFN_Vector1,id:6538,x:30642,y:32600,varname:node_6538,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:5197,x:30493,y:32731,ptovrint:False,ptlb:Offset-Clip (V),ptin:_OffsetClipV,varname:_OffsetClipV,prsc:2,min:-1.5,cur:-0.02357209,max:1;n:type:ShaderForge.SFN_Tex2d,id:2781,x:32346,y:33044,varname:_ShapeRGBA1,prsc:2,tex:1bcb939e6c72d0a40a10215d26b73416,ntxv:0,isnm:False|UVIN-1067-OUT,TEX-1770-TEX;n:type:ShaderForge.SFN_Multiply,id:1793,x:32710,y:33152,varname:node_1793,prsc:2|A-2942-OUT,B-2781-R,C-4944-OUT,D-1900-A,E-3866-OUT;n:type:ShaderForge.SFN_Slider,id:4944,x:32488,y:33631,ptovrint:False,ptlb:Mask-Clip,ptin:_MaskClip,varname:_MaskClip,prsc:2,min:0,cur:0.6572078,max:5;n:type:ShaderForge.SFN_Add,id:1205,x:31083,y:32232,varname:node_1205,prsc:2|A-707-OUT,B-1067-OUT;n:type:ShaderForge.SFN_Append,id:707,x:30796,y:32166,varname:node_707,prsc:2|A-5953-OUT,B-7146-OUT;n:type:ShaderForge.SFN_Vector1,id:5953,x:30478,y:32081,varname:node_5953,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:7146,x:30310,y:32209,ptovrint:False,ptlb:Gradient-Space,ptin:_GradientSpace,varname:_GradientSpace,prsc:2,min:0,cur:0.2416429,max:1.2;n:type:ShaderForge.SFN_Tex2d,id:2928,x:31719,y:32858,varname:_ShapeRGBA2_copy,prsc:2,tex:1bcb939e6c72d0a40a10215d26b73416,ntxv:0,isnm:False|UVIN-1205-OUT,TEX-1770-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:1770,x:31197,y:32977,ptovrint:False,ptlb:Shape(R),ptin:_ShapeR,varname:_ShapeR,tex:1bcb939e6c72d0a40a10215d26b73416,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Add,id:8159,x:31781,y:33375,varname:node_8159,prsc:2|A-9260-R,B-568-OUT;n:type:ShaderForge.SFN_Multiply,id:90,x:32093,y:32875,varname:node_90,prsc:2|A-8159-OUT,B-2928-R,C-3866-OUT;n:type:ShaderForge.SFN_Power,id:9380,x:32421,y:32435,varname:node_9380,prsc:2|VAL-529-OUT,EXP-1713-OUT;n:type:ShaderForge.SFN_Multiply,id:2942,x:32475,y:33433,varname:node_2942,prsc:2|A-957-OUT,B-4944-OUT;n:type:ShaderForge.SFN_Vector1,id:3866,x:31730,y:33056,varname:node_3866,prsc:2,v1:8;n:type:ShaderForge.SFN_Vector3,id:5951,x:32420,y:32624,varname:node_5951,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Multiply,id:529,x:32001,y:32532,varname:node_529,prsc:2|A-1085-RGB,B-2308-OUT,C-5347-RGB;n:type:ShaderForge.SFN_TexCoord,id:3151,x:30829,y:33261,varname:node_3151,prsc:2,uv:0;n:type:ShaderForge.SFN_SwitchProperty,id:8348,x:31146,y:33261,ptovrint:True,ptlb:2UV,ptin:_2UV,varname:_2UV,prsc:2,on:False|A-3151-UVOUT,B-1205-OUT;n:type:ShaderForge.SFN_Vector1,id:2308,x:31774,y:32456,varname:node_2308,prsc:2,v1:1.2;n:type:ShaderForge.SFN_Multiply,id:957,x:32017,y:33566,varname:node_957,prsc:2|A-2609-OUT,B-8159-OUT;n:type:ShaderForge.SFN_Vector1,id:2609,x:31783,y:33672,varname:node_2609,prsc:2,v1:1.5;proporder:1900-1085-5347-1713-479-1770-5197-7146-9260-4944-8348;pass:END;sub:END;*/

Shader "EJoyShader/Mesh/Destroy-VGlow-Mask4" {
    Properties {
        _DiffuseRGBA ("DiffuseRGBA", 2D) = "white" {}
        _GlowRGB ("GlowRGB", 2D) = "white" {}
        _GlowColor ("Glow-Color", Color) = (0.9705882,0.9705882,0.9705882,1)
        _Glowpower ("Glow-power", Range(0.1, 10)) = 2
        _GlowMaskClip ("Glow-Mask-Clip", Range(0, 1.05)) = 0.241623
        _ShapeR ("Shape(R)", 2D) = "bump" {}
        _OffsetClipV ("Offset-Clip (V)", Range(-1.5, 1)) = -0.02357209
        _GradientSpace ("Gradient-Space", Range(0, 1.2)) = 0.2416429
        _NoiseR ("Noise(R)", 2D) = "white" {}
        _MaskClip ("Mask-Clip", Range(0, 5)) = 0.6572078
        [MaterialToggle] _2UV ("2UV", Float ) = 0
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
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float _Glowpower;
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _GlowMaskClip;
            uniform float4 _GlowColor;
            uniform float _OffsetClipV;
            uniform float _MaskClip;
            uniform float _GradientSpace;
            uniform sampler2D _ShapeR; uniform float4 _ShapeR_ST;
            uniform fixed _2UV;
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
                
                float2 node_1067 = (float2(0.0,_OffsetClipV)+i.uv1);
                float2 node_1205 = (float2(0.0,_GradientSpace)+node_1067);
                float2 _2UV_var = lerp( i.uv0, node_1205, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float node_8159 = (_NoiseR_var.r+0.1);
                float4 _ShapeRGBA1 = tex2D(_ShapeR,TRANSFORM_TEX(node_1067, _ShapeR));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                float node_3866 = 8.0;
                clip((((1.5*node_8159)*_MaskClip)*_ShapeRGBA1.r*_MaskClip*_DiffuseRGBA_var.a*node_3866) - 0.5);
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
                float4 _GlowRGB_var = tex2D(_GlowRGB,TRANSFORM_TEX(node_1205, _GlowRGB));
                float4 _ShapeRGBA2_copy = tex2D(_ShapeR,TRANSFORM_TEX(node_1205, _ShapeR));
                float3 emissive = lerp(pow((_GlowRGB_var.rgb*1.2*_GlowColor.rgb),_Glowpower),float3(0,0,0),step(_GlowMaskClip,(node_8159*_ShapeRGBA2_copy.r*node_3866)));
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
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float _Glowpower;
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _GlowMaskClip;
            uniform float4 _GlowColor;
            uniform float _OffsetClipV;
            uniform float _MaskClip;
            uniform float _GradientSpace;
            uniform sampler2D _ShapeR; uniform float4 _ShapeR_ST;
            uniform fixed _2UV;
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
                
                float2 node_1067 = (float2(0.0,_OffsetClipV)+i.uv1);
                float2 node_1205 = (float2(0.0,_GradientSpace)+node_1067);
                float2 _2UV_var = lerp( i.uv0, node_1205, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float node_8159 = (_NoiseR_var.r+0.1);
                float4 _ShapeRGBA1 = tex2D(_ShapeR,TRANSFORM_TEX(node_1067, _ShapeR));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                float node_3866 = 8.0;
                clip((((1.5*node_8159)*_MaskClip)*_ShapeRGBA1.r*_MaskClip*_DiffuseRGBA_var.a*node_3866) - 0.5);
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
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _OffsetClipV;
            uniform float _MaskClip;
            uniform float _GradientSpace;
            uniform sampler2D _ShapeR; uniform float4 _ShapeR_ST;
            uniform fixed _2UV;
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
                float2 node_1067 = (float2(0.0,_OffsetClipV)+i.uv1);
                float2 node_1205 = (float2(0.0,_GradientSpace)+node_1067);
                float2 _2UV_var = lerp( i.uv0, node_1205, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float node_8159 = (_NoiseR_var.r+0.1);
                float4 _ShapeRGBA1 = tex2D(_ShapeR,TRANSFORM_TEX(node_1067, _ShapeR));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                float node_3866 = 8.0;
                clip((((1.5*node_8159)*_MaskClip)*_ShapeRGBA1.r*_MaskClip*_DiffuseRGBA_var.a*node_3866) - 0.5);
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
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _OffsetClipV;
            uniform float _MaskClip;
            uniform float _GradientSpace;
            uniform sampler2D _ShapeR; uniform float4 _ShapeR_ST;
            uniform fixed _2UV;
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
                float2 node_1067 = (float2(0.0,_OffsetClipV)+i.uv1);
                float2 node_1205 = (float2(0.0,_GradientSpace)+node_1067);
                float2 _2UV_var = lerp( i.uv0, node_1205, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float node_8159 = (_NoiseR_var.r+0.1);
                float4 _ShapeRGBA1 = tex2D(_ShapeR,TRANSFORM_TEX(node_1067, _ShapeR));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                float node_3866 = 8.0;
                clip((((1.5*node_8159)*_MaskClip)*_ShapeRGBA1.r*_MaskClip*_DiffuseRGBA_var.a*node_3866) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
