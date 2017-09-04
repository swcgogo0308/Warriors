//조이스틱
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image backgroundImage;
    private Image joystickImage;

    private Vector2 backgroundSize;
    private Vector2 pivot;

    private Vector2 inputVector;

    public bool isDrag;

    void Start()
    {
        backgroundImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();

        backgroundSize = backgroundImage.rectTransform.sizeDelta;

        pivot = backgroundImage.rectTransform.pivot;
        pivot.x -= 0.5f;
        pivot.y -= 0.5f;
        pivot *= 2;
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            backgroundImage.rectTransform,
            data.position,
            data.pressEventCamera,
            out pos
        ))
        {
            isDrag = true;

            pos.x = pos.x / backgroundSize.x * 2 + pivot.x;
            pos.y = pos.y / backgroundSize.y * 2 + pivot.y;

            if (pos.magnitude > 1)
            {
                pos.Normalize();
            }

            inputVector = pos;

            MoveJoystick(inputVector);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        isDrag = true;

        OnDrag(data);
    }

    public void OnPointerUp(PointerEventData data)
    {
        isDrag = false;

        inputVector = Vector2.zero;
        MoveJoystick(Vector2.zero);
    }

    private void MoveJoystick(Vector2 vector)
    {
        vector.x *= backgroundSize.x / 3f;
        vector.y *= backgroundSize.y / 3f;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 90;
        if (vector.magnitude <= 0)
        {
            angle = 0;
        }

        joystickImage.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        joystickImage.rectTransform.anchoredPosition = vector;
    }

    public Vector3 GetInputVector()
    {
        return inputVector;
    }
}
