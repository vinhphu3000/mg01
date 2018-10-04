// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:False,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:3,bsrc:0,bdst:6,culm:0,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:32914,y:32715,varname:node_4883,prsc:2|emission-7801-OUT;n:type:ShaderForge.SFN_Tex2d,id:1114,x:31371,y:32554,ptovrint:False,ptlb:Base(RGBA),ptin:_BaseRGBA,varname:_BassRGB,prsc:2,tex:d1c5c311c0cb8fa4fac17e1ca4a70c9f,ntxv:0,isnm:False|UVIN-4623-OUT;n:type:ShaderForge.SFN_ViewVector,id:9327,x:30168,y:32853,varname:node_9327,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4406,x:30171,y:33061,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4011,x:30461,y:32951,varname:node_4011,prsc:2,dt:4|A-9327-OUT,B-4406-OUT;n:type:ShaderForge.SFN_Lerp,id:7801,x:31949,y:32793,varname:node_7801,prsc:2|A-1554-OUT,B-6498-OUT,T-5370-OUT;n:type:ShaderForge.SFN_Slider,id:608,x:30345,y:33298,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:_Edge,prsc:2,min:1,cur:1.3,max:2;n:type:ShaderForge.SFN_Power,id:5370,x:31488,y:33059,varname:node_5370,prsc:2|VAL-414-OUT,EXP-3985-OUT;n:type:ShaderForge.SFN_Slider,id:3985,x:30945,y:33399,ptovrint:False,ptlb:Soft,ptin:_Soft,varname:_Soft,prsc:2,min:0.01,cur:9,max:40;n:type:ShaderForge.SFN_Multiply,id:980,x:30787,y:32969,varname:node_980,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Clamp01,id:414,x:31001,y:32888,varname:node_414,prsc:2|IN-980-OUT;n:type:ShaderForge.SFN_Tex2d,id:3695,x:31367,y:32101,ptovrint:False,ptlb:Edge(RGB),ptin:_EdgeRGB,varname:node_3695,prsc:2,tex:342e2d671ce84e2418f30c0f78d34955,ntxv:0,isnm:False|UVIN-9993-OUT;n:type:ShaderForge.SFN_ScreenPos,id:42,x:30885,y:32209,varname:node_42,prsc:2,sctp:0;n:type:ShaderForge.SFN_SwitchProperty,id:9993,x:31115,y:32149,ptovrint:False,ptlb:Screen_Out,ptin:_Screen_Out,varname:node_9993,prsc:2,on:True|A-3925-UVOUT,B-42-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3925,x:30867,y:32054,varname:node_3925,prsc:2,uv:0;n:type:ShaderForge.SFN_Color,id:2632,x:31375,y:32830,ptovrint:False,ptlb:InnerColor,ptin:_InnerColor,varname:node_2632,prsc:2,glob:False,c1:0.01171399,c2:0.006433822,c3:0.05147058,c4:1;n:type:ShaderForge.SFN_Color,id:4052,x:31378,y:32363,ptovrint:False,ptlb:OutColor,ptin:_OutColor,varname:node_4052,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1554,x:31669,y:32318,varname:node_1554,prsc:2|A-3695-RGB,B-4052-RGB;n:type:ShaderForge.SFN_Multiply,id:6498,x:31648,y:32699,varname:node_6498,prsc:2|A-1114-RGB,B-2632-RGB,C-1114-A;n:type:ShaderForge.SFN_SwitchProperty,id:4623,x:31101,y:32584,ptovrint:False,ptlb:Sceen_Inner,ptin:_Sceen_Inner,varname:_Sceen_copy,prsc:2,on:True|A-929-UVOUT,B-436-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:929,x:30894,y:32498,varname:node_929,prsc:2,uv:0;n:type:ShaderForge.SFN_ScreenPos,id:436,x:30900,y:32655,varname:node_436,prsc:2,sctp:0;proporder:1114-4623-3695-9993-608-3985-2632-4052;pass:END;sub:END;*/

Shader "EJoyShader/Role/Role-Rim-2T-ScreenEdge" {
    Properties {
        _BaseRGBA ("Base(RGBA)", 2D) = "white" {}
        [MaterialToggle] _Sceen_Inner ("Sceen_Inner", Float ) = 0
        _EdgeRGB ("Edge(RGB)", 2D) = "white" {}
        [MaterialToggle] _Screen_Out ("Screen_Out", Float ) = 0
        _Edge ("Edge", Range(1, 2)) = 1.3
        _Soft ("Soft", Range(0.01, 40)) = 9
        _InnerColor ("InnerColor", Color) = (0.01171399,0.006433822,0.05147058,1)
        _OutColor ("OutColor", Color) = (1,1,1,1)
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform float _Edge;
            uniform float _Soft;
            uniform sampler2D _EdgeRGB; uniform float4 _EdgeRGB_ST;
            uniform fixed _Screen_Out;
            uniform float4 _InnerColor;
            uniform float4 _OutColor;
            uniform fixed _Sceen_Inner;
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
                float4 screenPos : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float2 _Screen_Out_var = lerp( i.uv0, i.screenPos.rg, _Screen_Out );
                float4 _EdgeRGB_var = tex2D(_EdgeRGB,TRANSFORM_TEX(_Screen_Out_var, _EdgeRGB));
                float2 _Sceen_Inner_var = lerp( i.uv0, i.screenPos.rg, _Sceen_Inner );
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(_Sceen_Inner_var, _BaseRGBA));
                float3 emissive = lerp((_EdgeRGB_var.rgb*_OutColor.rgb),(_BaseRGBA_var.rgb*_InnerColor.rgb*_BaseRGBA_var.a),pow(saturate((0.5*dot(viewDirection,i.normalDir)+0.5*_Edge)),_Soft));
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
