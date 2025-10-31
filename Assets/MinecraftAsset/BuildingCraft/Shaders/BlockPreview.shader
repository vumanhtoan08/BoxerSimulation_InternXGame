// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Block/DiffusePreview"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Outline ("Outline Color", Color) = (0,0,0,1)
        _MaxHeight ("Max Height", Float) = 1.5
        _OutlineSize ("Outline Thickness", Float) = 1.5
        _AOColor("AOColor", float) = 1
        _LightIntensity("LightIntensity", float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 150

        Pass
        {
            Cull Front
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            half _OutlineSize;
            float _MaxHeight;
            fixed4 _Outline;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 vertex : POSITION_1;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                v.vertex.xyz += normalize(v.vertex.xyz) * _OutlineSize;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.vertex = v.vertex;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                if (i.vertex.y >= _MaxHeight)
                {
                    discard;
                }
                return _Outline;
            }
            ENDCG
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _AOColor;
        fixed _MaxHeight;
        float _LightIntensity;

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
            v.texcoord.w = ((int)v.texcoord.w & 0x1FF) / 15.0;
            o.ao = v.texcoord.w + .25;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            if (IN.height >= _MaxHeight)
            {
                discard;
            }
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb * (IN.ao * _AOColor * _LightIntensity + IN.light * (1.2 - _LightIntensity));
            o.Albedo += max(0, 1 - (_MaxHeight - IN.height));
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Diffuse"
}