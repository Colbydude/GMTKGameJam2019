using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    public GameObject _text;
    public string textVal;
    public float transitionTime = 1.0f;

    private TextMeshProUGUI _tmp;
    private float startTime;
    private bool triggered = false;
    private bool exited = false;
    private bool forward = true;
    private bool done = false;
    private Color invisible;
    private Color visible;

    // Start is called before the first frame update
    void Start()
    {
        _tmp = _text.GetComponent<TextMeshProUGUI>();
        _tmp.color = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 0);
        invisible = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 0);
        visible = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !done)
            LerpText();
    }

    private void LerpText ()
    {
        float interval = (Time.time - startTime) / transitionTime;
        if (forward)
            _tmp.color = Color.Lerp(invisible, visible, interval);
        else
            _tmp.color = Color.Lerp(visible, invisible, interval);

        if (interval >= 1.0f && !forward)
            done = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "Player" && !triggered)
        {
            _tmp.SetText(textVal);
            startTime = Time.time;
            triggered = true;
            forward = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && triggered && !exited)
        {
            exited = true;
            startTime = Time.time;
            forward = false;
        }
    }
}
