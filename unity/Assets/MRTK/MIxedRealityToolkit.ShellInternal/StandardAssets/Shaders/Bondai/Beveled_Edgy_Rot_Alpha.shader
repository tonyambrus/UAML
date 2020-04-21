// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'


Shader "Beveled_Edgy_Rot_Alpha" {

Properties {

    [Header(Round Rect)]
        _Radius_("Radius", Range(0,0.5)) = 0.2
        _Bevel_Front_("Bevel Front", Range(0,1)) = 0.07
        _Bevel_Front_Stretch_("Bevel Front Stretch", Range(0,1)) = 0
        _Bevel_Back_("Bevel Back", Range(0,1)) = 0.02
        _Bevel_Back_Stretch_("Bevel Back Stretch", Range(0,1)) = 0
     
    [Header(Radii Multipliers)]
        _Radius_Top_Left_("Radius Top Left", Range(0,1)) = 1
        _Radius_Top_Right_("Radius Top Right", Range(0,1)) = 1.0
        _Radius_Bottom_Left_("Radius Bottom Left", Range(0,1)) = 1.0
        _Radius_Bottom_Right_("Radius Bottom Right", Range(0,1)) = 1.0
     
    [Header(Bulge)]
        [Toggle] _Bulge_Enabled_("Bulge Enabled", Float) = 0
        _Bulge_Height_("Bulge Height", Range(-1,1)) = -0.323
        _Bulge_Radius_("Bulge Radius", Range(0,1.25)) = 0.73
     
    [Header(Sun)]
        _Sun_Intensity_("Sun Intensity", Range(0,2)) = 0
        _Sun_Theta_("Sun Theta", Range(0,1)) = 0.73
        _Sun_Phi_("Sun Phi", Range(0,1)) = 0.48
        _Indirect_Diffuse_("Indirect Diffuse", Range(0,1)) = 0.51
     
    [Header(Diffuse And Specular)]
        _Albedo_("Albedo", Color) = (1,1,1,1)
        _Specular_("Specular", Range(0,5)) = 0
        _Shininess_("Shininess", Range(0,10)) = 10
        _Sharpness_("Sharpness", Range(0,1)) = 0
        _Subsurface_("Subsurface", Range(0,1)) = 0
     
    [Header(Gradient)]
        _Left_Color_("Left Color", Color) = (1,1,1,1)
        _Right_Color_("Right Color", Color) = (1,1,1,1)
     
    [Header(Reflection)]
        _Reflection_("Reflection", Range(0,2)) = 0
        _Front_Reflect_("Front Reflect", Range(0,1)) = 0
        _Edge_Reflect_("Edge Reflect", Range(0,1)) = 1
        _Power_("Power", Range(0,10)) = 1
     
    [Header(Sky Environment)]
        [Toggle(_SKY_ENABLED_)] _Sky_Enabled_("Sky Enabled", Float) = 1
        _Sky_Color_("Sky Color", Color) = (0.866667,0.917647,1,1)
        _Horizon_Color_("Horizon Color", Color) = (0.843137,0.87451,1,1)
        _Ground_Color_("Ground Color", Color) = (0.603922,0.611765,0.764706,1)
        _Horizon_Power_("Horizon Power", Range(0,10)) = 1
     
    [Header(Mapped Environment)]
        [Toggle(_ENV_ENABLE_)] _Env_Enable_("Env Enable", Float) = 0
        [NoScaleOffset] _Reflection_Map_("Reflection Map", Cube) = "" {}
        [NoScaleOffset] _Indirect_Environment_("Indirect Environment", Cube) = "" {}
     
    [Header(FingerOcclusion)]
        [Toggle(_OCCLUSION_ENABLED_)] _Occlusion_Enabled_("Occlusion Enabled", Float) = 0
        _Width_("Width", Range(0,1)) = 0.02
        _Fuzz_("Fuzz", Range(0,1)) = 0.5
        _Min_Fuzz_("Min Fuzz", Range(0,1)) = 0.001
        _Clip_Fade_("Clip Fade", Range(0,1)) = 0.01
     
    [Header(View Based Color Shift)]
        _Hue_Shift_("Hue Shift", Range(-1,1)) = 0
        _Saturation_Shift_("Saturation Shift", Range(-1,1)) = 0
        _Value_Shift_("Value Shift", Range(-1,1)) = 0
     
    [Header(Blob)]
        [Toggle(_BLOB_ENABLE_)] _Blob_Enable_("Blob Enable", Float) = 0
        _Blob_Position_("Blob Position", Vector) = (0, 0, 0.1, 1)
        _Blob_Intensity_("Blob Intensity", Range(0,3)) = 0.5
        _Blob_Near_Size_("Blob Near Size", Range(0,1)) = 0.01
        _Blob_Far_Size_("Blob Far Size", Range(0,1)) = 0.03
        _Blob_Near_Distance_("Blob Near Distance", Range(0,1)) = 0
        _Blob_Far_Distance_("Blob Far Distance", Range(0,1)) = 0.08
        _Blob_Fade_Length_("Blob Fade Length", Range(0,1)) = 0.08
        _Blob_Pulse_("Blob Pulse", Range(0,1)) = 0
        _Blob_Fade_("Blob Fade", Range(0,1)) = 1
     
    [Header(Blob Texture)]
        [NoScaleOffset] _Blob_Texture_("Blob Texture", 2D) = "" {}
     
    [Header(Blob 2)]
        [Toggle(_BLOB_ENABLE_2_)] _Blob_Enable_2_("Blob Enable 2", Float) = 1
        _Blob_Position_2_("Blob Position 2", Vector) = (0.2, 0, 0.1, 1)
        _Blob_Near_Size_2_("Blob Near Size 2", Range(0,1)) = 0.01
        _Blob_Pulse_2_("Blob Pulse 2", Range(0,1)) = 0
        _Blob_Fade_2_("Blob Fade 2", Range(0,1)) = 1
     
    [Header(Finger Positions)]
        _Left_Index_Pos_("Left Index Pos", Vector) = (0, 0, 1, 1)
        _Right_Index_Pos_("Right Index Pos", Vector) = (-1, -1, -1, 1)
        _Left_Index_Middle_Pos_("Left Index Middle Pos", Vector) = (0, 0, 0, 1)
        _Right_Index_Middle_Pos_("Right Index Middle Pos", Vector) = (0, 0, 0, 1)
     
    [Header(Decal Texture)]
        [Toggle(_DECAL_ENABLE_)] _Decal_Enable_("Decal Enable", Float) = 0
        [NoScaleOffset] _Decal_("Decal", 2D) = "" {}
        _Decal_Scale_XY_("Decal Scale XY", Vector) = (1.5,1.5,0,0)
        [Toggle] _Decal_Front_Only_("Decal Front Only", Float) = 1
     
    [Header(Rim Light)]
        _Rim_Intensity_("Rim Intensity", Range(0,2)) = 0.3
        [NoScaleOffset] _Rim_Texture_("Rim Texture", 2D) = "" {}
        _Rim_Hue_Shift_("Rim Hue Shift", Range(-1,1)) = 0
        _Rim_Saturation_Shift_("Rim Saturation Shift", Range(-1,1)) = 0.0
        _Rim_Value_Shift_("Rim Value Shift", Range(-1,1)) = 0.0
        _Rim_View_Shift_("Rim View Shift", Range(-1,1)) = 0.0
        [Toggle] _Rim_Spread_Texture_("Rim Spread Texture", Float) = 0
     
    [Header(Iridescence)]
        [Toggle(_IRIDESCENCE_ENABLED_)] _Iridescence_Enabled_("Iridescence Enabled", Float) = 0
        _Iridescence_Intensity_("Iridescence Intensity", Range(0,1)) = 0
        [NoScaleOffset] _Iridescence_Texture_("Iridescence Texture", 2D) = "" {}
     
    [Header(Alpha Blend)]
        _Alpha_("Alpha", Range(0,1)) = 1
     

    [Header(Global)]
        [Toggle] Use_Global_Left_Index("Use Global Left Index", Float) = 0
        [Toggle] Use_Global_Right_Index("Use Global Right Index", Float) = 0
}

SubShader {
    Tags{ "RenderType" = "AlphaTest" "Queue" = "AlphaTest"}
    Blend One OneMinusSrcAlpha
    Tags {"DisableBatching" = "True"}

    LOD 100


    Pass

    {

    CGPROGRAM

    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile_instancing
    #pragma target 4.0
    #pragma multi_compile _ _ENV_ENABLE_
    #pragma multi_compile _ _OCCLUSION_ENABLED_
    #pragma multi_compile _ _BLOB_ENABLE_2_
    #pragma multi_compile _ _BLOB_ENABLE_
    #pragma multi_compile _ _DECAL_ENABLE_
    #pragma multi_compile _ _SKY_ENABLED_
    #pragma multi_compile _ _IRIDESCENCE_ENABLED_

    #include "UnityCG.cginc"

    sampler2D _Blob_Texture_;
    //bool _Env_Enable_;
    samplerCUBE _Reflection_Map_;
    samplerCUBE _Indirect_Environment_;
    float3 _Left_Index_Pos_;
    float3 _Right_Index_Pos_;
    float3 _Left_Index_Middle_Pos_;
    float3 _Right_Index_Middle_Pos_;
    //bool _Occlusion_Enabled_;
    float _Width_;
    float _Fuzz_;
    float _Min_Fuzz_;
    float _Clip_Fade_;
    //bool _Blob_Enable_2_;
    float3 _Blob_Position_2_;
    float _Blob_Near_Size_2_;
    float _Blob_Pulse_2_;
    float _Blob_Fade_2_;
    //bool _Blob_Enable_;
    float3 _Blob_Position_;
    float _Blob_Intensity_;
    float _Blob_Near_Size_;
    float _Blob_Far_Size_;
    float _Blob_Near_Distance_;
    float _Blob_Far_Distance_;
    float _Blob_Fade_Length_;
    float _Blob_Pulse_;
    float _Blob_Fade_;
    float _Radius_Top_Left_;
    float _Radius_Top_Right_;
    float _Radius_Bottom_Left_;
    float _Radius_Bottom_Right_;
    bool _Bulge_Enabled_;
    float _Bulge_Height_;
    float _Bulge_Radius_;
    //bool _Decal_Enable_;
    sampler2D _Decal_;
    float2 _Decal_Scale_XY_;
    bool _Decal_Front_Only_;
    float4 _Left_Color_;
    float4 _Right_Color_;
    float _Hue_Shift_;
    float _Saturation_Shift_;
    float _Value_Shift_;
    float _Sun_Intensity_;
    float _Sun_Theta_;
    float _Sun_Phi_;
    float _Indirect_Diffuse_;
    float _Radius_;
    float _Bevel_Front_;
    float _Bevel_Front_Stretch_;
    float _Bevel_Back_;
    float _Bevel_Back_Stretch_;
    float4 _Albedo_;
    float _Specular_;
    float _Shininess_;
    float _Sharpness_;
    float _Subsurface_;
    //bool _Sky_Enabled_;
    float4 _Sky_Color_;
    float4 _Horizon_Color_;
    float4 _Ground_Color_;
    float _Horizon_Power_;
    //bool _Iridescence_Enabled_;
    float _Iridescence_Intensity_;
    sampler2D _Iridescence_Texture_;
    float _Reflection_;
    float _Front_Reflect_;
    float _Edge_Reflect_;
    float _Power_;
    float _Rim_Intensity_;
    sampler2D _Rim_Texture_;
    float _Rim_Hue_Shift_;
    float _Rim_Saturation_Shift_;
    float _Rim_Value_Shift_;
    float _Rim_View_Shift_;
    bool _Rim_Spread_Texture_;
    float _Alpha_;

    bool Use_Global_Left_Index;
    bool Use_Global_Right_Index;
    float4 Global_Left_Index_Tip_Position;
    float4 Global_Right_Index_Tip_Position;
	float4 Global_Left_Index_Middle_Position;
	float4 Global_Right_Index_Middle_Position;
    float4 Global_Left_Thumb_Tip_Position;
    float4 Global_Right_Thumb_Tip_Position;
    float  Global_Left_Index_Tip_Proximity;
    float  Global_Right_Index_Tip_Proximity;



    struct VertexInput {
        float4 vertex : POSITION;
        half3 normal : NORMAL;
        float2 uv0 : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct VertexOutput {
        float4 pos : SV_POSITION;
        half4 normalWorld : TEXCOORD5;
        float2 uv : TEXCOORD0;
        float3 posWorld : TEXCOORD7;
        float4 tangent : TANGENT;
        float4 binormal : TEXCOORD6;
        float4 vertexColor : COLOR;
        float4 extra1 : TEXCOORD4;
        float4 extra2 : TEXCOORD3;
        float4 extra3 : TEXCOORD2;
      UNITY_VERTEX_OUTPUT_STEREO
    };

    // declare parm vars here


    //BLOCK_BEGIN Object_To_World_Pos 12

    void Object_To_World_Pos_B12(
        float3 Pos_Object,
        out float3 Pos_World    )
    {
        Pos_World=(mul(unity_ObjectToWorld, float4(Pos_Object, 1)));
        
    }
    //BLOCK_END Object_To_World_Pos

    //BLOCK_BEGIN Object_To_World_Normal 32

    void Object_To_World_Normal_B32(
        float3 Nrm_Object,
        out float3 Nrm_World    )
    {
        Nrm_World=UnityObjectToWorldNormal(Nrm_Object);
        
    }
    //BLOCK_END Object_To_World_Normal

    //BLOCK_BEGIN Blob_Vertex 23

    void Blob_Vertex_B23(
        float3 Position,
        float3 Normal,
        float3 Tangent,
        float3 Bitangent,
        float3 Blob_Position,
        float Intensity,
        float Blob_Near_Size,
        float Blob_Far_Size,
        float Blob_Near_Distance,
        float Blob_Far_Distance,
        float Blob_Fade_Length,
        float Blob_Pulse,
        float Blob_Fade,
        out float4 Blob_Info    )
    {
        
        float3 blob =  (Use_Global_Left_Index ? Global_Left_Index_Tip_Position.xyz :  Blob_Position);
        float3 delta = blob - Position;
        float dist = dot(Normal,delta);
        
        float lerpValue = saturate((abs(dist)-Blob_Near_Distance)/(Blob_Far_Distance-Blob_Near_Distance));
        float fadeValue = 1.0-clamp((abs(dist)-Blob_Far_Distance)/Blob_Fade_Length,0.0,1.0);
        
        float size = Blob_Near_Size + (Blob_Far_Size-Blob_Near_Size)*lerpValue;
        
        float2 blobXY = float2(dot(delta,Tangent),dot(delta,Bitangent))/(0.0001+size);
        
        float Fade = fadeValue*Intensity*Blob_Fade;
        
        float Distance = (lerpValue*0.5+0.5)*(1.0-Blob_Pulse);
        Blob_Info = float4(blobXY.x,blobXY.y,Distance,Fade);
        
    }
    //BLOCK_END Blob_Vertex

    //BLOCK_BEGIN Blob_Vertex 24

    void Blob_Vertex_B24(
        float3 Position,
        float3 Normal,
        float3 Tangent,
        float3 Bitangent,
        float3 Blob_Position,
        float Intensity,
        float Blob_Near_Size,
        float Blob_Far_Size,
        float Blob_Near_Distance,
        float Blob_Far_Distance,
        float Blob_Fade_Length,
        float Blob_Pulse,
        float Blob_Fade,
        out float4 Blob_Info    )
    {
        
        float3 blob =  (Use_Global_Right_Index ? Global_Right_Index_Tip_Position.xyz :  Blob_Position);
        float3 delta = blob - Position;
        float dist = dot(Normal,delta);
        
        float lerpValue = saturate((abs(dist)-Blob_Near_Distance)/(Blob_Far_Distance-Blob_Near_Distance));
        float fadeValue = 1.0-clamp((abs(dist)-Blob_Far_Distance)/Blob_Fade_Length,0.0,1.0);
        
        float size = Blob_Near_Size + (Blob_Far_Size-Blob_Near_Size)*lerpValue;
        
        float2 blobXY = float2(dot(delta,Tangent),dot(delta,Bitangent))/(0.0001+size);
        
        float Fade = fadeValue*Intensity*Blob_Fade;
        
        float Distance = (lerpValue*0.5+0.5)*(1.0-Blob_Pulse);
        Blob_Info = float4(blobXY.x,blobXY.y,Distance,Fade);
        
    }
    //BLOCK_END Blob_Vertex

    //BLOCK_BEGIN Move_Verts 130

    void Move_Verts_B130(
        float Anisotropy,
        float3 P,
        float Radius,
        float Bevel,
        float3 Normal_Object,
        float ScaleZ,
        float Stretch,
        out float3 New_P,
        out float2 New_UV,
        out float Radial_Gradient,
        out float3 Radial_Dir,
        out float3 New_Normal    )
    {
        float2 UV = P.xy * 2 + 0.5;
        float2 center = saturate(UV);
        float2 delta = UV - center;
        float deltad = (length(delta)*2);
        float f = (Bevel+(Radius-Bevel)*Stretch)/Radius;
        //float br = saturate((deltad-(1-f))/f);
        float innerd = saturate(deltad*2);
        float outerd = saturate(deltad*2-1);
        float bevelAngle = outerd*3.14159*0.5;
        float sinb = sin(bevelAngle);
        float cosb = cos(bevelAngle);
        float beveld = (1-f)*innerd + f * sinb;
        float br = outerd;
        float2 r2 = 2.0 * float2(Radius / Anisotropy, Radius);
        
        float dir = P.z<0.0001 ? 1.0 : -1.0;
        
        //New_UV = center + r2 * (UV - 2 * center + 0.5);
        New_UV = center + r2 * ((0.5-center)+normalize(delta+float2(0.0,0.000001))*beveld*0.5);
        New_P = float3(New_UV - 0.5, P.z+dir*(1-cosb)*Bevel*ScaleZ);
                
        Radial_Gradient = saturate((deltad-0.5)*2);
        Radial_Dir = float3(delta * r2, 0.0);
        
        float3 beveledNormal = cosb*Normal_Object + sinb*float3(delta.x,delta.y,0.0);
        New_Normal = Normal_Object.z==0 ? Normal_Object : beveledNormal;
        
    }
    //BLOCK_END Move_Verts

    //BLOCK_BEGIN Object_To_World_Dir 60

    void Object_To_World_Dir_B60(
        float3 Dir_Object,
        out float3 Normal_World,
        out float3 Normal_World_N,
        out float Normal_Length    )
    {
        Normal_World = (mul((float3x3)unity_ObjectToWorld, Dir_Object));
        Normal_Length = length(Normal_World);
        Normal_World_N = Normal_World / Normal_Length;
    }
    //BLOCK_END Object_To_World_Dir

    //BLOCK_BEGIN To_XYZ 78

    void To_XYZ_B78(
        float3 Vec3,
        out float X,
        out float Y,
        out float Z    )
    {
        X=Vec3.x;
        Y=Vec3.y;
        Z=Vec3.z;
        
    }
    //BLOCK_END To_XYZ

    //BLOCK_BEGIN Conditional_Float 93

    void Conditional_Float_B93(
        bool Which,
        float If_True,
        float If_False,
        out float Result    )
    {
        Result = Which ? If_True : If_False;
        
    }
    //BLOCK_END Conditional_Float

    //BLOCK_BEGIN Object_To_World_Dir 28

    void Object_To_World_Dir_B28(
        float3 Dir_Object,
        out float3 Binormal_World,
        out float3 Binormal_World_N,
        out float Binormal_Length    )
    {
        Binormal_World = (mul((float3x3)unity_ObjectToWorld, Dir_Object));
        Binormal_Length = length(Binormal_World);
        Binormal_World_N = Binormal_World / Binormal_Length;
    }
    //BLOCK_END Object_To_World_Dir

    //BLOCK_BEGIN Pick_Radius 69

    void Pick_Radius_B69(
        float Radius,
        float Radius_Top_Left,
        float Radius_Top_Right,
        float Radius_Bottom_Left,
        float Radius_Bottom_Right,
        float3 Position,
        out float Result    )
    {
        bool whichY = Position.y>0;
        Result = Position.x<0 ? (whichY ? Radius_Top_Left : Radius_Bottom_Left) : (whichY ? Radius_Top_Right : Radius_Bottom_Right);
        Result *= Radius;
    }
    //BLOCK_END Pick_Radius

    //BLOCK_BEGIN Conditional_Float 36

    void Conditional_Float_B36(
        bool Which,
        float If_True,
        float If_False,
        out float Result    )
    {
        Result = Which ? If_True : If_False;
        
    }
    //BLOCK_END Conditional_Float

    //BLOCK_BEGIN Greater_Than 37

    void Greater_Than_B37(
        float Left,
        float Right,
        out bool Not_Greater_Than,
        out bool Greater_Than    )
    {
        Greater_Than = Left > Right;
        Not_Greater_Than = !Greater_Than;
        
    }
    //BLOCK_END Greater_Than

    //BLOCK_BEGIN Remap_Range 105

    void Remap_Range_B105(
        float In_Min,
        float In_Max,
        float Out_Min,
        float Out_Max,
        float In,
        out float Out    )
    {
        Out = lerp(Out_Min,Out_Max,clamp((In-In_Min)/(In_Max-In_Min),0,1));
        
    }
    //BLOCK_END Remap_Range


    VertexOutput vert(VertexInput vertInput)
    {
        UNITY_SETUP_INSTANCE_ID(vertInput);
        VertexOutput o;
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


        // Tex_Coords
        float2 XY_Q85;
        XY_Q85 = (vertInput.uv0-float2(0.5,0.5))*_Decal_Scale_XY_ + float2(0.5,0.5);
        
        // Object_To_World_Dir
        float3 Tangent_World_Q27;
        float3 Tangent_World_N_Q27;
        float Tangent_Length_Q27;
        Tangent_World_Q27 = (mul((float3x3)unity_ObjectToWorld, float3(1,0,0)));
        Tangent_Length_Q27 = length(Tangent_World_Q27);
        Tangent_World_N_Q27 = Tangent_World_Q27 / Tangent_Length_Q27;

        float3 Normal_World_Q60;
        float3 Normal_World_N_Q60;
        float Normal_Length_Q60;
        Object_To_World_Dir_B60(float3(0,0,1),Normal_World_Q60,Normal_World_N_Q60,Normal_Length_Q60);

        float X_Q78;
        float Y_Q78;
        float Z_Q78;
        To_XYZ_B78(vertInput.vertex.xyz,X_Q78,Y_Q78,Z_Q78);

        // Object_To_World_Dir
        float3 Nrm_World_Q26;
        Nrm_World_Q26 = normalize((mul((float3x3)unity_ObjectToWorld, vertInput.normal)));
        
        float3 Binormal_World_Q28;
        float3 Binormal_World_N_Q28;
        float Binormal_Length_Q28;
        Object_To_World_Dir_B28(float3(0,1,0),Binormal_World_Q28,Binormal_World_N_Q28,Binormal_Length_Q28);

        // Divide
        float Anisotropy_Q29 = Tangent_Length_Q27 / Binormal_Length_Q28;

        float Result_Q69;
        Pick_Radius_B69(_Radius_,_Radius_Top_Left_,_Radius_Top_Right_,_Radius_Bottom_Left_,_Radius_Bottom_Right_,vertInput.vertex.xyz,Result_Q69);

        // Divide
        float Anisotropy_Q53 = Binormal_Length_Q28 / Normal_Length_Q60;

        bool Not_Greater_Than_Q37;
        bool Greater_Than_Q37;
        Greater_Than_B37(Z_Q78,0,Not_Greater_Than_Q37,Greater_Than_Q37);

        // FastsRGBtoLinear
        float4 Linear_Q101;
        Linear_Q101.rgb = saturate(_Left_Color_.rgb*_Left_Color_.rgb);
        Linear_Q101.a=_Left_Color_.a;
        
        // FastsRGBtoLinear
        float4 Linear_Q102;
        Linear_Q102.rgb = saturate(_Right_Color_.rgb*_Right_Color_.rgb);
        Linear_Q102.a=_Right_Color_.a;
        
        // Subtract3
        float3 Difference_Q61 = float3(0,0,0) - Normal_World_N_Q60;

        // From_RGBA
        float4 Out_Color_Q34 = float4(X_Q78, Y_Q78, Z_Q78, 1);

        float Result_Q36;
        Conditional_Float_B36(Greater_Than_Q37,_Bevel_Back_,_Bevel_Front_,Result_Q36);

        float Result_Q94;
        Conditional_Float_B36(Greater_Than_Q37,_Bevel_Back_Stretch_,_Bevel_Front_Stretch_,Result_Q94);

        float3 New_P_Q130;
        float2 New_UV_Q130;
        float Radial_Gradient_Q130;
        float3 Radial_Dir_Q130;
        float3 New_Normal_Q130;
        Move_Verts_B130(Anisotropy_Q29,vertInput.vertex.xyz,Result_Q69,Result_Q36,vertInput.normal,Anisotropy_Q53,Result_Q94,New_P_Q130,New_UV_Q130,Radial_Gradient_Q130,Radial_Dir_Q130,New_Normal_Q130);

        // To_XY
        float X_Q98;
        float Y_Q98;
        X_Q98 = New_UV_Q130.x;
        Y_Q98 = New_UV_Q130.y;

        float3 Pos_World_Q12;
        Object_To_World_Pos_B12(New_P_Q130,Pos_World_Q12);

        float3 Nrm_World_Q32;
        Object_To_World_Normal_B32(New_Normal_Q130,Nrm_World_Q32);

        float4 Blob_Info_Q23;
        #if defined(_BLOB_ENABLE_)
          Blob_Vertex_B23(Pos_World_Q12,Nrm_World_Q26,Tangent_World_N_Q27,Binormal_World_N_Q28,_Blob_Position_,_Blob_Intensity_,_Blob_Near_Size_,_Blob_Far_Size_,_Blob_Near_Distance_,_Blob_Far_Distance_,_Blob_Fade_Length_,_Blob_Pulse_,_Blob_Fade_,Blob_Info_Q23);
        #else
          Blob_Info_Q23 = float4(0,0,0,0);
        #endif

        float4 Blob_Info_Q24;
        #if defined(_BLOB_ENABLE_2_)
          Blob_Vertex_B24(Pos_World_Q12,Nrm_World_Q26,Tangent_World_N_Q27,Binormal_World_N_Q28,_Blob_Position_2_,_Blob_Intensity_,_Blob_Near_Size_2_,_Blob_Far_Size_,_Blob_Near_Distance_,_Blob_Far_Distance_,_Blob_Fade_Length_,_Blob_Pulse_2_,_Blob_Fade_2_,Blob_Info_Q24);
        #else
          Blob_Info_Q24 = float4(0,0,0,0);
        #endif

        float Out_Q105;
        Remap_Range_B105(0,1,0,1,X_Q98,Out_Q105);

        float X_Q86;
        float Y_Q86;
        float Z_Q86;
        To_XYZ_B78(Nrm_World_Q32,X_Q86,Y_Q86,Z_Q86);

        // Mix_Colors
        float4 Color_At_T_Q97 = lerp(Linear_Q101, Linear_Q102,float4( Out_Q105, Out_Q105, Out_Q105, Out_Q105));

        // Negate
        float Minus_F_Q87 = -Z_Q86;

        // To_RGBA
        float R_Q99;
        float G_Q99;
        float B_Q99;
        float A_Q99;
        R_Q99=Color_At_T_Q97.r; G_Q99=Color_At_T_Q97.g; B_Q99=Color_At_T_Q97.b; A_Q99=Color_At_T_Q97.a;

        // Clamp
        float ClampF_Q88=clamp(0,Minus_F_Q87,1);

        float Result_Q93;
        Conditional_Float_B93(_Decal_Front_Only_,ClampF_Q88,1,Result_Q93);

        // From_XYZW
        float4 Vec4_Q89 = float4(Result_Q93, Radial_Gradient_Q130, G_Q99, B_Q99);

        float3 Position = Pos_World_Q12;
        float3 Normal = Nrm_World_Q32;
        float2 UV = XY_Q85;
        float3 Tangent = Tangent_World_N_Q27;
        float3 Binormal = Difference_Q61;
        float4 Color = Out_Color_Q34;
        float4 Extra1 = Vec4_Q89;
        float4 Extra2 = Blob_Info_Q23;
        float4 Extra3 = Blob_Info_Q24;


        o.pos = mul(UNITY_MATRIX_VP, float4(Position,1));
        o.posWorld = Position;
        o.normalWorld.xyz = Normal; o.normalWorld.w=1.0;
        o.uv = UV;
        o.tangent.xyz = Tangent; o.tangent.w=1.0;
        o.binormal.xyz = Binormal; o.binormal.w=1.0;
        o.vertexColor = Color;
        o.extra1=Extra1;
        o.extra2=Extra2;
        o.extra3=Extra3;

        return o;
    }

    //BLOCK_BEGIN Blob_Fragment 30

    void Blob_Fragment_B30(
        sampler2D Blob_Texture,
        float4 Blob_Info1,
        float4 Blob_Info2,
        out half4 Blob_Color    )
    {
        half k1 = dot(Blob_Info1.xy,Blob_Info1.xy);
        half k2 = dot(Blob_Info2.xy,Blob_Info2.xy);
        half3 closer = k1<k2 ? half3(k1,Blob_Info1.z,Blob_Info1.w) : half3(k2,Blob_Info2.z,Blob_Info2.w);
        Blob_Color = closer.z * tex2D(Blob_Texture,float2(float2(sqrt(closer.x),closer.y).x,1.0-float2(sqrt(closer.x),closer.y).y))*saturate(1.0-closer.x);
        
    }
    //BLOCK_END Blob_Fragment

    //BLOCK_BEGIN FastLinearTosRGB 42

    void FastLinearTosRGB_B42(
        float4 Linear,
        out float4 sRGB    )
    {
        sRGB.rgb = sqrt(saturate(Linear.rgb));
        sRGB.a = Linear.a;
        
    }
    //BLOCK_END FastLinearTosRGB

    //BLOCK_BEGIN Scale_RGB 59

    void Scale_RGB_B59(
        float4 Color,
        float Scalar,
        out float4 Result    )
    {
        Result = float4(Scalar,Scalar,Scalar,1) * Color;
    }
    //BLOCK_END Scale_RGB

    //BLOCK_BEGIN Fragment_Main 121

    void Fragment_Main_B121(
        float Sun_Intensity,
        float Sun_Theta,
        float Sun_Phi,
        float3 Normal,
        float4 Albedo,
        float Fresnel_Reflect,
        float Shininess,
        float3 Incident,
        float4 Horizon_Color,
        float4 Sky_Color,
        float4 Ground_Color,
        float Indirect_Diffuse,
        float Specular,
        float Horizon_Power,
        float Reflection,
        float4 Reflection_Sample,
        half4 Indirect_Sample,
        float Sharpness,
        float SSS,
        float Subsurface,
        float4 Translucence,
        float4 Rim_Light,
        float4 Iridescence,
        out float4 Result    )
    {
        
        float theta = Sun_Theta * 2.0 * 3.14159;
        float phi = Sun_Phi * 3.14159;
        
        float3 lightDir =  float3(cos(phi)*cos(theta),sin(phi),cos(phi)*sin(theta));
        float NdotL = max(dot(lightDir,Normal),0.0);
        
        //float3 H = normalize(Normal-Incident);
        float3 R = reflect(Incident,Normal);
        float RdotL = max(0.0,dot(R,lightDir));
        float specular = pow(RdotL,Shininess);
        specular = lerp(specular,smoothstep(0.495*Sharpness,1.0-0.495*Sharpness,specular),Sharpness);
        
        float4 gi = lerp(Ground_Color,Sky_Color,float4(Normal.y*0.5+0.5,Normal.y*0.5+0.5,Normal.y*0.5+0.5,Normal.y*0.5+0.5));
        //SampleEnv(Normal,Sky_Color,Horizon_Color,Ground_Color,1);
        
        Result = ((Sun_Intensity*NdotL + Indirect_Sample * Indirect_Diffuse + Translucence)*(1.0 + SSS * Subsurface)) * Albedo * (1.0-Fresnel_Reflect) + (Sun_Intensity*specular*Specular + Fresnel_Reflect * Reflection*Reflection_Sample) + Fresnel_Reflect * Rim_Light + Iridescence;
        
    }
    //BLOCK_END Fragment_Main

    //BLOCK_BEGIN Bulge 79

    void Bulge_B79(
        bool Enabled,
        float3 Normal,
        float3 Tangent,
        float Bulge_Height,
        float4 UV,
        float Bulge_Radius,
        float3 ButtonN,
        out float3 New_Normal    )
    {
        float2 xy = clamp(UV.xy*2.0,float2(-1,-1),float2(1,1));
        
        float3 B = (cross(Normal,Tangent));
        
        //float3 dirX = Normal * cosa.x + Tangent * sina.x;
        //New_Normal = Normal; // * cosa.y + B * sina.y;
        //New_Normal = normalize(Normal + (New_Normal-Normal)*(1-saturate(xy.x))*(1-saturate(xy.y)));
        
        //float r = saturate(length(xy))*Bulge_Height;
        float k = -saturate(1-length(xy)/Bulge_Radius)*Bulge_Height;
        k = sin(k*3.14159*0.5);
        k *= smoothstep(0.9998,0.9999,abs(dot(ButtonN,Normal)));
        New_Normal = Normal * sqrt(1-k*k)+(xy.x*Tangent + xy.y*B)*k;
        New_Normal = Enabled ? New_Normal : Normal;
    }
    //BLOCK_END Bulge

    //BLOCK_BEGIN SSS 77

    void SSS_B77(
        float3 ButtonN,
        float3 Normal,
        float3 Incident,
        out float Result    )
    {
        float NdotI = abs(dot(Normal,Incident));
        float BdotI = abs(dot(ButtonN,Incident));
        Result = (abs(NdotI-BdotI)); //*abs(ButtonN.y); //*sqrt(1.0-NdotI);
        //Result = abs(NdotI-BdotI)*exp(-1.0/max(NdotI,0.01));
        
        
        
    }
    //BLOCK_END SSS

    //BLOCK_BEGIN FingerOcclusion 67

    void FingerOcclusion_B67(
        float Width,
        float DistToCenter,
        float Fuzz,
        float Min_Fuzz,
        float3 Position,
        float3 Forward,
        float3 Nearest,
        float Fade_Out,
        out float NotInShadow    )
    {
        float d = dot((Nearest-Position),Forward);
        float sh = smoothstep(Width*0.5,Width*0.5+Fuzz*max(d,0)+Min_Fuzz,DistToCenter);
        NotInShadow = 1-(1-sh)*smoothstep(-Fade_Out,0,d);
        
    }
    //BLOCK_END FingerOcclusion

    //BLOCK_BEGIN FingerOcclusion 68

    void FingerOcclusion_B68(
        float Width,
        float DistToCenter,
        float Fuzz,
        float Min_Fuzz,
        float3 Position,
        float3 Forward,
        float3 Nearest,
        float Fade_Out,
        out float NotInShadow    )
    {
        float d = dot((Nearest-Position),Forward);
        float sh = smoothstep(Width*0.5,Width*0.5+Fuzz*max(d,0)+Min_Fuzz,DistToCenter);
        NotInShadow = 1-(1-sh)*smoothstep(-Fade_Out,0,d);
        
    }
    //BLOCK_END FingerOcclusion

    //BLOCK_BEGIN Scale_Color 91

    void Scale_Color_B91(
        float4 Color,
        float Scalar,
        out float4 Result    )
    {
        Result = Scalar * Color;
    }
    //BLOCK_END Scale_Color

    //BLOCK_BEGIN From_HSV 73

    void From_HSV_B73(
        float Hue,
        float Saturation,
        float Value,
        float Alpha,
        out float4 Color    )
    {
        
        // from http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
        
        float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
        
        float3 p = abs(frac(float3(Hue,Hue,Hue) + K.xyz) * 6.0 - K.www);
        
        Color.rgb = Value * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), Saturation);
        Color.a = Alpha;
        
    }
    //BLOCK_END From_HSV

    //BLOCK_BEGIN Fast_Fresnel 122

    void Fast_Fresnel_B122(
        float Front_Reflect,
        float Edge_Reflect,
        float Power,
        float3 Normal,
        float3 Incident,
        out float Transmit,
        out float Reflect    )
    {
        
        float d = max(-dot(Incident,Normal),0);
        Reflect = Front_Reflect+(Edge_Reflect-Front_Reflect)*pow(1-d,Power);
        Transmit=1-Reflect;
        
    }
    //BLOCK_END Fast_Fresnel

    //BLOCK_BEGIN Mapped_Environment 51

    void Mapped_Environment_B51(
        samplerCUBE Reflected_Environment,
        samplerCUBE Indirect_Environment,
        float3 Dir,
        out float4 Reflected_Color,
        out float4 Indirect_Diffuse    )
    {
        // main code goes here
        Reflected_Color = texCUBE(Reflected_Environment,Dir);
        Indirect_Diffuse = texCUBE(Indirect_Environment,Dir);
        
    }
    //BLOCK_END Mapped_Environment

    //BLOCK_BEGIN Sky_Environment 50

    float4 SampleEnv_Bid50(float3 D, float4 S, float4 H, float4 G, float exponent)
    {
        float k = pow(abs(D.y),exponent);
        float4 C;
        if (D.y>0.0) {
            C=lerp(H,S,float4(k,k,k,k));
        } else {
            C=lerp(H,G,float4(k,k,k,k));    
        }
        return C;
    }
    
    void Sky_Environment_B50(
        half3 Normal,
        float3 Reflected,
        half4 Sky_Color,
        half4 Horizon_Color,
        half4 Ground_Color,
        half Horizon_Power,
        out half4 Reflected_Color,
        out half4 Indirect_Color    )
    {
        // main code goes here
        Reflected_Color = SampleEnv_Bid50(Reflected,Sky_Color,Horizon_Color,Ground_Color,Horizon_Power);
        Indirect_Color = lerp(Ground_Color,Sky_Color,float4(Normal.y*0.5+0.5,Normal.y*0.5+0.5,Normal.y*0.5+0.5,Normal.y*0.5+0.5));
        
    }
    //BLOCK_END Sky_Environment

    //BLOCK_BEGIN Min_Segment_Distance 65

    void Min_Segment_Distance_B65(
        float3 P0,
        float3 P1,
        float3 Q0,
        float3 Q1,
        out float3 NearP,
        out float3 NearQ,
        out float Distance    )
    {
        float3 u = P1 - P0;
        float3 v = Q1 - Q0;
        float3 w = P0 - Q0;
        
        float a = dot(u,u);
        float b = dot(u,v);
        float c = dot(v,v);
        float d = dot(u,w);
        float e = dot(v,w);
        
        float D = a*c-b*b;
        float sD = D;
        float tD = D;
        float sc, sN, tc, tN;
        
        if (D<0.00001) {
            sN = 0.0;
            sD = 1.0;
            tN = e;
            tD = c;
        } else {
            sN = (b*e - c*d);
            tN = (a*e - b*d);
            if (sN < 0.0) {
                sN = 0.0;
                tN = e;
                tD = c;
            } else if (sN > sD) {
                sN = sD;
                tN = e + b;
                tD = c;
            }
        }
        
        if (tN < 0.0) {
            tN = 0.0;
            if (-d < 0.0) {
                sN = 0.0;
            } else if (-d > a) {
                sN = sD;
            } else {
                sN = -d;
                sD = a;
            }
        } else if (tN > tD) {
            tN = tD;
            if ((-d + b) < 0.0) {
                sN = 0.0;
            } else if ((-d + b) > a) {
                sN = sD;
            } else {
                sN = (-d + b);
                sD = a;
            }
        }
        
        sc = abs(sN)<0.000001 ? 0.0 : sN / sD;
        tc = abs(tN)<0.000001 ? 0.0 : tN / tD;
        
        NearP = P0 + sc * u;
        NearQ = Q0 + tc * v;
        
        Distance = distance(NearP,NearQ);
        
    }
    //BLOCK_END Min_Segment_Distance

    //BLOCK_BEGIN To_XYZ 74

    void To_XYZ_B74(
        float3 Vec3,
        out float X,
        out float Y,
        out float Z    )
    {
        X=Vec3.x;
        Y=Vec3.y;
        Z=Vec3.z;
        
    }
    //BLOCK_END To_XYZ

    //BLOCK_BEGIN Finger_Positions 64

    void Finger_Positions_B64(
        float3 Left_Index_Pos,
        float3 Right_Index_Pos,
        float3 Left_Index_Middle_Pos,
        float3 Right_Index_Middle_Pos,
        out float3 Left_Index,
        out float3 Right_Index,
        out float3 Left_Index_Middle,
        out float3 Right_Index_Middle    )
    {
        Left_Index =  (Use_Global_Left_Index ? Global_Left_Index_Tip_Position.xyz :  Left_Index_Pos);
        Right_Index =  (Use_Global_Right_Index ? Global_Right_Index_Tip_Position.xyz :  Right_Index_Pos);
        
        Left_Index_Middle =  (Use_Global_Left_Index ? Global_Left_Index_Middle_Position.xyz :  Left_Index_Middle_Pos);
        Right_Index_Middle =  (Use_Global_Right_Index ? Global_Right_Index_Middle_Position.xyz :  Right_Index_Middle_Pos);
        
    }
    //BLOCK_END Finger_Positions

    //BLOCK_BEGIN VaryHSV 108

    void VaryHSV_B108(
        float3 HSV_In,
        float Hue_Shift,
        float Saturation_Shift,
        float Value_Shift,
        out float3 HSV_Out    )
    {
        HSV_Out = float3(frac(HSV_In.x+Hue_Shift),saturate(HSV_In.y+Saturation_Shift),saturate(HSV_In.z+Value_Shift));
    }
    //BLOCK_END VaryHSV

    //BLOCK_BEGIN Remap_Range 114

    void Remap_Range_B114(
        float In_Min,
        float In_Max,
        float Out_Min,
        float Out_Max,
        float In,
        out float Out    )
    {
        Out = lerp(Out_Min,Out_Max,clamp((In-In_Min)/(In_Max-In_Min),0,1));
        
    }
    //BLOCK_END Remap_Range

    //BLOCK_BEGIN To_HSV 75

    void To_HSV_B75(
        float4 Color,
        out float Hue,
        out float Saturation,
        out float Value,
        out float Alpha,
        out float3 HSV    )
    {
        
        // from http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
        
        float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
        float4 p = Color.g < Color.b ? float4(Color.bg, K.wz) : float4(Color.gb, K.xy);
        float4 q = Color.r < p.x ? float4(p.xyw, Color.r) : float4(Color.r, p.yzx);
        
        float d = q.x - min(q.w, q.y);
        float e = 1.0e-10;
        
        Hue = abs(q.z + (q.w - q.y) / (6.0 * d + e));
        Saturation = d / (q.x + e);
        Value = q.x;
        Alpha = Color.a;
        HSV = float3(Hue,Saturation,Value);
    }
    //BLOCK_END To_HSV

    //BLOCK_BEGIN Code 110

    void Code_B110(
        float X,
        out float Result    )
    {
        Result = (acos(X)/3.14159-0.5)*2;
    }
    //BLOCK_END Code

    //BLOCK_BEGIN Rim_Light 138

    void Rim_Light_B138(
        float3 Front,
        float3 Normal,
        float3 Incident,
        float Rim_Intensity,
        sampler2D Texture,
        float Rim_View_Shift,
        float Y,
        out float4 Result    )
    {
        float3 R = reflect(Incident,Normal);
        float RdotF = dot(R,Front);
        float RdotL = sqrt(1.0-RdotF*RdotF);
        float IdotF = abs(dot(Front,Incident));
        float2 UV = float2(R.y*0.5+0.5+IdotF*Rim_View_Shift,Y);
        float4 Color = tex2D(Texture,UV);
        Result = Color;
        
    }
    //BLOCK_END Rim_Light

    //BLOCK_BEGIN Conditional_Float 137

    void Conditional_Float_B137(
        bool Which,
        float If_True,
        float If_False,
        out float Result    )
    {
        Result = Which ? If_True : If_False;
        
    }
    //BLOCK_END Conditional_Float


    //fixed4 frag(VertexOutput fragInput, fixed facing : VFACE) : SV_Target
    half4 frag(VertexOutput fragInput) : SV_Target
    {
        half4 result;

        half4 Blob_Color_Q30;
        #if defined(_BLOB_ENABLE_)
          Blob_Fragment_B30(_Blob_Texture_,fragInput.extra2,fragInput.extra3,Blob_Color_Q30);
        #else
          Blob_Color_Q30 = half4(0,0,0,0);
        #endif

        // Incident3
        float3 Incident_Q39 = normalize(fragInput.posWorld - _WorldSpaceCameraPos);

        // Normalize3
        float3 Normalized_Q38 = normalize(fragInput.normalWorld.xyz);

        // Normalize3
        float3 Normalized_Q71 = normalize(fragInput.tangent.xyz);

        // Color_Texture
        float4 Color_Q83;
        #if defined(_DECAL_ENABLE_)
          Color_Q83 = tex2D(_Decal_,fragInput.uv);
        #else
          Color_Q83 = float4(0,0,0,0);
        #endif

        // To_XYZW
        float X_Q90;
        float Y_Q90;
        float Z_Q90;
        float W_Q90;
        X_Q90=fragInput.extra1.x;
        Y_Q90=fragInput.extra1.y;
        Z_Q90=fragInput.extra1.z;
        W_Q90=fragInput.extra1.w;

        // FastsRGBtoLinear
        float4 Linear_Q43;
        Linear_Q43.rgb = saturate(_Sky_Color_.rgb*_Sky_Color_.rgb);
        Linear_Q43.a=_Sky_Color_.a;
        
        // FastsRGBtoLinear
        float4 Linear_Q44;
        Linear_Q44.rgb = saturate(_Horizon_Color_.rgb*_Horizon_Color_.rgb);
        Linear_Q44.a=_Horizon_Color_.a;
        
        // FastsRGBtoLinear
        float4 Linear_Q45;
        Linear_Q45.rgb = saturate(_Ground_Color_.rgb*_Ground_Color_.rgb);
        Linear_Q45.a=_Ground_Color_.a;
        
        float3 Left_Index_Q64;
        float3 Right_Index_Q64;
        float3 Left_Index_Middle_Q64;
        float3 Right_Index_Middle_Q64;
        Finger_Positions_B64(_Left_Index_Pos_,_Right_Index_Pos_,_Left_Index_Middle_Pos_,_Right_Index_Middle_Pos_,Left_Index_Q64,Right_Index_Q64,Left_Index_Middle_Q64,Right_Index_Middle_Q64);

        // FastsRGBtoLinear
        float4 Linear_Q46;
        Linear_Q46.rgb = saturate(_Albedo_.rgb*_Albedo_.rgb);
        Linear_Q46.a=_Albedo_.a;
        
        // Normalize3
        float3 Normalized_Q107 = normalize(fragInput.binormal.xyz);

        // Incident3
        float3 Incident_Q70 = normalize(fragInput.posWorld - _WorldSpaceCameraPos);

        // One_Minus
        float One_Minus_F_Q124 = 1.0 - Y_Q90;

        float3 New_Normal_Q79;
        Bulge_B79(_Bulge_Enabled_,Normalized_Q38,Normalized_Q71,_Bulge_Height_,fragInput.vertexColor,_Bulge_Radius_,fragInput.binormal.xyz,New_Normal_Q79);

        float Result_Q77;
        SSS_B77(fragInput.binormal.xyz,New_Normal_Q79,Incident_Q39,Result_Q77);

        float4 Result_Q91;
        Scale_Color_B91(Color_Q83,X_Q90,Result_Q91);

        float Transmit_Q122;
        float Reflect_Q122;
        Fast_Fresnel_B122(_Front_Reflect_,_Edge_Reflect_,_Power_,New_Normal_Q79,Incident_Q39,Transmit_Q122,Reflect_Q122);

        // Multiply
        float Product_Q125 = Y_Q90 * Y_Q90;

        float3 NearP_Q65;
        float3 NearQ_Q65;
        float Distance_Q65;
        Min_Segment_Distance_B65(Left_Index_Q64,Left_Index_Middle_Q64,fragInput.posWorld,_WorldSpaceCameraPos,NearP_Q65,NearQ_Q65,Distance_Q65);

        float3 NearP_Q63;
        float3 NearQ_Q63;
        float Distance_Q63;
        Min_Segment_Distance_B65(Right_Index_Q64,Right_Index_Middle_Q64,fragInput.posWorld,_WorldSpaceCameraPos,NearP_Q63,NearQ_Q63,Distance_Q63);

        // Reflect
        float3 Reflected_Q47 = reflect(Incident_Q39, New_Normal_Q79);

        // Multiply_Colors
        float4 Product_Q103 = Linear_Q46 * float4(1,1,1,1);

        // DotProduct3
        float Dot_Q72 = dot(Incident_Q70, Normalized_Q71);

        float Result_Q137;
        Conditional_Float_B137(_Rim_Spread_Texture_,One_Minus_F_Q124,0.5,Result_Q137);

        // Max
        float MaxAB_Q123=max(Reflect_Q122,Product_Q125);

        float NotInShadow_Q67;
        #if defined(_OCCLUSION_ENABLED_)
          FingerOcclusion_B67(_Width_,Distance_Q65,_Fuzz_,_Min_Fuzz_,fragInput.posWorld,fragInput.binormal.xyz,NearP_Q65,_Clip_Fade_,NotInShadow_Q67);
        #else
          NotInShadow_Q67 = 1;
        #endif

        float NotInShadow_Q68;
        #if defined(_OCCLUSION_ENABLED_)
          FingerOcclusion_B68(_Width_,Distance_Q63,_Fuzz_,_Min_Fuzz_,fragInput.posWorld,fragInput.binormal.xyz,NearP_Q63,_Clip_Fade_,NotInShadow_Q68);
        #else
          NotInShadow_Q68 = 1;
        #endif

        float4 Reflected_Color_Q51;
        float4 Indirect_Diffuse_Q51;
        #if defined(_ENV_ENABLE_)
          Mapped_Environment_B51(_Reflection_Map_,_Indirect_Environment_,Reflected_Q47,Reflected_Color_Q51,Indirect_Diffuse_Q51);
        #else
          Reflected_Color_Q51 = float4(0,0,0,1);
          Indirect_Diffuse_Q51 = float4(0,0,0,1);
        #endif

        half4 Reflected_Color_Q50;
        half4 Indirect_Color_Q50;
        #if defined(_SKY_ENABLED_)
          Sky_Environment_B50(New_Normal_Q79,Reflected_Q47,Linear_Q43,Linear_Q44,Linear_Q45,_Horizon_Power_,Reflected_Color_Q50,Indirect_Color_Q50);
        #else
          Reflected_Color_Q50 = half4(0,0,0,1);
          Indirect_Color_Q50 = half4(0,0,0,1);
        #endif

        float Hue_Q75;
        float Saturation_Q75;
        float Value_Q75;
        float Alpha_Q75;
        float3 HSV_Q75;
        To_HSV_B75(Product_Q103,Hue_Q75,Saturation_Q75,Value_Q75,Alpha_Q75,HSV_Q75);

        float Result_Q110;
        Code_B110(Dot_Q72,Result_Q110);

        // Abs
        float AbsA_Q76 = abs(Result_Q110);

        float4 Result_Q138;
        Rim_Light_B138(Normalized_Q107,Normalized_Q38,Incident_Q70,_Rim_Intensity_,_Rim_Texture_,_Rim_View_Shift_,Result_Q137,Result_Q138);

        // Min
        float MinAB_Q58=min(NotInShadow_Q67,NotInShadow_Q68);

        // Add_Colors
        half4 Sum_Q48 = Reflected_Color_Q51 + Reflected_Color_Q50;

        // Add_Colors
        half4 Sum_Q49 = Indirect_Diffuse_Q51 + Indirect_Color_Q50;

        float Out_Q114;
        Remap_Range_B114(-1,1,0,1,Result_Q110,Out_Q114);

        // Modify
        float Product_Q106;
        Product_Q106 = AbsA_Q76 * _Hue_Shift_;
        //Product_Q106 = sign(AbsA_Q76)*sqrt(abs(AbsA_Q76))*_Hue_Shift_;

        float Hue_Q127;
        float Saturation_Q127;
        float Value_Q127;
        float Alpha_Q127;
        float3 HSV_Q127;
        To_HSV_B75(Result_Q138,Hue_Q127,Saturation_Q127,Value_Q127,Alpha_Q127,HSV_Q127);

        // From_XY
        float2 Vec2_Q112 = float2(Out_Q114,0.5);

        float3 HSV_Out_Q108;
        VaryHSV_B108(HSV_Q75,Product_Q106,_Saturation_Shift_,_Value_Shift_,HSV_Out_Q108);

        float3 HSV_Out_Q126;
        VaryHSV_B108(HSV_Q127,_Rim_Hue_Shift_,_Rim_Saturation_Shift_,_Rim_Value_Shift_,HSV_Out_Q126);

        // Color_Texture
        float4 Color_Q111;
        #if defined(_IRIDESCENCE_ENABLED_)
          Color_Q111 = tex2D(_Iridescence_Texture_,Vec2_Q112);
        #else
          Color_Q111 = float4(0,0,0,0);
        #endif

        float X_Q74;
        float Y_Q74;
        float Z_Q74;
        To_XYZ_B74(HSV_Out_Q108,X_Q74,Y_Q74,Z_Q74);

        float X_Q128;
        float Y_Q128;
        float Z_Q128;
        To_XYZ_B74(HSV_Out_Q126,X_Q128,Y_Q128,Z_Q128);

        // Scale_Color
        float4 Result_Q113 = _Iridescence_Intensity_ * Color_Q111;

        float4 Color_Q73;
        From_HSV_B73(X_Q74,Y_Q74,Z_Q74,0,Color_Q73);

        float4 Color_Q129;
        From_HSV_B73(X_Q128,Y_Q128,Z_Q128,0,Color_Q129);

        // Blend_Over
        float4 Result_Q84 = Result_Q91 + (1.0 - Result_Q91.a) * Color_Q73;

        // Scale_Color
        float4 Result_Q131 = _Rim_Intensity_ * Color_Q129;

        float4 Result_Q121;
        Fragment_Main_B121(_Sun_Intensity_,_Sun_Theta_,_Sun_Phi_,New_Normal_Q79,Result_Q84,MaxAB_Q123,_Shininess_,Incident_Q39,_Horizon_Color_,_Sky_Color_,_Ground_Color_,_Indirect_Diffuse_,_Specular_,_Horizon_Power_,_Reflection_,Sum_Q48,Sum_Q49,_Sharpness_,Result_Q77,_Subsurface_,float4(0,0,0,0),Result_Q131,Result_Q113,Result_Q121);

        float4 Result_Q59;
        Scale_RGB_B59(Result_Q121,MinAB_Q58,Result_Q59);

        float4 sRGB_Q42;
        FastLinearTosRGB_B42(Result_Q59,sRGB_Q42);

        // Blend_Over
        half4 Result_Q31 = Blob_Color_Q30 + (1.0 - Blob_Color_Q30.a) * sRGB_Q42;

        // Set_And_Apply_Alpha
        float4 Result_Q40;
        Result_Q40.rgb = Result_Q31.rgb*_Alpha_; 
        Result_Q40.a = _Alpha_;

        float4 Out_Color = Result_Q40;
        float Clip_Threshold = 0.001;
        bool To_sRGB = false;

        result = Out_Color;
        return result;
    }

    ENDCG
  }
 }
}
