Shader "Unlit/ProgressBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0,1)) = 0.5
        _BarThickness("Thickness", Range(0,1)) = 0.5
        _BarTint("Bar Tint", Color) = (1,1,1,0)
    }
    SubShader
    {
        Tags {"Quene" ="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Progress;
            float _BarThickness;
            float4 _BarTint;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float gradient = i.uv.y;
                if (gradient > 0.5)
                {
                    gradient = 1-0.5;
                }
                gradient += 0.5;
                gradient = pow(gradient, 4);
                _BarTint*=gradient;


                float alpha = 0;

                if (i.uv.y < 0.5 + _BarThickness / 2 && i.uv.y > 0.5 - _BarThickness / 2)
                {
                    if (i.uv.x < _Progress-0.02)
                    {
                        alpha = 1;
                    }
                }
                _BarTint.a = alpha;
                _BarTint *= alpha;

                fixed4 OutColor = lerp(_BarTint, col, col.a);
                return OutColor;
            }
            ENDCG
        }
    }
}
