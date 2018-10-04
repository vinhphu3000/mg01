// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

// Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

#ifndef PROJ_X_CG_INCLUDE
#define PROJ_X_CG_INCLUDE

#include "ProjX_Mid.cginc"

//#define PX_TARGET30

// 存放ProjX的VS和PS

// UVFrame_XXX.shader相关----------------------------------------------START
//#define PX_UVFrameProperty \
//	PX_BaseTex ("Base(RGBA)", 2D) = "white" {}	\
//	PX_FrameX("XNum", Float) = 2				\
//	PX_FrameY("YNum", Float) = 2				\
//	PX_FrameSpeed("Rate", Float) = 10			\
//	PX_MaterialColor ("Material_Color", Color) = (1,1,1,1.0)

// 为Frame材质专门提供的参数
half _SizeX;		// 关键帧贴图的x轴单图数目
half _SizeY;		// 关键帧贴图的y轴单图数目
half _Speed;		// 关键帧贴图的速度

// ly_2014_12_01 勿删,此代码为带顶点色的帧序列的vs
//PX_T_VC_outvs Effect_Vert_T_UVFrame(PX_T_VC_invs i)
//{
//	PX_T_VC_outvs o;	
//	//Set position.
//	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
//	
//	//Set texcoord.
//	int ncount = floor(max(0, (_Time.y - _StartTime)) * _Speed);
//	int ny = ncount / _SizeX;
//	int nx = ncount - ny * _SizeX;
//	half2 uvScale = float2(i.texcoord.x / _SizeX, i.texcoord.y / _SizeY);
//	uvScale.x += nx / _SizeX;
//	uvScale.y -= ny / _SizeY;
//	o.texcoord = uvScale;
//	
//	o.color = i.color;
//
//	return o;
//}
// 无顶点色的帧序列的vs
PX_T_outvs Effect_Vert_T_UVFrame_NoVC(PX_T_invs i)
{
	PX_T_outvs o;	
	//Set position.
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	
	//Set texcoord.
	half2 krevertxy = half2(1.0/_SizeX, 1.0/_SizeY);
	half2 uvScale = i.texcoord * krevertxy;
	int ncount = floor(max(0, (_Time.y - _StartTime)) * _Speed);
	int ny = ncount * krevertxy.x;
	int nx = ncount - ny * _SizeX;
//	uvScale.x += nx / _SizeX;
//	uvScale.y -= ny / _SizeY;
//	o.texcoord = uvScale;//i.texcoord;//
	
	o.texcoord.x = uvScale.x + nx * krevertxy.x;
	o.texcoord.y = uvScale.y - (ny + 1) * krevertxy.y + 1;

	return o;
}
// UVFrame_XXX.shader相关----------------------------------------------END

// Lightmap相关--------------------------------------------------------START
lm_vsout Vert_Lightmap(PX_NTT_invs v)
{
	lm_vsout o;
	
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.texcoord.xy = v.texcoord;
#ifdef LIGHTMAP_OFF
	//--------------- Dir光照的 NDotL
	o.texcoord.z = dot(PX_CalcWorldNormal(v.normal), _WorldSpaceLightPos0);
	//	o.worldNormal = PX_CalcWorldNormal(v.normal);//normalize( PX_mulVec3(_Object2World, v.normal).xyz );
	//---------------
#else
	o.uv2_LightMap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;		
#endif
    
	return o;
}

lm_vsout_vc Vert_Lightmap_VC(PX_NTT_VC_invs v)
{
	lm_vsout_vc o;
	
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.texcoord.xy = v.texcoord;
#ifdef LIGHTMAP_OFF
	//--------------- Dir光照的 NDotL
	o.texcoord.z = dot(PX_CalcWorldNormal(v.normal), _WorldSpaceLightPos0);
	//---------------
#else
	o.uv2_LightMap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;		
#endif
    
    o.color = v.color;
    
	return o;
}

fixed4 Frag_LightMap_NoA( lm_vsout IN ) : COLOR
{
	fixed4 c = tex2D (_MainTex, IN.texcoord);
	// ly_2014_12_10 把Clip去掉
//	PX_ClipAlpha_NoA(c.a);
	c *= _MaterialColor;

#ifdef LIGHTMAP_OFF	
	PX_Ambienting(c.rgb);
//	PX_LightDir(IN.worldNormal, c.rgb);
	c.rgb += IN.texcoord.z * _LightColor0;
#else
	//	fixed4 color;
	fixed3 lm = PX_DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,IN.uv2_LightMap)); 
	c.rgb = c.rgb * lm;
//	return fixed4(lm,1);
#endif
	
	return c;
}
fixed4 Frag_LightMap_VC( lm_vsout_vc IN ) : COLOR
{
	fixed4 c = tex2D (_MainTex, IN.texcoord);
	// ly_2014_12_10 把Clip去掉
//	PX_ClipAlpha_NoA(c.a);
	c *= _MaterialColor;

#ifdef LIGHTMAP_OFF	
	PX_Ambienting(c.rgb);
//	PX_LightDir(IN.worldNormal, c.rgb);
	c.rgb += IN.texcoord.z * _LightColor0;
#else
	//	fixed4 color;
	fixed3 lm = PX_DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,IN.uv2_LightMap)); 
	c.rgb = c.rgb * lm;
//	return fixed4(lm,1);
#endif

	c *= IN.color;
		
	return c;
}


// rimlight+lightmap
PX_TNVT_outvs Vert_Rimlight_Lightmap(PX_NTT_invs v)
{
	PX_TNVT_outvs o;
	
	//Set position.
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.texcoord.xy = v.texcoord;
	o.normal_view = PX_CalcViewNormal(v.normal);
    o.viewDir_view = PX_CalcViewDir(v.vertex);
#ifdef LIGHTMAP_OFF
	//--------------- Dir光照的 NDotL
	o.texcoord.z = dot(PX_CalcWorldNormal(v.normal), _WorldSpaceLightPos0);
	//---------------
#else
	o.uv2_LightMap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
    
	return o;
}
PX_TNVT_VC_outvs Vert_Rimlight_Lightmap_VC(PX_NTT_VC_invs v)
{
	PX_TNVT_VC_outvs o;
	
	//Set position.
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.texcoord.xy = v.texcoord;	
    o.normal_view = PX_CalcViewNormal(v.normal);
    o.viewDir_view = PX_CalcViewDir(v.vertex);
#ifdef LIGHTMAP_OFF
	//--------------- Dir光照的 NDotL
	o.texcoord.z = dot(PX_CalcWorldNormal(v.normal), _WorldSpaceLightPos0);
	//---------------	
#else
	o.uv2_LightMap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
    
    o.color = v.color;
    
	return o;
}

PX_TTNVT_outvs Vert_Rimlight_Lightmap_Env(PX_NTT_invs v)
{
	PX_TTNVT_outvs o;
	
	//Set position.
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.texcoord.xy = v.texcoord;
    o.normal_view = PX_CalcViewNormal(v.normal);
    o.viewDir_view = PX_CalcViewDir(v.vertex);
#ifdef LIGHTMAP_OFF
	 //--------------- Dir光照的 NDotL
	o.texcoord.z = dot(PX_CalcWorldNormal(v.normal), _WorldSpaceLightPos0);
	//---------------	
#else
    o.uv2_LightMap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;   
#endif
    
 	float3 kviewpos = mul (UNITY_MATRIX_MV, v.vertex);
 	float3 kviewnormal = o.normal_view;//mul (UNITY_MATRIX_MV, v.normal);
 	PX_CalcEnvUv_Sphere(kviewpos, kviewnormal, o.texcoord1);
 //	float3 kViewReflect = normalize(reflect(-v.vertex, v.normal));
 //	kViewReflect = mul (UNITY_MATRIX_MV, kViewReflect);
 //	o.texcoord1 = kViewReflect.xy; 	
	
	return o;
}

// _AmbientFlag: 只能是0,1
// 0_不支持环境光
// 1_支持环境光
int _AmbientFlag;
fixed4 Frag_Rimlight_Lightmap( PX_TNVT_outvs IN ) : COLOR
{
	fixed4 c = tex2D (_MainTex, IN.texcoord);
	PX_ClipAlpha_NoA(c.a);	
	c *= _MaterialColor;
	
	fixed3 kRimLighting;
	CalcRimLigting_OnlyViewPos(IN.viewDir_view, IN.normal_view, kRimLighting);
	c.rgb += kRimLighting;
	
#ifdef LIGHTMAP_OFF
	if(_AmbientFlag != 0)
	{
		PX_Ambienting(c.rgb);
	}
	c.rgb += IN.texcoord.z * _LightColor0;
#else
	fixed3 lm = PX_DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,IN.uv2_LightMap)); 
	c.rgb *= lm;
#endif
		
	c.rgb += _Luminance;
	
	return c;
}

fixed4 Frag_Rimlight_Lightmap_VC( PX_TNVT_VC_outvs IN ) : COLOR
{
	fixed4 c = tex2D (_MainTex, IN.texcoord);
	PX_ClipAlpha_NoA(c.a);
	c *= _MaterialColor;
	
	fixed3 kRimLighting;
	CalcRimLigting_OnlyViewPos(IN.viewDir_view, IN.normal_view, kRimLighting);
	c.rgb += kRimLighting;

#ifdef LIGHTMAP_OFF
	PX_Ambienting(c.rgb);
	c.rgb += IN.texcoord.z * _LightColor0;
#else
	fixed3 lm = PX_DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,IN.uv2_LightMap)); 
	c.rgb *= lm;
#endif
	
	c.rgb += _Luminance;	
	c.rgb *= IN.color;
	
	return c;
}

fixed4 Frag_Rimlight_Lightmap_Mult( PX_TNVT_outvs IN ) : COLOR
{
	fixed4 c = tex2D (_MainTex, IN.texcoord);
	PX_ClipAlpha_NoA(c.a);
	c *= _MaterialColor;
	
	c.rgb += _Luminance;
	
	fixed rimValue;
	CalcRimValue_OnlyViewPos(IN.viewDir_view, IN.normal_view, rimValue);
	fixed3 fColorLerpRim = lerp(fixed3(1,1,1), _RimlightColor, rimValue);
	c.rgb *= fColorLerpRim;
	
//	return float4(fColorLerpRim, 1);

#ifdef LIGHTMAP_OFF
	PX_Ambienting(c.rgb);
	c.rgb += IN.texcoord.z * _LightColor0;
#else
	fixed3 lm = PX_DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,IN.uv2_LightMap)); 
	c.rgb *= lm;
#endif
	
	return c;
}

fixed4 Frag_Rimlight_Lightmap_Env( PX_TTNVT_outvs IN ) : COLOR
{
	fixed4 c = tex2D (_MainTex, IN.texcoord);
//	PX_ClipAlpha_NoA(c.a);	
	c *= _MaterialColor;
	
	c.rgb += _Luminance;
	
	fixed rimValue;
	CalcRimValue_OnlyViewPos(-IN.viewDir_view, IN.normal_view, rimValue);
//	fixed3 fColorLerpRim = lerp(fixed3(1,1,1), _RimlightColor, rimValue);
//	c.rgb *= fColorLerpRim;
	c.rgb += _RimlightColor * rimValue;

#ifdef LIGHTMAP_OFF
	PX_Ambienting(c.rgb);
	c.rgb += IN.texcoord.z * _LightColor0;
#else
	fixed3 lm = PX_DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,IN.uv2_LightMap)); 
	c.rgb *= lm;
#endif
	
	fixed4 envc = tex2D (_EnvTex, IN.texcoord1);
	c.rgb += envc.rgb * c.a;
//	c.a = 1.0;
	
	return c;
}


fixed4 Frag_Rimlight_Lightmap_Gray( PX_TNVT_outvs IN ) : COLOR
{
	fixed4 c = tex2D (_MainTex, IN.texcoord);
	PX_ClipAlpha_NoA(c.a);
	c *= _MaterialColor;
	
	PX_CalcColorByGrayColor(c.rgb);
	
	fixed3 kRimLighting;
	CalcRimLigting_OnlyViewPos(IN.viewDir_view, IN.normal_view, kRimLighting);
	c.rgb += kRimLighting;

#ifdef LIGHTMAP_OFF
	PX_Ambienting(c.rgb);
	c.rgb += IN.texcoord.z * _LightColor0;
#else
	fixed3 lm = PX_DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,IN.uv2_LightMap)); 
	c.rgb *= lm;
#endif
	
	c.rgb += _Luminance;
	return c;
}
// Lightmap相关--------------------------------------------------------END




// 一些标准函数---------------------------------------------------------START
//VS
PX_T_outvs Vert_T(PX_T_invs i)
{
	PX_T_outvs o;
	
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	o.texcoord = i.texcoord;

	return o;
}
PX_T_VC_outvs Vert_T_VC(PX_T_VC_invs i)
{
	PX_T_VC_outvs o;
	
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	o.texcoord = i.texcoord;
	o.color = i.color;

	return o;
}
PX_T_VC_outvs Effect_Vert_T(PX_T_VC_invs i)
{
	PX_T_VC_outvs o;
	
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	o.texcoord = FX_TRANSFORM_TEX(i.texcoord, _MainTex);
	o.color = i.color;

	return o;
}
PX_TT_VC_outvs Effect_Vert_TT_DKM(PX_T_VC_invs i)
{
	PX_TT_VC_outvs o;
	
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	o.texcoord = FX_TRANSFORM_TEX(i.texcoord, _MainTex);
	o.texcoord1 = FX_TRANSFORM_TEX(i.texcoord, _DarkTex);
	o.color = i.color;

	return o;
}
PX_TT_VC_outvs Effect_Vert_TT_GLM(PX_T_VC_invs i)
{
	PX_TT_VC_outvs o;
	
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	o.texcoord = FX_TRANSFORM_TEX(i.texcoord, _MainTex);
	o.texcoord1 = FX_TRANSFORM_TEX(i.texcoord, _GlowTex);
	o.color = i.color;

	return o;
}
PX_TTNV_VC_outvs Effect_Vert_TT_GLM_Rimlight(PX_NT_VC_invs i)
{
	PX_TTNV_VC_outvs o;
	
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	o.texcoord = FX_TRANSFORM_TEX(i.texcoord, _MainTex);
	o.texcoord1 = FX_TRANSFORM_TEX(i.texcoord, _GlowTex);
	o.normal_view = PX_CalcViewNormal(i.normal);
    o.viewDir_view = PX_CalcViewDir(i.vertex);
	o.color = i.color;

	return o;
}
PX_TT_VC_outvs Effect_Vert_TT_GLM_NoVC(PX_T_VC_invs i)
{
	PX_TT_VC_outvs o;
	
	o.pos = mul (UNITY_MATRIX_MVP, i.vertex);  
	o.texcoord = FX_TRANSFORM_TEX(i.texcoord, _MainTex);
	o.texcoord1 = FX_TRANSFORM_TEX(i.texcoord, _GlowTex);
	o.color = i.color;

	return o;
}

//PS
fixed4 Frag_T( PX_T_outvs i ) : COLOR
{
	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
	return colorTex;
}
//// 带有材质改变
//fixed4 Frag_MaterialColor( PX_T_outvs i ) : COLOR
//{
//	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
//	PX_ClipAlpha(colorTex.a);
//	colorTex *= _MaterialColor;
//	return colorTex;
//}
fixed4 Frag_MaterialColor_VC( PX_T_VC_outvs i ) : COLOR
{
	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
	PX_ClipAlpha(colorTex.a);
	PX_Ambienting(colorTex.rgb);
	colorTex *= _MaterialColor;
	colorTex *= i.color;
	return colorTex;
}
//fixed4 Frag_MaterialColor_DKM( PX_TT_outvs i ) : COLOR
//{
//	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
//	fixed4 colorDark = tex2D(_DarkTex, i.texcoord1);
//	colorTex *= colorDark;
//	PX_ClipAlpha(colorTex.a);
////	colorTex.a = smoothstep(_AlphaRef, 1.009, colorTex.a);
//	colorTex *= _MaterialColor;
//	return colorTex;
//}
// abedo的颜色需要去乘上alpha，这一般用于特效的Add模式
fixed4 Effect_Frag_MaterialColor_AbedoDotAlpha_NoVC( PX_T_outvs i ) : COLOR
{
	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
	PX_ClipAlpha(colorTex.a);
	colorTex *= _MaterialColor;
	colorTex.rgb *= colorTex.a;
	return colorTex;
}
fixed4 Effect_Frag_MaterialColor_AbedoDotAlpha( PX_T_VC_outvs i ) : COLOR
{
	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
	PX_ClipAlpha(colorTex.a);
	colorTex *= _MaterialColor;
	colorTex.rgb *= colorTex.a;
	colorTex *= i.color;
	return colorTex;
}
fixed4 Effect_Frag_MaterialColor_AbedoDotAlpha_DKM( PX_TT_VC_outvs i ) : COLOR
{
	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
	fixed4 colorDark = tex2D(_DarkTex, i.texcoord1);
	colorTex *= colorDark;
	PX_ClipAlpha(colorTex.a);
	colorTex *= _MaterialColor;
	colorTex.rgb *= colorTex.a;
	colorTex *= i.color;
	return colorTex;
}
fixed4 Effect_Frag_MaterialColor_AbedoDotAlpha_GLM( PX_TT_VC_outvs i ) : COLOR
{
	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
	fixed4 colorGlow = tex2D(_GlowTex, i.texcoord1);
	PX_ClipAlpha(colorTex.a);
	colorTex *= _MaterialColor;
	colorTex.rgb *= colorTex.a;
	colorTex.rgb += colorGlow;
	colorTex *= i.color;
	return colorTex;
}
fixed4 Effect_Frag_MaterialColor_AbedoDotAlpha_GLM_Rimlight( PX_TTNV_VC_outvs i ) : COLOR
{
	fixed4 colorTex = tex2D(_MainTex, i.texcoord);
	fixed4 colorGlow = tex2D(_GlowTex, i.texcoord1);
	PX_ClipAlpha(colorTex.a);
	colorTex *= _MaterialColor;
	colorTex.a = max(colorTex.a, dot(colorGlow.rgb, LumParameter));
	colorTex.rgb *= colorTex.a;
	colorTex.rgb += colorGlow;
	
	fixed3 kRimLighting;
	CalcRimLigting_OnlyViewPos(i.viewDir_view, i.normal_view, kRimLighting);
	colorTex.a = max(colorTex.a, dot(kRimLighting, LumParameter));
	colorTex.rgb += kRimLighting;
	
	return colorTex;
}
// 一些标准函数---------------------------------------------------------END

#endif