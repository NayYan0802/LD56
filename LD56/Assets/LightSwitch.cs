using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public new InteractableObject light;
    public SpriteRenderer shadow;

    private void Start()
    {
        shadow = GameObject.Find("Shadow").GetComponent<SpriteRenderer>();
    }

    public void Switch()
    {
        if (light.isOn)
        {
            Color color = shadow.color;
            color.a = 0.25f;
            shadow.color = color;
        }
        else
        {
            Color color = shadow.color;
            color.a = 0.75f;
            shadow.color = color;
        }
    }
}
