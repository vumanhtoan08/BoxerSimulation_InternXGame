Shader "Skybox/SkyboxFogExtended"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _Alpha ("Alpha", float) = 1
        _Color ("Main Color", Color) = (1,1,1,1)
        _FogNear ("Fog Near", range (0,100)) = 5
        _FogFar ("Fog Far", range (0,100)) = 10
        _FogAltScale ("Fog Alt. Scale", range (0,100)) = 10
        _FogThinning ("Fog Thinning", range (0,100)) = 100
        _FogColor ("Fog Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader
    {
        Pass // ind: 1, name: 
        {
            Tags
            {
                "QUEUE" = "Transparent"
                "RenderType" = "Transparent"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            // m_ProgramMask = 6
            CGPROGRAM
            //#pragma target 4.0

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            #define CODE_BLOCK_VERTEX
            uniform float _Alpha;
            uniform float4 _MainTex_ST;
            uniform sampler2D _MainTex;
            float4 _Color;
            float _FogNear, _FogFar, _FogAltScale, _FogThinning;
            float4 _FogColor;

            struct appdata_t
            {
                float4 vertex :POSITION0;
                float2 texcoord :TEXCOORD0;
                float3 worldPos :TEXCOORD2;
                float4 color :COLOR0;
            };

            struct OUT_Data_Vert
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD2;
                float4 color :COLOR0;
                float3 viewDir : TEXCOORD3;
            };

            struct OUT_Data_Frag
            {
                float4 color :SV_Target0;
            };

            OUT_Data_Vert vert(appdata_t in_v)
            {
                OUT_Data_Vert out_v;
                out_v.vertex = UnityObjectToClipPos(in_v.vertex);
                out_v.uv = TRANSFORM_TEX(in_v.texcoord, _MainTex);
                out_v.worldPos = mul(unity_ObjectToWorld, in_v.vertex);
                out_v.viewDir = normalize(UnityWorldSpaceViewDir(in_v.worldPos));
                out_v.color.xyz = float3(in_v.color.xyz);
                out_v.color.w = 1;
                return out_v;
            }

            #define CODE_BLOCK_FRAGMENT

            OUT_Data_Frag frag(OUT_Data_Vert in_f)
            {
                OUT_Data_Frag out_f;
                fixed4 tex = tex2D(_MainTex, in_f.uv);
                out_f.color = tex * in_f.color;
                out_f.color.a *= _Alpha;

                half4 c = tex2D(_MainTex, in_f.uv);
                float d = length(in_f.viewDir);
                float l = saturate((d - _FogNear) / (_FogFar - _FogNear) / clamp(in_f.worldPos.y / _FogAltScale + 1, 1, _FogThinning));
                //o.Albedo = c.rgb * _Color.rgb;
                out_f.color.rgb = lerp(c.rgb * _Color.rgb, _FogColor, l);
                out_f.color.a = c.a * _Color.a * _Alpha;
                return out_f;
            }
            ENDCG

        } // end phase
    }
    FallBack Off
}