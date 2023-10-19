using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowSliderPercentage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text percentText;
    [SerializeField] private Slider slider;


    void Start()
    {
        percentText.gameObject.SetActive(false);
        slider.value = PlayerPrefs.GetFloat(slider.name, 1f);
    }



    
    public void ShowSliderPercent()
    {
        percentText.text = (int)((slider.value/(slider.maxValue))*100) + "%";
        PlayerPrefs.SetFloat(slider.name, slider.value);
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        percentText.gameObject.SetActive(false);

    }

}
