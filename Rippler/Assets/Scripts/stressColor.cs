using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class stressColor : MonoBehaviour
{
    public Color unstressedColor;
    public Color stressedColor;
    public int stressLevel;
    public int deltaStress;
    public int maxStress;

    // keys used for testing. Leave in 2 allow 4 hax
    public KeyCode keyUp;
    public KeyCode keyDown;

    public Image image { get; private set; }

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        updateColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyDown))
        {
            stressDown();
        }
        else if (Input.GetKeyUp(keyUp))
        {
            stressUp();
        }
    }

    public void stressDown()
    {
        if (stressLevel > 0)
        {
            stressLevel -= deltaStress;
            if (stressLevel < 0)
                stressLevel = 0;
        }
        updateColor();
    }

    public void stressUp()
    {
        if (stressLevel < maxStress)
        {
            stressLevel += deltaStress;
            if (stressLevel > maxStress)
                stressLevel = maxStress;
        }
        updateColor();
    }

    public void updateColor()
    {
        image.color = Color.Lerp(unstressedColor, stressedColor, (float)stressLevel / (float)maxStress);
    }

}