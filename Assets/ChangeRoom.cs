using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRoom : MonoBehaviour
{
    [SerializeField] List<Room> rooms;
    int index = 0;
    Text butText;

    private void Start()
    {
        foreach (Room r in rooms)
            r.room.SetActive(false);

        rooms[0].room.SetActive(true);
        index = 0;

        butText = GetComponentInChildren<Text>();
        butText.text = rooms[0].name;
    }

    public void NextRoom()
    {
        index = (index + 1) % rooms.Count;

        foreach (Room r in rooms)
            r.room.SetActive(false);

        rooms[index].room.SetActive(true);
        butText.text = rooms[index].name;
    }
}

[System.Serializable]
class Room
{
    public string name;
    public GameObject room;
}