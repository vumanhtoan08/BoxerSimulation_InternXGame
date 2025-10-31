Shader "Env/RimLight"
{
    Properties
    {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _Shininess ("Shininess", Range (0.03, 1)) = 0.078125
        _RimColor ("Rim Color" , Color) = (0.7, 0.8, 0.7, 0)
        _RimPower ("Rim Power" , Range (0.1, 10)) = 6.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200
        // first pass writes to depth buffer only (make sure sorting is done back-to-front! otherwise occluded fragments will get discarded!)
        Pass
        {
            ZWrite On
            // disable rendering to color channels
            ColorMask 0
 
        }
 
        // second pass renders frag color and alpha ( using transparent surface shader )
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add
        
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        half4 _Color;
        half _Shininess;
        half4 _RimColor;
        half _RimPower;

        struct Input
        {
            half2 uv_MainTex;
            half2 uv_BumpMap;
            half3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
            //o.Gloss = _Color.a;
            o.Alpha = _Color.a * _Color.a;
            o.Gloss = _Shininess;
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow(rim, _RimPower);
        }
        ENDCG
    }

    FallBack "Specular"
}