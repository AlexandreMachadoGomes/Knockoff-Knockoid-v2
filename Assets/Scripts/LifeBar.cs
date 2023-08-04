using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{

    public Color colorMin;
    public Color colorMax;

    public Player player;
    private Text thisText;

    // Start is called before the first frame update
    void Start()
    {
        thisText = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thisText.text = ((int)player.lifePercentage).ToString();
        thisText.color = Color.Lerp(colorMin, colorMax, player.lifePercentage / 600);
    }
}
