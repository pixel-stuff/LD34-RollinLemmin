// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Fx/Ice/IceUnlit"
{
	Properties
	{
		_DiffuseTex ("DiffuseTex", 2D) = "white" {}
		_NormalTex("NormalTex", 2D) = "white" {}
		_IceColor("IceColor", Color) = (0.69,0.92,1,1)
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

	GrabPass
	{
		"_BackgroundTex"
	}
	Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
		float4 screenPos : TEXCOORD1;
		float3 viewDir : TEXCOORD2;
	};

	sampler2D _BackgroundTex : register(s0);
	float4 _BackgroundTex_ST;
	sampler2D _DiffuseTex : register(s1);
	float4 _DiffuseTex_ST;
	sampler2D _NormalTex : register(s2);
	float4 _NormalTex_ST;

	fixed3 _IceColor;

	v2f vert(appdata v)
	{
		v2f o = (v2f)0;
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _BackgroundTex);

		o.viewDir = normalize(WorldSpaceViewDir(v.vertex));

		half4 screenpos = ComputeGrabScreenPos(o.vertex);
		o.screenPos.xy = screenpos.xy / screenpos.w;
		half depth = length(mul(UNITY_MATRIX_MV, v.vertex));
		o.screenPos.z = depth;
		o.screenPos.w = depth;
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	float SchlickFresnel(float n1, float n2, float cosO)
	{
		float R0 = (n1 - n2) / (n1 + n2);
		float cosO5 = cosO*cosO*cosO*cosO*cosO;
		return R0 + (1.0 - R0)*cosO5;
	}

	fixed4 RenderWater(v2f i)
	{
		fixed2 uv = i.uv.xy;
		fixed2 uvscreen = i.screenPos.xy;

		fixed4 diffuse = tex2D(_DiffuseTex, uv).rgba;
		fixed3 normal = normalize(tex2D(_NormalTex, uv).rgb);

		float Fresnel = SchlickFresnel(1.309, 1.0, max(0.0, dot(-i.viewDir, normal)));

		fixed3 bgcolor = tex2D(_BackgroundTex, uvscreen + normal.xz*0.1).rgb;
		fixed3 ftcolor = tex2D(_BackgroundTex, uvscreen - normal.xz*0.1).rgb;
		fixed3 wcolor = diffuse.rgb;

		fixed3 color = lerp(bgcolor, wcolor, Fresnel);

		color = lerp(color, ftcolor, clamp(1.0 - Fresnel, 0.0, 1.0));

		color = lerp(color, _IceColor, diffuse.a);

		return fixed4(color, 1.0);
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 col = RenderWater(i)
		// apply fog
		UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
	}
		ENDCG
	}
	}
}
