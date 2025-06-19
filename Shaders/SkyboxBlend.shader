Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _SkyboxDay ("Day Skybox (Cubemap)", CUBE) = "" {}
        _SkyboxNight ("Night Skybox (Cubemap)", CUBE) = "" {}
        _Blend ("Day/Night Blend", Range(0,1)) = 0.0
        _SunColor ("Sun Light Color", Color) = (1,1,1,1)
        _SunDirection ("Sun Direction", Vector) = (0,1,0,0)
        _MoonColor ("Moon Light Color", Color) = (1,1,1,1)
        _MoonDirection ("Moon Direction", Vector) = (0,1,0,0)
        // ...existing properties...
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // ...existing includes...

            samplerCUBE _SkyboxDay;
            samplerCUBE _SkyboxNight;
            float _Blend;
            float4 _SunColor;
            float4 _SunDirection;
            float4 _MoonColor;
            float4 _MoonDirection;
            // ...existing uniforms...

            struct appdata
            {
                float4 vertex : POSITION;
                // ...existing code...
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldDir : TEXCOORD0;
                // ...existing code...
            };

            v2f vert (appdata v)
            {
                v2f o;
                // ...existing code...
                o.worldDir = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 dir = normalize(i.worldDir);

                // Sample both skyboxes
                fixed4 dayColor = texCUBE(_SkyboxDay, dir);
                fixed4 nightColor = texCUBE(_SkyboxNight, dir);

                // Blend between day and night skyboxes
                fixed4 skyColor = lerp(nightColor, dayColor, _Blend);

                // Calculate sun and moon lighting
                float sunIntensity = saturate(dot(dir, normalize(_SunDirection.xyz)));
                float moonIntensity = saturate(dot(dir, normalize(_MoonDirection.xyz)));

                // Blend sun and moon light based on _Blend
                float lightBlend = _Blend;
                float4 lightColor = lerp(_MoonColor * moonIntensity, _SunColor * sunIntensity, lightBlend);

                // Combine sky color and light
                fixed4 finalColor = skyColor * (1.0 + lightColor.rgb);

                finalColor.a = 1.0;
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "RenderFX/Skybox"
}
