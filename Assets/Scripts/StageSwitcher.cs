using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using Klak.Chromatics;

namespace Xola {

public sealed class StageSwitcher : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public StageConfig[] Stages { get; set; }

    #endregion

    #region Private methods

    CosineGradient Lerp(in CosineGradient g1, in CosineGradient g2, float t)
      => new CosineGradient { R = math.lerp(g1.R, g2.R, t),
                              G = math.lerp(g1.G, g2.G, t),
                              B = math.lerp(g1.B, g2.B, t)};

    int? CheckStageInvocation()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) return 0;

        for (var i = 0; i < 9; i++)
        {
            var key = (KeyCode)((int)KeyCode.Alpha1 + i);
            if (Input.GetKeyDown(key)) return i + 1;
        }

        return null;
    }

    VisualEffect[] EnumerateVfx(StageConfig stage)
      => stage.transform.GetComponentsInChildren<VisualEffect>();

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        var idKill = Shader.PropertyToID("Kill");

        var bg = Camera.main.GetComponent<GradientBackgroundController>();
        var current = Stages[0];
        var next = (StageConfig)null;

        #if !UNITY_EDITOR
        Cursor.visible = false;
        #endif

        while (true)
        {
            while (next == null)
            {
                await Awaitable.NextFrameAsync();
                var inv = CheckStageInvocation();
                if (inv != null) next = Stages[(int)inv];
            }

            foreach (var vfx in EnumerateVfx(current))
            {
                if (vfx.HasBool(idKill)) vfx.SetBool(idKill, true);
                vfx.Stop();
            }

            var grad0 = current.Background;
            var grad1 = next.Background;

            for (var t = 0.0f; t < 1; t += Time.deltaTime / 2)
            {
                bg.Gradient = Lerp(grad0, grad1, t);
                await Awaitable.NextFrameAsync();
            }

            foreach (var vfx in EnumerateVfx(next))
            {
                if (vfx.HasBool(idKill)) vfx.SetBool(idKill, false);
                vfx.Play();
            }

            current = next;
            next = null;
        }
    }

    #endregion
}

} // namespace Xola
