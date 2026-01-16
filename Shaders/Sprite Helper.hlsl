//UNITY_SHADER_NO_UPGRADE
#define MYHLSLINCLUDE_INCLUDED

void FlipSprite_float(float3 input, out float3 output)
{
    #if !defined(SHADERGRAPH_PREVIEW)
    output = float3(unity_SpriteProps.x == 1 ? input.x : -input.x,input.y, input.z);
    #else
    output = input;
    #endif
}