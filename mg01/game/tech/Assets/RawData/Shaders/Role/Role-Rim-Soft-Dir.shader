// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:False,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:32725,y:32712,varname:node_4883,prsc:2|emission-7801-OUT;n:type:ShaderForge.SFN_Tex2d,id:1114,x:31041,y:32044,ptovrint:False,ptlb:Bass-RGB,ptin:_BassRGB,varname:_BassRGB,prsc:2,tex:d1c5c311c0cb8fa4fac17e1ca4a70c9f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ViewVector,id:9327,x:30636,y:32725,varname:node_9327,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4406,x:30645,y:32894,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4011,x:30935,y:32784,varname:node_4011,prsc:2,dt:4|A-9327-OUT,B-4406-OUT;n:type:ShaderForge.SFN_Lerp,id:7801,x:32339,y:32904,varname:node_7801,prsc:2|A-9344-OUT,B-1227-OUT,T-5370-OUT;n:type:ShaderForge.SFN_Color,id:5440,x:31773,y:32799,ptovrint:False,ptlb:Rim-Color,ptin:_RimColor,varname:_RimColor,prsc:2,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:608,x:30819,y:33131,ptovrint:False,ptlb:Rim-Edge,ptin:_RimEdge,varname:_RimEdge,prsc:2,min:1,cur:1.5,max:2;n:type:ShaderForge.SFN_Power,id:5370,x:31918,y:33058,varname:node_5370,prsc:2|VAL-414-OUT,EXP-3985-OUT;n:type:ShaderForge.SFN_Add,id:1655,x:31537,y:33205,varname:node_1655,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Slider,id:3985,x:31375,y:33398,ptovrint:False,ptlb:Rim-Soft,ptin:_RimSoft,varname:_RimSoft,prsc:2,min:0.01,cur:2,max:40;n:type:ShaderForge.SFN_Multiply,id:980,x:31301,y:32852,varname:node_980,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Clamp01,id:414,x:31535,y:32842,varname:node_414,prsc:2|IN-980-OUT;n:type:ShaderForge.SFN_Add,id:9344,x:31976,y:32755,varname:node_9344,prsc:2|A-5440-RGB,B-1114-RGB;n:type:ShaderForge.SFN_Vector4Property,id:5199,x:30498,y:32023,ptovrint:False,ptlb:V-Lighting-Direct,ptin:_VLightingDirect,varname:_VLightingDirect,prsc:2,glob:False,v1:0,v2:-6,v3:-8,v4:0;n:type:ShaderForge.SFN_Dot,id:3524,x:30697,y:32144,varname:node_3524,prsc:2,dt:0|A-5199-XYZ,B-8007-OUT;n:type:ShaderForge.SFN_NormalVector,id:8007,x:30502,y:32221,prsc:2,pt:False;n:type:ShaderForge.SFN_Lerp,id:1227,x:32027,y:32447,varname:node_1227,prsc:2|A-2579-OUT,B-9858-OUT,T-5241-OUT;n:type:ShaderForge.SFN_Color,id:6504,x:31286,y:32275,ptovrint:False,ptlb:Ambinet,ptin:_Ambinet,varname:_Ambinet,prsc:2,glob:False,c1:0.3078504,c2:0.3189014,c3:0.5367647,c4:1;n:type:ShaderForge.SFN_Clamp01,id:5241,x:31386,y:32522,varname:node_5241,prsc:2|IN-9054-OUT;n:type:ShaderForge.SFN_Blend,id:9858,x:31833,y:32292,varname:node_9858,prsc:2,blmd:1,clmp:True|SRC-1114-RGB,DST-6504-RGB;n:type:ShaderForge.SFN_Multiply,id:7786,x:30875,y:32297,varname:node_7786,prsc:2|A-3524-OUT,B-2742-OUT;n:type:ShaderForge.SFN_Slider,id:2742,x:30492,y:32458,ptovrint:False,ptlb:Soft,ptin:_Soft,varname:_Soft,prsc:2,min:0,cur:0.5128205,max:2;n:type:ShaderForge.SFN_Add,id:9054,x:31117,y:32521,varname:node_9054,prsc:2|A-7786-OUT,B-6867-OUT;n:type:ShaderForge.SFN_Slider,id:6867,x:30643,y:32599,ptovrint:False,ptlb:Offset,ptin:_Offset,varname:_Offset,prsc:2,min:-5,cur:0.5299146,max:5;n:type:ShaderForge.SFN_Blend,id:2579,x:31765,y:32044,varname:node_2579,prsc:2,blmd:12,clmp:True|SRC-1114-RGB,DST-5079-RGB;n:type:ShaderForge.SFN_Color,id:5079,x:31336,y:31823,ptovrint:False,ptlb:Light,ptin:_Light,varname:_Light,prsc:2,glob:False,c1:1,c2:0.9742965,c3:0.2867647,c4:1;n:type:ShaderForge.SFN_Add,id:7309,x:31754,y:31768,varname:node_7309,prsc:2|A-5079-RGB,B-1114-RGB;proporder:1114-5079-6504-2742-6867-5199-5440-608-3985;pass:END;sub:END;*/

Shader "EJoyShader/Role/Rim-Soft-Dir" {
    Properties {
        _BassRGB ("Bass-RGB", 2D) = "white" {}
        _Light ("Light", Color) = (1,0.9742965,0.2867647,1)
        _Ambinet ("Ambinet", Color) = (0.3078504,0.3189014,0.5367647,1)
        _Soft ("Soft", Range(0, 2)) = 0.5128205
        _Offset ("Offset", Range(-5, 5)) = 0.5299146
        _VLightingDirect ("V-Lighting-Direct", Vector) = (0,-6,-8,0)
        _RimColor ("Rim-Color", Color) = (1,0,0,1)
        _RimEdge ("Rim-Edge", Range(1, 2)) = 1.5
        _RimSoft ("Rim-Soft", Range(0.01, 40)) = 2
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BassRGB; uniform float4 _BassRGB_ST;
            uniform float4 _RimColor;
            uniform float _RimEdge;
            uniform float _RimSoft;
            uniform float4 _VLightingDirect;
            uniform float4 _Ambinet;
            uniform float _Soft;
            uniform float _Offset;
            uniform float4 _Light;
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
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _BassRGB_var = tex2D(_BassRGB,TRANSFORM_TEX(i.uv0, _BassRGB));
                float node_4011 = 0.5*dot(viewDirection,i.normalDir)+0.5;
                float3 emissive = lerp((_RimColor.rgb+_BassRGB_var.rgb),lerp(saturate((_BassRGB_var.rgb > 0.5 ?  (1.0-(1.0-2.0*(_BassRGB_var.rgb-0.5))*(1.0-_Light.rgb)) : (2.0*_BassRGB_var.rgb*_Light.rgb)) ),saturate((_BassRGB_var.rgb*_Ambinet.rgb)),saturate(((dot(_VLightingDirect.rgb,i.normalDir)*_Soft)+_Offset))),pow(saturate((node_4011*_RimEdge)),_RimSoft));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
