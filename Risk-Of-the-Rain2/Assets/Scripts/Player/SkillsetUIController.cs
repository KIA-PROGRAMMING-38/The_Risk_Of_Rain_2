
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsetUIController : MonoBehaviour
{

    [SerializeField]
    private Color notReadyColor;
    [SerializeField]
    private Color initialColor;


    [SerializeField]
    private Image _ultimateUI;
    [SerializeField]
    private TextMeshProUGUI _ultimateTMP;

    [SerializeField]
    private Image _dashUI;
    [SerializeField] 
    private TextMeshProUGUI _dashTMP;



    private void Update()
    {
        PlayDashUI();
        PlayUltimateUI();
    }

    private void PlayDashUI()
    {
        if(CommandoController.DashCoolTime > CommandoController.DashElapsedTime) 
        {
            _dashUI.color = notReadyColor;
            _dashTMP.text =
                $"{(int)CommandoController.DashCoolTime - (int)CommandoController.DashElapsedTime}";
        }

        else 
        {
            _dashTMP.text = string.Empty;
            _dashUI.color = initialColor;
        }
    }

    private void PlayUltimateUI()
    {
        if (CommandoSkillSpawner.UltimateCoolTime > CommandoSkillSpawner.UltimateElapsedTime)
        {
            _ultimateUI.color = notReadyColor;
            _ultimateTMP.text =
                $"{(int)CommandoSkillSpawner.UltimateCoolTime - (int)CommandoSkillSpawner.UltimateElapsedTime}";
        }

        else
        {
            _ultimateTMP.text = string.Empty;
            _ultimateUI.color = initialColor;
        }

        
    }

}
