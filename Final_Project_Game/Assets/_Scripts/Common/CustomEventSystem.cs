using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventSystem : MonoBehaviorInstance<CustomEventSystem>
{
    EventSystem eventSystem;
    public GameObject LastSelectedObject { get; set; }

    void Awake()
    {
        eventSystem = EventSystem.current;
    }
    void Update()
    {
        if (eventSystem == null)
            eventSystem = EventSystem.current;

        if(eventSystem.currentSelectedGameObject != null)
        {
            if(eventSystem.currentSelectedGameObject != LastSelectedObject || LastSelectedObject == null)
            {
                LastSelectedObject = eventSystem.currentSelectedGameObject;
            }
        }
    }
}
