
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

using System.Threading;
using UniRx;
using System;
using UnityEngine.Animations;

public class GolemHpUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _slider;

    [SerializeField]
    private GameObject _hpPart;
    [SerializeField]
    private Image _hpFill;

    [SerializeField]
    private Color initialColor;
    [SerializeField]
    private Color DamangedColor;
    [SerializeField]
    private Color DyingColor;

   
    private Slider sliderHP;

    private Golem_Controller _golemController;

    public float hpBarDuration;
    private bool isonDamaged;
    CancellationTokenSource _damageSouce = new();

    void Awake()
    {
        _golemController = GetComponentInParent<Golem_Controller>();
        sliderHP = GetComponentInChildren<Slider>();
        
        _slider.SetActive(false);
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
        _slider.SetActive(true);

        if (sliderHP != null)
        {

            if (_golemController.hp != 0)
            {
                sliderHP.value = Utils.Percent(_golemController.HP, _golemController.maxHp);
                if (sliderHP.value < 0.8 && sliderHP.value > 0.2)
                {
                    _hpFill.color = DamangedColor;
                }
                else if (sliderHP.value < 0.2f)
                {
                    _hpFill.color = DyingColor;
                }

               
            }
            if (_golemController.hp < 0)
            {
                _hpPart.SetActive(false);
            }

        }

        await UniTask.Delay((int)(hpBarDuration * 2000), cancellationToken: _damageSouce.Token);

        isonDamaged = false;

        if (_golemController.onDamaged == false && isonDamaged == false)
        {
            _slider.SetActive(false);
        }
        else
        {
            _damageSouce.Cancel();
        }

    }



}
