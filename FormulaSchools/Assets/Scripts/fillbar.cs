using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fillbar : MonoBehaviour {

    public Gradient gradient;
    Slider slid;
    [SerializeField]
    private bool up = true;
    public bool IsGathering=true;
    [Range (0.1f,5f)]
    public float SpeedFill;
    // Use this for initialization
    void Start () {
        slid = GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        if (IsGathering)
        {
            updateColor();
            updateValue();
        }
	}
    void updateColor()
    {
        
        Color col = gradient.Evaluate((float)slid.maxValue - (float)slid.value);
        Debug.Log((float)slid.maxValue - (float)slid.value);
        ColorBlock colorblock = new ColorBlock();
        colorblock.normalColor = col;
        slid.colors = colorblock;
        slid.fillRect.GetComponent<Image>().color = col;
    }
    void updateValue()
    {
        if (slid.value >= slid.maxValue &&up)
        {
            up = false;
        }
        else if (slid.value <= 0.2f)
        {
            up = true;
        }
        
        if (up)
        {
            slid.value = Mathf.MoveTowards(slid.value, slid.maxValue+0.1f,  slid.value*SpeedFill * Time.deltaTime);
        }
        else
        {

            slid.value = Mathf.MoveTowards(slid.value, slid.minValue,  slid.value*SpeedFill * Time.deltaTime);
        }
    }
    public float SliderValue()
    {
        return slid.value;
    }
}
