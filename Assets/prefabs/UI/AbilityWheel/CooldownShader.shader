Shader "Unlit/CooldownShader"
{
    Properties
    {
        _Color("Color", color) = (1,1,1,1)
        _Thickness("Thickness", range(0,0.5)) = 0.1
        _Progress("Progress", range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
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

            float4 _Color;
            float _Thickness;
            float _Progress;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = _Color;
                float2 coord = i.uv - float2(0.5, 0.5);
                float DistanceToCenter = length(coord);
                if (DistanceToCenter > 0.5 || DistanceToCenter < 0.5 - _Thickness)
                {
                    color = float4(0, 0, 0, 0);
                }
                float2 dir = normalize(coord);
                float angle = degrees(acos(dot(dir, float2(0, -1))))/360;
                if (coord.x > 0)
                {
                    angle = 0.5 + degrees(acos(dot(dir, float2(0, 1)))) / 360;
                }

                if (angle < _Progress)
                {
                    color = float4(0, 0, 0, 0);
                }

                return color;
            }
            ENDCG
        }
    }
}
