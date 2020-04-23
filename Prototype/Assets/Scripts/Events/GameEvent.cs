using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent
{
    StartMatch,
    StartRound,
    EndRound,
    StartRedraft,
    AttackAbilitySelected,
    DefenseAbilitySelected,
    SpecialAbilitySelected,
    SpecialAbilityDeselected,
    DraftFinished,
    EndRedraft,
    KnockOut,
    ItemPickedUp,
    SpawnItem,
    ShieldDestroyed,
    NotEnoughMana
}
