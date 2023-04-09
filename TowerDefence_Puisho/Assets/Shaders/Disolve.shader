Shader "Custom/Disolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Ammount("Ammount", Range(0,1)) = 0.0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseDissolve ("Noise (RGB)", 2D) = "white" {}
        [HDR]_EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _AmmountEmission ("Ammount Emission", Range(0,1)) = 0.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        sampler2D _NoiseDissolve;
        half _Ammount;
        fixed4 _EmissionColor;
        half _AmmountEmission;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half _dissolve_value = tex2D(_NoiseDissolve, IN.uv_MainTex).r;
            clip(_dissolve_value - _Ammount);

            if(_dissolve_value - _Ammount < 0.05f)
            o.Emission = _EmissionColor * _AmmountEmission;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
