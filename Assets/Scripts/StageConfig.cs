using UnityEngine;
using Klak.Chromatics;

namespace Xola {

public sealed class StageConfig : MonoBehaviour
{
    [field:SerializeField] public CosineGradient Background { get; set; }
      = CosineGradient.DefaultGradient;
}

} // namespace Xola
