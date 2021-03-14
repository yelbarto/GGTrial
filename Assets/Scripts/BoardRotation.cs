using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardRotation : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private Vector3 mouseDownPos;
    private Vector3 mouseUpPos;

    private Animator anim;


    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseDownPos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mouseUpPos = Input.mousePosition;
        float difference = mouseUpPos.x - mouseDownPos.x;
        if (difference < -50)
        {
            anim = gameObject.GetComponent<Animator>();
            switch (transform.rotation.y)
            {
                case 0f:
                    anim.ResetTrigger("270to360");
                    anim.ResetTrigger("90to0");
                    anim.SetTrigger("0to90");
                    break;
                case 0.7071068f:
                    if(transform.rotation.w == transform.rotation.y)
                    {           
                        anim.ResetTrigger("0to90");
                        anim.ResetTrigger("180to90");
                        anim.SetTrigger("90to180");
                    break;
                    } else
                    {
                        anim.ResetTrigger("180to270");
                        anim.ResetTrigger("360to270");
                        anim.SetTrigger("270to360");
                        break;
                    }
                case 1f:
                    anim.ResetTrigger("90to180");
                    anim.ResetTrigger("270to180");
                    anim.SetTrigger("180to270");
                    break;                   
            }
        }
        else if (difference > 50)
        {
            anim = gameObject.GetComponent<Animator>();
            switch (transform.rotation.y)
            {
                case 0f:
                    anim.ResetTrigger("270to360");
                    anim.ResetTrigger("90to0");
                    anim.SetTrigger("360to270");
                    break;
                case 0.7071068f:
                    if (transform.rotation.w == transform.rotation.y)
                    {
                        anim.ResetTrigger("0to90");
                        anim.ResetTrigger("180to90");
                        anim.SetTrigger("90to0");
                        break;
                    }
                    else
                    {
                        anim.ResetTrigger("180to270");
                        anim.ResetTrigger("360to270");
                        anim.SetTrigger("270to180");
                        break;
                    }
                case 1f:
                    anim.ResetTrigger("90to180");
                    anim.ResetTrigger("270to180");
                    anim.SetTrigger("180to90");
                    break;
            }

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
