Shader "Fx/Fullscreen/AccelerationBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Perturbation("Perturbation", Float) = 0.1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Perturbation;
			fixed2 _Acceleration;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0.0, 0.0, 0.0, 0.0);
				float nPasses = 6.0;
				float iter = 2.0*_Perturbation / nPasses;
				for (float offset = -_Perturbation; offset < _Perturbation; offset += iter)
				{
					col += tex2D(_MainTex, i.uv + clamp((_Acceleration)*offset, -0.1, 0.1))/(nPasses);
				}
				// just invert the colors
				//col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
