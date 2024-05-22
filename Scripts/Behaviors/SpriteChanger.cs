using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    SpriteRenderer spriteRenderer; // 스프라이트를 변경할 스프라이트 렌더러
    public string spritePath = "PlayerSprite"; // Resources 폴더 안의 경로 (확장자 제외)

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        // 캐릭터가 생존 상태인 경우에만 스프라이트를 업데이트
        if(UIManager.Instance.getCurrentLife() > 0)
            LoadAndSetSprite();
    }
    void LoadAndSetSprite()
    {
        Sprite[] newSprites = Resources.LoadAll<Sprite>(spritePath);

        int characterIndex = DataManager.instance.characterNum;
        if (characterIndex >= 0 && characterIndex < newSprites.Length)
        {
            spriteRenderer.sprite = newSprites[characterIndex];
        }
        else
        {
            Debug.LogError("Invalid characterNum: " + characterIndex);
        }

    }
}
