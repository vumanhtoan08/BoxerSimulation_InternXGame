// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Block/MobileDiffuse"
{

    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 150

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd vertex:vert

        sampler2D _MainTex;
        fixed _VPLightInten;

        struct Input
        {
            float2 uv_MainTex;
            float light;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.light = v.texcoord.w / (4096.0 * 15.0);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * (_VPLightInten + IN.light * (1.2 - _VPLightInten));
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}