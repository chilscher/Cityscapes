//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinApplicator : MonoBehaviour {

    public List<Image> menuButtonInsides;
    public List<Image> menuButtonBorders;
    public List<Text> settingsOnTexts;
    public List<Text> settingsOffTexts;
    public List<Image> popupInsides;
    public List<Image> popupBorders;
    
    public void ApplySkin(Skin skin){
        foreach (Image im in menuButtonInsides)
            im.color = skin.menuButtonInside;
        foreach (Image im in menuButtonBorders)
            im.color = skin.menuButtonBorder;
        foreach (Text txt in settingsOnTexts)
            txt.color = skin.settingsText_On;
        foreach (Text txt in settingsOffTexts)
            txt.color = skin.settingsText_Off;
        foreach (Image im in popupInsides)
            im.color = skin.popupInside;
        foreach (Image im in popupBorders)
            im.color = skin.popupBorder;
    }
}
