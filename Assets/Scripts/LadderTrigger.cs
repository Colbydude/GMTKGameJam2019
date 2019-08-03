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

    public GameObject player;
    public TriggerType triggerType = TriggerType.Ladder;

    private SpriteRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 2) {
            _renderer.enabled = true;
        } else {
            _renderer.enabled = false;
        }
    }
}
