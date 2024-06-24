using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterPrefabs; // Mảng chứa các Prefab của các nhân vật
    public Transform spawnPoint; // Điểm xuất hiện của nhân vật
    private GameObject currentCharacter; // Biến lưu trữ nhân vật hiện tại

    public event Action<GameObject> OnChildObjectSpawned;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
            SelectCharacter(0);
        else
            SelectCharacter(PlayerPrefs.GetInt("selectedOption"));
    }
    private void Update()
    {
        OnChildObjectSpawned?.Invoke(currentCharacter);

    }
    // Phương thức chọn nhân vật
    public void SelectCharacter(int characterIndex)
    {
        // Kiểm tra xem chỉ số nhân vật có hợp lệ hay không
        if (characterIndex >= 0 && characterIndex < characterPrefabs.Length)
        {
            // Xóa nhân vật hiện tại nếu có
            if (currentCharacter != null)
            {
                Destroy(currentCharacter);
            }

            // Instantiate và hiển thị nhân vật mới
            currentCharacter = Instantiate(characterPrefabs[characterIndex], spawnPoint.position, Quaternion.identity);
            currentCharacter.transform.parent = this.transform;
        }
        else
        {
            Debug.LogWarning("Character index out of range.");
        }
    }
}
