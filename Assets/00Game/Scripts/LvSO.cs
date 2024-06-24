using System.Threading;
using UnityEngine;
[CreateAssetMenu(menuName = "SO", fileName = "LvSO")]
public class LvSO : ScriptableObject
{
    [SerializeField] GameObject[] lvs;
    static LvSO instance;
    public int currentLv;
    public static LvSO Instance
    {
        get
        {
            if (instance == null) 
                Setup();
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    static void Setup()
    {
        instance = LoadSource.LoadObject<LvSO>("LvSO");
    }
    public GameObject GetLv(int id)
    {
        currentLv = id;
        return lvs[id];
    }
   

}