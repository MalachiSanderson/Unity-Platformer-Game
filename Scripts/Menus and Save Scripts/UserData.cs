using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// GAGE: A key aspect in our persistent user data storage process. 
/// Basically, this class just holds all the variable types you want to be saving. DOesn't mess with those values in anyway. Just a location to store them.
/// I learned these fundamentals of data management from Brackeys: https://www.youtube.com/watch?v=XOjd_qU2Ido
/// </summary>
public class UserData 
{
    public string USER_NAME;
    public int PROGRESS_POINTS;

    #region Player Controller Data...
    public float PLAYER_moveSpeed;
    public int PLAYER_Jumps;
    public float PLAYER_jumpVelocity;
    public bool PLAYER_canDash;
    public int PLAYER_money;

    public float[] PLAYER_SpawnLoc = new float[3];

    #endregion

    /// <summary>
    /// GAGE: Put stuff you want saved and how/where it will get the information in here. 
    /// </summary>
    public UserData()
    {
        USER_NAME = DataGM.userName;
        PROGRESS_POINTS = DataGM.progressPoints;

        PLAYER_moveSpeed = DataGM.player_MoveSpeed;
        PLAYER_Jumps = DataGM.player_Jumps;
        PLAYER_jumpVelocity = DataGM.player_jumpVelocity;
        PLAYER_canDash = DataGM.player_canDash;
        PLAYER_SpawnLoc = DataGM.playerSpawnZone;
        PLAYER_money = DataGM.playerMoney;

    }






}
