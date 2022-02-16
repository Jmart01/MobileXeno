Shader "Unlit/StaminaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress("Progress", range(0,1))=0.5
        _ProgressMaskTex("Texture", 2D) = "white" {}
        _FillingColor("FillingColor", color) = (0,1,0,1)
        _FilledColor("FillingColor", color) = (0,0,1,1)
        
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector" = "True" "RenderType"="Transparent" }
        ZWrite off
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

            sampler2D _MainTex;
            sampler2D _ProgressMaskTex;
            float4 _MainTex_ST;
            float _Progress;
            float4 _FillingColor;
            float4 _FilledColor;


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
                fixed4 progressMaskTex = tex2D(_ProgressMaskTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //this gets the coordinate uvs and shifts them over to the left 1 and down .5
                float2 coord = i.uv- float2(1,0.5);

                //makes the coordinates "face" the same way
                float2 dir = normalize(coord);
                //is serving as a new float that is positiones at (0,-1)
                float2 downDir = float2(0, -1);
                //this gets the angle between the down direction and the other direction
                float angle = degrees(acos(dot(dir, downDir)));
                //sets up a minimum to be colored in on the shifted coordinate grid, not sure why these values specifically
                float rangeMin = (180 - 45) / 2;
                //sets the max for the shifted coordinate grid, again not sure why these values
                float rangeMax = (180 - 45) / 2 + 45;
                //sets a float value to 0
                float progressMask = progressMaskTex.x;
                float4 green = float4(0, 1, 0, 1);
                float4 blue = float4(0, 0, 1, 1);
                if (angle > rangeMin && angle < rangeMax)
                {
                    //sets a new float to the angle subtraction and divides by 45 but not sure what that should do
                    float normalizedRange = (angle-rangeMin)/45;
                    if (_Progress < normalizedRange)
                    {
                        progressMask = 0;
                    }
                }

                if (progressMask > 0)
                {
                    float4 progressCol = _FillingColor;
                    if (_Progress == 1)
                    {
                        progressCol = _FilledColor;
                    }
                    col = progressCol;
                }
                //define two colors that the bar changes to

                //make the mask only affect the bar portion of the texture
                
                return col;
            }
            ENDCG
        }
    }
}
