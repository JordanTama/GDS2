﻿Shader "Crowd/Umbrella"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _Offset ("Animation Cycle Offset", Range(0, 1)) = 0
        
        _BobSpd ("Bob Speed", Float) = 1
        _BobAmt ("Bob Amount", Float) = 1
        _SwayAmt ("Sway Amount", Float) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow
        #pragma target 3.0

        #include "Helper.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float _Offset;

        float _BobSpd;
        float _BobAmt;

        float _SwayAmt;
        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v)
        {
            float time = _Time[1] * _BobSpd;
            float offset = _Offset * PI * 2;
            
            // Calculate bob axis
            float3 bobAxis = normalize(mul(unity_WorldToObject, float3(0, 1, 0)));

            // Calculate the sway axis
            float3 swayAxis = normalize(ProjectOnPlane(float3(0, 0, 1), bobAxis));

            // Apply bob and sway to vertex position in local space
            v.vertex.xyz = mul(AngleAxis3x3(_SwayAmt * sin(time * 0.5 + offset), swayAxis), v.vertex.xyz);
            v.vertex.xyz += bobAxis * (_BobAmt * sin(time + offset));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
        
        
        
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Front

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow
        #pragma target 3.0

        #include "Helper.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float _Offset;

        float _BobSpd;
        float _BobAmt;

        float _SwayAmt;
        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v)
        {
            float time = _Time[1] * _BobSpd;
            float offset = _Offset * PI * 2;
            
            // Calculate bob axis
            float3 bobAxis = normalize(mul(unity_WorldToObject, float3(0, 1, 0)));

            // Calculate the sway axis
            float3 swayAxis = normalize(ProjectOnPlane(float3(0, 0, 1), bobAxis));

            // Apply bob and sway to vertex position in local space
            v.vertex.xyz = mul(AngleAxis3x3(_SwayAmt * sin(time * 0.5 + offset), swayAxis), v.vertex.xyz);
            v.vertex.xyz += bobAxis * (_BobAmt * sin(time + offset));

            v.normal = -v.normal;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
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
