Shader "EJoyShader/Effect/Soft/SoftEdge-2T-DarkBlender_rot" {
    Properties {
        _BaseRGBA ("Base-RGBA", 2D) = "white" {}
        _ShapeRGBA ("Shape-RGBA", 2D) = "white" {}
        _Alpha ("Alpha", Range(0, 10)) = 1
        _BaseColor ("BaseColor", Color) = (1,1,1,1)
        _RotationSpeed ("Rotation Speed", Float) = 2.0
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
            Blend SrcAlpha OneMinusSrcAlpha
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
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform float _Alpha;
            uniform sampler2D _ShapeRGBA; uniform float4 _ShapeRGBA_ST;
            uniform float4 _BaseColor;
            uniform float _RotationSpeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
               
                float dt = _RotationSpeed*_Time;
                float s = sin(dt);
                float c = cos(dt);
                float2x2 rotationMatrix = float2x2( c, -s, s, c);
                o.uv1.xy = mul ( v.texcoord0.xy - 0.5, rotationMatrix ) + 0.5; 

                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv1, _BaseRGBA));
                float3 emissive = (_BaseRGBA_var.rgb*_BaseColor.rgb);
                float3 finalColor = emissive;
                float4 _ShapeRGBA_var = tex2D(_ShapeRGBA,TRANSFORM_TEX(i.uv0, _ShapeRGBA));
                return fixed4(finalColor,((_BaseRGBA_var.a*_ShapeRGBA_var.r)*_Alpha));
            }
            ENDCG
        }
    }
}
