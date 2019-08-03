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
    private bool forward = true;
    private Color invisible;
    private Color visible;

    // Start is called before the first frame update
    void Start()
    {
        _tmp = _text.GetComponent<TextMeshProUGUI>();
        _tmp.SetText(textVal);
        _tmp.color = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 0);
        invisible = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 0);
        visible = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
            LerpText();
    }

    private void LerpText ()
    {
        if (forward)
            _tmp.color = Color.Lerp(invisible, visible, (Time.time - startTime) / transitionTime);
        else
            _tmp.color = Color.Lerp(visible, invisible, (Time.time - startTime) / transitionTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !triggered)
        {
            startTime = Time.time;
            triggered = true;
            forward = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && triggered)
        {
            startTime = Time.time;
            forward = false;
        }
    }
}
