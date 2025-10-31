Shader "FixedLight/Diffuse"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Rotate("Rotate", Vector) = (30,50,0,0)
        _LightColor("Color", Color) = (1,1,1,0)
        _Intensity("Intensity", Float) = 1
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
        half3 _Rotate;
        half4 _LightColor;
        half _Intensity;

        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        // Allows us to use the SimpleLambert lighting mode
        half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            // First calculate the dot product of the light direction and the
            // surface's normal
            half NdotL = dot(s.Normal, _Rotate);

            // Next, set what color should be returned
            half4 color;

            color.rgb = s.Albedo * _LightColor.rgb * (NdotL * atten) * _Intensity;
            color.a = s.Alpha;

            // Return the calculated color
            return color;
        }


        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}