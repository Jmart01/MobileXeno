using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform Handle;
    [SerializeField] RectTransform Background;
    [SerializeField] RectTransform Pivot;

    public Vector2 Input
    {
        get;
        private set;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 DragPos = eventData.position;
        Vector2 BgPosition = Background.position;

        Debug.DrawLine(DragPos, BgPosition);

        //make the drag move the actual thumb stick
        Input = Vector2.ClampMagnitude(DragPos - BgPosition, Background.rect.width / 2);
        Handle.localPosition = Input;
        Input = Input / (Background.rect.width / 2);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pivot.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer up");
        Handle.position = Background.position;
        Pivot.localPosition = Vector2.zero;
        Input = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
