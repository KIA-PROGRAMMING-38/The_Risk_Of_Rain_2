
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _inPlayUI;
    [SerializeField]
    private Slider sliderHP;

    [SerializeField]
    private TextMeshProUGUI textHP;

    [SerializeField]
    private TextMeshProUGUI secondTextHP;

    private string title = "Distant Roost";
    private string subtitle = "Ground Zero";
    private string escapeText = "E Exit Escape Pod";
    private string spawnBossText = "E Spawn Boss";
    private readonly int MIL_TO_SEC = 1000;
    [SerializeField]
    private GameObject escapeTMP;

    [SerializeField]
    private GameObject bossSpawnTMP;

    [SerializeField]
    private TextMeshProUGUI titleTMP;

    [SerializeField]
    private TextMeshProUGUI subtitleTMP;
    public float exitUIDelaySec;
    public float _interval;
    public float _titleShowingDelay;
    public float _titleShowingDuration;
    private bool isTitlePrinted;
    // Update is called once per frame

    private CancellationTokenSource _source = new();
    private CancellationTokenSource _exitSource = new();

    [Header("Reference")]
    [SerializeField]
    CommandoController commandoController;

    private void Start()
    {
        _inPlayUI.SetActive(false);

        titleTMP.text = string.Empty;
        subtitleTMP.text = string.Empty;
        escapeTMP.SetActive(false);
        bossSpawnTMP.SetActive(false);
        PrintTitle(_source.Token);
        PlayExitText(_exitSource.Token);


    }
    void Update()
    {
        if (GameManager.IsGameStarted == true)
        {
            _inPlayUI.SetActive(true);

            escapeTMP.SetActive(false);
            _exitSource.Cancel();
            _source.Cancel();

        }




        if (GameManager.IsPlayerArrived == true)
        {

        }

        if (sliderHP != null) sliderHP.value = Utils.Percent(commandoController.Hp, commandoController.commandoMaxHp);
        if (textHP != null) textHP.text = $"{commandoController.Hp} / {commandoController.commandoMaxHp}";
        if (textHP != null) secondTextHP.text = $"{commandoController.Hp} / {commandoController.commandoMaxHp}";
    }

  
    async private UniTaskVoid PlayExitText(CancellationToken cancellationToken)
    {
       

        await UniTask.Delay((int)(exitUIDelaySec * MIL_TO_SEC));
        escapeTMP.SetActive(true);

    }

    public void ShowBossSpawnableMessage(bool spawnable)
    {
       
            bossSpawnTMP.SetActive(spawnable);
     
    }
    async private UniTaskVoid PrintTitle(CancellationToken cancellationToken)
    {


        Debug.Log("title is showing : unitask");
        if (!isTitlePrinted)
        {
            isTitlePrinted = true;
            await UniTask.Delay((int)_titleShowingDelay * MIL_TO_SEC);

            for (int i = 0; i < title.Length; i++)
            {
                titleTMP.text += title[i];
                await UniTask.Delay((int)(_interval * MIL_TO_SEC));
            }

            for (int i = 0; i < subtitle.Length; i++)
            {
                subtitleTMP.text += subtitle[i];
                await UniTask.Delay((int)(_interval * MIL_TO_SEC));
            }


            await UniTask.Delay((int)(_titleShowingDuration * MIL_TO_SEC));

            while (subtitleTMP.text.Length > 0)
            {
                // remove the last character
                subtitleTMP.text = subtitleTMP.text.Substring(0, subtitleTMP.text.Length - 1);
                await UniTask.Delay((int)(_interval * MIL_TO_SEC)); // convert to milliseconds
            }


            while (titleTMP.text.Length > 0)
            {
                // remove the last character
                titleTMP.text = titleTMP.text.Substring(0, titleTMP.text.Length - 1);
                await UniTask.Delay((int)(_interval * MIL_TO_SEC)); // convert to milliseconds
            }

            isTitlePrinted = true;
        }


    }
}


public class Utils
{
    public static float Percent(float current, float max)
    {
        return current != 0 && max != 0 ? current / max : 0;
    }
}
