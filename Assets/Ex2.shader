Shader "Unlit/Ex2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _MainTex2 ("Texture2", 2D) = "black" {}
        _Rotation ("Rotation", float) = 0
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
            sampler2D _MainTex2;
            float4 _MainTex_ST;
            float4 _MainTex2_ST;
            float _Rotation;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv - 0.5) * (sin(_Time.x * 20) * 3 + 4) + 0.5;

                o.uv.xy = o.uv * 2 - 1;
                float c = cos(_Rotation + _Time.y);
                float s = sin(_Rotation + _Time.y);
                float2x2 mat = float2x2(c, -s,s,c);
                o.uv.xy = mul(o.uv.xy, mat);
                o.uv.xy = o.uv * 0.5 + 0.5f;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                if(i.uv.x < 0 || i.uv.y < 0 || i.uv.x > 1 || i.uv.y > 1) {
                    col = tex2D(_MainTex, float2(0, 0));
                }
                else {
                    col = tex2D(_MainTex, i.uv);
                }
                fixed4 col2 = tex2D(_MainTex2, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
