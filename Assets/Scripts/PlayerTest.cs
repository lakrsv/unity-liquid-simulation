using UnityEngine;
using System.Collections;

public class PlayerTest : MonoBehaviour
{
    Rigidbody2D _thisRigid;
    Vector3 velocity;
    bool noclip = false;
    void Start()
    {
        _thisRigid = GetComponent<Rigidbody2D>();
        Invoke("DelayedInitialize", 0.25f);
    }
    void Update()
    {
        var inputH = Input.GetAxis("Horizontal");
        var inputV = Input.GetAxis("Vertical");

        velocity.Set(inputH * 20, inputV * 20, 0);

        _thisRigid.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            noclip = !noclip;
            _thisRigid.isKinematic = noclip;
        }


    }

    void DelayedInitialize()
    {
        gameObject.transform.SetParent(null);
        Camera.main.transform.SetParent(this.gameObject.transform);
    }
}
