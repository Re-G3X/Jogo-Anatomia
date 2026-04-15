using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler
{
    public Lane lane;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("TOQUE DETECTADO");
        lane.OnInput();
    }
}