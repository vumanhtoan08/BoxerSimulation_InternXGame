Shader "Shader Graphs/ArrowLineRenderer" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		[NoScaleOffset] _ArrowTexture ("ArrowTexture", 2D) = "white" {}
		_Speed ("Speed", Float) = 0.1
		_FadeSpeed ("FadeSpeed", Float) = 0.1
		_Tiling ("Tiling", Vector) = (1,1,0,0)
		[NoScaleOffset] _SampleTexture2D_d3d32546602641d8adef20308ac2f214_Texture_1_Texture2D ("Texture2D", 2D) = "white" {}
		[HideInInspector] _BUILTIN_QueueOffset ("Float", Float) = 0
		[HideInInspector] _BUILTIN_QueueControl ("Float", Float) = -1
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

			float4 _Color;

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return _Color; // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}