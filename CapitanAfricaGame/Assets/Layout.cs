using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Layout :  MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform nextButton;
    public RectTransform menuButton;
    public RectTransform repeatButton;

    public RectTransform textLvlCompleteRt;

    private RectTransform transform;

    float width;
    float height;

    void Start()
    {
        if(transform == null)
            transform = GetComponent<RectTransform>();

    }

    float calcYPos(float procent)
    {
        return height*procent - height/2.0f;
    }

    float calcXPos(float procent)
    {
        return width*procent - width/2.0f;
    }

    float calcWidth(float procent)
    {
        return width*procent;
    }

    float calcHeight(float procent)
    {
        return height*procent;
    }

    #warning Read how to update sizez only on rezise changed
    void Update()
    {
        width = transform.rect.width;
        height = transform.rect.height;

        repeatButton.localPosition = new Vector3(calcXPos(0.2f),calcYPos(0.2f),0);
        menuButton.localPosition = new Vector3(calcXPos(0.5f),calcYPos(0.2f),0);
        nextButton.localPosition = new Vector3(calcXPos(0.8f),calcYPos(0.2f),0);

        repeatButton.sizeDelta = new Vector2 (calcWidth(0.2f), calcHeight(0.2f));
        menuButton.sizeDelta = new Vector2 (calcWidth(0.2f), calcHeight(0.2f));
        nextButton.sizeDelta = new Vector2 (calcWidth(0.2f), calcHeight(0.2f));

        textLvlCompleteRt.localPosition = new Vector3(calcXPos(0.5f),calcYPos(0.9f), 0);
        textLvlCompleteRt.sizeDelta = new Vector2 (calcWidth(0.9f), calcHeight(1.0f));
    }
}
