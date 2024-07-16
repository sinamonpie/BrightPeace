Shader"Custom/OutlineShader"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,1,0,1) // 노란색
        _OutlineThickness ("Outline Thickness", Range (0.002, 0.03)) = 0.01
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            
Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 pos : POSITION;
    float4 color : COLOR;
};

float _OutlineThickness;
float4 _OutlineColor;

v2f vert(appdata v)
{
                
    v2f o;
    float3 norm = mul((float3x3) UNITY_MATRIX_IT_MV, v.normal);
    o.pos = UnityObjectToClipPos(v.vertex + float4(norm * _OutlineThickness, 0));
    o.color = _OutlineColor;
    return o;
}

half4 frag(v2f i) : COLOR
{
    return i.color;
}
            ENDCG
        }
        Pass
        {
            // 기본 표면 렌더링
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 pos : POSITION;
    float2 uv : TEXCOORD0;
};

sampler2D _MainTex;

v2f vert(appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

half4 frag(v2f i) : COLOR
{
    return tex2D(_MainTex, i.uv);
}
            ENDCG
        }
    }
FallBack"Diffuse"
}
