Shader "LP/Role/Role_Outline_Color" {
    Properties {
        _BassRGB ("Bass-RGB", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Float) = 0.002
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _Soft ("Soft", Range(0.0, 1)) = 1
        _Color("color multiply", Color) = (1, 1, 1, 1)
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest+10"
            "RenderType"="Opaque"
        }
        LOD 300
        UsePass "Hidden/LP/Common/ROLEBASECOLOR"        
        UsePass "Hidden/LP/Common/OUTLINE"
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest+10"
            "RenderType"="Opaque"
        }
        LOD 200
        UsePass "Hidden/LP/Common/ROLEBASECOLOR"
    }
    FallBack "A1/VertexLit"
}
