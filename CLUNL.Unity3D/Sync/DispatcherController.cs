using UnityEngine;

namespace CLUNL.Unity3D.Sync
{
    /// <summary>
    /// Default of a dispatcher controller.
    /// </summary>
    public class DispatcherController : MonoBehaviour
    {
        static bool __started = false;
        public void Start()
        {
            if (__started)
            {
                Destroy(gameObject);
                return;
            }
            Dispatcher.MarkInit();
            DontDestroyOnLoad(gameObject);
        }
        public void Update()
        {
            Dispatcher.Sync();
        }
    }
}
