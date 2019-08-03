using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    [SerializeField] Object sceneToGoTo;
    [SerializeField] float xPosition;
    [SerializeField] float yPosition;

    BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Cllided With: " + collider.gameObject.name);

        Transform playerTransform = collider.gameObject.GetComponent<Transform>();

        playerTransform.position = new Vector3(xPosition, yPosition, 0);
    }
}
