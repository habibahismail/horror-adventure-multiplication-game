using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Sprite normalCursor;
    [SerializeField] private Sprite rightClickCursor;

    private Vector2 mousePosition;
    private RectTransform rectTransform;
    private Image cursorImage;
    private Vector3 defaultRotation = new Vector3(0, 0, 0);
    private Vector3 mouseIconRotation = new Vector3(0, 0, 20);

    private void Start()
    {
        Cursor.visible = false;
        rectTransform = GetComponent<RectTransform>();
        cursorImage = GetComponent<Image>();
    }

    private void Update()
    {
        mousePosition = Input.mousePosition;
        rectTransform.position = mousePosition;
    }

    public void ShowRightMouseClickIcon()
    {
        cursorImage.sprite = rightClickCursor;
        rectTransform.rotation = Quaternion.Euler(mouseIconRotation);

    }

    public void HideRightMouseClickIcon()
    {
        cursorImage.sprite = normalCursor;
        rectTransform.rotation = Quaternion.Euler(defaultRotation);
    }
}
