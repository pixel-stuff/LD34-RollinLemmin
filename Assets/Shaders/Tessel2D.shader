/******************************************
 * @author Pierre-Marie Plans             *
 * @mail pierre.plans@gmail.com           *
 ******************************************/

Shader "Custom/Tessel2D" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_Displacement("Displacement", Range(0, 10.0)) = 0.3
		_Tess("Tessellation", Range(1,32)) = 4
		/*_p0("P0", Vector) = (1, 1, 1, 1)
		_handlerP0("handler P0", Vector) = (1, 1, 1, 1)
		_p1("P1", Vector) = (1, 1, 1, 1)
		_handlerP1("handler P1", Vector) = (1, 1, 1, 1)*/
	}
	// dx 11
	SubShader {
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf TerrainLight fullforwardshadows vertex:disp tessellate:tessel nolightmap

		// Use shader model 5.0 target, to get nicer looking lighting
		#pragma target 5.0
		#include "Tessellation.cginc"

		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
			float3 normal;
		};

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Displacement;
		float _Tess;
		/*half3 _p0;
		half3 _handlerP0;
		half3 _p1;
		half3 _handlerP1;*/

		half3 CalculateBezierPoint(float t, half3 p0, half3 handlerP0, half3 handlerP1, half3 p1) {
			float u = 1.0f - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;

			half3 p = uuu * p0; //first term
			p += 3.0f * uu * t * handlerP0; //second term
			p += 3.0f * u * tt * handlerP1; //third term
			p += ttt * p1; //fourth term

			return p;
		}

		half4 LightingTerrainLight(SurfaceOutput s, half3 lightDir, half atten) {
			return half4(s.Albedo.rgb, s.Alpha)*dot(s.Normal, lightDir);
		}

		void disp(inout appdata v)
		{
			float d = tex2Dlod(_MainTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
			//v.vertex.xyz += v.normal * d;
			//v.vertex.z += cos(v.vertex.x)*_Displacement;
			//v.vertex.z += CalculateBezierPoint(v.vertex.x, _p0, _handlerP0, _handlerP1, _p1).z;
			v.vertex.y += d;
		}

		float4 tessel(appdata v0, appdata v1, appdata v2)
		{
			return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _Tess);
		}

		void surf(Input IN, inout SurfaceOutput  o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Normal = IN.normal;
			o.Alpha = c.a;
		}
		ENDCG
	}
	// dx 9
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:disp nolightmap
		#pragma target 3.0

		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Displacement;
		float _Tess;
		/*half3 _p0;
		half3 _handlerP0;
		half3 _p1;
		half3 _handlerP1;*/

		half3 CalculateBezierPoint(float t, half3 p0, half3 handlerP0, half3 handlerP1, half3 p1) {
			float u = 1.0f - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;

			half3 p = uuu * p0; //first term
			p += 3.0f * uu * t * handlerP0; //second term
			p += 3.0f * u * tt * handlerP1; //third term
			p += ttt * p1; //fourth term

			return p;
		}

		void disp(inout appdata v)
		{
			//float d = tex2Dlod(_MainTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
			//v.vertex.xyz += v.normal * d;
			//v.vertex.z += cos(v.vertex.x)*_Displacement;
			//v.vertex.z += CalculateBezierPoint(v.vertex.x, _p0, _handlerP0, _handlerP1, _p1).z;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = /*tex2D(_MainTex, IN.uv_MainTex) **/ _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	/*SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

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
		};

		half3 CalculateBezierPoint(float t, half3 p0, half3 handlerP0, half3 handlerP1, half3 p1) {
			float u = 1.0f - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;

			half3 p = uuu * p0; //first term
			p += 3.0f * uu * t * handlerP0; //second term
			p += 3.0f * u * tt * handlerP1; //third term
			p += ttt * p1; //fourth term

			return p;
		}

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _Displacement;
		half3 _p0;
		half3 _handlerP0;
		half3 _p1;
		half3 _handlerP1;

		v2f vert(appdata v)
		{
			v2f o;
			//v.vertex.z += sin(v.vertex.x)*_Displacement;
			v.vertex.z += CalculateBezierPoint(v.vertex.x, _p0, _handlerP0, _handlerP1, _p1).z;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = tex2D(_MainTex, i.uv);
			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);
			return col;
		}
		ENDCG
	}*/
	// fallback
	FallBack "Diffuse"
}
