// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5856044,fgcg:0.4677768,fgcb:0.7573529,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4659,x:33150,y:32631,varname:node_4659,prsc:2|emission-5885-OUT,clip-9875-OUT;n:type:ShaderForge.SFN_Tex2d,id:1900,x:31886,y:32324,ptovrint:False,ptlb:DiffuseRGBA,ptin:_DiffuseRGBA,varname:_DiffuseRGBA,prsc:2,tex:fb26ac23a47244743baa300d02f48609,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1085,x:31610,y:32045,ptovrint:False,ptlb:GlowRGB,ptin:_GlowRGB,varname:_GlowRGB,prsc:2,tex:f97d4e069d1f6054b9d587777a66ede6,ntxv:0,isnm:False|UVIN-8348-OUT;n:type:ShaderForge.SFN_TexCoord,id:9156,x:30767,y:32972,varname:node_9156,prsc:2,uv:1;n:type:ShaderForge.SFN_Slider,id:1713,x:31843,y:32115,ptovrint:False,ptlb:Glow-power,ptin:_Glowpower,varname:_Glowpower,prsc:2,min:0.1,cur:3,max:3;n:type:ShaderForge.SFN_Tex2d,id:9260,x:31562,y:33061,ptovrint:False,ptlb:Noise(R),ptin:_NoiseR,varname:_NoiseR,prsc:2,tex:3c68d0ac7a8e16d4ab21af0f9cc7a836,ntxv:0,isnm:False|UVIN-8348-OUT;n:type:ShaderForge.SFN_Lerp,id:5885,x:32806,y:32602,varname:node_5885,prsc:2|A-8375-OUT,B-3674-OUT,T-174-OUT;n:type:ShaderForge.SFN_Slider,id:479,x:31800,y:32831,ptovrint:False,ptlb:Glow-Edge-Width,ptin:_GlowEdgeWidth,varname:_GlowMaskClip,prsc:2,min:0,cur:2,max:2;n:type:ShaderForge.SFN_Step,id:174,x:32472,y:32843,varname:node_174,prsc:2|A-479-OUT,B-2942-OUT;n:type:ShaderForge.SFN_Color,id:5347,x:31606,y:32363,ptovrint:False,ptlb:Glow-Color,ptin:_GlowColor,varname:_GlowColor,prsc:2,glob:False,c1:0.9705882,c2:0.9705882,c3:0.9705882,c4:1;n:type:ShaderForge.SFN_Slider,id:4944,x:31610,y:32947,ptovrint:False,ptlb:Mask-Clip,ptin:_MaskClip,varname:_MaskClip,prsc:2,min:0,cur:1,max:20;n:type:ShaderForge.SFN_Power,id:9380,x:32154,y:32276,varname:node_9380,prsc:2|VAL-529-OUT,EXP-1713-OUT;n:type:ShaderForge.SFN_Multiply,id:2942,x:32115,y:33045,varname:node_2942,prsc:2|A-4944-OUT,B-4261-OUT,C-4068-OUT;n:type:ShaderForge.SFN_Multiply,id:529,x:31900,y:32287,varname:node_529,prsc:2|A-1085-RGB,B-2308-OUT,C-5347-RGB;n:type:ShaderForge.SFN_TexCoord,id:3151,x:30775,y:32807,varname:node_3151,prsc:2,uv:0;n:type:ShaderForge.SFN_SwitchProperty,id:8348,x:31092,y:32869,ptovrint:True,ptlb:2UV,ptin:_2UV,varname:_2UV,prsc:2,on:False|A-3151-UVOUT,B-9156-UVOUT;n:type:ShaderForge.SFN_Vector1,id:2308,x:31613,y:32238,varname:node_2308,prsc:2,v1:1.2;n:type:ShaderForge.SFN_Tex2d,id:2283,x:31525,y:33343,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_2283,prsc:2,tex:3720f407e2e235248ab00cb5ad9aed46,ntxv:0,isnm:False|UVIN-8348-OUT;n:type:ShaderForge.SFN_Add,id:8375,x:32462,y:32346,varname:node_8375,prsc:2|A-9380-OUT,B-3674-OUT;n:type:ShaderForge.SFN_Multiply,id:3674,x:32177,y:32456,varname:node_3674,prsc:2|A-1900-RGB,B-5508-RGB;n:type:ShaderForge.SFN_Color,id:5508,x:31890,y:32636,ptovrint:False,ptlb:Base-Color,ptin:_BaseColor,varname:node_5508,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:9875,x:32459,y:33050,varname:node_9875,prsc:2|A-2942-OUT,B-1900-A;n:type:ShaderForge.SFN_Slider,id:2920,x:31454,y:33226,ptovrint:False,ptlb:Light-Control,ptin:_LightControl,varname:_MaskClip_copy,prsc:2,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Add,id:4261,x:31796,y:33106,varname:node_4261,prsc:2|A-9260-R,B-2920-OUT;n:type:ShaderForge.SFN_Add,id:4068,x:31778,y:33316,varname:node_4068,prsc:2|A-2920-OUT,B-2283-R;proporder:1900-1085-5347-1713-9260-5508-2283-8348-479-4944-2920;pass:END;sub:END;*/

Shader "EJoyShader/Mesh/Destroy-Glow-Edge" {
    Properties {
        _DiffuseRGBA ("DiffuseRGBA", 2D) = "white" {}
        _GlowRGB ("GlowRGB", 2D) = "white" {}
        _GlowColor ("Glow-Color", Color) = (0.9705882,0.9705882,0.9705882,1)
        _Glowpower ("Glow-power", Range(0.1, 3)) = 3
        _NoiseR ("Noise(R)", 2D) = "white" {}
        _BaseColor ("Base-Color", Color) = (1,1,1,1)
        _Mask ("Mask", 2D) = "white" {}
        [MaterialToggle] _2UV ("2UV", Float ) = 0
        _GlowEdgeWidth ("Glow-Edge-Width", Range(0, 2)) = 2
        _MaskClip ("Mask-Clip", Range(0, 20)) = 1
        _LightControl ("Light-Control", Range(0, 1)) = 1
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _DiffuseRGBA; uniform float4 _DiffuseRGBA_ST;
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float _Glowpower;
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _GlowEdgeWidth;
            uniform float4 _GlowColor;
            uniform float _MaskClip;
            uniform fixed _2UV;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float4 _BaseColor;
            uniform float _LightControl;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float2 _2UV_var = lerp( i.uv0, i.uv1, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(_2UV_var, _Mask));
                float node_2942 = (_MaskClip*(_NoiseR_var.r+_LightControl)*(_LightControl+_Mask_var.r));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                clip((node_2942*_DiffuseRGBA_var.a) - 0.5);
////// Lighting:
////// Emissive:
                float4 _GlowRGB_var = tex2D(_GlowRGB,TRANSFORM_TEX(_2UV_var, _GlowRGB));
                float3 node_3674 = (_DiffuseRGBA_var.rgb*_BaseColor.rgb);
                float3 emissive = lerp((pow((_GlowRGB_var.rgb*1.2*_GlowColor.rgb),_Glowpower)+node_3674),node_3674,step(_GlowEdgeWidth,node_2942));
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
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _DiffuseRGBA; uniform float4 _DiffuseRGBA_ST;
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _MaskClip;
            uniform fixed _2UV;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _LightControl;
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
                float2 _2UV_var = lerp( i.uv0, i.uv1, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(_2UV_var, _Mask));
                float node_2942 = (_MaskClip*(_NoiseR_var.r+_LightControl)*(_LightControl+_Mask_var.r));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                clip((node_2942*_DiffuseRGBA_var.a) - 0.5);
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
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _DiffuseRGBA; uniform float4 _DiffuseRGBA_ST;
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _MaskClip;
            uniform fixed _2UV;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _LightControl;
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
                float2 _2UV_var = lerp( i.uv0, i.uv1, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(_2UV_var, _Mask));
                float node_2942 = (_MaskClip*(_NoiseR_var.r+_LightControl)*(_LightControl+_Mask_var.r));
                float4 _DiffuseRGBA_var = tex2D(_DiffuseRGBA,TRANSFORM_TEX(i.uv0, _DiffuseRGBA));
                clip((node_2942*_DiffuseRGBA_var.a) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
