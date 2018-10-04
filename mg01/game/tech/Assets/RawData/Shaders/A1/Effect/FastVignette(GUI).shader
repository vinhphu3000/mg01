Shader "A1/ImageEffect/GUI Layer/Fast Vignette"
{
	Properties
	{
		_Color( "Main Color", Color ) = ( 0, 0, 0, 1 )

		_sharpness( "Sharpness", Range( -1, 1 ) ) = 0.35		// copy from CC_FastVignette
		_darkness( "Darkness", Range( 0, 2 ) ) = 0.4
	}
	
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue"				= "Transparent"
			"IgnoreProjector"	= "True"
			"RenderType"		= "Transparent"
		}
		
		Cull		Off
		Lighting	Off
		ZWrite		Off
		Fog			{ Mode Off }
		Blend		SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma only_renderers glcore gles gles3 metal d3d9
				#pragma fragmentoption ARB_precision_hint_fastest 
				
				#include "UnityCG.cginc"
	
				struct appdata_t
				{
					fixed4		vertex : POSITION;
				};
	
				struct v2f
				{
					fixed4		vertex : SV_POSITION;
					fixed2		texcoord : TEXCOORD0;
				};
	
				fixed4		_Color;

				fixed		_sharpness;
				fixed		_darkness;
				
				v2f vert( appdata_t v )
				{
					v2f		o;

					o.vertex = mul( UNITY_MATRIX_MVP, v.vertex );
					o.texcoord = o.vertex * 0.5;

					return o;
				}
				
				fixed4 frag( v2f i ) : COLOR
				{
					fixed		d = distance( i.texcoord, fixed2( 0, 0 ) );
					fixed		darkness = smoothstep( _sharpness * 0.799, 0.8, d * ( _darkness + _sharpness ) );

					return fixed4( _Color.rgb, _Color.a * darkness );
				}
			ENDCG
		}
	}

	FallBack off
}