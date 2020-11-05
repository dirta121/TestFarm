using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class InputController : MonoBehaviour
{
    public static InputEvent onMouseClick = new InputEvent();
    public static InputEvent onMouseDown = new InputEvent();
    public static InputEvent onMouseUp = new InputEvent();
    public static InputEvent onMouseDrag = new InputEvent();
    public static bool canInput = true;
    private const float _clickDelayConst = 0.25f;
    private const float _clickTrashhold = 0.2f;

    private void Update()
    {
        if (canInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Down(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Up(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                Drag(Input.mousePosition);
            }
        }
    }
    /// <summary>
    /// Time when keydown to check delay between down and up
    /// </summary>
    private float _time;
    /// <summary>
    /// Mouse position to check distance 
    /// </summary>
    private Vector3 _mousePosition;
    IEnumerator DelayCoroutine(Vector3 mousePosition)
    {
        _time = 0;
        _mousePosition = mousePosition;
        while (_time <= _clickDelayConst)
        {
            _time += Time.deltaTime;
            yield return null;
        }
    }
    private void Drag(Vector3 mousePosition)
    {
        Debug.Log("Drag");
        onMouseDrag?.Invoke(mousePosition);
    }
    private void Click(Vector3 mousePosition)
    {
        Debug.Log("Mouse Click");
        onMouseClick?.Invoke(mousePosition);
    }
    private void Down(Vector3 mousePosition)
    {
        onMouseDown?.Invoke(mousePosition);
        Debug.Log("Mouse Down");
        StartCoroutine(DelayCoroutine(mousePosition));
    }
    private void Up(Vector3 mousePosition)
    {
        Debug.Log("Mouse Up");
        onMouseUp?.Invoke(mousePosition);
        if (_time <= _clickDelayConst && Vector3.Distance(_mousePosition, mousePosition) < _clickTrashhold)
        {
            Click(mousePosition);
        }
    }
    public class InputEvent : UnityEvent<Vector3> { }
}

