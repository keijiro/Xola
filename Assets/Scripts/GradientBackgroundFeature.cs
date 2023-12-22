using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Xola {

public sealed class GradientBackgroundFeature : ScriptableRendererFeature
{
    GradientBackgroundPass _pass;

    public override void Create()
    {
        _pass = new GradientBackgroundPass();
        _pass.renderPassEvent = RenderPassEvent.BeforeRenderingSkybox;
    }

    public override void AddRenderPasses
      (ScriptableRenderer renderer, ref RenderingData data)
    {
        if (data.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(_pass);
    }
}

public sealed class GradientBackgroundPass : ScriptableRenderPass
{
    public override void Execute
      (ScriptableRenderContext context, ref RenderingData data)
    {
        var ctr = data.cameraData.camera.GetComponent<GradientBackgroundController>();
        if (ctr == null || ctr.Material == null || !ctr.enabled) return;

        var cmd = CommandBufferPool.Get();
        CoreUtils.DrawFullScreen(cmd, ctr.Material, ctr.Properties);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}

} // namespace Xola
