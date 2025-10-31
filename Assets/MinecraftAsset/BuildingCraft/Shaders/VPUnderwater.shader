Shader "Voxel Play/FX/Underwater"
{
    Properties
    {
        _MainTex ("Distort Texture", 2D) = "white" {}

        _DistortValue ("Distortion", float) = 30

        _DistortSpeedOnX ("Distort Speed X", float) = 6
        _DistortSpeedOnY ("Distort Speed Y", float) = 6
        _Color ("Main Color", Color) = (1,1,1,1)
        _WaterLevel ("Water Level", float) = 60

    }
    SubShader
    {
        // In tags we use Transparent+1 to get everything behind
        Tags
        {
            "Queue"="Transparent+1" "RenderType"="Opaque"
        }

        LOD 200

        GrabPass
        {
            "_GrabTex"
        }

        CGPROGRAM
        #pragma surface surf Unlit vertex:vert
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
            float2 uv_MainTex;
            float4 GrabTexUV : TEXCOORD1;
            half2 wpos;
        };

        sampler2D _GrabTex;
        sampler2D _MainTex;
        fixed4 _Color;
        half _WaterLevel;
        half _DistortValue;
        half _DistortSpeedOnX;
        half _DistortSpeedOnY;

        void vert(inout appdata_base v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.wpos = mul(unity_ObjectToWorld, v.vertex);
            float4 hpos = UnityObjectToClipPos(v.vertex);
            o.GrabTexUV = ComputeGrabScreenPos(hpos); // compute the uvs for the grab texture (using Unity's utilities)
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            if (_WaterLevel > IN.wpos.y)
            {
                IN.uv_MainTex.x += _Time * _DistortSpeedOnX;
                IN.uv_MainTex.y += _Time * _DistortSpeedOnY;
                half2 distortion = tex2D(_MainTex, IN.uv_MainTex).rg * _DistortValue;

                IN.GrabTexUV.xy = distortion * IN.GrabTexUV.z + IN.GrabTexUV.xy;
                half4 col = tex2Dproj(_GrabTex, UNITY_PROJ_COORD(IN.GrabTexUV));
                o.Albedo = col.rgb * _Color;
            }
            else
            {
                o.Albedo = tex2Dproj(_GrabTex, UNITY_PROJ_COORD(IN.GrabTexUV));
            }
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}