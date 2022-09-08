using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RadialMenuEntry : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI Label;
    [SerializeField]
    RawImage Icon;
    MenuSystem.Screen link;

    public void SetLabel(string labelText)
    {
        Label.text = labelText;
    }


    public void SetIcon(Texture texture)
    {
        Icon.texture = texture;
    }

    public RawImage GetIcon()
    {
        return Icon;
    }
    public void SetLink(MenuSystem.Screen pLink)
    {
        link = pLink;
    }

    public MenuSystem.Screen GetLink()
    {
        return link;
    }

}


