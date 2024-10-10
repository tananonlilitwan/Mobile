using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlPlayer : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    private Vector2 moveDirection;
    private Rigidbody2D rb; // อ้างอิงถึง Rigidbody2D

    public Joystick joystick; // อ้างอิงถึง Joystick

    public float jumpForce = 10f;  // แรงกระโดด
    private bool isGrounded = false;  // ตรวจสอบว่าผู้เล่นยืนอยู่บนพื้นหรือไม่
    public LayerMask groundLayer;  // ระบุเลเยอร์ของพื้นดิน

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // เข้าถึง Rigidbody2D ของ Player
    }

    void Update()
    {
        // ใช้ค่า x ของ Joystick เพื่อกำหนดทิศทางการเคลื่อนที่
        moveDirection.x = joystick.Horizontal();
        // หากค่า x เป็นศูนย์ ให้หยุดการเคลื่อนที่
        if (moveDirection.x == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // หยุดการเคลื่อนที่ในแนวนอน
        }
    }

    // ใช้ FixedUpdate ในการจัดการฟิสิกส์
    void FixedUpdate()
    {
        // ตรวจสอบว่าผู้เล่นอยู่บนพื้นดิน
        CheckGrounded();

        // คำนวณตำแหน่งใหม่ของ Player
        Vector2 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // อัปเดตตำแหน่งของ Player
        rb.MovePosition(targetPosition);
    }

    // ฟังก์ชันกระโดด
    public void Jump()
    {
        Debug.Log("Jump button clicked"); // ตรวจสอบการกดปุ่ม
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            Debug.Log("Player jumped!"); // ตรวจสอบว่าผู้เล่นกระโดด
        }
    }



    // ตรวจสอบว่าผู้เล่นอยู่บนพื้นดินหรือไม่
    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, groundLayer);
        isGrounded = hit.collider != null;
        Debug.Log("Is Grounded: " + isGrounded); // ดูค่า isGrounded ใน Console
    }


}