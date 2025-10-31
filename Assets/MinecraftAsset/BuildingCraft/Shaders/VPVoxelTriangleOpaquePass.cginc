// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

struct appdata
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members fogVar)
#pragma exclude_renderers d3d11
{
    float4 vertex : POSITION;
    float4 uv : TEXCOORD0;
    float3 normal : NORMAL;
};


