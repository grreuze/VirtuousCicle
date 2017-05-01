
Shader "Hidden/ScreenOverlayGrr" {
    Properties {
        _MainTex("-", 2D) = "" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    sampler2D _MainTex;
	half4 _Color;

    half4 frag(v2f_img i) : SV_Target {

        half4 src = tex2D(_MainTex, i.uv);
        return lerp(src, _Color, _Color.a);
    }
    ENDCG

    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}