using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] GameObject mapContent;

    

    private void Update()
    {
        float wheelScroll = Input.GetAxis("Mouse ScrollWheel");
        if (wheelScroll != 0)
        {
            mapContent.transform.localScale += new Vector3(wheelScroll, wheelScroll, 0);
        }
    }
}
