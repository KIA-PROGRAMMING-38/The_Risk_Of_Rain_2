Shader "Custom/TreeShader" {
    Properties{
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Cutoff("Cutoff", Float) = 0.5
        _Move("Move", Float) = 0.5
        _Timing("Timing", Float) = 0.5
    }
        SubShader{
            Tags {"Queue" = "AlphaTest" "RenderType" = "TransparentCutout"}
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert alphatest:_Cutoff addshadow
            #pragma vertex vert

            struct Input {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            float _Move;
            float _Timing;

            void vert(inout appdata_full v) {
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                worldPos.y += sin(_Time.y * _Timing) * _Move;
                v.vertex = mul(unity_WorldToObject, float4(worldPos, 1.0));
            }

            void surf(Input IN, inout SurfaceOutput o) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Legacy Shaders/Transparent/Cutout/Diffuse"
}