// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Block/BlockPreviewCutoutNoLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _AOColor("AOColor", float) = 1
        _Alpha("Alpha", float) = .3
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 150
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _AOColor;
        float _VPLightInten;
        float _Alpha;

        struct Input
        {
            float2 uv_MainTex;
            float ao;
            float height;
            float alpha;
            float light;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.height = v.vertex.y;
            o.alpha = v.texcoord.z;
            o.light = v.texcoord.w / (4096.0 * 15.0);
            v.texcoord.w = 1; //((int)v.texcoord.w & 0x1FF) / 15.0;
            o.ao = v.texcoord.w + .25;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            if (c.a < _Alpha)
            {
                discard;
            }
            o.Albedo = c.rgb * (IN.ao * _AOColor/* * _VPLightInten + IN.light * (1.2 - _VPLightInten)*/);
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Diffuse"
}