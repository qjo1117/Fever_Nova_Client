Shader "Tutorial/054_UnlitDynamicDecal"{
	// 인스펙터 상에서 편집할 수 있도록 값 표시
	Properties{
		[HDR] _Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{
		// 해당 셰이더를 적용한 마테리얼은 완전히 투명하며, 이 마테리얼을 적용한 투명 도형 앞에 렌더링됩니다.
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent-400" "DisableBatching" = "True"}

		// 알파값을 통해 혼합 (blend)
		Blend SrcAlpha OneMinusSrcAlpha
		// 반투명성이 있어서 zbuffer에 쓸 수 없다.
		ZWrite off

		Pass{
			CGPROGRAM

			// 유용한 셰이더 기능 include
			#include "UnityCG.cginc"

			// vertex shader, fragment (pixel) shader 사용한다고 정의
			#pragma vertex vert
			#pragma fragment frag

			// Texture와 Texture의 Transform (위치정보)
			sampler2D _MainTex;
			float4 _MainTex_ST;

			// 텍스쳐의 색감
			fixed4 _Color;

			// 깊이 정보를 가지는 global texture
			sampler2D_float _CameraDepthTexture;

			// Vertex Shader로 읽는 Mesh Data
			struct appdata {
				float4 vertex : POSITION;
			};

			// 래스터라이저 : 3d 벡터 정보 -> 2d의 픽셀로 표현해줌
			// vertex에서 fragment (pixel) shader로 전달되고, 래스터라이저에 의해 보간되는 데이터
			struct v2f {
				float4 position : SV_POSITION;
				float4 screenPos : TEXCOORD0;
				float3 ray : TEXCOORD1;
			};

			// object space?? , world space??, clip space??

			// vertex shader 함수
			v2f vert(appdata v) {
				v2f o;
				// 올바르게 렌더링 될수 있도록 정점 위치를 object space에서 clip space로 변환
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.position = UnityWorldToClipPos(worldPos);
				// 카메라와 정점 사이의 ray(거리?,벡터값?) 을 구한다.
				o.ray = worldPos - _WorldSpaceCameraPos;
				// 변환된 Clip Position으로 Screen Position을 구한다.
				o.screenPos = ComputeScreenPos(o.position);
				return o;
			}

			float3 getProjectedObjectPos(float2 screenPos, float3 worldRay) {
				// Screen Position에 해당되는 지점의 depth값을 depthBuffer의 texture 이용하여 알아낸다.
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenPos);
				depth = Linear01Depth(depth) * _ProjectionParams.z;
				// decal을 그려줄 위치를 바라보는 길이가 1 짜리인 ray를 가져온다.
				worldRay = normalize(worldRay);
				// 뷰행렬의 3번쨰 행은 카메라의 forward vector를 나타내기떄문에, worldRay와 3번쨰행의 역방향 벡터를 dot product하면 worldRay의
				// 역방향 거리를 알 수 있다.
				worldRay /= dot(worldRay, -UNITY_MATRIX_V[2].xyz);
				// World position과 Object position을 재구성한다.
				float3 worldPos = _WorldSpaceCameraPos + worldRay * depth;
				float3 objectPos = mul(unity_WorldToObject, float4(worldPos,1)).xyz;
				// discard pixels where any component is beyond +-0.5
				clip(0.5 - abs(objectPos));
				// -0.5 | 0.5 로 되어있는 공간을 0 | 1 로 바꾼다. (clip space가 0 | 1 로 되어있기 떄문이 아닐까?)
				objectPos += 0.5;
				return objectPos;
			}

			// fragment (pixel) shader 함수
			fixed4 frag(v2f i) : SV_TARGET{
				//unstretch screenspace uv and get uvs from function
				float2 screenUv = i.screenPos.xy / i.screenPos.w;
				float2 uv = getProjectedObjectPos(screenUv, i.ray).xz;
				//read the texture color at the uv coordinate
				  fixed4 col = tex2D(_MainTex, uv);
				  //multiply the texture color and tint color
				  col *= _Color;
				  //return the final color to be drawn on screen
				  return col;
			}

			  ENDCG
			}

		}
}

	