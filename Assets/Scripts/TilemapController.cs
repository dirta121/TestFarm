using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
namespace TestFarm
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapController : MonoBehaviour
    {
        public TilemapEvent onTileClick = new TilemapEvent();
        public TilemapEvent onTileDragBegin = new TilemapEvent();
        public TilemapEvent onTileDrag = new TilemapEvent();
        public TilemapEvent onTileDragEnd = new TilemapEvent();
        private Grid _grid;
        private Tilemap _tilemap;
        private (Vector3Int, Color, TileFlags)? _previousTileColor;
        private Vector3Int? _cell;
        private void Start()
        {
            _grid = GetComponentInParent<Grid>();
            _tilemap = GetComponent<Tilemap>();
            InputController.onMouseDown.AddListener(OnTileDown);
            InputController.onMouseClick.AddListener(OnTileClick);
            InputController.onMouseDrag.AddListener(OnTileDrag);
            InputController.onMouseUp.AddListener(OnTileUp);
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
        public Vector3 WorldToCell(Vector3Int cell)
        {
            return _grid.CellToWorld(cell);
        }
        public Vector3 WorldToCellCenter(Vector3Int cell)
        {
            Vector3 vector = _grid.CellToWorld(cell);
            return new Vector3(vector.x + _grid.cellSize.x / 2, vector.y + _grid.cellSize.y / 2, vector.z);
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
        /// <summary>
        /// Recieve cell coordinates from raycast
        /// </summary>
        /// <returns></returns>
        private bool TryGetTileCell(Vector3 mousePosition, out Vector3Int? cell)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
            cell = _grid.WorldToCell(mouseWorldPos);
            if (cell.Value.x <= _tilemap.size.x - 1 && cell.Value.y <= _tilemap.size.y - 1)
            {
                return true;
            }
            cell = null;
            return false;
        }
        private Coroutine _dragDelayCroroutine;
        private Vector3 _mousePosition;
        private const float _dragDelay = 0.25f;
        private float _time;
        private bool _drag;
        IEnumerator DragDelayCoroutine(Vector3 mousePosition)
        {
            _time = 0;
            _mousePosition = mousePosition;
            while (_time <= _dragDelay)
            {
                _time += Time.deltaTime;
                yield return null;
            }
            OnTileDragBegin(Input.mousePosition);
        }
        private void OnTileDragBegin(Vector3 mousePosition)
        {
            Debug.Log(Vector3.Distance(_mousePosition, mousePosition));
            if (_cell.HasValue && Vector3.Distance(_mousePosition, mousePosition) < _grid.cellSize.x)
            {
                SetSelection(_cell.Value, Color.green);
                _drag = true;
                Debug.Log($"TileBeginDrag cell {_cell}");
                onTileDragBegin?.Invoke(_cell.Value, mousePosition);
            }
        }
        private void OnTileDown(Vector3 mousePosition)
        {
            if (TryGetTileCell(mousePosition, out _cell))
            {
                SetSelection(_cell.Value, Color.green);
                if (_dragDelayCroroutine != null)
                {
                    StopCoroutine(_dragDelayCroroutine);
                }
                _dragDelayCroroutine = StartCoroutine(DragDelayCoroutine(mousePosition));
                Debug.Log($"TileDown cell {_cell}");
            }
        }
        private void OnTileClick(Vector3 mousePosition)
        {
            if (_cell.HasValue)
            {
                if (_dragDelayCroroutine != null)
                {
                    StopCoroutine(_dragDelayCroroutine);
                }
                Debug.Log($"TileClick on cell {_cell}");
                onTileClick?.Invoke(_cell.Value, mousePosition);
                ClearSelection();
            }
        }
        private void OnTileDragEnd(Vector3 mousePosition)
        {
            if (_drag && _cell.HasValue)
            {
                Debug.Log($"TileBeginDrag cell {_cell}");
                onTileDragEnd?.Invoke(_cell.Value, mousePosition);
            }
            _drag = false;
        }
        private void OnTileUp(Vector3 mousePosition)
        {
            if (_cell.HasValue)
            {
                Debug.Log($"TileUp on cell {_cell.Value}");
                if (_drag)
                {
                    OnTileDragEnd(mousePosition);
                }
                ClearSelection();
            }
        }
        private void OnTileDrag(Vector3 mousePosition)
        {
            if (_drag && _cell.HasValue)
            {
                Debug.Log($"TileDrag cell {_cell}");
                onTileDrag?.Invoke(_cell.Value, mousePosition);
            }
        }
        public class TilemapEvent : UnityEvent<Vector3Int, Vector3>
        {
        }
    }
}
