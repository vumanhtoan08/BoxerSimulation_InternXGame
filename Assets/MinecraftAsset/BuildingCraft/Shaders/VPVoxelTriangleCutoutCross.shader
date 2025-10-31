Shader "Voxel Play/Voxels/Triangle/Cutout Cross"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _VPGrassWindSpeed("_VPGrassWindSpeed", float) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"
        }
        LOD 150
        ZWrite On
        Cull Off
        Offset -2, -1
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _VPLightInten;
        fixed _VPGrassWindSpeed;

        struct Input
        {
            float2 uv_MainTex;
            float light;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            float3 wpos = v.vertex;
            int iuvz = (int)v.texcoord.z;
            float disp = /*(iuvz >> 16) **/ sin(wpos.x + _Time.w) * _VPGrassWindSpeed;
            
            v.vertex.x += disp * v.texcoord.w;
            
            //o.light = v.texcoord.w / (4096.0 * 15.0);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            if (c.a < .3)
            {
                discard;
            }

            o.Albedo = c.rgb * (_VPLightInten + IN.light * (1.2 - _VPLightInten));
            o.Alpha = c.a * _Color.a;
        }
        ENDCG
    }

    //Fallback "Legacy Shaders/Transparent/VertexLit"
}