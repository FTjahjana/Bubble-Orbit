using UnityEngine;

public enum SurfaceEnum
{
    Grass,
    Concrete,
    Metal
    // Add more surfaces if needed, e.g. Carpet, Wood, etc.
}

public class SurfaceType : MonoBehaviour
{
    [Tooltip("Select what type of surface this object represents.")]
    public SurfaceEnum surfaceEnum;
}
