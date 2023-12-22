using UnityEngine;
using Klak.Chromatics;

namespace Xola {

[ExecuteInEditMode]
public sealed class GradientBackgroundController : MonoBehaviour
{
    [field:SerializeField] public Material Material { get; set; }
    [field:SerializeField] public CosineGradient Gradient { get; set; }
      = CosineGradient.DefaultGradient;

    public MaterialPropertyBlock Properties => GetProperties();

    MaterialPropertyBlock _props;

    public MaterialPropertyBlock GetProperties()
    {
        if (_props == null) _props = new MaterialPropertyBlock();
        _props.SetMatrix("_Gradient", Gradient);
        return _props;
    }
}

} // namespace Xola
