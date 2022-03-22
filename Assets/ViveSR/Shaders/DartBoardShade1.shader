Shader "Universal Render Pipeline/Vive/DartBoardShader1" {
	Properties{
		_Color0("Color_0", Color) = (0.48, 0.44, 0.32, 1)
		_Color1("Color_1", Color) = (0.07, 0.07, 0.08, 1)
		_Color2("Color_2", Color) = (0.39, 0.09, 0.09, 1)
		_Color3("Color_3", Color) = (0.09, 0.27, 0.09, 1)
		_ColorHit("Color_Hit", Color) = (0.8, 0.78, 0, 1)
		
		_FrameWidth("FrameWidth", Int) = 1
		_MeshForward("MeshForward", Vector) = (0, 0, 0, 0)
		_MeshRight("MeshRight", Vector) = (0, 0, 0, 0)
		_MeshCenter("MeshCenter", Vector) = (0, 0, 0, 0)
		_BendCount("BendCount", Int) = 1
		_PieCount("PieCount", Int) = 1
		_BendIndex("BendIndex", Int) = -2
		_PieIndex("PieIndex", Int) = 1
		_Scale("Scale", Float) = 1
		_MainTex("BaseMap", 2D) = "white" {}

	// ObsoleteProperties
		//[HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
		//[HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
		[HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
		[HideInInspector] _Glossiness("Smoothness", Float) = 0.0
		[HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0
	}
		SubShader{
			Tags {"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
			LOD 300

            // ------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
                // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
                // no LightMode tag are also rendered by Universal Render Pipeline
                Name "ForwardLit"
                Tags{"LightMode" = "UniversalForward"}

                Blend[_SrcBlend][_DstBlend]
                ZWrite[_ZWrite]
                Cull[_Cull]

                HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            // All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _OCCLUSIONMAP

            #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _RECEIVE_SHADOWS_OFF

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "LitInput.hlsl"
            #include "LitForwardPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

            // This pass it not used during regular rendering, only for lightmap baking.
            Pass
            {
                Name "Meta"
                Tags{"LightMode" = "Meta"}

                Cull Off

                HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMeta

            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma shader_feature _SPECGLOSSMAP

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
            ENDHLSL
        }

			CGPROGRAM
			#pragma surface surf NoLighting alpha

			fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
			{
				return fixed4(s.Albedo, s.Alpha);
			}

			sampler _MainTex;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
			};

			fixed3 _Color0, _Color1, _Color2, _Color3, _ColorHit;
			float4 _MeshCenter, _MeshRight, _MeshForward;
			uint _BendIndex, _PieIndex, _BendCount, _PieCount, _FrameWidth;
			float _Scale;

			int signedAngle(float3 v1, float3 v2, float3 v_forward)
			{
				float dotP = dot(v1, v2);
				float unsignedAngle = acos(dotP) * (180 / 3.14159);

				float sign = dot(v_forward, cross(v1, v2));
				float signedAngle = unsignedAngle * (sign >= 0 ? 1 : -1) + 180;

				return signedAngle;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed tex_a = tex(_MainTex, IN.uv_MainTex).a;

				fixed4 c;
				c.rgb = _Color0 / 2;
				c.a = 1;

				float maxDist = 0.42 * _Scale;
				float dist;

				if (_BendIndex == -2) dist = distance(fixed2(0, 0), IN.uv_MainTex - fixed2(0.5, 0.5));
				else dist = distance(IN.worldPos, _MeshCenter);

				if (dist > maxDist) clip(-1);
				else if (_BendIndex != -2)
				{
					float3 v1 = _MeshRight;
					float3 v2 = normalize(IN.worldPos - _MeshCenter);
					int ang = signedAngle(v1, v2, _MeshForward);
					float pieDegree = 360 / _PieCount;
					uint pieIndex = ang / pieDegree;

					float bendSectionLength = maxDist / _BendCount;
					uint bendIndex = dist / bendSectionLength;

					if (bendIndex <= _BendCount - 1 && bendIndex >= _BendCount - _FrameWidth)
						c.rgb = _Color1;
					else if (bendIndex == 0)
						c.rgb = (_BendIndex == bendIndex) ? _ColorHit : _Color2;
					else if (bendIndex == 1)
						c.rgb = (_BendIndex == bendIndex) ? _ColorHit : _Color3;
					else if (bendIndex % 4 == 2)
					{
						if (_BendIndex >= bendIndex && _BendIndex <= bendIndex + 2 && _PieIndex == pieIndex)
							c.rgb = _ColorHit;
						else c.rgb = (pieIndex % 2 == 0) ? _Color1 : _Color0;
					}
					else if (bendIndex % 4 == 3)
					{
						if (_BendIndex >= bendIndex - 1 && _BendIndex <= bendIndex + 1 && _PieIndex == pieIndex)
							c.rgb = _ColorHit;
						else c.rgb = (pieIndex % 2 == 0) ? _Color1 : _Color0;
					}
					else if (bendIndex % 4 == 0)
					{
						if (_BendIndex >= bendIndex - 2 && _BendIndex <= bendIndex && _PieIndex == pieIndex)
							c.rgb = _ColorHit;
						else c.rgb = (pieIndex % 2 == 0) ? _Color1 : _Color0;
					}
					else if (bendIndex % 4 == 1)
					{
						if (_BendIndex == bendIndex && _PieIndex == pieIndex)
							c.rgb = _ColorHit;
						else c.rgb = (pieIndex % 2 == 0) ? _Color2 : _Color3;
					}
				}

				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}