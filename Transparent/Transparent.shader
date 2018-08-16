Shader "Mobile/Transparent" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Spec Color", Color) = (1,1,1,0)
		_Emission ("Emissive Color", Color) = (0,0,0,0)
		_Shininess ("Shininess", Range (0.1, 1)) = 0.7
	    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
	
	SubShader
	{ 
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		LOD 200
		
        // Controls whether pixels from this object are written to the depth buffer (default is On).
        // If you’re drawing solid objects, leave this on.
        // If you’re drawing semitransparent effects, switch to ZWrite Off
        Zwrite Off
        // the value of this stage is multiplied by (1 - source alpha)
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		    Material {
		        Diffuse [_Color]
                Ambient [_Color]
                Shininess [_Shininess]
                Specular [_SpecColor]
                Emission [_Emission]
		    }
		    
		    Lighting On
            SeparateSpecular On
		    
		    // How to combine Texture with Color
		    // texture: current texture
		    // https://docs.unity3d.com/Manual/SL-SetTexture.html
		    SetTexture [_MainTex] {
		        Combine texture
		    }
		    
		    // previous: result value of first SetTexture function
		    // Combine A, Z: A is calculate RGB, Z is calculate A 
		    SetTexture [_MainTex] {
		        ConstantColor [_Color]
		        Combine previous, previous * constant
		    }
		    
			// CGPROGRAM
			// ENDCG
		}
	}
}