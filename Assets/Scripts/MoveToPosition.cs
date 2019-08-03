using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log("Collided With: " + collider.gameObject.name);

        Rigidbody2D playerRigidBody = collider.gameObject.GetComponent<Rigidbody2D>();
        playerRigidBody.position = new Vector2(xPosition, yPosition);
        playerRigidBody.velocity = new Vector2(0, 0);

        if (sceneToGoTo != null && !SceneManager.GetActiveScene().Equals(sceneToGoTo)) {
            SceneManager.LoadScene(sceneToGoTo.name);
        }
    }
}
