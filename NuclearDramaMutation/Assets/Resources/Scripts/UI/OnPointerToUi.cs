using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class OnPointerToUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 startVec;
    private Vector3 offsetVec;
    private RectTransform rectTransform;
    private Outline outline;
    private float doSpeed = 0.4f;
    private void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        startVec = rectTransform.anchoredPosition;
        outline= transform.GetComponent<Outline>();

    }
    public void OnPointerEnter(PointerEventData _eventData)
    {
        transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), doSpeed);
        outline.enabled = true;
        switch (transform.name)
        {
            case "Btn01":
                offsetVec = new Vector3(-600, 600);
                break;
            case "Btn02":
                offsetVec = new Vector3(600, 600);
                break;
            case "Btn03":
                offsetVec = new Vector3(-600, -600);
                break;
            case "Btn04":
                offsetVec = new Vector3(600, -600);
                break;
        }
        if (offsetVec != null)
            rectTransform.DOAnchorPos(offsetVec,doSpeed);
    }
    public void OnPointerExit(PointerEventData _eventData)
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), doSpeed);
        outline.enabled = false;
            rectTransform.DOAnchorPos(startVec,doSpeed);
    }
}

