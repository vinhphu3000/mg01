Shader "EJoyShader/Effect/Soft/SoftEdge-2T-DarkAdd_rot_UI" {
    Properties {
        _BaseRGBA ("Base-RGBA", 2D) = "white" {}
        _ShapeRGB ("Shape-RGB", 2D) = "white" {}
        _Alpha ("Alpha", Range(0, 10)) = 6.74703
        _BaseColor ("BaseColor", Color) = (0.5,0.5,0.5,1)
		_RotationSpeed ("Rotation Speed", Float) = 2.0
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
            uniform sampler2D _BaseRGBA;
             uniform float4 _BaseRGBA_ST;
            uniform float _Alpha;
            uniform sampler2D _ShapeRGB;
            uniform float4 _ShapeRGB_ST;
            uniform float4 _BaseColor;
       		uniform float _RotationSpeed;
			uniform float4 _Clip;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
				float3 world_pos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				o.world_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv0 = v.texcoord0;

                float dt = _RotationSpeed*_Time;
			    float s = sin(dt);
			    float c = cos(dt);
			    float2x2 rotationMatrix = float2x2( c, -s, s, c);
			    o.uv1.xy = mul ( v.texcoord0.xy - 0.5, rotationMatrix ) + 0.5; 

                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR{
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv1, _BaseRGBA));
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
}
