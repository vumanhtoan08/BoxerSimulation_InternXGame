Shader "VPVoxelTrianglePartical"
{
    Properties
    {
        _MainTex ("Particle Texture Top", 2D) = "white" {}
        _TexSides ("Particle Texture Sides", 2D) = "white" {}
        _TexBottom ("Particle Texture Bottom", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1)
    }

    SubShader
    {
        Pass
        {
           Tags
        {
            "RenderType"="Opaque"
        }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD2;
                float3 normal: TEXCOORD1;
            };

            sampler2D _TexSides, _TexBottom, _MainTex;
            float4 _MainTex_ST,_TexSides_ST;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                o.uv2 = TRANSFORM_TEX (v.texcoord, _TexSides);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col1 = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_TexSides, i.uv2);
                fixed4 col3 = tex2D(_TexSides, i.uv2);
                fixed4 col = lerp(col1, col2, i.normal.y == 0);
                col = lerp(col, col3, i.normal.y < 0);
                col = lerp(col, col * 1, abs(sin((_Time.w + i.uv.y + i.uv.x))));
                col.rgb *= _Color;
                col.a = col3.a;
                return col;
            }
            ENDCG
        }
    }
}