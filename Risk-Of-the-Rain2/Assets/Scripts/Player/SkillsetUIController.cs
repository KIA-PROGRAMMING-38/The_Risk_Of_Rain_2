
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

    [SerializeField]
    Material _dashMaterial;
    public float initialAlpha = 1f;
    public float notReadyAlpha = -0.4f;
    public float offsetY;

    private void PlayDashUI()
    {
        if (CommandoController.DashCoolTime > CommandoController.DashElapsedTime)
        {

            _dashTMP.text =
                $"{(int)CommandoController.DashCoolTime - (int)CommandoController.DashElapsedTime}";


            UIShaderController.ControlBasicAttackShader(_dashMaterial, notReadyAlpha,
                Utils.Percent(CommandoController.DashElapsedTime, CommandoController.DashCoolTime) - 0.5f);
        }

        else
        {
            _dashTMP.text = string.Empty;
            _dashUI.color = initialColor;

            UIShaderController.ControlBasicAttackShader(_dashMaterial, initialAlpha, -0.5f);
        }
    }


    [SerializeField]
    Material _UltimateMaterial;

    private void PlayUltimateUI()
    {
        if (CommandoSkillSpawner.UltimateCoolTime > CommandoSkillSpawner.UltimateElapsedTime)
        {
            _ultimateUI.color = notReadyColor;
            _ultimateTMP.text =
                $"{(int)CommandoSkillSpawner.UltimateCoolTime - (int)CommandoSkillSpawner.UltimateElapsedTime}";

            UIShaderController.ControlBasicAttackShader(_UltimateMaterial, notReadyAlpha,
                Utils.Percent(CommandoSkillSpawner.UltimateElapsedTime, CommandoSkillSpawner.UltimateCoolTime) - 0.5f);
           
        }

        else
        {
            UIShaderController.ControlBasicAttackShader(_UltimateMaterial, initialAlpha, -0.5f);

            _ultimateTMP.text = string.Empty;
            _ultimateUI.color = initialColor;
        }


    }

}

public class UIShaderController
{
    public static void ControlBasicAttackShader(Material material, float alpha, float offsetY)
    {
       
        Vector4 offsetValue = new Vector4(0, offsetY, 0, 0);

        material.SetFloat(UIShaderParamID.EXTRA_ALPHA, alpha);
        material.SetVector(UIShaderParamID.MASK_OFFSET, offsetValue);
    }
}

