Shader "Unlit/Text"
{
	Properties
	{
		_MainTex ("Alpha (A)", 2D) = "white" {}
		_OutlineWidth("Outline Width", float) = 0.1
		_OutlineColor("Outline Color",Color) = (1,1,1,1)
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"DisableBatching" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Offset -1, -1
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		
		Pass
		{
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform half4 _OutlineColor;
			uniform float _OutlineWidth;

			struct vertexInput
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			float4 Outline(float4 vertPos, float width)
			{
				float4x4 scaleMat;
				scaleMat[0][0] = 1.0 + width;
				scaleMat[0][1] = 0.0;
				scaleMat[0][2] = 0.0;
				scaleMat[0][3] = 0.0;
				scaleMat[1][0] = 0.0;
				scaleMat[1][1] = 1.0 + width;
				scaleMat[1][2] = 0.0;
				scaleMat[1][3] = 0.0;
				scaleMat[2][0] = 0.0;
				scaleMat[2][1] = 0.0;
				scaleMat[2][2] = 1.0 + width;
				scaleMat[2][3] = 0.0;
				scaleMat[3][0] = 0.0;
				scaleMat[3][1] = 0.0;
				scaleMat[3][2] = 0.0;
				scaleMat[3][3] = 1.0;

				return mul(scaleMat, vertPos);
			}

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.vertex = UnityObjectToClipPos(Outline(v.vertex,_OutlineWidth));
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			half4 frag (vertexOutput i) : SV_Target
			{
				half4 col = i.color;
				col.a *= tex2D(_MainTex, i.texcoord).a;
				return col * _OutlineColor;
			}
			ENDCG
		}
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
				half4 col = i.color;
				col.a *= tex2D(_MainTex, i.texcoord).a;
				return col;
			}
			ENDCG
		}

	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"DisableBatching" = "True"
		}
		
		Lighting Off
		Cull Off
		ZTest Always
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		
		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		Pass
		{
			SetTexture [_MainTex]
			{ 
				combine primary, texture
			}
		}
	}
}
