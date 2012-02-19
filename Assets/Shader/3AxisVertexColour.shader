  Shader "3-Axis Projection with Vertex Colours" {
	Properties {
      _MainTex("Texture", 2D) = "white" {}
      _TileFac("Tile Factor", Float) = 5
	}
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float4 color : COLOR;
		  float3 worldNormal;
		  float3 worldPos;
      };
	  
	  sampler2D _MainTex;
	  float _TileFac;
	  
      void surf (Input IN, inout SurfaceOutput o) {
		  
		  float2 s = float2(_TileFac, -_TileFac);
		  
		  float2 tex0 = IN.worldPos.xy/s;
          float2 tex1 = IN.worldPos.zx/s;
          float2 tex2 = IN.worldPos.zy/s;
                    
          float4 color0_ = tex2D(_MainTex, tex0);
          float4 color1_ = tex2D(_MainTex, tex1);
		  float4 color2_ = tex2D(_MainTex, tex2);
		  
		  
		  float3 projNormal = saturate( pow( normalize(IN.worldNormal) *1.5, 4) );
          
          float3 projColor = lerp(color1_, color0_, projNormal.z);
          
		  projColor = lerp(projColor, color2_, projNormal.x);
          
          o.Albedo = IN.color.rgb * projColor;
		  
          //o.Albedo = IN.color.rgb;
		  
		  
      }
      ENDCG
    }
    Fallback "Diffuse"
  }