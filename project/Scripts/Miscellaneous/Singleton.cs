using UnityEngine;
using System.Collections;

public abstract class Singleton<Class> where Class : new() {
    private static Class instance = new Class();

    /// <summary>
    /// The singleton instance of the class.
    /// </summary>
    public static Class Instance {
        get {
            return instance;
        }
    }

    /// <summary>
    /// Reinstantiates the singleton instance of the class and returns the new instance.
    /// </summary>
    /// <returns>The new singleton instance.</returns>
    public static Class ResetSingletonInstace() {
        instance = new Class();
        return Instance;
    }
}
