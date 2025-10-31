Shader "Custom/Emotion"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _BGColor ("BGColor", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Illum ("Illumin (A)", 2D) = "white" {}
        _Emission ("Emission (Lightmapper)", Float) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Opaque"
        }
        LOD 150
        ZTest Less
        ZWrite On
        Cull Back
        Offset -2, -1
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        sampler2D _Illum;
        fixed4 _Color;
        fixed4 _BGColor;
        fixed _Emission;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Illum;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 c = tex * _Color;

            if(tex.a < .5)
            {
                c = _BGColor;
            }
            o.Albedo = c.rgb;
            o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Legacy Shaders/Self-Illumin/VertexLit"
    CustomEditor "LegacyIlluminShaderGUI"
}