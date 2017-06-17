using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour
{
    private GameObject _chunkRender;

    void Awake()
    {
        GetRender();
        _chunkRender.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "RenderCheck")
            _chunkRender.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "RenderCheck")
            _chunkRender.SetActive(false);
    }

    void GetRender()
    {
        if (_chunkRender == null)
        {
            _chunkRender = gameObject.transform.GetChild(0).gameObject;
        }
    }
}
