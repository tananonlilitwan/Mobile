using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplatform2 : MonoBehaviour
{
    public Transform posA, posB, posC; // เพิ่ม posC
    [SerializeField] float Speed;
    Vector3 targetPos; // เปลี่ยนเป็น Vector3
    private bool movingForward = true; // ตัวแปรสำหรับเช็คทิศทางการเคลื่อนที่
    void Update()
    {
        // ตรวจสอบว่ามีการเคลื่อนที่วัตถุขณะที่เมนูหยุดเกมหรือไม่
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.unscaledDeltaTime);
    }
    private AudioSource platformAudioSource; // AudioSource สำหรับแพลตฟอร์ม

    public AudioClip moveplatformClip; // คลิปเสียงแพลตฟอร์ม
    
    // Start is called before the first frame update
    void Start()
    {
        targetPos = posA.position; // เริ่มต้นที่ posA
        StartCoroutine(MovePlatform()); // เรียก Coroutine
        // เริ่มเล่นเสียงของแพลตฟอร์ม
        platformAudioSource.Play();
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            // เคลื่อนที่ไปยังตำแหน่งเป้าหมาย
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);

            // เช็คว่าถึงตำแหน่งเป้าหมายหรือยัง
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                // หยุดและรอ 2 วินาที
                yield return new WaitForSeconds(2f);

                // เปลี่ยนตำแหน่งเป้าหมาย
                if (movingForward)
                {
                    if (targetPos == posA.position)
                    {
                        targetPos = posB.position;
                    }
                    else if (targetPos == posB.position)
                    {
                        targetPos = posC.position;
                    }
                    else if (targetPos == posC.position)
                    {
                        movingForward = false; // เปลี่ยนทิศทางเมื่อถึง posC
                    }
                }
                else // ถ้ากลับมา
                {
                    if (targetPos == posC.position)
                    {
                        targetPos = posB.position;
                    }
                    else if (targetPos == posB.position)
                    {
                        targetPos = posA.position;
                    }
                    else if (targetPos == posA.position)
                    {
                        movingForward = true; // เปลี่ยนทิศทางเมื่อถึง posA
                    }
                }
            }

            yield return null; // รอจนถึงเฟรมถัดไป
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(posA.position, posB.position);
        Gizmos.DrawLine(posB.position, posC.position);
        Gizmos.DrawLine(posC.position, posA.position); // วาดเส้นเชื่อมระหว่าง posC และ posA
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ตรวจสอบว่า collision กับ Player หรือ Box หรือ Small Box หรือ Big Box
        if (collision.gameObject.CompareTag("Player"))
        {
            // ตั้งให้ Player เป็นลูกของ moveplatform
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // ตรวจสอบว่า collision กับ Player หรือ Box หรือ Small Box หรือ Big Box
        if (collision.gameObject.CompareTag("Player"))
        {
            // ลบการตั้งเป็นลูกของ moveplatform
            collision.gameObject.transform.SetParent(null);
        }
    }
}
