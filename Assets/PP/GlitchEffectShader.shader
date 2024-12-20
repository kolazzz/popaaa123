Shader "Custom/GlitchEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0,5)) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Intensity;
            float _TimeY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // �������� ������ ��� �������� �����-�������
                float glitchOffset = sin(_TimeY * 10.0) * _Intensity * 0.1;
                uv.x += glitchOffset;

                float4 color = tex2D(_MainTex, uv);

                // �������� ��� �������� � ������ �������
                color.g = tex2D(_MainTex, uv + float2(glitchOffset * 0.5, 0)).g;
                color.b = tex2D(_MainTex, uv - float2(glitchOffset * 0.5, 0)).b;

                return color;
            }
            ENDCG
        }
    }
}
