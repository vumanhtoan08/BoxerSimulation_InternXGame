// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Voxel Play/Voxels/Triangle/Cloud"
{
    Properties
    {
        _MainTex ("Main Texture Array", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Color_1 ("Color Tint", Color) = (1,1,1,1)
        _Color_2 ("Color Tint", Color) = (1,1,1,1)
        _FogColor ("Fog Color (RGB)", Color) = (0.5, 0.5, 0.5, 1.0)
        _FogStart ("Fog Start", Float) = 0.0
        _FogEnd ("Fog End", Float) = 10.0
    }

    SubShader
    {

        Tags
        {
            "Queue" = "Geometry" "RenderType" = "Opaque" "DisableBatching" = "True"
        }
        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
            }
            CGPROGRAM
            #pragma target 3.5
            #pragma vertex   vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                fixed2 light: TEXCOORD1;
                float fog : TEXCOORD2;
            };

            struct vertexInfo
            {
                float4 vertex;
            };

            float4 _Color;
            float4 _Color_1;
            float4 _Color_2;
            float4 _FogColor;
            float _FogStart;
            float _FogEnd;

            v2f vert(appdata v)
            {
                v2f o;

                v.vertex.xyz *= float3(4, 5, 4);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.light = v.normal;

                float zpos = UnityObjectToClipPos(v.vertex).z;
                o.fog = saturate(1.0 - (_FogEnd - zpos) / (_FogEnd - _FogStart));
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col2 = _Color_1;
                fixed4 col1 = _Color;
                fixed4 col3 = _Color_2;
                fixed4 col = lerp(col1, col2, i.light.y == 0);
                col = lerp(col, col3, i.light.y < 0);
                col = lerp(col, col, abs(sin((_Time.w + i.uv.y + i.uv.x))));
                col.rgb *= _Color;
                col.rgb = lerp(col.rgb, _FogColor, i.fog);
                return col;
            }
            ENDCG
        }
    }

    Fallback Off
}