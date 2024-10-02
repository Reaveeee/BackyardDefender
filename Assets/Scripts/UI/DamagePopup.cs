using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    Camera cam;
    
    public Vector2 position;
    float offset = 0;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Invoke("Remove", 1);
    }

    void Update()
    {
        GetComponent<RectTransform>().position = cam.WorldToScreenPoint(position) + new Vector3(0, offset);
        GetComponent<TextMeshProUGUI>().alpha = 1 - offset * .014f;
        offset += Time.deltaTime * 100;
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
