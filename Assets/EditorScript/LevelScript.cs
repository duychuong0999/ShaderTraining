using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelScript : MonoBehaviour
{
    public int experience;

    public int Level
    {
        get { return experience / 750; }
    }

    public void Action()
    {
        Debug.Log(Level);
    }

    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        Debug.Log("ClearData");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/New Option %p")]
    private static void NewMenuOption2()
    {
        Debug.Log("ClearData2");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Assets/Create/Add Configuration")]
    private static void AddConfig()
    {
        Debug.Log("Add Configuration");
    }


    [MenuItem("CONTEXT/Rigidbody/New Option")]
    private static void NewOpenForRigidBody()
    {
        Debug.Log("Rb option");
    }

    [ContextMenuItem("Randomize Name", "Randomize")]
    public string Name;

    private void Randomize()
    {
        Name = "Some Random Name";
    }
}
