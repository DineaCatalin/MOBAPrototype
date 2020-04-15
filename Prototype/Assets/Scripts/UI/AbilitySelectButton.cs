using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonType
{
    AttackAbility,
    DefenseAbility,
    SpecialAbility
}

public class AbilitySelectButton : MonoBehaviour
{
    [SerializeField] ButtonType buttonType;

    [SerializeField] GameObject border;
    [SerializeField] TextMeshProUGUI buttonText;

    Button button;

    private void Start()
    {
        button = GetComponentInChildren<Button>();

        EventManager.StartListening(GameEvent.AttackAbilitySelected, new Action(OnAttackAbilityPressed));
        EventManager.StartListening(GameEvent.DefenseAbilitySelected, new Action(OnDefenseAbilityPressed));
        EventManager.StartListening(GameEvent.SpecialAbilitySelected, new Action(OnSpecialAbilityPressed));
        EventManager.StartListening(GameEvent.SpecialAbilityDeselected, new Action(OnSpecialAbilityPressed));
    }

    public void ActivateBorder()
    {
        border.SetActive(true);
    }

    public void DisableBorder()
    {
        border.SetActive(false);
    }

    public void OnClick()
    {
        Debug.Log("AbilitySelectButton OnClick Ability name is " + name);

        switch (buttonType)
        {
            case ButtonType.AttackAbility:
                {
                    if (AbilityDraftLogic.Instance.SelectAttackAbility(name))
                    {
                        EventManager.TriggerEvent(GameEvent.AttackAbilitySelected);
                        ActivateBorder();
                    }

                    break;
                }
            case ButtonType.DefenseAbility:
                {
                    if (AbilityDraftLogic.Instance.SelectDefenseAbility(name))
                    {
                        EventManager.TriggerEvent(GameEvent.DefenseAbilitySelected);
                        ActivateBorder();
                    }

                    break;
                }
            case ButtonType.SpecialAbility:
                {
                    if (AbilityDraftLogic.Instance.SelectSpecialbility(name))
                    {
                        EventManager.TriggerEvent(GameEvent.SpecialAbilitySelected);
                        ActivateBorder();
                    }

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void OnAttackAbilityPressed()
    {
        if (buttonType == ButtonType.AttackAbility)
        {
            DisableBorder();
        }
    }

    void OnDefenseAbilityPressed()
    {
        if (buttonType == ButtonType.DefenseAbility)
        {
            DisableBorder();
        }
    }

    void OnSpecialAbilityPressed()
    {

        if(!AbilityDraftLogic.Instance.AbilityIsSelected(name) &&
            buttonType == ButtonType.SpecialAbility)
        {
            Debug.Log("AbilitySelectButton OnSpecialAbilityPressed disabling border for " + name);
            DisableBorder();
        }
    }
}
