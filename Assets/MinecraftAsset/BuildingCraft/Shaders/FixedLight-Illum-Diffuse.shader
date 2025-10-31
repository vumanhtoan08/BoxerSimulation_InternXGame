Shader "FixedLight/Illum/Diffuse"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Rotate("Rotate", Vector) = (30,50,0,0)
        _LightColor("Color", Color) = (1,1,1,0)
        _Intensity("Intensity", Float) = 1
        _Illum ("Illumin (A)", 2D) = "white" {}
        _Emission ("Emission (Lightmapper)", Float) = 1.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 150

        CGPROGRAM
        #pragma surface surf SimpleLambert

        sampler2D _MainTex;
        sampler2D _Illum;
        half3 _Rotate;
        half4 _LightColor;
        half _Intensity;
        half _Emission;

        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Illum;
        };
        
        // Allows us to use the SimpleLambert lighting mode
        half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            // First calculate the dot product of the light direction and the
            // surface's normal
            half NdotL = dot(s.Normal, _Rotate);

            // Next, set what color should be returned
            half4 color;

            color.rgb = s.Albedo * _LightColor.rgb * (NdotL * atten) + _Intensity;
            color.a = s.Alpha;

            // Return the calculated color
            return color;
        }


        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
             o.Emission *= _Emission.rrr;
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}