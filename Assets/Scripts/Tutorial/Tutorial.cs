using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private Waypoint curWaypoint;
    [SerializeField] TMP_Text taskText;
    [SerializeField] GameObject waypointUI;
    [TextArea(5, 3)] private string tutorialTxt;
    int currTaskIndex = 0;
    private string[] tasks = { "Press A or D to move right and left",
        "Now go to the Tree of life,you must deffend it at all cost! its the source of all life and magic in the forest! \n (ITS THE BIG PINK TREE BRUH)",
        "Press E on the building spot to build Hatchery",
        "press E while standing on the hatchery to recruit Ammiga","Build up an Archery",
        "Recruit a archer by pressing E on your archery \n In order to recruit an archer make sure you have enough seeds and avilable Unemplyed Amigga" };

    private void Start()
    {
        curWaypoint = FindAnyObjectByType<Waypoint>();
        TaskUpdate(currTaskIndex);
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        while(!(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D)))//Task 1
        {
            yield return new WaitForEndOfFrame();
        }
        currTaskIndex++;
        TaskUpdate(currTaskIndex);
        waypointUI.SetActive(true);
        while (!curWaypoint.playerInRange)//task 2
        {
            yield return null;
        }
        waypointUI.SetActive(false);
        taskText.text = "GoodJob! Now remember without this tree evreything will die in your forest \n you can always upgrade it and make it stronger";
        yield return new WaitForSeconds(5);
        currTaskIndex++;
        TaskUpdate(currTaskIndex);

    }

    private void TaskUpdate(int taskIndex)
    {
        taskText.text = tasks[taskIndex];
    }
}
