using System;
using System.Collections;
using UnityEngine;
public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void ResetAnimation()
    {
        // 게임 오브젝트가 활성화되어 있는지 확인
        if (animator != null && spriteRenderer != null && originalSprite != null)
        {
            animator.ResetTrigger("Destroy");
            animator.ResetTrigger("Hit");
            //animator.Play("Idle");

            if(gameObject.tag.Contains("Enemy"))
                spriteRenderer.sprite = originalSprite;
        }
    }

    
    public void setAnimTrigger(string name)
    {
        StartCoroutine(StartAnim(name));
    }
    public void TriggerDestroy(string destroyAnimationName, Action onComplete)
    {
        StartCoroutine(DisableAfterAnimation(destroyAnimationName, onComplete));
    }
    public void TriggerSkillDestroy(string destroyAnimationName, Action onComplete)
    {
        StartCoroutine(TriggerSkillThenDestroy(destroyAnimationName, onComplete));
    }

    private IEnumerator StartAnim(string name)
    {
        animator.SetTrigger(name);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
    }
    private IEnumerator DisableAfterAnimation(string destroyAnimationName, Action onComplete)
    {
        animator.SetTrigger(destroyAnimationName);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        spriteRenderer.sprite = originalSprite;
        onComplete?.Invoke(); //메서드를 통해 등록된 Action 함수를 Invoke하여 실행
    }
    private IEnumerator TriggerSkillThenDestroy(string destroyAnimationName, Action onComplete)
    {
        // Skill 애니메이션을 먼저 실행한 후에, 
        animator.SetTrigger("Skill");
        float skillAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(skillAnimationLength);

        // 파괴 애니메이션을 실행
        animator.SetTrigger(destroyAnimationName);
        float destroyAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(destroyAnimationLength);

        // 마지막에 Invoke에 등록된 메서드(대부분 SetActive 트리거) 실행 후 종료
        spriteRenderer.sprite = originalSprite;
        onComplete?.Invoke();
    }
}