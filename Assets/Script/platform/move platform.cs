using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplatform : MonoBehaviour
{
    #region <ชื่อ>
    /*
    float dirX, moveSpeed = 4f;
    bool moveRight = true;
    
    void Update()
    {
        if (transform.position.x > 16.45f)
        {
            moveRight = false;
        }

        if (transform.position.x < 23.86f)
        {
            moveRight = true;
        }

        if (moveRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
    }*/
    #endregion
    
   public Transform posA, posB;
    [SerializeField] float Speed;
    Vector2 targetPos;

    private AudioSource platformAudioSource; // AudioSource สำหรับแพลตฟอร์ม
    public AudioClip moveplatformClip; // คลิปเสียงแพลตฟอร์ม

    private bool isMoving; // ตัวแปรเพื่อตรวจสอบว่าแพลตฟอร์มกำลังเคลื่อนที่หรือไม่

    private void Awake()
    {
        // หา AudioSource ที่ติดอยู่กับแพลตฟอร์ม
        platformAudioSource = gameObject.AddComponent<AudioSource>();

        // ตั้งค่า AudioSource ให้ใช้เสียง moveplatform
        platformAudioSource.clip = moveplatformClip;
        platformAudioSource.loop = true; // ตั้งค่าให้เสียงนี้วนลูป
    }

    // Start is called before the first frame update
    void Start()
    {
        targetPos = posB.position;
    }

    // Update is called once per frame
    void Update()
    {
        // ตรวจสอบระยะห่างกับตำแหน่ง posA และ posB
        if (Vector2.Distance(transform.position, posA.position) < .1f)
        {
            targetPos = posB.position;
        }
        if (Vector2.Distance(transform.position, posB.position) < .1f)
        {
            targetPos = posA.position;
        }

        // แพล็ตฟอร์มเคลื่อนที่
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);

        // ตรวจสอบว่ามีการเคลื่อนไหวหรือไม่
        bool movingNow = Vector2.Distance(transform.position, targetPos) > 0.01f;

        // เปิดเสียงเมื่อแพลตฟอร์มเคลื่อนที่ และหยุดเสียงเมื่อแพลตฟอร์มหยุด
        if (movingNow && !isMoving)
        {
            platformAudioSource.Play(); // เริ่มเล่นเสียงเมื่อเริ่มเคลื่อนที่
            isMoving = true;
        }
        else if (!movingNow && isMoving)
        {
            platformAudioSource.Stop(); // หยุดเสียงเมื่อหยุดเคลื่อนที่
            isMoving = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(posA.position, posB.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ตรวจสอบว่า collision กับ Player หรือ Box หรือ Small Box หรือ Big Box
        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.CompareTag("Box") || 
            collision.gameObject.CompareTag("Small Box") || 
            collision.gameObject.CompareTag("Big Box"))
        {
            // ตั้งให้ Player หรือวัตถุอื่นเป็นลูกของ moveplatform
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // ตรวจสอบว่า collision กับ Player หรือ Box หรือ Small Box หรือ Big Box
        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.CompareTag("Box") || 
            collision.gameObject.CompareTag("Small Box") || 
            collision.gameObject.CompareTag("Big Box"))
        {
            // ลบการตั้งเป็นลูกของ moveplatform
            collision.gameObject.transform.SetParent(null);
        }
    }


    
    
    
}
