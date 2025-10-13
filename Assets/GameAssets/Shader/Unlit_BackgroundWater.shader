Shader "Unlit/BackgroundWater" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_Rotation ("Rotation Angle", Range(0, 360)) = 0
		_Exposure ("Exposure", Range(0, 10)) = 1
		_Speed ("Speed", Range(0, 10)) = 1
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Height ("Height", Range(0, 1)) = 0.5
		_EdgeColor ("Edge Color", Vector) = (0,0,0,1)
		_BubbleTex ("Bubble Texture", 2D) = "white" {}
		_Color1 ("Color1", Vector) = (1,1,1,1)
		_Color2 ("Color2", Vector) = (1,1,1,1)
		_Color3 ("Color3", Vector) = (1,1,1,1)
		_Color4 ("Color4", Vector) = (1,1,1,1)
		_Color5 ("Color5", Vector) = (1,1,1,1)
		_CenterPressure ("Center Pressure", Range(-20, 20)) = 0.5
		_WaveCount ("Wave Count", Range(1, 100)) = 1
		_WaveSize ("Wave Size", Range(0, 10)) = 0.5
		_AboveAlpha ("Above Alpha", Range(0, 1)) = 1
		_OverallAlpha ("Overall Alpha", Range(0, 1)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;
			float4 _Color;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy) * _Color;
			}

			ENDHLSL
		}
	}
}