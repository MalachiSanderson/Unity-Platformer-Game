using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// GAGE: A key aspect in our persistent user data storage process. 
/// I learned these fundamentals of data management from Brackeys: https://www.youtube.com/watch?v=XOjd_qU2Ido
/// </summary>
public static class DataStorage
{
    public static string fileSaveLocation = Application.persistentDataPath + "/Saved Data/Platformer_PlayerData.ratgrub";

    public static void SaveUserData()
    {
        if(IsItBigDaddyMatty()) //Ignore this lol, its a dumb thing I'm testing it shouldn't have any effect on anything.
        {
            //MattyFileSave();
        }
        else ///NORMAL SAVE METHOD
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory(Application.persistentDataPath + "/Saved Data");
            FileStream stream = new FileStream(fileSaveLocation, FileMode.Create); //FileMode.OpenOrCreate

            UserData data = new UserData();

            formatter.Serialize(stream, data);
            stream.Close();
        }
        
    }


    public static UserData LoadUserData()
    {
        if(File.Exists(fileSaveLocation))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fileSaveLocation, FileMode.Open);
            
            UserData data = formatter.Deserialize(stream) as UserData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("[LOADING ERROR: NO FILE EXISTS IN THE PATH:  ' "+ fileSaveLocation + " ' ]");
            return null;
        }
    }


    public static void DELETE_SAVED_DATA()
    {
        DataGM.ResetValues();
        MonoBehaviour.print("[WARINING!!!! ERASING ALL SAVED DATA AND RESETTING TO INITIAL VALUES]");
        File.Delete(fileSaveLocation);
    }

    public static bool Check_For_Save_File()
    {
        bool isThereSaveData = false;

        if (File.Exists(fileSaveLocation))
        {
            isThereSaveData = true;
        }
        return isThereSaveData;
    }


    public static void ShowFileSavePath()
    {
        MonoBehaviour.print(System.Environment.UserName);
        MonoBehaviour.print(fileSaveLocation);
    }



    public static void SeriouslyDeleteAllSaveFiles()
    {
        //string path = Application.persistentDataPath + "/Platformer_PlayerData.ratgrub";
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath + "/Saved Data");
        directory.Delete(true);
        Directory.CreateDirectory(fileSaveLocation);
    }


    //**************************************************************************************

    /// <summary>
    /// GAGE: Ignore this, this is just a fun experiment I'm working on ;)
    /// </summary>
    public static void DeleteMattysRatGrub()
    {
        string marty = System.Environment.UserName;
        string rat = @"C:\Users\Mo'Jama\Pictures\Saved Pictures\ratgrub\ratgrub.exe";
        MonoBehaviour.print("[WARINING!!!! MURDERING RAT GOD]");
        File.Delete(rat);
    }

    /// <summary>
    /// GAGE: Ignore this, this is just a fun experiment I'm working on ;)
    /// </summary>
    public static bool IsItBigDaddyMatty()
    {
        /*
        //string zoom = @"C:\Users\";
        bool isMatty = false;
        if (System.Environment.UserName.Contains("Mo'Jama"))
        {
            fileSaveLocation = @"C:\Users\Mo'Jama\Pictures\Saved Pictures\ratgrub\ratgrub.POG";
            isMatty = true;
        }
        else
        {
            fileSaveLocation = Application.persistentDataPath + "/NS_UserData.ratgrub";
        }
        return isMatty;
        */
        return false;
    }

    public static void MattyFileSave()
    {
        /*
        string ratZone = @"C:\Users\Mo'Jama\Pictures\Saved Pictures\ratgrub\ratgrub.exe";

        if (File.Exists(ratZone))
        {
            // DeleteMattysRatGrub();
            MonoBehaviour.print("[FOUNND MATTY'S RATGRUB]");
        }

        string nuRatCore = @"C:\Users\Mo'Jama\Pictures\Saved Pictures\ratgrub\ratgrub.POG";

        fileSaveLocation = nuRatCore;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(fileSaveLocation, FileMode.Create); //FileMode.OpenOrCreate

        UserData data = new UserData();

        formatter.Serialize(stream, data);
        stream.Close();
        MonoBehaviour.print("[WARINING!!!! YOU ARE MATTY]");
        */
    }

}
