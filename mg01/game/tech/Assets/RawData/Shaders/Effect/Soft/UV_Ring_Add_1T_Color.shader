// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:34772,y:32653,varname:node_4013,prsc:2|emission-3186-OUT;n:type:ShaderForge.SFN_TexCoord,id:9740,x:30526,y:32829,varname:node_9740,prsc:2,uv:0;n:type:ShaderForge.SFN_ComponentMask,id:7986,x:30997,y:32858,varname:node_7986,prsc:2,cc1:1,cc2:0,cc3:-1,cc4:-1|IN-1283-OUT;n:type:ShaderForge.SFN_RemapRange,id:1283,x:30697,y:32829,varname:node_1283,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-9740-UVOUT;n:type:ShaderForge.SFN_Length,id:5912,x:31381,y:32837,varname:node_5912,prsc:2|IN-7986-OUT;n:type:ShaderForge.SFN_OneMinus,id:31,x:31766,y:32807,varname:node_31,prsc:2|IN-5912-OUT;n:type:ShaderForge.SFN_Slider,id:4627,x:31558,y:32575,ptovrint:False,ptlb:width,ptin:_width,varname:node_4627,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:4.046204,max:30;n:type:ShaderForge.SFN_Multiply,id:2657,x:32124,y:32739,varname:node_2657,prsc:2|A-4627-OUT,B-31-OUT;n:type:ShaderForge.SFN_OneMinus,id:4292,x:32571,y:32841,varname:node_4292,prsc:2|IN-2657-OUT;n:type:ShaderForge.SFN_Multiply,id:7857,x:33127,y:33293,varname:node_7857,prsc:2|A-3970-OUT,B-4292-OUT;n:type:ShaderForge.SFN_Floor,id:6851,x:32046,y:33255,varname:node_6851,prsc:2|IN-5912-OUT;n:type:ShaderForge.SFN_OneMinus,id:3970,x:32315,y:33278,varname:node_3970,prsc:2|IN-6851-OUT;n:type:ShaderForge.SFN_Multiply,id:5831,x:33319,y:33607,varname:node_5831,prsc:2|A-7857-OUT,B-9645-OUT;n:type:ShaderForge.SFN_Slider,id:9645,x:32297,y:33709,ptovrint:False,ptlb:power,ptin:_power,varname:_node_1327_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2d,id:815,x:34734,y:34024,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_Tex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3264-OUT;n:type:ShaderForge.SFN_Multiply,id:3186,x:35161,y:33634,varname:node_3186,prsc:2|A-815-RGB,B-9355-OUT,C-9553-OUT,D-8774-RGB;n:type:ShaderForge.SFN_ArcTan2,id:528,x:31794,y:34049,varname:node_528,prsc:2,attp:3|A-7986-R,B-7986-G;n:type:ShaderForge.SFN_RemapRange,id:707,x:32157,y:34053,varname:node_707,prsc:2,frmn:-3.14,frmx:3.14,tomn:0,tomx:1|IN-528-OUT;n:type:ShaderForge.SFN_Append,id:3264,x:34249,y:34016,varname:node_3264,prsc:2|A-707-OUT,B-3657-OUT;n:type:ShaderForge.SFN_Clamp01,id:3657,x:33967,y:33711,varname:node_3657,prsc:2|IN-7857-OUT;n:type:ShaderForge.SFN_Add,id:9227,x:31565,y:33080,varname:node_9227,prsc:2|A-5912-OUT,B-76-OUT;n:type:ShaderForge.SFN_Vector1,id:76,x:31404,y:33213,varname:node_76,prsc:2,v1:0.001;n:type:ShaderForge.SFN_Floor,id:7263,x:31735,y:33447,varname:node_7263,prsc:2|IN-9227-OUT;n:type:ShaderForge.SFN_OneMinus,id:9553,x:32089,y:33494,varname:node_9553,prsc:2|IN-7263-OUT;n:type:ShaderForge.SFN_Color,id:8774,x:35031,y:33258,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_8774,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Clamp01,id:9355,x:33747,y:33316,varname:node_9355,prsc:2|IN-5831-OUT;proporder:4627-9645-815-8774;pass:END;sub:END;*/

Shader "EJoyShader/Effect/Soft/UV_Ring_Add_1T_Color" {
    Properties {
        _width ("width", Range(1, 30)) = 4.046204
        _power ("power", Range(0, 1)) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 2.0
            uniform float _width;
            uniform float _power;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float2 node_7986 = (i.uv0*2.0+-1.0).gr;
                float node_5912 = length(node_7986);
                float node_7857 = ((1.0 - floor(node_5912))*(1.0 - (_width*(1.0 - node_5912))));
                float2 node_3264 = float2(((1-abs(atan2(node_7986.r,node_7986.g)/3.14159265359))*0.1592357+0.5),saturate(node_7857));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_3264, _MainTex));
                float3 emissive = (_MainTex_var.rgb*saturate((node_7857*_power))*(1.0 - floor((node_5912+0.001)))*_Color.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
