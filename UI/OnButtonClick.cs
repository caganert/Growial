using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonClick : MonoBehaviour
{
    public Sprite newImage;
    public Button button;

    public void ChangeButtonImage()
    {
        button.image.sprite = newImage;
    }
}
