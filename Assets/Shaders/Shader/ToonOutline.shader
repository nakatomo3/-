Shader "Shader/Toon"
{
	Properties{
	   _Color("Color", Color) = (1,1,1,1)
	   _MainTex("Albedo (RGB)", 2D) = "white" {}
	   _RampTex("Ramp", 2D) = "white"{}
	}

	SubShader
	{

		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf ToonRamp
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _RampTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		fixed4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten) {
			half d = dot(s.Normal, lightDir)*0.5f + 0.3;
			fixed3 ramp = tex2D(_RampTex, fixed2(d, 0.5)).rgb;
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = 1;			
			return c;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG

//		CGPROGRAM
//#pragma surface surf Lambert
//
//		struct Input {
//			float3 worldPos;
//		};
//
//		float4 _Color;
//
//		void surf(Input IN, inout SurfaceOutput o) {
//			o.Albedo = _Color;
//			clip(frac(IN.worldPos.y * 0.4) - 0.7);
//		}
//		ENDCG

		Pass {
			Cull Front

				CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

				struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v) {
				v2f o;
				v.vertex += float4(v.normal * 0.03f, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0.1,0.1,0.1,1);
				return col;
			}
				ENDCG
		}

		Pass{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v) {
				v2f o;
				v.vertex += float4(v.normal * 0.05f, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0.1,0.1,0.1,1);
				return col;
			}
			ENDCG
		}
				Pass{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v) {
				v2f o;
				v.vertex += float4(v.normal * 0.08f, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0.1,0.1,0.1,1);
				return col;
			}
			ENDCG
			}

	}
	FallBack "Diffuse"
}