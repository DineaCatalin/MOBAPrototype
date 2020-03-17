using UnityEngine;
using System.Collections;

public class RushAbility : Ability
{
    StateManager rushAreaManager;

    private void Start()
    {
        // Find and cache the GO of the RushArea
        rushAreaManager = GameObject.Find("RushAreaContainer" + playerID).GetComponent<StateManager>();

        // Get the layer the ability shoould be for this specific player
        // If player is in team 1 then the layer should be Team1Player and vice versa
        string layerName = LayerMask.LayerToName(GameObject.Find("Player" + playerID).layer);
        layerName = layerName.Replace("Player", "Ability");

        // Switch the layer to the layer of the abilities from the other team so that
        // the rush area will interact with our team mates and not with the enemy
        layerName = Utils.SwitchPlayerLayerName(layerName);

        // Set the layer of the rush area through the manager
        rushAreaManager.SetLayer(LayerMask.NameToLayer(layerName));

        // Deactivate so that the player doesn't start wthe
        // Rush ability activated
        rushAreaManager.Deactivate();
    }   

    public override bool Cast()
    {
        //rushAreaManager.Activate(abilityData.stats.duration);
        GameManager.Instance.ActivateRushArea(abilityData.stats.duration, playerID);
        abilityUI.ActivateCooldown();

        return base.Cast();
    }

}
