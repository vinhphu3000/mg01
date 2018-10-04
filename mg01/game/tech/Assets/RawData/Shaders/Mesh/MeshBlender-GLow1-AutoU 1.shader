// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5297399,fgcg:0.4848616,fgcb:0.8676471,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1528,x:32899,y:32616,varname:node_1528,prsc:2|diff-3009-RGB,emission-6076-OUT;n:type:ShaderForge.SFN_Tex2d,id:3009,x:32177,y:32246,ptovrint:False,ptlb:BaseRGBA,ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,tex:1ffb70c6266abdd41b3768331e81815a,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3458,x:32132,y:32531,ptovrint:False,ptlb:GlowRGB,ptin:_GlowRGB,varname:_GlowRGB,prsc:2,tex:33743177ea5ffc349bd748cd7c4eb9ec,ntxv:0,isnm:False|UVIN-9653-UVOUT;n:type:ShaderForge.SFN_Multiply,id:9706,x:32429,y:32506,varname:node_9706,prsc:2|A-3009-A,B-3458-RGB,C-33-RGB;n:type:ShaderForge.SFN_Color,id:33,x:32097,y:32796,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1899,x:31840,y:32735,varname:node_1899,prsc:2|A-4658-T,B-581-OUT;n:type:ShaderForge.SFN_Slider,id:581,x:31461,y:32825,ptovrint:False,ptlb:Auto-Speed,ptin:_AutoSpeed,varname:_AutoSpeed,prsc:2,min:-1,cur:-0.2,max:1;n:type:ShaderForge.SFN_Panner,id:9653,x:31851,y:32514,varname:node_9653,prsc:2,spu:1,spv:0|UVIN-8149-OUT,DIST-1899-OUT;n:type:ShaderForge.SFN_TexCoord,id:9462,x:31356,y:32218,varname:node_9462,prsc:2,uv:0;n:type:ShaderForge.SFN_VertexColor,id:6481,x:32397,y:32796,varname:node_6481,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6076,x:32639,y:32665,varname:node_6076,prsc:2|A-9706-OUT,B-6481-A;n:type:ShaderForge.SFN_SwitchProperty,id:8149,x:31602,y:32243,ptovrint:False,ptlb:useUV2,ptin:_useUV2,varname:_useUV2,prsc:2,on:True|A-9462-UVOUT,B-635-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:635,x:31352,y:32386,varname:node_635,prsc:2,uv:1;n:type:ShaderForge.SFN_Time,id:4658,x:31563,y:32589,varname:node_4658,prsc:2;proporder:3009-3458-33-581-8149;pass:END;sub:END;*/

Shader "EJoyShader/Mesh/MeshGlow-Blender-AutoU" {
    Properties {
        _BaseRGBA ("BaseRGBA", 2D) = "white" {}
        _GlowRGB ("GlowRGB", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _AutoSpeed ("Auto-Speed", Range(-1, 1)) = -0.2
        [MaterialToggle] _useUV2 ("useUV2", Float ) = 0
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float4 _Color;
            uniform float _AutoSpeed;
            uniform fixed _useUV2;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(4,5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
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
                float3 normalDirection = i.normalDir;
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
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float3 diffuseColor = _BaseRGBA_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 node_4658 = _Time + _TimeEditor;
                float2 node_9653 = (lerp( i.uv0, i.uv1, _useUV2 )+(node_4658.g*_AutoSpeed)*float2(1,0));
                float4 _GlowRGB_var = tex2D(_GlowRGB,TRANSFORM_TEX(node_9653, _GlowRGB));
                float3 emissive = ((_BaseRGBA_var.a*_GlowRGB_var.rgb*_Color.rgb)*i.vertexColor.a);
/// Final Color:
                float3 finalColor = diffuse + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
