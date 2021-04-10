Shader "TextureChange" {
	Properties {
		_Blend ("Blend", Range(0,1)) = 0.5
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Uncooked", 2D) = "white" {}
		_Texture2 ("Cooked", 2D) = "" {}
		//_BumpMap ("Normalmap", 2D) = "bump" {}
		
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque"  }
		LOD 100
		/*Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			struct Input {
				float2 uv_MainTex;
			};
			
			float _Blend;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Texture2;
			float4 _Texture2_ST;
			float4 _Color;
			//sampler2D _BumpMap;

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				fixed4 col = lerp(tex2D(_MainTex, i.uv),tex2D(_Texture2, i.uv),_Blend);
				return col * _Color;
			}

			ENDCG
		}*/
		CGPROGRAM
		
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0
		
		float _Blend;
		sampler2D _MainTex;
		sampler2D _Texture2;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Texture2;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = lerp(tex2D(_MainTex, IN.uv_MainTex), tex2D(_Texture2, IN.uv_Texture2), _Blend) * _Color;
			//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * (1 - _Blend) + tex2D(_Texture2, IN.uv_Texture2) * _Blend;
			o.Albedo = c;

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c;
		}

		ENDCG
	}
	Fallback "Diffuse"
}