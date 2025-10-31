// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparentScroll"
{

    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _ScrollX ("Base layer Scroll speed X", Float) = 1.0
        _ScrollY ("Base layer Scroll speed Y", Float) = 0.0
        _Intensity ("Intensity", Float) = 1.0
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"
        }
        LOD 150
        ZWrite Off
        Cull Off
        Offset -2, -1
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        fixed4 _Color;

        float _ScrollX;
        float _ScrollY;
        //float _Scroll2X;
        //float _Scroll2Y;
        float _Intensity;
        float _VPLightInten;
        float _Alpha;

        struct Input
        {
            float2 uv_MainTex;
        };


        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed2 uv = IN.uv_MainTex.xy + frac(float2(_ScrollX, _ScrollY) * _Time);
            fixed4 tex = tex2D(_MainTex, uv);

            o.Albedo = tex.rgb * _VPLightInten;
            o.Alpha = _Alpha * _Color.a;
            o.Emission = tex.rgb * _Intensity;
        }
        ENDCG
    }

    //Fallback "Legacy Shaders/Transparent/VertexLit"
}