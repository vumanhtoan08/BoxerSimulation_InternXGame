Shader "Skybox/Cubemap Blend" {
	Properties {
		[StyledBanner(Skybox Cubemap Blend)] _SkyboxExtended ("< SkyboxExtended >", Float) = 1
		[StyledCategory(Cubemap Settings, 5, 10)] _Cubemapp ("[ Cubemapp ]", Float) = 1
		[StyledTextureSingleLine] [NoScaleOffset] _Tex ("Cubemap (HDR)", Cube) = "black" {}
		[StyledTextureSingleLine] [NoScaleOffset] _Tex_Blend ("Cubemap Blend (HDR)", Cube) = "black" {}
		[Space(10)] _CubemapTransition ("Cubemap Transition", Range(0, 1)) = 0
		[Space(10)] _Exposure ("Cubemap Exposure", Range(0, 8)) = 1
		[Gamma] _TintColor ("Cubemap Tint Color", Vector) = (0.5,0.5,0.5,1)
		_CubemapPosition ("Cubemap Position", Float) = 0
		[StyledCategory(Rotation Settings)] _Rotationn ("[ Rotationn ]", Float) = 1
		[Toggle(_ENABLEROTATION_ON)] _EnableRotation ("Enable Rotation", Float) = 0
		[IntRange] [Space(10)] _Rotation ("Rotation", Range(0, 360)) = 0
		_RotationSpeed ("Rotation Speed", Float) = 1
		[StyledCategory(Fog Settings)] _Fogg ("[ Fogg ]", Float) = 1
		[Toggle(_ENABLEFOG_ON)] _EnableFog ("Enable Fog", Float) = 0
		[StyledMessage(Info, The fog color is controlled by the fog color set in the Lighting panel., _EnableFog, 1, 10, 0)] _FogMessage ("# FogMessage", Float) = 0
		[Space(10)] _FogIntensity ("Fog Intensity", Range(0, 1)) = 1
		_FogHeight ("Fog Height", Range(0, 1)) = 1
		_FogSmoothness ("Fog Smoothness", Range(0.01, 1)) = 0.01
		_FogFill ("Fog Fill", Range(0, 1)) = 0.5
		[HideInInspector] _Tex_HDR ("DecodeInstructions", Vector) = (0,0,0,0)
		[HideInInspector] _Tex_Blend_HDR ("DecodeInstructions", Vector) = (0,0,0,0)
		[ASEEnd] _FogPosition ("Fog Position", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Skybox/Cubemap"
	//CustomEditor "SkyboxExtendedShaderGUI"
}