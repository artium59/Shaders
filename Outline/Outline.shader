Shader "Mobile/Outline" {
    Properties {
        // _Color ("Main Color", Color) = (.5,.5,.5,1)
        // _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineWidth ("Outline Width", Range(0,5.0)) = 0.003
    }
    
    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        
        Pass {
            Cull Off
            ZWrite Off
            ZTest Always
            ColorMask 0
            
            Stencil {
                Ref 1
                Pass Replace
            }
        }
        
        Pass {
            Tags { "Queue" = "Transparent+100" "RenderType" = "Transparent" }
            
            Cull Off
            ZWrite Off
            ZTest Always
            ColorMask RGB
            
            Blend SrcAlpha OneMinusSrcAlpha
            
            Stencil {
                Ref 1
                Comp NotEqual
            }
            
            CGPROGRAM
            #include "UnityCG.cginc"
            
            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
    
            struct v2f {
                float4 pos : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;
            };
    
            float _OutlineWidth;
            float4 _OutlineColor;
            
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
 
	            float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	            float2 offset = TransformViewToProjection(norm.xy);
 
	            o.pos.xy += offset * o.pos.z * _OutlineWidth;
	            o.color = _OutlineColor;
	            
	            return o;
            }
            
            half4 frag(v2f i) : COLOR { return i.color; }
            ENDCG
        }
    }
}