Shader "Voxel Play/Voxels/Triangle/Unlit/Cutout Cross"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _LightIntent ("_LightIntent", float) = 1
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
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        float _LightIntent;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            if (c.a < .3)
            {
                discard;
            }

            o.Albedo = c.rgb * _LightIntent;
            o.Alpha = c.a * _Color.a;
        }
        ENDCG
    }

    //Fallback "Legacy Shaders/Transparent/VertexLit"
}