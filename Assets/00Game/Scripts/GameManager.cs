using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    GameObject LevelObject;
    public bool isGameOver;
    void Start()
    {
        LevelObject = Instantiate(LvSO.Instance.GetLv(0), transform);
    }
    //xem lai
    public void NextLevel()
    {
        if (LvSO.Instance.currentLv < 4)
        Destroy(LevelObject);
        LvSO.Instance.currentLv++;
        LevelObject = Instantiate(LvSO.Instance.GetLv(LvSO.Instance.currentLv), transform);
    }
    public void GameOver()
    {
        isGameOver = true;
    }

}


