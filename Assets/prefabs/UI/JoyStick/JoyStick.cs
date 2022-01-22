using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform Handle;
    [SerializeField] RectTransform Background;
    Player player;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 DragPos = eventData.position;
        Vector2 BgPosition = Background.position;

        Debug.DrawLine(DragPos, BgPosition);

        //make the drag move the actual thumb stick
        Handle.localPosition = Vector2.ClampMagnitude(DragPos - BgPosition, Background.rect.width/2);
        player.SetJoystickData(Handle.localPosition);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer up");
        Handle.position = Background.position;
        Vector2 reset = new Vector2(0, 0);
        player.SetJoystickData(reset);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
