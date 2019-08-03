using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToPosition : MonoBehaviour
{
    public Object sceneToGoTo = null;
    public float xPosition = 0;
    public float yPosition = 0;

    private BoxCollider2D _collider;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("MoveToPosition Collided With: " + collider.gameObject.name);

        Rigidbody2D playerRigidBody = collider.gameObject.GetComponent<Rigidbody2D>();
        playerRigidBody.position = new Vector2(xPosition, yPosition);
        playerRigidBody.velocity = new Vector2(0, 0);

        if (sceneToGoTo != null && !SceneManager.GetActiveScene().Equals(sceneToGoTo)) {
            SceneManager.LoadScene(sceneToGoTo.name);
        }
    }
}
