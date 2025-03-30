using UnityEngine;

public static class DestroyUtils
{
    /// <summary>
    ///     Destroys the object safely, regardless of Edit or Play mode.
    ///     Depending on the mode it calls either DestroyImmediate or Destroy.
    /// </summary>
    /// <param name="self"></param>
#if UNITY_EDITOR
    public static void DestroyInAnyMode(this Object self)
    {
        if (Application.isPlaying == false)
            Object.DestroyImmediate(self);
        else
            Object.Destroy(self);
    }
#else
        public static void DestroyInAnyMode(this Object self) => Object.Destroy(self);
#endif
}