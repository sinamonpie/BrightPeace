using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
/// <summary>
/// 활성화되면 칼관련된 기능 수행
/// 활성화는 아이템 매니저에서 동작함
/// </summary>
public class KnifeManager : MonoBehaviour
{
    UseItemManager UseItemManager;
    GameObject knife;
    BoxCollider knifeCollider;
    Animator animator;

    [Header("공격 딜레이 시간")]
    [SerializeField]
    float SwingDelay = 2.5f;
    float rate;
    bool isSwingReady;
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        UseItemManager = FindObjectOfType<UseItemManager>();
        knife = UseItemManager.setKnife;
        knifeCollider = knife.GetComponent<BoxCollider>();
        knifeCollider.enabled = false;
        rate = 0f;
    }

    void Update()
    {
        rate += Time.deltaTime;
        isSwingReady = rate > SwingDelay;

        if (Input.GetButtonDown("Fire1") && isSwingReady) 
        {
            animator.SetTrigger("isSwing");
            StopCoroutine("Swing");
            StartCoroutine("Swing");
            rate = 0;
        }
    }

    IEnumerator Swing() // 나이프 피격 시간 설정
    {
        yield return new WaitForSeconds(0.1f);
        knifeCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        knifeCollider.enabled = false;
    }

    void HitPlayer()
    {

    }
}
