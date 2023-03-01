using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Groups")]
    public GameObject mainMenuHolder;
    public GameObject playMenuHolder;

    [Header("Scene Stuff")]
    
    private static string sceneOneName = "Test Level";
    private static string mainMenuName = "Main Menu";
    public enum GameScenes { MainMenu, TestScene1 };
    public GameScenes gameSceneToLoad;

    [Header("LOAD SAVE STUFF")]
    public InputField userNameInput;
    public Text userNameText;
    public Text progressMeterText;
    private Slider progress_bar;

    public static int maxProgress = 100;

    //private static CrossSceneData InfoObj;

    private void Start()
    {
        Time.timeScale = 1f;
        progress_bar = progressMeterText.gameObject.GetComponentInChildren<Slider>();
        progress_bar.maxValue = maxProgress;

        if (DataStorage.Check_For_Save_File())
        {
            print("SAVE GAME FOUND IN MAIN MENU START");
            DataGM.LoadGameDataValuesToDataGM();
            userNameText.text = DataGM.userName;
            mainMenuHolder.SetActive(true);
            userNameInput.gameObject.SetActive(false);
            progressValueUpdate();
        }
        else
        {
            print("SAVE GAME NOT FOUND IN MAIN MENU START");
            userNameText.text = "";
            mainMenuHolder.SetActive(false);
            userNameInput.gameObject.SetActive(true);
        }
    }

    #region Button Methods...

    public void ContinueGame()
    {
        determineWhichSceneToLoad();
        try
        {
            LoadGameScene(GameScenes.TestScene1);
        }
        catch(UnityException e)
        {
            Debug.LogError("[ERROR: '"+ sceneOneName + "' was not found in the list of scenes in the game build!]");
            print(e);
        }
       
    }

    public void NewGame()
    {
       
    }

    /// <summary>
    /// Called when the quit button is pressed in the main menu. 
    /// </summary>
    public void Quit()
    {
        GameMaster.CloseApplication();
    }


    /// <summary>
    /// GAGE: Just a basic confirm button that sends data through, saves it and reloads the menu.
    /// </summary>
    public void ConfirmUserName_BTN()
    {
        if (userNameInput.text != null)
        {

            DataGM.userName = userNameInput.text;
            print("Name set to Username: " + DataGM.userName);
            DataGM.progressPoints = 0;
            DataGM.SaveCurrentGameData();
            progressValueUpdate();
            if (DataStorage.Check_For_Save_File())
            {
                userNameInput.gameObject.SetActive(false);
                DataGM.LoadGameDataValuesToDataGM();
                progressValueUpdate();
            }
            else
            {
                userNameInput.gameObject.SetActive(true);
            }
            userNameInput.gameObject.SetActive(false);
            LoadMainMenuScene();
        }
    }


    /// <summary>
    /// GAGE: This deletes the file specified to be a user's save data and reloads scene when called.
    /// (Only called by clicking the button that is the User's name itself).
    /// </summary>
    public void DeleteSaveData_BTN()
    {
        DataStorage.DELETE_SAVED_DATA();
        LoadMainMenuScene();

    }

    #endregion

    /// <summary>
    /// GAGE: Literally just here to make sure the progress meter and data is updated
    /// </summary>
    private void progressValueUpdate()
    {
        progress_bar.value = DataGM.progressPoints;
        progress_bar.maxValue = maxProgress;
        progressMeterText.text = "Progress: " + DataGM.progressPoints + " / " + maxProgress;
        if (GameObject.Find("Slider Fill") != null)
        {
            if (DataGM.progressPoints >= 0.8 * maxProgress)
            {
                GameObject.Find("Slider Fill").GetComponentInChildren<Image>().color = Color.yellow;
            }
            else
            {
                GameObject.Find("Slider Fill").GetComponentInChildren<Image>().color = Color.green;
            }
        }
        else
        {
            Debug.LogWarning("Failed to find exp bar fill");
        }

    }


    /// <summary>
    /// Just calls DataGM.addProgress() then updates values on main menu HUD.
    /// </summary>
    /// <param name="prog"> how much progress you want to add.</param> 
    private void addProgress(int prog)
    {
        DataGM.addProgress(prog);
        progressValueUpdate();
    }

    #region Scene Loading Methods...

    /// <summary>
    /// GAGE: Static call for easy loading of main menu scene.
    /// </summary>
    public static void LoadMainMenuScene()
    {
        LoadGameScene(GameScenes.MainMenu);
    }

    /// <summary>
    /// Gage: this method just searches for what "scene" is tied to what "scene name" and 
    /// returns the correct name as a string.
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private static string getSceneName(GameScenes scene)
    {
        string sceneName = null;
        switch(scene)
        {
            case GameScenes.MainMenu:
                sceneName = mainMenuName;

                break;

            case GameScenes.TestScene1:
                sceneName = sceneOneName;

                break;
        }
        return sceneName;
    }
    /// <summary>
    /// Gage: A basic static method any script can call that loads a scene the user specifies.
    /// </summary>
    /// <param name="scene">GameScenes is an enum defined in MainMenuManager </param>
    public static void LoadGameScene(GameScenes scene)
    {
        SceneManager.LoadScene(getSceneName(scene));

        //SceneManager.MoveGameObjectToScene(crossDataObj, SceneManager.GetSceneAt(1));
    }

    /// <summary>
    /// [TODO] Gage: This method should help decide which scene should be loaded based on the context 
    /// such as what level was the player last in and stuff...
    /// </summary>
    private void determineWhichSceneToLoad()
    {
        print("NOTE: Currently the game just has it hardcoded which levels to load and understands no context or additional info. Need to finish this method so it does this stuff.");
    }


    #endregion
}
