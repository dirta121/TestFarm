using UnityEngine;
namespace TestFarm
{
    [RequireComponent(typeof(Camera))]
    public class CalibrateCamera : MonoBehaviour
    {
        [Tooltip("Parent's transform of a tilemap")]
        public Transform tilemap;
        [Tooltip("Cell To move camera")]
        public Vector3Int cell;
        private Camera _camera;
        void Start()
        {
            _camera = GetComponent<Camera>();
            Calibrate();
        }
        /// <summary>
        /// Перемещает камеру таким образом, чтобы tilemap отображался в левом нижнем углу
        /// </summary>
        private void Calibrate()
        {
            var vertExtent = _camera.orthographicSize;
            var horzExtent = vertExtent * Screen.width / Screen.height;
            var tilemapPosition = tilemap.transform.position;
            _camera.transform.position = new Vector3(tilemapPosition.x + horzExtent, tilemapPosition.y + vertExtent, _camera.transform.position.z);
        }
    }
}