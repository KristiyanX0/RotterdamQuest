using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;

public class UITools
{
    
    public static bool IsPointerOverUIObject(GameObject map, int fingerId)
    {
        return IsPointerOverUIObject(map.GetComponent<RectTransform>(), fingerId);   
    }

    
    public static bool IsPointerOverAnyUIObject(List<GameObject> maps, int fingerId)
    {
        foreach(GameObject map in maps)
        {
            if (IsPointerOverUIObject(map, fingerId))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsPointerOverAllUIObject(List<GameObject> maps, int fingerId)
    {
        foreach(GameObject map in maps)
        {
            if (!IsPointerOverUIObject(map, fingerId))
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsPointerOverUIObject(RectTransform mapRectTransform, int fingerId)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.GetTouch(fingerId).position
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            // The first hit element should be our mapRectTransform
            if (raycastResults[0].gameObject == mapRectTransform.gameObject)
            {
                return true; // The map is directly touched
            }
        }
        
        // ******* TESTING *******
        if (raycastResults.Count == 0) {
            return true;
        }

        return false;
    }
}

