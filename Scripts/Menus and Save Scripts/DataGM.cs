using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGM : MonoBehaviour
{
    public static string userName;
    public static int progressPoints;

    #region Player Controller Data...
    public static float player_MoveSpeed = 7.5f;
    public static int player_Jumps;
    public static float player_jumpVelocity = 10f;
    public static bool player_canDash;


    public static float[] playerSpawnZone = new float[3];
    public static int playerMoney;

    #endregion

    //public DataGM dm;

    private void Start()
    {
        //dm = this;
    }


    public static void SaveCurrentGameData()
    {
        //readOutCurrentData();
        DataStorage.SaveUserData();
        //print("DATA SAVED SUCCESSFULLY AT...");
        //DataStorage.ShowFileSavePath();
    }


    public static void LoadGameDataValuesToDataGM()
    {
        if(DataStorage.Check_For_Save_File())
        {
            //DataStorage.ShowFileSavePath();
            UserData data = DataStorage.LoadUserData();
            userName = data.USER_NAME;
            progressPoints = data.PROGRESS_POINTS;
            GM_loadPlayerControllerValues(data);

            playerSpawnZone = data.PLAYER_SpawnLoc;
            playerMoney = data.PLAYER_money;

        }
        else
        {
            Debug.LogWarning("[SAVE DATA: FAILED TO FIND GAME SAVE DATA].");
        }
        
    }

    /// <summary>
    /// Only used in this class just to clump all the controller stuff together...
    /// </summary>
    /// <param name="data"></param>
    private static void GM_loadPlayerControllerValues(UserData data)
    {
        player_MoveSpeed = data.PLAYER_moveSpeed;
        player_Jumps = data.PLAYER_Jumps;
        player_jumpVelocity = data.PLAYER_jumpVelocity;
        player_canDash = data.PLAYER_canDash;
    }


    #region Player Stuff...
    public static void PlayerControllerSave(PlayerControllerMain c)
    {
        player_MoveSpeed = c.speed;
        player_Jumps = c.amountOfJumpsAfterJumping;
        player_jumpVelocity = c.jumpVelocity;
        player_canDash = c.canDash;
    }

    public static void SetupSavedControllerValues(PlayerControllerMain controller)
    {
        LoadGameDataValuesToDataGM();
        controller.playerMoveSpeed = player_MoveSpeed;
        controller.speed = player_MoveSpeed;
        controller.amountOfJumpsAfterJumping = player_Jumps;
        controller.jumpVelocity = player_jumpVelocity;
        controller.canDash = player_canDash;
    }


    #endregion

    public static void addProgress(int additionalProgress)
    {
        progressPoints += additionalProgress;
       //readOutCurrentData();
    }


    public static void readOutCurrentData()
    {
        print("DATA--User Name:  " + userName);
        print("DATA--XP:  " + progressPoints);
    }



    public static void ResetValues()
    {
        progressPoints = 0;
        userName = null;
        player_MoveSpeed = 7.5f;
        player_Jumps = 0;
        player_jumpVelocity = 10f;
        player_canDash = false;
        playerSpawnZone[0] = 0f;
        playerSpawnZone[1] = 0f;
    }

}
