// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/magma" {
	Properties{

		_MainTex("Texture", 2D) = "white"{} //���z

	}

		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex:POSITION;
				float2 uv:TEXCOORD0;
			};

			struct v2f
			{
				float2 uv:TEXCOORD0;
				float4 vertex:SV_POSITION;
			};


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;;
				return o;
			}

			sampler2D _MainTex;


			fixed4 frag(v2f i) :SV_Target
			{
				float2 tmpUV = i.uv;
				tmpUV.x += _Time.x*0.1;
				tmpUV.y += _Time.y*0.1;
				fixed4 col = tex2D(_MainTex, tmpUV);
				//col.x *= 2; //�ܦ��L���C��F
				//col.rgb =float3 (1, 0, 0);//�ܦ��`���C��F
				return col;
			}
			ENDCG
		}
	}
		FallBack "Diffuse"
}
