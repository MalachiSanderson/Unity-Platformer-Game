using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSFX : MonoBehaviour
{
    public static AllSFX sfx;
    public AudioClip DeathSound;
    public AudioClip CoinCollectedSound;
    public AudioClip DashSound;
    public AudioClip DoubleJumpSound;
    public AudioClip PlayerStrikeSound;
    public AudioClip BuzzSawSound;
    public AudioClip StrikingEnemySound;
    public AudioClip HealingSound;
    public AudioClip UpgradeSound;
    //public AudioClip
    //public AudioClip

    public static AudioClip getDeathSound()
    {
        return sfx.DeathSound;
    }

    public static AudioClip getCoinCollectedSound()
    {
        return sfx.CoinCollectedSound;
    }

    public static AudioClip getDashSound()
    {
        return sfx.DashSound;
    }

    public static AudioClip getDoubleJumpSound()
    {
        return sfx.DoubleJumpSound;
    }

    public static AudioClip getPlayerStrikeSound()
    {
        return sfx.PlayerStrikeSound;
    }

    public static AudioClip getBuzzSawSound()
    {
        return sfx.BuzzSawSound;
    }

    public static AudioClip getStrikingEnemySound()
    {
        return sfx.StrikingEnemySound;
    }

    public static AudioClip getHealingSound()
    {
        return sfx.HealingSound;
    }

    public static AudioClip getUpgradeSound()
    {
        return sfx.UpgradeSound;
    }

    void Start()
    {
        findAudioManager();
    }

    public void findAudioManager()
    {
        if (sfx == null)
        {
            sfx = GetComponentInParent<AllSFX>();
        }
    }

}
