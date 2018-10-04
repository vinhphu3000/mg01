Shader "A2/Hobbit" {
    Properties {
        _MaskRGBA ("MaskRGBA", 2D) = "white" {}
        _R ("R", 2D) = "white" {}
        _G ("G", 2D) = "white" {}
        _B ("B", 2D) = "white" {}
        _A ("A", 2D) = "bump" {}
        _MainColor ("Main-Color", Color) = (0.8308824,0.8308824,0.8308824,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 150
        Pass {
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 

            uniform sampler2D _A; uniform float4 _A_ST;
            uniform sampler2D _MaskRGBA; uniform float4 _MaskRGBA_ST;
            uniform sampler2D _G; uniform float4 _G_ST;
            uniform sampler2D _B; uniform float4 _B_ST;
            uniform sampler2D _R; uniform float4 _R_ST;
            uniform float4 _MainColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 pack0 : TEXCOORD0; // _Control _Splat0
                float4 pack1 : TEXCOORD1; // _Splat1 _Splat2
                float4 pack2 : TEXCOORD2; // _Splat3 _Splat4
                float4 lmap : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                UNITY_INITIALIZE_OUTPUT(VertexOutput,o);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MaskRGBA);
                o.pack0.zw = TRANSFORM_TEX(v.texcoord, _A);
                o.pack1.xy = TRANSFORM_TEX(v.texcoord, _G);
                o.pack1.zw = TRANSFORM_TEX(v.texcoord, _B);
                o.pack2.xy = TRANSFORM_TEX(v.texcoord, _R);
                o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {

                float4 _MaskRGBA_var = tex2D(_MaskRGBA, i.pack0.xy);
                float4 _A_var = tex2D(_A, i.pack0.zw);
                float4 _R_var = tex2D(_R, i.pack1.xy);
                float4 _G_var = tex2D(_G, i.pack1.zw);
                float4 _B_var = tex2D(_B, i.pack2.xy);
                float3 diffuse = ((lerp( lerp( lerp( _A_var.rgb, _R_var.rgb, _MaskRGBA_var.rgb.r ), _G_var.rgb, _MaskRGBA_var.rgb.g ), _B_var.rgb, _MaskRGBA_var.rgb.b ))*_MainColor.rgb);
                
                fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap);
                half3 bakedColor = DecodeLightmap(bakedColorTex);
                fixed4 c = fixed4(1.0, 1.0, 1.0, 1.0);
                c.rgb = bakedColor.rgb *diffuse;

                UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            ENDCG
        }
    }
    Fallback "A1/VertexLit"
}
