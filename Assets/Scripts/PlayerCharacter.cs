using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    PlayerController controller;
    Rigidbody2D rigid;
    Animator anim;
    public float speed = 3;
    public float jumpSpeed = 8;
    public int maxHp = 5;
    int hp;
    bool jump = false;
    bool isGround = false;
    bool faceLeft = true;
    float outControlTime = 0; // ʧȥ���Ƶ�ʱ�䣨����֡��

    Transform checkGround;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        checkGround = transform.Find("CheckGround");
        hp = maxHp;
    }

    void Update()
    {
        if(controller.jump)
        {
            jump = true;
        }

        // ���¶���״̬
        anim.SetBool("IsGround", isGround);
        anim.SetFloat("Speed", Mathf.Abs(controller.h));
        if(controller.attack)
        {
            anim.SetTrigger("Attack");
        }
    }

    // ȥ����״̬���м�⣬�Ƿ��ڹ���״̬���Ƿ��������Ź���������
    bool IsAttacking()
    {
        AnimatorStateInfo asi = anim.GetCurrentAnimatorStateInfo(0);
        // IsName�жϵ�ǰ��������
        return asi.IsName("Attack1") || asi.IsName("Attack2") || asi.IsName("Attack3");
    }
    private void FixedUpdate()
    {
        CheckGround();
        // ����ֵΪ����ִ��������߼�
        if (!IsAttacking())
        {
            Move(controller.h);
        }

        jump = false;
        outControlTime--;
    }
    private void Move(float h)
    {
        if(outControlTime > 0) { return; }
        Flip(h);
        float vy = rigid.velocity.y;
        if(jump && isGround)
        {
            anim.SetTrigger("Jump");
            vy = jumpSpeed;
        }


        rigid.velocity = new Vector2(h * speed, vy);
    }

    void CheckGround()
    {
        isGround = Physics2D.OverlapCircle(checkGround.position, 0.1f,~LayerMask.GetMask("Player"));
    }
    void Flip(float h)
    {
        Vector3 scaleLeft = new Vector3(1, 1, 1);
        Vector3 scaleRight = new Vector3(-1, 1, 1);
        if(h > 0.1f)
        {
            faceLeft = false;
            transform.localScale = scaleRight;
        }
        else if(h < -0.1f)
        {
            faceLeft = true;
            transform.localScale = scaleLeft;
        }
    }

    // Gizemos���
    private void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            Gizmos.color = Color.white;
            if (isGround)
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawSphere(checkGround.position, 0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Boss") || collision.transform.CompareTag("BossHit"))
        {
            GetHit(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("BossHit"))
        {
            GetHit(1);
        }
    }
    void GetHit(int damage)
    {
        //Debug.Log("get hit" + damage);
        hp -= damage;
        if(hp < 0) { hp = 0; }

        UIManager.Instance.SetPlayerHp(hp, maxHp);

        // ���˶���
        anim.SetTrigger("GetHit");

        // ����ʱ���򷴷��򵯷�
        Vector2 force = new Vector2(100, 200);
        if(!faceLeft)
        {
            force.x *= -1;
        }
        rigid.AddForce(force);

        outControlTime = 30;
    }
}
