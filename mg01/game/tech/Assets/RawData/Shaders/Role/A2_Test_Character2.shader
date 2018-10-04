Shader "A2/Character2" {
    Properties {
		_MainTex("Tex_Diffuse", 2D) = "white" {}
		_Tex_Reflection("_Tex_Reflection", 2D) = "_Skybox" {}
		_Tex_Mask("Tex_Mask", 2D) = "black" {}
		[MaterialToggle]  _Falloff("Falloff", Float) = 0
		_Falloff_range("Falloff_range", Float) = 5
		_Falloff_Color("Falloff_Color", Color) = (1,1,0.0705882,1)
		[MaterialToggle]  _Specular_Tex("Specular_Tex", Float) = 0.960784
		_Specular_Color("Specular_Color", Color) = (0.470588,0.449827,0.449827,1)
		_Gloss("Gloss", Float) = 0.4
		_Reflection_intensity("Reflection_intensity", Float) = 0.5
		_A2LightPos("LightPos", Vector) = (0.5,0.5,0.5, 1)
		_A2LightColor("LightColor", Color) = (0.5,0.5,0.5,1)
		_AmbientColor("Ambient", Color) = (1,1,1,1)
		//_OutlineColor("Outline Color", Color) = (0,0,0,1)
		//_Outline("Outline width", Range (0.0, 0.03)) = .0

		_RimColor ("Rim-Color", Color) = (1,0,0,1)
        _Edge ("Edge", Range(1, 2)) = 1.25
        _Soft ("Soft", Range(0.01, 40)) = 0
		_Color1("colorful 1", Color) = (1,1,1,1)
		_Color2("colorful 2", Color) = (0,0,0,0)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
				"LightMode" = "ForwardBase"
			}
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#pragma only_renderers glcore gles gles3 metal d3d9

			uniform float _Gloss;
			uniform sampler2D _Tex_Reflection;
			uniform sampler2D _MainTex;
			uniform float4 _Specular_Color;
			uniform float4 _Falloff_Color;
			uniform float _Specular_Tex;
			uniform sampler2D _Tex_Mask;
			uniform float _Falloff;
			uniform float _Reflection_intensity;
			uniform float _Falloff_range;
			uniform float4 _A2LightColor;
			uniform float4 _A2LightPos;
			uniform float4 _AmbientColor;
			uniform float4 _Color1;
			uniform float4 _Color2;
			uniform float4 _RimColor;
            uniform float _Edge;
            uniform float _Soft;


            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 _MainTex_UI : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
				float specPow : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o._MainTex_UI = v.texcoord0;
				o.normalDir = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal); //UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(UNITY_MATRIX_MV, v.vertex);
				o.specPow = exp2(((_Gloss * 10.0) + 1.0));
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
				float3 viewDirection = normalize(-i.posWorld.xyz);
				float3 negViewDir = -(viewDirection);
				float3 worldNormal = normalize(i.normalDir);
				float xlv_VertexOutput_specPow = i.specPow;
				float3 reflectDir = reflect(negViewDir, worldNormal);// (negViewDir - (2.0 * (dot(worldNormal, negViewDir)* worldNormal)));
				//reflectDir = normalize(reflectDir);

				float3 lightDirection_10 = normalize(_A2LightPos.xyz);

				float4 mainColor = tex2D(_MainTex, i._MainTex_UI);
				float4 maskRGB = tex2D (_Tex_Mask, i._MainTex_UI);
				float3 mc2 = _Color1.rgb*mainColor.a + _Color2.rgb*(1.0 - mainColor.a);
				mainColor.xyz = mainColor.rgb *(1.0 -maskRGB.a) + mainColor.rgb*mc2*maskRGB.a;
				float4 reflectRGB = tex2D (_Tex_Reflection, 0.5*(reflectDir.xz) + 0.5);
				float3 tmpvar_27 = saturate(1.0 - ((1.0 - mainColor.xyz)*(1.0 - ((reflectRGB.xyz * _Reflection_intensity) * maskRGB.x))));
				float3 node_1801_5 = (tmpvar_27 - mainColor.xyz);
				float x_29 = (1.0 - max(0.0, dot(worldNormal, viewDirection)));
				float3 fallOff = float3(_Falloff, _Falloff, _Falloff);
				float3 emissive_4 = lerp(node_1801_5, (((pow(x_29, _Falloff_range) * _Falloff_Color.xyz)*maskRGB.z) + node_1801_5), fallOff);
				float3 specTex = float3(_Specular_Tex, _Specular_Tex, _Specular_Tex);
				float3 specularColor_3 = (lerp(_Specular_Color.xyz, (mainColor.xyz + _Specular_Color.xyz), specTex) * maskRGB.y);
				float specPower = pow(max(0.0, dot(normalize((viewDirection + lightDirection_10)), worldNormal)), xlv_VertexOutput_specPow);
				float3 specular_2 = ((_A2LightColor.xyz * specPower) * specularColor_3);
				// float4 finalColor = (float4)1;

				float3 diffuse_7 = (max(0.0, dot(worldNormal, lightDirection_10)) * _A2LightColor.xyz) + _AmbientColor.xyz;
				float4 finalColor = float4((((diffuse_7*mainColor.xyz) + specular_2) + emissive_4), 1.0);

				float node_4011 = 0.5*dot(viewDirection,worldNormal)+0.5;
                finalColor.xyz = (lerp((_RimColor.rgb+finalColor.rgb),finalColor.rgb,pow(saturate((node_4011*_Edge)),_Soft)));
				return finalColor;
			}
            ENDCG
        }
    }
    FallBack "Mobile/Diffuse"
}
