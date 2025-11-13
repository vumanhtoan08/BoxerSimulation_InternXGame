Shader "Human/HumanOpaque"
{
    Properties
    {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _BumpMap ("Normal Mapa", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="TransparentCutout"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf HalfLambert

        inline half4 LightingHalfLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            half difLight = max(0, dot(s.Normal, lightDir));
            half hLambert = difLight * 0.5 + 0.5;
            half4 col;
            col.rgb = s.Albedo * _LightColor0.rgb * (hLambert * atten);
            col.a = s.Alpha;
            return col;
        }
        
        sampler2D _MainTex;
        half _VPLightInten;
        half4 _Color;

        struct Input
        {
            half2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb * min(_VPLightInten * 2, 1);
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}