// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9754,x:33077,y:32697,varname:node_9754,prsc:2|diff-8767-OUT,emission-8010-OUT,alpha-5263-OUT,refract-6574-OUT;n:type:ShaderForge.SFN_Tex2d,id:2493,x:32253,y:32600,ptovrint:False,ptlb:Smoke,ptin:_Smoke,varname:node_2493,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:80819398a9ea6684389a1f47526fc8cc,ntxv:0,isnm:False|UVIN-1732-OUT;n:type:ShaderForge.SFN_Tex2d,id:9736,x:31862,y:32619,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_9736,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f122d155dbe20c44a8832f99dc904455,ntxv:0,isnm:False|UVIN-8320-UVOUT;n:type:ShaderForge.SFN_Panner,id:8320,x:31690,y:32619,varname:node_8320,prsc:2,spu:-0.3,spv:-0.2|UVIN-444-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:444,x:31522,y:32619,varname:node_444,prsc:2,uv:0;n:type:ShaderForge.SFN_Lerp,id:1732,x:32083,y:32600,varname:node_1732,prsc:2|A-5300-UVOUT,B-9736-RGB,T-8867-OUT;n:type:ShaderForge.SFN_Slider,id:8867,x:31705,y:32801,ptovrint:False,ptlb:Deformation,ptin:_Deformation,varname:node_8867,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_Panner,id:5300,x:31862,y:32452,varname:node_5300,prsc:2,spu:0.7,spv:0|UVIN-8140-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8140,x:31690,y:32452,varname:node_8140,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:5263,x:32688,y:32987,cmnt:Alpha,varname:node_5263,prsc:2|A-2493-A,B-9980-A,C-9206-A,D-6561-A;n:type:ShaderForge.SFN_Tex2d,id:6561,x:32253,y:33177,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_6561,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:4747,x:32096,y:33083,ptovrint:False,ptlb:Emissive power,ptin:_Emissivepower,varname:node_4747,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Color,id:9980,x:32253,y:32785,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_9980,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:8767,x:32874,y:32678,cmnt:Diffuse,varname:node_8767,prsc:2|A-2493-RGB,B-9980-RGB,C-9206-RGB;n:type:ShaderForge.SFN_VertexColor,id:9206,x:32253,y:32929,varname:node_9206,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8010,x:32874,y:32841,cmnt:Emissive,varname:node_8010,prsc:2|A-9980-RGB,B-9206-RGB,C-4747-OUT;n:type:ShaderForge.SFN_Multiply,id:6574,x:32874,y:33043,cmnt:Refraction,varname:node_6574,prsc:2|A-5263-OUT,B-5560-OUT;n:type:ShaderForge.SFN_Slider,id:8612,x:32521,y:32403,ptovrint:False,ptlb:Refraction power,ptin:_Refractionpower,varname:node_8612,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:1;n:type:ShaderForge.SFN_Append,id:5168,x:32678,y:32478,varname:node_5168,prsc:2|A-2493-R,B-2493-R;n:type:ShaderForge.SFN_Set,id:4760,x:33057,y:32403,varname:Refraction,prsc:2|IN-133-OUT;n:type:ShaderForge.SFN_Multiply,id:133,x:32884,y:32403,varname:node_133,prsc:2|A-8612-OUT,B-5168-OUT;n:type:ShaderForge.SFN_Get,id:5560,x:32667,y:33111,varname:node_5560,prsc:2|IN-4760-OUT;proporder:2493-9736-8867-6561-4747-9980-8612;pass:END;sub:END;*/

Shader "Grr/Smoke" {
    Properties {
        _Smoke ("Smoke", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _Deformation ("Deformation", Range(0, 1)) = 0.2
        _Mask ("Mask", 2D) = "white" {}
        _Emissivepower ("Emissive power", Range(0, 1)) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _Refractionpower ("Refraction power", Range(0, 1)) = 0.1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Smoke; uniform float4 _Smoke_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Deformation;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _Emissivepower;
            uniform float4 _Color;
            uniform float _Refractionpower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 normalDirection = i.normalDir;
                float4 node_4193 = _Time + _TimeEditor;
                float2 node_8320 = (i.uv0+node_4193.g*float2(-0.3,-0.2));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_8320, _Noise));
                float3 node_1732 = lerp(float3((i.uv0+node_4193.g*float2(0.7,0)),0.0),_Noise_var.rgb,_Deformation);
                float4 _Smoke_var = tex2D(_Smoke,TRANSFORM_TEX(node_1732, _Smoke));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float node_5263 = (_Smoke_var.a*_Color.a*i.vertexColor.a*_Mask_var.a); // Alpha
                float2 Refraction = (_Refractionpower*float2(_Smoke_var.r,_Smoke_var.r));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_5263*Refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = (_Smoke_var.rgb*_Color.rgb*i.vertexColor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (_Color.rgb*i.vertexColor.rgb*_Emissivepower);
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,node_5263),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Smoke; uniform float4 _Smoke_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Deformation;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _Emissivepower;
            uniform float4 _Color;
            uniform float _Refractionpower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 normalDirection = i.normalDir;
                float4 node_7085 = _Time + _TimeEditor;
                float2 node_8320 = (i.uv0+node_7085.g*float2(-0.3,-0.2));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_8320, _Noise));
                float3 node_1732 = lerp(float3((i.uv0+node_7085.g*float2(0.7,0)),0.0),_Noise_var.rgb,_Deformation);
                float4 _Smoke_var = tex2D(_Smoke,TRANSFORM_TEX(node_1732, _Smoke));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float node_5263 = (_Smoke_var.a*_Color.a*i.vertexColor.a*_Mask_var.a); // Alpha
                float2 Refraction = (_Refractionpower*float2(_Smoke_var.r,_Smoke_var.r));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_5263*Refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = (_Smoke_var.rgb*_Color.rgb*i.vertexColor.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * node_5263,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
