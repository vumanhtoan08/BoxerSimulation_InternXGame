// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Block/Cutoff"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _AOColor("_AOColor", float) = 1
        _GridSize ("Grid Size", float) = 32
        _Size ("Size", float) = 1
        _MaxMipMapLevel ("Max MipMap level", float) = 5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 150

        CGPROGRAM
        // #pragma multi_compile_fwdadd_fullshadows

        #pragma surface surf Lambert forwardadd fullforwardshadows vertex:vert

        // #pragma skip_variants UNITY_PASS_FORWARDBASE POINT
        // #pragma

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _AOColor;
        float _Size;
        float _GridSize;
        float _VPLightInten;

        struct Input
        {
            float2 uv_MainTex;
            float ao;
            float alpha;
            float2 worldNormal;
            float3 worldPos;
            float light;
        };
        
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.alpha = v.texcoord.z;
            // o.gg = ShadePointLights(o.worldPos, o.worldNormal);
            // o.light = v.texcoord1;
            o.light = v.texcoord.w / (4096.0 * 15.0);
            v.texcoord.w = ((int)v.texcoord.w & 0x1FF) / 15.0;
            o.ao = v.texcoord.w + .25;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
            const float cellSize = (1 / _GridSize);
            float2 uv = float2(localPos.x * (1 - IN.worldNormal.x) + localPos.z * IN.worldNormal.x, localPos.y * (1 - IN.worldNormal.y) + localPos.z * IN.worldNormal.y) / _Size;
            fixed4 c = tex2Dgrad(_MainTex, frac(uv) * cellSize + IN.uv_MainTex, ddx(uv * cellSize), ddy(uv * cellSize)) * _Color;
            if(c.a < .1) discard;
            o.Albedo = c.rgb * ((IN.ao * _AOColor) * _VPLightInten + IN.light * (1.2 - _VPLightInten));
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Diffuse"
}