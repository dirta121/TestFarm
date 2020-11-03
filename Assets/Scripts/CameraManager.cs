using UnityEngine;
namespace TestFarm
{
    [ExecuteInEditMode]
    public class CameraManager : MonoBehaviour
    {
        public float horizontalResolution = 1125;
        void OnGUI()
        {
            float currentAspect = (float)Screen.width / (float)Screen.height;
            Camera.main.orthographicSize = horizontalResolution / currentAspect / 200;
        }
    }
}