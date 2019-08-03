using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTrigger : MonoBehaviour
{
    public enum TriggerType {
        Ladder,
        Bridge,
        Surfboard,
        Platform,
        Seesaw,
        Helicopter
    };

    public GameObject climbableLadderObject;
    public GameObject bridgeLadderObject;

    public TriggerType triggerType = TriggerType.Ladder;

    private SpriteRenderer _renderer;
    private CapsuleCollider2D _triggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _triggerCollider = GetComponent<CapsuleCollider2D>();

        _renderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") {
            _renderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") {
            _renderer.enabled = false;
        }
    }

    public GameObject spawnLadder()
    {
        Debug.Log("Spawning ladder...");
        GameObject ladderObject;

        switch (triggerType) {
            case TriggerType.Ladder:
                ladderObject = climbableLadderObject;
            break;
            case TriggerType.Bridge:
                ladderObject = bridgeLadderObject;
            break;
            default:
                ladderObject = climbableLadderObject;
            break;
        }

        return Instantiate(ladderObject, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
    }
}
