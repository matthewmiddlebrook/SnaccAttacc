using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour, IDragHandler
{
    public float speed;
    private ScrollRect credits;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        credits = GetComponent<ScrollRect>();
    }

    void OnEnable() {
        timer = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            if (credits.verticalNormalizedPosition > 0) {
                credits.verticalNormalizedPosition -= Time.deltaTime*(speed/100);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        timer = 2;
    }
}
