Shader "Legacy Shaders/UIModelPreview"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Alpha("Alpha", float) = .3
        [Toggle(ENABLE_MASK)] _EnableMask ("Enable Mask", Int) = 0
    }
    SubShader
    {
		Tags
		{ 
			"Queue"="Opaque" 
			"RenderType"="Opaque" 
		}

        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _ModelRectMaskUI;
        half _EnableMask;
        half _Alpha;

        struct Input
        {
            float2 uv_MainTex;
            float2 vertex;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.vertex = UnityObjectToViewPos(v.vertex);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            if(_EnableMask == 1 && (IN.vertex.x < _ModelRectMaskUI.x || IN.vertex.x > _ModelRectMaskUI.z || IN.vertex.y < _ModelRectMaskUI.y || IN.vertex.y > _ModelRectMaskUI.w))
            {
                discard;
            }
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
           
            if (c.a < _Alpha)
            {
                discard;
            }
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/VertexLit"
}