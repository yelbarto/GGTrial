using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class BlockHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    Vector3 mOffSet;
    float mZCoord;
    public Block block;
    private GameController engine;
    public int cubeAmount;
    private Tuple<int, int> mazeLoc;
    public int spawnPlace;
    Plane objPlane;

    void Start()
    {
        engine = (GameController)GameObject.Find("GameController").GetComponent(typeof(GameController));
        cubeAmount = block.cubeAmount;
    }

    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        
        float rotate = engine.parent.transform.rotation.y;

        if (rotate == 0f || rotate == 0.7071068f || rotate == 1f)
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffSet = gameObject.transform.position - GetMouseWorldPos();
            transform.localScale = block.originalScale;
            transform.parent = engine.parent.transform;
            objPlane = new Plane(Vector3.up, transform.position);
        }       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float rotate = engine.parent.transform.rotation.y;
        if(rotate==0f || rotate== 0.7071068f || rotate == 1f)
        {
            if (mazeLoc != null)
            {
                Collider col = transform.GetComponent<BoxCollider>();
                col.enabled = false;
                engine.setAvailableBlock(block.place);
                engine.fillMaze(block, transform, mazeLoc, 0);
            }
            else
            {
                transform.SetParent(null);
                transform.localPosition = engine.spawnPlaceLoc[spawnPlace];
                transform.localScale = transform.localScale / 2;
            }
        }      
    }

    public void OnDrag(PointerEventData eventData)
    {       
        float rotate = engine.parent.transform.rotation.y;
        if (rotate == 0f || rotate == 0.7071068f || rotate == 1f)
        {
            Transform child = transform.GetChild(0);
            mazeLoc = engine.placeChecker(block, child);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                transform.position = mRay.GetPoint(rayDistance) + mOffSet;
            }
        }
    }
}
