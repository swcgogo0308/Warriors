﻿//조이스틱
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;

	public bool isDrag;

    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
			isDrag = true;

            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //move Joystick IMG
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
		isDrag = true;

        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
		isDrag = false;
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
		return inputVector.x;
    }

    public float Vertical()
    {
		return inputVector.z;
    }

}
