using UnityEngine;
using System.Collections;

public class RenderChecker : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var collider = gameObject.GetComponent<BoxCollider2D>();
        var height = Camera.main.orthographicSize * 2;
        var width = height * Screen.width / Screen.height;

        collider.size = new Vector2(width+2, height+2);
        collider.isTrigger = true;
    }
}
