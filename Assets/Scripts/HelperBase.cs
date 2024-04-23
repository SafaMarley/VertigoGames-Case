using UnityEngine;

public class HelperBase<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        instance = this as T;
    }
}
