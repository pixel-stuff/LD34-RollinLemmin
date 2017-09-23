// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Fx/Water/WaterUnlit"
{
	Properties
	{
		//_BackgroundTex ("BackgroundTex", 2D) = "white" {}
		_PerturbationTex("PerturbationTex", 2D) = "white" {}
		_heightPercent("WaterHeight", Float) = 0.5
		_belowWaterHeight("BelowWaterHeight", Float) = 0.3
		_Amplitude("WaterAmplitudeMax", Float) = 0.02
		_Frequency("WaterFrequencyMax", Float) = 4.0
		_Speed("WaterSpeedMax", Float) = 1.0
		_PerturbationStrength("PerturbationStrength", Float) = 1.0
		_Temp("Temperature", Float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				float4 worldPos : TEXCOORD2;
			};

#define AMPMAX 0.5

			sampler2D _BackgroundTex : register(s0);
			float4 _BackgroundTex_ST;
			sampler2D _PerturbationTex : register(s1);
			float4 _PerturbationTex_ST;
			// constants and inputs
			float _heightPercent;
			float _belowWaterHeight;
			float _Amplitude;
			float _Frequency;
			float _Speed;
			float _PerturbationStrength;
			fixed3 LApex = fixed3(0.0, 1.0, 0.0);
			fixed3 LTarget = fixed3(0.0, 0.0, 0.0);
			float LAperture = 30.0;
			float _Temp;

			// defines
#define TIME _Speed*_Time.y
			
			v2f vert (appdata v)
			{
				v2f o = (v2f)0;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _BackgroundTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

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
				fixed2 wuv = i.worldPos.xy;
				fixed2 uvscreen = i.screenPos.xy;

				float FTemp = max(0.0, _Temp + 20.0) / 100.0;

				_Amplitude *= FTemp;
				_Amplitude = min(AMPMAX, _Amplitude);
				_Frequency *= FTemp;
				_Speed *= FTemp;

				float HPF = max(0.0, uv.y + (_Amplitude*cos(TIME))*sin(_Frequency*wuv.x + TIME) - _heightPercent);
				HPF /= max(0.1, HPF);
				HPF = min(1.0, HPF + max(0.0, uv.y) / _heightPercent*_belowWaterHeight);

				/*HPF *= 1.9;
				HPF = min(1.0, HPF);*/

				float height = lerp(0.0, 1.0, HPF);

				fixed2 offset = tex2D(_PerturbationTex, uv + TIME*0.01).rg*(1.0 - HPF)*_PerturbationStrength;

				// cone computations
				float halfAperture = LAperture / 2.0;

				fixed3 LtoP = normalize(fixed3(uv + offset*0.5, 0.0) - LApex);
				fixed3 Ldir = normalize(LTarget - LApex);
				float extinction = height;
				float inCone = max(0.0,
					1.0 - min(1.0,
						acos(dot(LtoP, Ldir)
						)
						/ radians(halfAperture)
					)

				);//?0.0f:1.0f;

				inCone = clamp(inCone, 0.0, 0.5)*(1.0 / 0.5);

				float LIntensity = lerp(FTemp, 1.0, height) * extinction;

				// colouring

				fixed3 bgcolor = tex2D(_BackgroundTex, uvscreen + offset).rgb;
				fixed3 wcolor = fixed3(0.2, 0.6, 1.0);

				// blending
				wcolor = lerp(fixed3(0.0, 0.0, 0.0), wcolor, height);


				// lighting
				wcolor += inCone*LIntensity;
				bgcolor += inCone*LIntensity;

				fixed3 color = lerp(wcolor, bgcolor, height);

				return fixed4(color, 1.0);
			}
			
			fixed4 frag (v2f i) : SV_Target
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
