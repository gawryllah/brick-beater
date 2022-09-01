using UnityEngine;

public class AllScript : MonoBehaviour
{
    private static AllScript instance;

    public static AllScript Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(instance);
    }
}
