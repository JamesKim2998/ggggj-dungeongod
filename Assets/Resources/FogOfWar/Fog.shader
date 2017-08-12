Shader "Unlit/Fog"
{
	Properties
	{
	}
	SubShader
	{
		Tags {
			"Queue"="Transparent"
			"RenderType"="Transparent"
		}
		Blend Zero OneMinusSrcAlpha

		Pass
		{
			zwrite off
			ztest always
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				fixed2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed alpha : COLOR0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.alpha = v.uv.x;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(0, 0, 0, i.alpha);
			}
			ENDCG
		}
	}
}
