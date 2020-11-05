using UnityEngine;
namespace TestFarm
{
    public class MoveCamera : MonoBehaviour
    {
        public BoxCollider2D limit;
        public float speed;
        private float _vertExtent;
        private float _horzExtent;
        public bool canMove=true;
        private void Start()
        {
            _vertExtent = Camera.main.orthographicSize;
            _horzExtent = _vertExtent * Screen.width / Screen.height;
            InputController.onMouseDrag.AddListener(Drag);
#if !UNITY_EDITOR
            speed = speed / 10.0f;
#endif
        }
        private void Drag(Vector3 mousePosition)
        {
            if (canMove)
            {
                float xAxis = 0;
                float yAxis = 0;
                var camera = Camera.main.transform;
#if UNITY_EDITOR

                xAxis = Input.GetAxis("Mouse X");
                yAxis = Input.GetAxis("Mouse Y");
#else
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                xAxis = Input.touches[0].deltaPosition.x;
                yAxis = Input.touches[0].deltaPosition.y;
            }    
#endif
                var x = camera.position.x - xAxis * Time.deltaTime * speed;
                var y = camera.position.y - yAxis * Time.deltaTime * speed;
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
}