Shader "Env/EnvTransp"
{
    Properties
    {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _BumpMap ("Normal Mapa", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"
        }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade vertex:vert

        sampler2D _MainTex;
        half _VPLightInten;

        half4 _Color;

        struct Input
        {
            half2 uv_MainTex;
            float light;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.light = v.texcoord.w / (4096.0 * 15.0);
        }
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * 1.175 * _Color * (_VPLightInten + IN.light * (1.2 - _VPLightInten));
            o.Alpha = c.a * _Color.a;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}