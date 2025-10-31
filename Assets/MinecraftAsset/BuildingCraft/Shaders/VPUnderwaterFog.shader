Shader "Voxel Play/FX/VPUnderwaterFog"
{
    Properties
    {
        _MainTex ("Distort Texture", 2D) = "white" {}
        _RenText ("_RenText", 2D) = "white" {}

        _DistortValue ("Distortion", float) = 30

        _DistortSpeedOnX ("Distort Speed X", float) = 6
        _DistortSpeedOnY ("Distort Speed Y", float) = 6
        _Color ("Main Color", Color) = (1,1,1,1)
        _WaterLevel ("Water Level", float) = 60

    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Unlit vertex:vert alpha:fade
        #pragma target 3.5
        #include "UnityCG.cginc"

        half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
        {
            half4 c;
            c.rgb = s.Albedo * .5f;
            c.a = s.Alpha;
            return c;
        }

        struct Input
        {
            half2 uv_MainTex;
            half2 uv_RenText;
            half2 wpos;

            half2 uv_CausticTex1;
            half2 uv_CausticTex2;
            half2 uv_CausticMask;
        };

        sampler2D _MainTex;
        sampler2D _RenText;

        half4 _Color;
        half _DistortValue;
        half _DistortSpeedOnX;
        half _DistortSpeedOnY;
        half _WaterLevel;


        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.wpos = mul(unity_ObjectToWorld, v.vertex);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            if (_WaterLevel > IN.wpos.y)
            {
                IN.uv_MainTex.x += _Time * _DistortSpeedOnX;
                IN.uv_MainTex.y += _Time * _DistortSpeedOnY;
                half2 distortion = tex2D(_MainTex, IN.uv_MainTex).rg * _DistortValue;
            
                IN.uv_RenText.xy = distortion + IN.uv_RenText.xy;
                half4 col = tex2D(_RenText, IN.uv_RenText);
                o.Albedo = col.rgb * _Color;
                o.Alpha = _Color.a;
            }
            else
            {
                half4 col = tex2D(_RenText, IN.uv_RenText);
                o.Albedo = col.rgb;
                o.Alpha = _Color.a;
            }
        }
        ENDCG
    }
}