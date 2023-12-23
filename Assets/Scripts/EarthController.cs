using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using Klak.Motion;

namespace Xola {

public sealed class EarthController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public Transform FloatTarget { get; set; }
    [field:SerializeField] public Transform FixedTarget { get; set; }

    #endregion

    #region Private methods

    async Awaitable WaitKeyPress()
    {
        await Awaitable.NextFrameAsync();
        while (!Input.GetKeyDown(KeyCode.Z))
            await Awaitable.NextFrameAsync();
    }

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        // Subnode component references
        var follower = GetComponentInChildren<SmoothFollow>();
        var vfx = GetComponentInChildren<VisualEffect>();

        // Floating mode
        follower.target = FloatTarget;

        // Key press wait
        await WaitKeyPress();

        // VFX start
        vfx.Play();

        // Key press wait
        await WaitKeyPress();

        // Fixed mode
        follower.target = FixedTarget;

        // Key press wait
        await WaitKeyPress();

        // Floating mode again
        follower.target = FloatTarget;

        // Transition to flatten mode
        for (var t = 0.0f; t < 1; t += Time.deltaTime * 0.3f)
        {
            vfx.SetFloat("Flatten", math.smoothstep(0, 1, t));
            await Awaitable.NextFrameAsync();
        }
        vfx.SetFloat("Flatten", 1);

        // Key press wait
        await WaitKeyPress();

        // Termination
        vfx.SetBool("Kill", true);
    }

    #endregion
}

} // namespace Xola
