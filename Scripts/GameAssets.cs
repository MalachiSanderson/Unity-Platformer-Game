using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    //some god damn retard made this shitty script necessacary for the damage popups.
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("Game Assets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }


    [Tooltip("The Text that pops up when player damages something.")]
    public Transform prefabDamagePopup;
    [Tooltip("The Text that pops up when something damages player.")]
    public Transform playerDamagePopup;
    [Tooltip("The Text that pops up in the world similar to damage text.")]
    public Transform genericWorldPopupText;
    public Transform upgradeTextPopup;
    public Transform tipsTextPopup;
    public Transform coinObject;
    public PhysicsMaterial2D frictionlessMaterial;
    public PhysicsMaterial2D highFrictionMaterial;
    public Transform bodySwapParticleEffect;
    public Transform upgradeCollectedParticleEffect;
}
