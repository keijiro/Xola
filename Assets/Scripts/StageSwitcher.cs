using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using Klak.Chromatics;

namespace Xola {

public sealed class StageSwitcher : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public StageConfig[] Stages { get; set; }

    #endregion

    #region Private methods

    static int _idKill = Shader.PropertyToID("Kill");

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

    void StartVfx(StageConfig stage)
    {
        foreach (var vfx in EnumerateVfx(stage))
        {
            if (vfx.HasBool(_idKill)) vfx.SetBool(_idKill, false);
            vfx.Play();
        }
    }

    void StopVfx(StageConfig stage)
    {
        foreach (var vfx in EnumerateVfx(stage))
        {
            if (vfx.HasBool(_idKill)) vfx.SetBool(_idKill, true);
            vfx.Stop();
        }
    }

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
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
                if (Input.GetKeyDown(KeyCode.Space)) StopVfx(current);
            }

            StopVfx(current);

            var grad0 = current.Background;
            var grad1 = next.Background;

            for (var t = 0.0f; t < 1; t += Time.deltaTime / 2)
            {
                bg.Gradient = Lerp(grad0, grad1, t);
                await Awaitable.NextFrameAsync();
            }

            StartVfx(next);

            current = next;
            next = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
    }

    #endregion
}

} // namespace Xola
