Shader"Outlined/Regular" {
    Properties {
        _Color ("Main Color", Color) = (0.1, 0.1, 0.1, 1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1) // 노란색 외곽선
        _Outline ("Outline width", Range (0, 1)) = 0.1
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }

    SubShader {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType"="Transparent" }

        // Outline pass
        Pass {
Name"OUTLINE"
            Tags
{"Queue" = "Overlay" "IgnoreProjector" = "True"
}

Cull Front

ZWrite Off

            CGPROGRAM
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

float _Outline;
float4 _Color;
float4 _OutlineColor;

v2f vert(appdata v)
{
                // just make a copy of incoming vertex data but scaled according to normal direction
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);

    float3 norm = mul((float3x3) UNITY_MATRIX_IT_MV, v.normal);
    float2 offset = TransformViewToProjection(norm.xy);

    o.pos.xy += offset * o.pos.z * _Outline;
    o.color = _OutlineColor;
    return o;
}

half4 frag(v2f i) : COLOR
{
    return i.color;
}

            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }

        // Main texture and color pass
        CGPROGRAM
        #pragma surface surf Lambert alpha

sampler2D _MainTex;
fixed4 _Color;

struct Input
{
    float2 uv_MainTex;
};

void surf(Input IN, inout SurfaceOutput o)
{
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
}
        ENDCG
    }

Fallback"Diffuse"
}
