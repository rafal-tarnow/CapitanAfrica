using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLock : MonoBehaviour
{
    private static Sprite  lockButtonImage;
    private Sprite defaultButtonImage;

    private bool mlock = false;


    void Start()
    {
        lockButtonImage = Resources.Load<Sprite>("green_empty_lock_rectangle");

        if(defaultButtonImage == null)
            defaultButtonImage = this.GetComponent<Button>().GetComponent<Image>().sprite;
    }


    public void setLock(bool locked)
    {
        mlock = locked;

        if(mlock)
        {
            this.GetComponent<Button>().GetComponent<Image>().sprite = lockButtonImage;
        }
        else
        {
            this.GetComponent<Button>().GetComponent<Image>().sprite = defaultButtonImage;
        }
    }


    public bool getLock()
    {
        return mlock;
    }
}
