Shader "Unlit/Hologram" //A shader in Unity (using HLSL), to create a repeating, translucent holographic pattern shown on objects in the world
{
    Properties
    {
        _Color("Color", Color) = (1, 0, 0, 1)
        _Brightness("Brightness", Range(0, 1)) = 0.16
        _Contrast("Contrast", Range(0, 10)) = 1
        _Proximity("Band Proximity", Range(1, 50)) = 10 //Related to size of the grid in world space: larger means bands are closer together
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent Cutout" }
        LOD 100
        Blend SrcAlpha One
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
                float4 objVertex : TEXCOORD2;
            };

            fixed4 _Color;
            float _Brightness;
            float _Contrast;
            float _Proximity;

            //Gets each vertex in the object to color
            v2f vert (appdata v)
            {
                v2f o;
                o.objVertex = mul(unity_ObjectToWorld, v.vertex); //Needed to tie shader to the world, rather than object, coordinates
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //Applies a formula to a specific coordinate for use in the frag function
            float modCoordVal(float c, float mult)				{ return max(0.35, cos(c * _Proximity * mult)); }
            float modCoordVal(float c, float mult, float exp)	{ return max(0.35, pow(cos(c * _Proximity * mult), exp)); }

            //This function creates the repeating pattern using cosine and powers to color the object.
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color * (modCoordVal(i.objVertex.y, 0.2) + modCoordVal(i.objVertex.z, 0.1, 5) + modCoordVal(i.objVertex.x, 0.1, 5));
                col += _Color * (modCoordVal(i.objVertex.y, 1) + modCoordVal(i.objVertex.z, 0.5, 5) + modCoordVal(i.objVertex.x, 0.5, 5));
                col = pow(col * _Brightness, _Contrast);
                return col;
            }
            ENDCG
        }
    }
}
