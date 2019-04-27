Shader "Shader/Collect"
{
	Properties{
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("Albedo (RGB)", 2D) = "white" {}
			_RampTex("Ramp", 2D) = "white"{}
	}

		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
			LOD 200

			Pass{
			  ZWrite ON
			  ColorMask 0
			}

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows alpha:fade
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			fixed4 _Color;

			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = 0;
				o.Smoothness = 0;
				o.Alpha = c.a;
			}
			ENDCG


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

					fixed4 frag(v2f i) : SV_Target{
						fixed4 col = fixed4(0.1,0.1,0.1,1);
						return col;
					}
					ENDCG
				}
	}
		FallBack "Diffuse"
}