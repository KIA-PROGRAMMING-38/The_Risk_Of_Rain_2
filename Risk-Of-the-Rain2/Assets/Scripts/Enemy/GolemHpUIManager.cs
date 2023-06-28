
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

using System.Threading;
using UniRx;
using System;
using UnityEngine.Animations;

public class GolemHpUIManager : MonoBehaviour
{

  

   

 
    private GameObject _hpPart;

    private Image _hpFill;

   
    private Image backGround;

    [SerializeField]
    private Color initialColor;
    [SerializeField]
    private Color DamangedColor;
    [SerializeField]
    private Color DyingColor;


    private Slider slider;

    private Golem_Controller _golemController;

    public float hpBarDuration;
    private bool isonDamaged;
    CancellationTokenSource _damageSouce = new();

    private GameObject sliderObject;


    private CancellationTokenSource _cancelTokenSource;
    private CancellationTokenSource _playTokenSource;
    private CancellationToken _canceltoken;
    private void Start()
    {
        _cancelTokenSource = new CancellationTokenSource();
        _playTokenSource = new CancellationTokenSource();
        _cancelTokenSource.Cancel();
        _canceltoken = _playTokenSource.Token;

    }
   
    private void OnEnable()
    {

        _golemController = GetComponentInParent<Golem_Controller>();
        slider = GetComponentInChildren<Slider>();

         sliderObject = transform.Find("Golem HP Slider")?.gameObject;

     
        Transform hpColorTransform = transform.GetChild(0).GetChild(1).GetChild(0);
        _hpFill = hpColorTransform.GetComponent<Image>();

        _hpFill.color = initialColor;
    }

    void Update()
    {

        if (_golemController.onDamaged)
        {
            isonDamaged = true;
            ShowHpBar();
        }


    }

    async private UniTaskVoid ShowHpBar()
    {

        while (true)
        {
            sliderObject.SetActive(true);


            slider.value = Utils.Percent(_golemController.hp, _golemController.maxHp);

            if (slider.value < 0.8 && slider.value > 0.2)
            {
                _hpFill.color = DamangedColor;
            }

            else if (slider.value < 0.3)
            {
                _hpFill.color = DyingColor;
            }



            await UniTask.Delay((int)(hpBarDuration * 2000), cancellationToken: _damageSouce.Token);

            isonDamaged = false;

            if (_golemController.onDamaged == false && isonDamaged == false)
            {
                sliderObject.SetActive(false);
            }

            await UniTask.Yield(_canceltoken);
          
        }

    }



}
