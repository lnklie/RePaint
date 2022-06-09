using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : KJY
 * 파일명 : SaveData.cs
==============================
*/
public class Settings : MonoBehaviour
{
    private List<Resolution> resolutions = new List<Resolution>();
    private int resolutionNum = default;
    private FullScreenMode fullScreenMode;

    [SerializeField] 
    private Dropdown resolutionsDropdown = null;
    [SerializeField] 
    private Toggle fullScreenToggle = null;
    [SerializeField] 
    private Slider brightnessSlider = null;
    [SerializeField] 
    private Slider BackGroundVolume = null;
    [SerializeField] 
    private Slider EffectVolume = null;

    private void Start()
    {
        InitUI();
    }

    private void Update()
    {
        Brightness();
      
    }

    public void SoundControl(int _num)
    {
        if(_num == (int)Sound.BACKGROUND)
            GameManager.SoundManager.SetSoundOption(Sound.BACKGROUND, BackGroundVolume.value);
        else if(_num == (int)Sound.EFFECT)
            GameManager.SoundManager.SetSoundOption(Sound.EFFECT, EffectVolume.value);
        else if(_num == (int)Sound.EFFECTGROUND)
            GameManager.SoundManager.SetSoundOption(Sound.EFFECTGROUND, EffectVolume.value);
    }

    private void InitUI()
    {
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate > 55 && Screen.resolutions[i].refreshRate < 65)
                resolutions.Add(Screen.resolutions[i]);
        }
        resolutionsDropdown.options.Clear();

        int optionNum = 0;

        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " x " + item.height + " ";
            resolutionsDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionsDropdown.value = optionNum;

            optionNum++;
        }
        resolutionsDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropBoxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ResolutionAccept()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, fullScreenMode);
    }

    public void Brightness()
    {
        RenderSettings.ambientIntensity = brightnessSlider.value;
    }
    
}
