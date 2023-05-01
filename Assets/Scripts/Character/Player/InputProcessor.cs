using ProcessSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    SwipeUp,
    SwipeDown,
    SwipeRight,
    SwipeLeft,
    PointerDown,
    PointerUp,
    PointerClick,
}

public class InputProcessor : MonoBehaviour, IProcess
{
    [field: SerializeField] public bool Activated { get; set; } = true;


    public Action<InputType> OnInputAction { get; set; }

    private Vector3 _lastMousePos;
    private bool _isPointerDowned;
    private bool _isPointerUped;
    private bool _isPointerSwiped;
    private float _swipeOffset = 30f;


    public void AwakeMe()
    {

    }

    public void StartMe()
    {

    }

    public void UpdateMe()
    {
        CheckInput();
    }

    public void DestroyMe()
    {
        OnInputAction = null;
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //LMB
            IsPointerDown();
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsPointerUp();
            IsPointerClick();
            _lastMousePos = default;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            var result = IsSwipe();
            if (result != InputType.PointerDown)
            {
                _isPointerSwiped = true;
                OnInputAction?.Invoke(result);
            }

            _lastMousePos = Input.mousePosition;
        }
    }

    private InputType IsSwipe()
    {
        if (_isPointerSwiped || _lastMousePos == default)
            return InputType.PointerDown;

        var delta = Input.mousePosition - _lastMousePos;
        //delta = delta.normalized;
        print(delta);
        if(delta.x > _swipeOffset)
            return InputType.SwipeRight;
        else if(delta.x < -_swipeOffset)
            return InputType.SwipeLeft;
        else if(delta.y > _swipeOffset)
            return InputType.SwipeUp;
        else if(delta.y < -_swipeOffset)
            return InputType.SwipeDown;

        return InputType.PointerDown;
    }

    private void IsPointerDown()
    {
        if (_isPointerDowned)
            return;
        _isPointerDowned = true;
        OnInputAction?.Invoke(InputType.PointerDown);

    }

    private void IsPointerUp()
    {
        if (!_isPointerDowned || _isPointerUped)
            return;
        OnInputAction?.Invoke(InputType.PointerUp);

        _isPointerUped = true;
    }

    private void IsPointerClick()
    {
        if (!_isPointerDowned || !_isPointerUped)
            return;
        OnInputAction?.Invoke(InputType.PointerClick);

        _isPointerUped = false;
        _isPointerDowned = false;
        _isPointerSwiped = false;


    }

}
