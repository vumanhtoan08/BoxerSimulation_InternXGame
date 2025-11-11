Shader "XGame/DiffuseNew"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (1,1,1,1)
        _Intensity("Intensity", Range(0, 2)) = 1.0
        _SpecColor("Specular Color", Color) = (0,0,0,1)      // Màu specular mặc định là đen
        _Shininess("Shininess", Range(0.1, 10)) = 10        // Độ bóng (Specular power)
        _EmissionColor("Emission Color", Color) = (0,0,0,1)  // Màu phát sáng, mặc định là đen
        [HideInInspector] _TerrainCompatible("TerrainCompatible", Float) = 1
    }
        SubShader
        {
            Tags { "TerrainCompatible" = "True" }
            Tags { "RenderType" = "Opaque" }
            LOD 100

            CGPROGRAM
            #pragma surface surf BlinnPhong     // Sử dụng BlinnPhong để hỗ trợ Specular

            struct Input
            {
                float2 uv_MainTex;
                float3 worldNormal;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Intensity;
            float _Shininess;       // Biến cho độ bóng
            fixed4 _EmissionColor;  // Biến cho màu phát sáng
            float _Cutoff;

            void surf(Input IN, inout SurfaceOutput o)
            {
                // Lấy màu từ Main Texture
                fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

                // Áp dụng cường độ vào Albedo
                o.Albedo = texColor.rgb * _Intensity;
                o.Alpha = texColor.a;

                // Áp dụng Specular
                o.Specular = _Shininess;    // Độ bóng (sức mạnh của specular)
                o.Gloss = 1.0;              // Độ sáng bóng (có thể điều chỉnh nếu muốn)

                // Áp dụng Emission
                o.Emission = _EmissionColor.rgb;  // Thêm màu phát sáng
            }
            ENDCG
        }
            FallBack "Diffuse"
}