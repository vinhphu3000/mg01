// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

#ifndef PROJ_X_MID_CG_INCLUDE
#define PROJ_X_MID_CG_INCLUDE

// ###: [ProjX的中间层-主要存放一些中间函数以及一些定义]

//#include "UnityCG.cginc"
//#include "AutoLight.cginc"
#include "UnityShaderVariables.cginc"

// DirLight的颜色,定义在 Lighting.cginc 中的
#if ( defined(LIGHTMAP_OFF) && defined(USING_DIRECTIONAL_LIGHT) )
fixed4 _LightColor0;
fixed4 _SpecColor;
#endif

// base材质
uniform sampler2D _MainTex;
float4 _MainTex_ST;
// dark贴图 DKM
uniform sampler2D _DarkTex;
float4 _DarkTex_ST;
// glow贴图 GLM
uniform sampler2D _GlowTex;
float4 _GlowTex_ST;
// Env贴图 ENV
uniform sampler2D _EnvTex;
float4 _EnvTex_ST;

// 外置的AlphaRef
fixed _AlphaRef = 0.005;
// 外置的材质颜色_提供给美术k帧的
fixed4 _MaterialColor;
// 特效起始时间
float _StartTime;
// Rimshader中，被击打时，需要闪光，也可用于其他shader
fixed3 _Luminance;

// 石化颜色,其中A为石化颜色和本来颜色之间的Lerp值
fixed4 _GrayColor;

#define LumParameter fixed3(0.2125, 0.7154, 0.0721)

// 顶点输入，输出
struct PX_T_invs
{
	float4 vertex : POSITION;
	half2 texcoord : TEXCOORD0;
}; 
struct PX_TT_invs
{
	float4 vertex : POSITION;
	half2 texcoord : TEXCOORD0;
	half2 texcoord1 : TEXCOORD1;
};
struct PX_TT_VC_invs
{
	float4 vertex : POSITION;
	half2 texcoord : TEXCOORD0;
	half2 texcoord1 : TEXCOORD1;
	fixed4 color : COLOR;
}; 
struct PX_NTT_invs
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	half2 texcoord : TEXCOORD0;
	half2 texcoord1 : TEXCOORD1;
}; 
struct PX_NTT_VC_invs
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	half2 texcoord : TEXCOORD0;
	half2 texcoord1 : TEXCOORD1;
	fixed4 color : COLOR;
}; 

struct PX_T_VC_invs
{
	float4 vertex : POSITION;
	half2 texcoord : TEXCOORD0;
	fixed4 color : COLOR;
}; 

struct PX_NT_VC_invs
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	half2 texcoord : TEXCOORD0;
	fixed4 color : COLOR;
}; 

struct PX_T_outvs
{
	float4 pos : POSITION; 
	half2 texcoord : TEXCOORD0;
};
struct PX_TT_outvs
{
	float4 pos : POSITION; 
	half2 texcoord : TEXCOORD0;
	half2 texcoord1 : TEXCOORD1;
};

struct PX_TNVT_outvs
{
    float4 pos : SV_POSITION;    
    float3 normal_view : TEXCOORD1;
    float3 viewDir_view : TEXCOORD2;
#ifdef LIGHTMAP_OFF
	float3 texcoord : TEXCOORD0;	// z_NDotL
#else
	float2 texcoord : TEXCOORD0;
    float2 uv2_LightMap : TEXCOORD3;
#endif
};
struct PX_TNVT_VC_outvs
{
    float4 pos : SV_POSITION;
    float3 normal_view : TEXCOORD1;
    float3 viewDir_view : TEXCOORD2;
#ifdef LIGHTMAP_OFF
	float3 texcoord : TEXCOORD0;	// z_NDotL
#else
	float2 texcoord : TEXCOORD0;
    float2 uv2_LightMap : TEXCOORD3;
#endif
    fixed4 color : COLOR;
};
struct PX_TTNVT_outvs
{
    float4 pos : SV_POSITION;
    float2 texcoord1 : TEXCOORD1;
    float3 normal_view : TEXCOORD2;
    float3 viewDir_view : TEXCOORD3;
#ifdef LIGHTMAP_OFF
	float3 texcoord : TEXCOORD0;	// z_NDotL
#else
	float2 texcoord : TEXCOORD0;
    float2 uv2_LightMap : TEXCOORD4;
#endif
};

struct PX_T_VC_outvs
{
	float4 pos : POSITION; 
	half2 texcoord : TEXCOORD0;
	fixed4 color : COLOR;
};
struct PX_TT_VC_outvs
{
	float4 pos : POSITION; 
	half2 texcoord : TEXCOORD0;
	half2 texcoord1 : TEXCOORD1;
	fixed4 color : COLOR;
};
struct PX_TTNV_VC_outvs
{
	float4 pos : POSITION; 
	half2 texcoord : TEXCOORD0;
	half2 texcoord1 : TEXCOORD1;
	float3 normal_view : TEXCOORD2;
    float3 viewDir_view : TEXCOORD3;
	fixed4 color : COLOR;
};

// PP: Proj Pos
struct PX_PP_outvs
{
	float4 pos : POSITION; 
	float4 projPos : TEXCOORD0;
};

// 一些常用的中间函数----------------------------------------------START
void PX_ClipAlpha(fixed fAlpha)
{
	clip(fAlpha - _AlphaRef);
}
void PX_ClipAlpha_NoA(fixed fAlpha)
{
	clip(fAlpha - 0.01);
}

void PX_LightDir(fixed3 vWorldNormal, inout fixed3 inColor)
{
#if ( defined(LIGHTMAP_OFF) && defined(USING_DIRECTIONAL_LIGHT) )
//	fixed NDotL = saturate(dot(vWorldNormal, _WorldSpaceLightPos0));
	fixed NDotL = dot(vWorldNormal, _WorldSpaceLightPos0);
	inColor += NDotL * _LightColor0;
#endif
}
void PX_Ambienting(inout fixed3 inColor)
{
#ifdef LIGHTMAP_OFF
	// ly_2015_03_11 使用unity5,光照系统发生改变,可能有两遍光照,所以这里暂时去掉
//	inColor *= (UNITY_LIGHTMODEL_AMBIENT.rgb * 2);
#endif
}
// 根据输入颜色和GrayColor计算合成后的颜色
void PX_CalcColorByGrayColor(inout fixed3 kColor)
{
//	po.Diffuse.rgb = dot(po.Diffuse.rgb, LumParameter);				// 石化
//	po.Diffuse.rgb *= float3(0.85098040, 0.85098040, 0.098039217);	// 金黄色	D9D919
//	po.Diffuse.rgb *= float3(0x87/255.0, 0xCE/255.0, 0xFA/255.0);	// 冰蓝色
//	po.Diffuse.rgb *= float3(0xCD/255.0, 0x7F/255.0, 0x32/255.0);	// 金色
//	po.Diffuse.rgb *= float3(0x99/255.0, 0x32/255.0, 0xCD/255.0);	// 紫色

	fixed3 kTempColor = dot(kColor, LumParameter);
	kTempColor.rgb *= _GrayColor.rgb;
	
	kColor = lerp(kColor, kTempColor, _GrayColor.a);
}
// 计算反射Evn
//void PX_CalcEnvUv_Sphere(float3 kViewPos, float3 kNormal, float4x4 kEnvWPTransform, out float2 ouv)
void PX_CalcEnvUv_Sphere(float3 kViewPos, float3 kNormal, out float2 ouv)
{
	float3 kViewReflect = normalize(reflect(-kViewPos, kNormal));
//	float3 TexCoordOut = mul(float4(kViewReflect, 1.0), kEnvWPTransform);
//	ouv = TexCoordOut.rg;
	ouv = kViewReflect.rg;
}

float3 PX_mulVec3( float4x4 m, float3 v ) {
//	return m[0].xyz*v.x + (m[1].xyz*v.y + (m[2].xyz*v.z));
	return float3(m[0].x, m[1].x, m[2].x) * v.x + 
		float3(m[0].y, m[1].y, m[2].y) * v.y + 
		float3(m[0].z, m[1].z, m[2].z) * v.z;
}
//float3 PX_mulVec3( float3x3 m, float3 v ) {
//	return m[0].xyz*v.x + (m[1].xyz*v.y + (m[2].xyz*v.z));
//}
//float3 PX_mulPoint3( float4x4 m, float3 p ) {
//	return m[0].xyz*p.x + (m[1].xyz*p.y + (m[2].xyz*p.z + m[3].xyz));
//}

// 点光源 --------------------------未完成,还需修改
float3 PX_CalcPointLight (float4 viewPos, float3 viewNormal)
{
//	float3 viewpos = mul (UNITY_MATRIX_MV, vertex).xyz;
//	float3 viewN = mul ((float3x3)UNITY_MATRIX_IT_MV, normal);
	float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
	float3 toLight = unity_LightPosition[0].xyz - viewPos.xyz * unity_LightPosition[0].w;
	float lengthSq = dot(toLight, toLight);
	float atten = 1.0 / (1.0 + lengthSq * unity_LightAtten[0].z);
	float diff = max (0, dot (viewNormal, normalize(toLight)));
	lightColor += unity_LightColor[0].rgb * (diff * atten);
	return lightColor;
}
// [vs] 通过顶点中的inNormal来计算WorldNormal
float3 PX_CalcWorldNormal(in float3 inNormal)
{
	return normalize( PX_mulVec3(unity_ObjectToWorld, inNormal).xyz );
}
// [vs] 通过顶点中的inNormal来计算viewNormal
float3 PX_CalcViewNormal(in float3 inNormal)
{
	// 勿删,暂时用PX_mulVec3替代
//	return normalize( mul(UNITY_MATRIX_MV, float4(inNormal,0)).xyz );
	return normalize( PX_mulVec3(UNITY_MATRIX_MV, inNormal).xyz );
}
// [vs]通过ModelPos计算WorldDir
float3 PX_CalcWorldDir(in float3 vModelPos)
{
	return normalize( mul(unity_ObjectToWorld, float4(vModelPos, 1)).xyz );
}
// [vs]通过ModelPos计算ViewDir
float3 PX_CalcViewDir(in float3 vModelPos)
{
	return normalize( mul(UNITY_MATRIX_MV, float4(vModelPos, 1)).xyz );
}
// [vs] 通过WorldPos计算ViewDir
float3 PX_CalcViewDir_WorldPos(in float3 vWorldPos)
{
	float3 vModelPos = mul(unity_WorldToObject, float4(vWorldPos.xyz, 1)).xyz;
	return PX_CalcViewDir(vModelPos);
}
// 一些常用的中间函数----------------------------------------------END


// 重写的一些unity默认的函数----------------------------------------------START
// Decodes lightmaps:
// - doubleLDR encoded on GLES
// - RGBM encoded with range [0;8] on other platforms using surface shaders
inline fixed3 PX_DecodeLightmap( fixed4 color )
{
#if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
	return 2.0 * color.rgb;
#else
	// potentially faster to do the scalar multiplication
	// in parenthesis for scalar GPUs
	return (8.0 * color.a) * color.rgb;
#endif
}
// Computes object space view direction
inline float3 PX_ObjSpaceViewDir( in float4 v )
{
	float3 objSpaceCameraPos = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1)).xyz * 1.0;
	return objSpaceCameraPos - v.xyz;
}

#define FX_TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
// 重写的一些unity默认的函数----------------------------------------------END

// Rimlight相关--------------------------------------------------------START
// 下面的取值范围:[0,1],所以可以用fixed
fixed _RimlightStart;
fixed _RimlightEnd;
fixed3 _RimlightColor;

void CalcRimValue_OnlyViewPos(half3 kPosDir, half3 kNormal, out fixed rim)
{
	// leo_2013_06_30 Rim lighting - 视点相关,非光源相关
	#define Rim_Start		_RimlightStart	//0.2				// [0, 1]
	#define Rim_End			_RimlightEnd	//1.0				// [0, 1]
	
	#define fRimStart Rim_Start
	#define fRimEnd Rim_End

	fixed NdotE = saturate(dot(-kPosDir, kNormal));
    rim = smoothstep(fRimStart, fRimEnd, 1-NdotE);
}

void CalcRimLigting_OnlyViewPos(half3 kPosDir, half3 kNormal, out fixed3 kRimLiging)
{
	// leo_2013_06_30 Rim lighting - 视点相关,非光源相关
//	#define Rim_Start		_RimlightStart	//0.2				// [0, 1]
//	#define Rim_End			_RimlightEnd	//1.0				// [0, 1]
	#define Rim_Color		_RimlightColor	//fixed3(0.8, 0.6, 0.4)
//	
//	#define fRimStart Rim_Start
//	#define fRimEnd Rim_End
//
//	fixed NdotE = saturate(dot(-kPosDir, kNormal));
//  fixed rim = smoothstep(fRimStart, fRimEnd, 1-NdotE);
	fixed rim;
	CalcRimValue_OnlyViewPos(kPosDir, kNormal, rim);
    kRimLiging = rim * Rim_Color;	//Rim_Multiplier * 
}

#ifndef LIGHTMAP_OFF
// sampler2D unity_Lightmap;
// float4 unity_LightmapST;
#endif

struct lm_vsout
{
    float4 pos : SV_POSITION;    
#ifdef LIGHTMAP_OFF
	float3 texcoord : TEXCOORD0;	// z_NDotL
#else
	float2 texcoord : TEXCOORD0;
	float2 uv2_LightMap : TEXCOORD1;
#endif
};

struct lm_vsout_vc
{
    float4 pos : SV_POSITION;    
#ifdef LIGHTMAP_OFF
	float3 texcoord : TEXCOORD0;	// z_NDotL
#else
	float2 texcoord : TEXCOORD0;
	float2 uv2_LightMap : TEXCOORD1;
#endif
    fixed4 color : COLOR;
};
// Rimlight相关--------------------------------------------------------END



#endif