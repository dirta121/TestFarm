using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
namespace TestFarm
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        public TilemapEvent onTileSelected = new TilemapEvent();
        public TilemapEvent onTileDragBegin = new TilemapEvent();
        public TilemapEvent onTileDragEnd = new TilemapEvent();
        private Grid _grid;
        private Tilemap _tilemap;
        private (Vector3Int, Color, TileFlags)? _previousTileColor;
        private Vector3Int _cell;
        private float _exitTime;
        private bool _pointerDown;
        private void Start()
        {
            _grid = GetComponentInParent<Grid>();
            _tilemap = GetComponent<Tilemap>();

        }
        /// <summary>
        /// Set size of a tilemap
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(Vector3Int size)
        {
            if (_tilemap)
                _tilemap.size = size;
        }
        /// <summary>
        /// Recieve cell coordinates from raycast
        /// </summary>
        /// <returns></returns>
        private Vector3Int GetTileCell()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = _grid.WorldToCell(mouseWorldPos);
            return cell;
        }
        public Vector3 WorldToCell(Vector3Int cell)
        {
            return _grid.CellToWorld(cell);
        }
        public Vector3 WorldToCellCenter(Vector3Int cell)
        {
            Vector3 vector = _grid.CellToWorld(cell);
            return new Vector3(vector.x + _grid.cellSize.x / 2, vector.y + _grid.cellSize.y / 2, vector.z);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("PointerClick");   
            //onTileSelected?.Invoke(_cell);
        }
        /// <summary>
        /// Set the default color to the selected cell
        /// </summary>
        public void ClearSelection()
        {
            //remove selection color from last tile selected and return flags state
            if (_previousTileColor.HasValue)
            {
                _tilemap.SetColor(_previousTileColor.Value.Item1, _previousTileColor.Value.Item2);
                _tilemap.SetTileFlags(_previousTileColor.Value.Item1, _previousTileColor.Value.Item3);
                _previousTileColor = null;
            }
        }
        /// <summary>
        /// Set color to the cell
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="color"></param>
        public void SetSelection(Vector3Int cell, Color color)
        {
            _previousTileColor = (cell, _tilemap.GetColor(cell), _tilemap.GetTileFlags(cell));
            _tilemap.SetTileFlags(cell, TileFlags.None);
            _tilemap.SetColor(cell, color);
        }
        IEnumerator DragCoroutine()
        {
            _exitTime = 0;
            while (_pointerDown)
            {
                _exitTime += Time.deltaTime;
                if (_exitTime > 0.25f)
                {
                    Debug.Log($"Drag");
                    onTileDragBegin?.Invoke(_cell);
                    break;
                }
                yield return null;
            }
        }
        /// <summary>
        /// Drag is after 0.25sec else it is click
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
            _cell = GetTileCell();
            StartCoroutine(DragCoroutine());
            Debug.Log($"PointerDown on cell {_cell}");
            //onTileDragBegin?.Invoke(_cell);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log(_exitTime);
            if (_exitTime < 0.25f)
            {
                Debug.Log("PointerClick");

                onTileSelected?.Invoke(_cell);
            }
            else
            {
                Debug.Log("PointerUP");
                onTileDragEnd?.Invoke(_cell);
            }
            _pointerDown = false;
        }
        public class TilemapEvent : UnityEvent<Vector3Int>
        {
        }
    }
}
