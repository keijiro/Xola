#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"

void GetNoise_float(float2 UV, float Time, out float Output)
{
    float3 crd1 = float3(UV * float2(0.8, 0.2), 0);
    float3 crd2 = float3(UV * float2(0.4, 0.1), 0);
    crd1 += float3(+0.03, +0.08, 0.05) * Time;
    crd2 += float3(-0.01, -0.04, 0.04) * Time;
    Output = SimplexNoise(crd1) * 0.5 + SimplexNoise(crd2) * 0.5 + 0.5;
}
