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

    public Canvas c;
    private bool canvasDisplayed;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        updateColor();
        c.gameObject.SetActive(!c.gameObject.activeInHierarchy);
        InvokeRepeating("stressUp", 2.0f, 1.0f);
        InvokeRepeating("increaseDelta", 2.0f, 60.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (stressLevel == maxStress && !canvasDisplayed)
        {
            
            c.gameObject.SetActive(!c.gameObject.activeInHierarchy);
            canvasDisplayed = true;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UI"))
            {
                obj.SetActive(false);
            }
        }
    }

    public void stressDown()
    {
        if (stressLevel > 0)
        {
            Debug.Log("Task Completed! Stress Decreased.");
            stressLevel -= 40;
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

    public void increaseDelta()
    {
        deltaStress = deltaStress * 2;
    }

}