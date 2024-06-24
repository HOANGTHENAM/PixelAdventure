using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform selectedCharacter; // Đối tượng cha (selectedcharacter)
    private Transform player; // Đối tượng con (player) của selectedcharacter
    public float offsetX = 0f; // Khoảng cách giữa camera và nhân vật theo trục x
    public float minX = 0.36f; // Giới hạn x tối thiểu
    public float maxX = 16.3f; // Giới hạn x tối đa

    void Start()
    {
        if (selectedCharacter == null)
        {
            Debug.LogWarning("Selected Character is not assigned.");
            return;
        }

        // Đăng ký sự kiện lắng nghe khi có GameObject con được sinh ra
        selectedCharacter.GetComponent<CharacterSelection>().OnChildObjectSpawned += HandleChildObjectSpawned;
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player object not found.");
            return;
        }

        // Lấy vị trí hiện tại của camera
        Vector3 newPosition = transform.position;

        // Tính toán vị trí x mới của camera dựa trên vị trí x của player và offset
        float targetX = player.position.x + offsetX;

        // Giới hạn vị trí x của camera trong khoảng minX và maxX
        targetX = Mathf.Clamp(targetX, minX, maxX);

        // Cập nhật vị trí mới của camera (giữ nguyên trục y và z)
        newPosition.x = targetX;


        // Cập nhật vị trí của camera
        transform.position = newPosition;
    }

    // Phương thức được gọi khi có GameObject con được sinh ra
    private void HandleChildObjectSpawned(GameObject childObject)
    {
        player = childObject.transform;
    }
    void OnDestroy()
    {
        // Đừng quên hủy đăng ký hàm khi không cần thiết nữa để tránh rò rỉ bộ nhớ
        selectedCharacter.GetComponent<CharacterSelection>().OnChildObjectSpawned -= HandleChildObjectSpawned;
    }
}
