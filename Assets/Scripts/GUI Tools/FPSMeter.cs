using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSMeter : MonoBehaviour
{
    protected TMP_Text label;

    protected void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    protected void Update()
    {
        int current = (int) (1f / Time.unscaledDeltaTime);
        label.text = current.ToString() + " FPS";
        if (current >= 60) label.color = Color.green;
        else if (current >= 30) label.color = Color.yellow;
        else label.color = Color.red;
    }
}
