using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform background;
    private RectTransform handle;
    private Vector2 inputVector;
    private Canvas canvas; // อ้างอิงถึง Canvas ที่ Joystick อยู่ในนั้น

    void Start()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // หาค่า Canvas ที่ Joystick อยู่ในนั้น
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(null, background.position);
        Vector2 radius = background.sizeDelta / 2;

        // ใช้ค่า scaleFactor จาก canvas
        inputVector = (eventData.position - position) / (radius * canvas.scaleFactor);
        inputVector = inputVector.magnitude > 1.0f ? inputVector.normalized : inputVector;

        // ย้ายตำแหน่ง Handle
        handle.anchoredPosition = new Vector2(inputVector.x * radius.x, inputVector.y * radius.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }
}