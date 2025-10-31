Shader "Custom/Selector"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white" {}
    }

    SubShader
    {
        // Regular color & lighting pass
        Pass
        {
            Tags
            {
                "Queue"="Transparent"
                //"Queue" = "Transparent+1"
                "IgnoreProjector"="True"
                //"RenderType" = "Opaque"
                "RenderType"="Transparent"
                "PreviewType"="Plane"
                "CanUseSpriteAtlas"="True"
            }
            Cull Front // draw back faces
            ZWrite OFF
            ZTest Always
            Cull Off
            Lighting Off
            Blend One OneMinusSrcAlpha
            // Write to Stencil buffer (so that silouette pass can read)
            Stencil
            {
                Ref 4
                Comp always
                Pass replace
                ZFail keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase // shadows
            #include "AutoLight.cginc"
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            sampler2D _RampTex;
            float4 _LightColor0; // provided by Unity

            struct vertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 texCoord : TEXCOORD0;
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float3 texCoord : TEXCOORD0;
                LIGHTING_COORDS(1, 2) // shadows
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                // convert input to world space
                output.pos = UnityObjectToClipPos(input.vertex);
                float4 normal4 = float4(input.normal, 0.0); // need float4 to mult with 4x4 matrix
                output.normal = normalize(mul(normal4, unity_WorldToObject).xyz);

                output.texCoord = input.texCoord;

                // TRANSFER_VERTEX_TO_FRAGMENT(output); // shadows
                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                fixed4 t = tex2D(_MainTex, input.texCoord);
                t.rgb *= t.a;
                return t;
            }
            ENDCG
        }

        // Silouette pass 1 (backfaces)
//        Pass
//        {
//            Tags
//            {
//                "Queue"="Transparent"
//                //"Queue" = "Transparent+1"
//                "IgnoreProjector"="True"
//                //"RenderType" = "Opaque"
//                "RenderType"="Transparent"
//                "PreviewType"="Plane"
//                "CanUseSpriteAtlas"="True"
//            }
//            // Won't draw where it sees ref value 4
//            Cull Front // draw back faces
//            ZWrite OFF
//            ZTest Always
//            Cull Off
//            Lighting Off
//            Blend One OneMinusSrcAlpha
//            Stencil
//            {
//                Ref 3
//                Comp Greater
//                Fail keep
//                Pass replace
//            }
//
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//
//            // Properties
//            uniform float4 _SilColor;
//            sampler2D _MainTex;
//
//            struct vertexInput
//            {
//                float4 vertex : POSITION;
//                float2 texcoord : TEXCOORD0;
//            };
//
//            struct vertexOutput
//            {
//                float4 pos : SV_POSITION;
//                float2 texcoord : TEXCOORD0;
//            };
//
//            // vertexOutput vert(vertexInput input)
//            // {
//            //     vertexOutput output;
//            //     output.pos = UnityObjectToClipPos(input.vertex);
//            //     return output;
//            // }
//            //
//            // float4 frag(vertexOutput input) : COLOR
//            // {
//            //     fixed4 t = tex2D(_MainTex, input.texcoord);
//            //     t.rgb *= t.a;
//            //     return t;
//            // }
//            ENDCG
//        }


    }
}