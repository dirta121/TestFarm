using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
namespace TestFarm
{
    public class MoveCamera : MonoBehaviour
    {
        public BoxCollider2D limit;
        public float speed;
        private float _vertExtent;
        private float _horzExtent;
        private void Start()
        {
            _vertExtent = Camera.main.orthographicSize;
            _horzExtent = _vertExtent * Screen.width / Screen.height;
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                Drag();
            }
#else
        if (Input.touchCount > 0)
        {
            Drag();
        }
#endif
        }
        public void Drag()
        {
            var camera = Camera.main.transform;
            var x = camera.position.x - CrossPlatformInputManager.GetAxis("Mouse X") * Time.deltaTime * speed;
            var y = camera.position.y - CrossPlatformInputManager.GetAxis("Mouse Y") * Time.deltaTime * speed;
            camera.position = new Vector3(x, y, camera.position.z);
            if (camera.position.x <= limit.bounds.min.x + _horzExtent) camera.position = new Vector3(limit.bounds.min.x + _horzExtent,
                 camera.position.y,
                 camera.position.z);
            else if (camera.position.x >= limit.bounds.max.x - _horzExtent) camera.position = new Vector3(limit.bounds.max.x - _horzExtent,
                 camera.position.y,
                 camera.position.z);
            if (camera.position.y >= limit.bounds.max.y - _vertExtent) camera.position = new Vector3(camera.position.x,
                limit.bounds.max.y - _vertExtent,
                camera.position.z);
            else if (camera.position.y <= limit.bounds.min.y + _vertExtent) camera.position = new Vector3(camera.position.x,
             limit.bounds.min.y + _vertExtent,
             camera.position.z);
        }
    }
}