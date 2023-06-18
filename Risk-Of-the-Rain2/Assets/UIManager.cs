using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Slider sliderHP;

    [SerializeField] 
    private TextMeshProUGUI textHP;

    [SerializeField]
    private TextMeshProUGUI secondTextHP;

    // Update is called once per frame
    void Update()
    {
        if (sliderHP != null) sliderHP.value = Utils.Percent(CommandoController.Hp, CommandoController.commandoMaxHp);
      
        if (textHP != null) textHP.text = $"{CommandoController.Hp} / {CommandoController.commandoMaxHp}";
        if (textHP != null) secondTextHP.text = $"{CommandoController.Hp} / {CommandoController.commandoMaxHp}";
    }
}


public class Utils
{
    public static float Percent(float current, float max)
    {
        return current != 0 && max != 0 ? current / max : 0;
    }
}