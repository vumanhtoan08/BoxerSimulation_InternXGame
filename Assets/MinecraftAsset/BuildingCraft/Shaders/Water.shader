// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Water"
{
    Properties
    {
        _ReflectionTex ("Reflection Texture", 2D) = "white" {}
        _RenderTex ("Render Texture, Show Refraction", 2D) = "white" {}
        _MainTex ("Distort Texture", 2D) = "white" {}

        _DistortValue ("Distortion", Float) = 30

        _RefrReflBlendValue ("Refraction reflection blending", range(0, 1.0)) = 0.5

        _DistortSpeedOnX ("Distort Speed X", Float) = 6
        _DistortSpeedOnY ("Distort Speed Y", Float) = 6
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Opaque"
        }
        LOD 200

        GrabPass {}

        CGPROGRAM
        #pragma surface surf Unlit vertex:vert
         #include "UnityCG.cginc" 
         
         half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) {
           half4 c;
           c.rgb = s.Albedo * .5f;
           c.a = s.Alpha;
           return c;
         }
        #include "UnityCG.cginc"

        struct Input
        {
            float2 uv_MainTex;
            // float2 uv_DistortTex;
            float4 grabTex;
        };

        sampler2D _ReflectionTex;
        sampler2D _RenderTex;
        sampler2D _MainTex;
        float _DistortValue;
        float _RefrReflBlendValue;
        half _DistortSpeedOnX;
        half _DistortSpeedOnY;

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.grabTex = ComputeScreenPos(UnityObjectToClipPos(v.vertex));
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            IN.uv_MainTex.x += _Time * _DistortSpeedOnX;
            IN.uv_MainTex.y += _Time * _DistortSpeedOnY;
            float2 distortion = tex2D(_MainTex, IN.uv_MainTex).rg - half2(0.5, 0.5);
            distortion = distortion * _DistortValue; // * _RenderTex_TexelSize.xy;

            float4 reflUv = IN.grabTex;
            reflUv.xy = distortion * reflUv.z + reflUv.xy;
            half4 refl = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(reflUv)); //i.uvRefl + distortion);

            if (refl.a < 0.01)
            {
                distortion = half2(0, 0);
            }

            IN.grabTex.xy = distortion * IN.grabTex.z + IN.grabTex.xy;

            half4 col = tex2Dproj(_RenderTex, UNITY_PROJ_COORD(IN.grabTex));

            half4 fColor = lerp(col, refl, _RefrReflBlendValue * refl);
            if (refl.a < 0.01)
            {
                fColor = col;
            }


            o.Albedo = fColor.rgb;
        }
        ENDCG
    }
}