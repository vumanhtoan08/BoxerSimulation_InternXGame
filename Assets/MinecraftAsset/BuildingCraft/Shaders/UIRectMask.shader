Shader "Legacy Shaders/UIRectMask"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Alpha("Alpha", float) = .3
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _RectMaskUI;
        float _Alpha;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 vertex;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.vertex = UnityObjectToViewPos(v.vertex);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            if(IN.vertex.x < _RectMaskUI.x || IN.vertex.x > _RectMaskUI.z || IN.vertex.y < _RectMaskUI.y || IN.vertex.y > _RectMaskUI.w)
            {
                discard;
            }

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            if (c.a < _Alpha)
            {
                discard;
            }

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/VertexLit"
}