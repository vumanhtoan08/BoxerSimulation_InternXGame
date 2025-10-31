Shader "Block/Transparent/Diffuse"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
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
        #pragma surface surf Lambert alpha:fade vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _VPLightInten;

        struct Input
        {
            float2 uv_MainTex;
            float alpha;
            float2 worldNormal;
            float3 worldPos;
            float light;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.alpha = v.texcoord.z;
            o.light = v.texcoord.w / (4096.0 * 15.0);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            const float gridSize = 32;
            const float cellSize = (1 / gridSize);

            float2 uv = float2(IN.worldPos.x * (1 - IN.worldNormal.x) + IN.worldPos.z * IN.worldNormal.x, IN.worldPos.y * (1 - IN.worldNormal.y) + IN.worldPos.z * IN.worldNormal.y);
            fixed4 c = tex2Dgrad(_MainTex, frac(uv) * cellSize + IN.uv_MainTex, ddx(uv * cellSize), ddy(uv * cellSize)) * _Color;
            o.Albedo = c.rgb * (_VPLightInten + IN.light * (1.2 - _VPLightInten));
            o.Alpha = c.a * _Color.a * IN.alpha;
        }
        ENDCG
    }

    //Fallback "Legacy Shaders/Transparent/VertexLit"
}