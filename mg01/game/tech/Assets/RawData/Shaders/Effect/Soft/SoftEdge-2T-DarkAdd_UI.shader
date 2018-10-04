// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:3,spmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:False,hqlp:True,tesm:0,blpr:3,bsrc:0,bdst:6,culm:2,dpts:2,wrdp:False,dith:0,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2306556,fgcg:0.1892301,fgcb:0.7352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:40,x:32027,y:32574,varname:node_40,prsc:2|emission-9942-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:31195,y:31985,ptovrint:True,ptlb:Base-RGBA,ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,tex:f1b8bc6eadeddb24e9f3ba34972e9292,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:218,x:31485,y:32528,varname:node_218,prsc:2|A-2950-OUT,B-6096-OUT;n:type:ShaderForge.SFN_Slider,id:6096,x:31052,y:32677,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:_Alpha,prsc:2,min:0,cur:6.74703,max:10;n:type:ShaderForge.SFN_VertexColor,id:4009,x:31459,y:32782,varname:node_4009,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9942,x:31757,y:32715,varname:node_9942,prsc:2|A-218-OUT,B-4009-A,C-6505-RGB,D-4009-RGB;n:type:ShaderForge.SFN_Tex2d,id:5290,x:30938,y:32389,ptovrint:False,ptlb:Shape-RGB,ptin:_ShapeRGB,varname:_ShapeRGB,prsc:2,tex:3720f407e2e235248ab00cb5ad9aed46,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2950,x:31204,y:32302,varname:node_2950,prsc:2|A-217-RGB,B-5290-RGB,C-217-A,D-5290-A;n:type:ShaderForge.SFN_Color,id:6505,x:31450,y:33010,ptovrint:False,ptlb:BaseColor,ptin:_BaseColor,varname:_BaseColor,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Normalize,id:1019,x:31507,y:33584,varname:node_1019,prsc:2;proporder:217-5290-6096-6505;pass:END;sub:END;*/

Shader "EJoyShader/Effect/Soft/SoftEdge-2T-DarkAdd_UI" {
    Properties {
        _BaseRGBA ("Base-RGBA", 2D) = "white" {}
        _ShapeRGB ("Shape-RGB", 2D) = "white" {}
        _Alpha ("Alpha", Range(0, 10)) = 6.74703
        _BaseColor ("BaseColor", Color) = (0.5,0.5,0.5,1)
		_Clip("ClipRange", Vector) = (-10,-10,10,10)
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
            Blend One OneMinusSrcColor
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform float _Alpha;
            uniform sampler2D _ShapeRGB; uniform float4 _ShapeRGB_ST;
            uniform float4 _BaseColor;
			uniform float4 _Clip;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
				float3 world_pos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				o.world_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR{
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float4 _ShapeRGB_var = tex2D(_ShapeRGB,TRANSFORM_TEX(i.uv0, _ShapeRGB));
                float3 emissive = (((_BaseRGBA_var.rgb*_ShapeRGB_var.rgb*_BaseRGBA_var.a*_ShapeRGB_var.a)*_Alpha)*i.vertexColor.a*_BaseColor.rgb*i.vertexColor.rgb);
                float3 finalColor = emissive;
				float alpha = 1;
				alpha *= (i.world_pos.x >= _Clip.x);
				alpha *= (i.world_pos.y >= _Clip.y);
				alpha *= (i.world_pos.x <= _Clip.z);
				alpha *= (i.world_pos.y <= _Clip.w);
				finalColor.xyz *= alpha;
				return fixed4(finalColor, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
