using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    [SerializeField] Transform player; // ตัวผู้เล่นที่กล้องจะติดตาม
    //[SerializeField] float smoothTime = 0.125f; // ความเร็วในการเคลื่อนที่ของกล้อง
    [SerializeField] float baseSmoothTime; 
    public Vector2 offset; // การชดเชยของกล้องจากตำแหน่งผู้เล่น
    [SerializeField] float boundaryThreshold; // ระยะจากขอบมุมกล้องที่ผู้เล่นต้องเข้าใกล้ถึงจะให้กล้องเลื่อน

    private Camera cam;
    private Vector3 velocity = Vector3.zero; // ความเร็วของกล้อง
    
    // ติดตามผู้เล่นในแกน Y โดยปรับตำแหน่ง Y ของกล้อง
    [SerializeField] float baseSmoothTimeY; // ความเร็วการติดตามในแกน Y
    
    private float camHalfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        camHalfWidth = cam.orthographicSize * cam.aspect;
    }

    void LateUpdate()
    {
        // ตรวจสอบว่า player ยังไม่ถูกลบ
        if (player == null)
        {
            Debug.LogWarning("Player ถูกลบออกจากเกมแล้ว, กล้องจะไม่ตามผู้เล่นอีกต่อไป");
            return; // ออกจากฟังก์ชันถ้า Player ถูกลบ
        }
        
        // รับตำแหน่งขอบของหน้าจอกล้อง
        float camHalfWidth = cam.orthographicSize * cam.aspect; // ความกว้างครึ่งหนึ่งของกล้อง
        float leftBoundary = cam.transform.position.x - camHalfWidth + boundaryThreshold; // ขอบซ้าย
        float rightBoundary = cam.transform.position.x + camHalfWidth - boundaryThreshold; // ขอบขวา
        
        Vector3 desiredPosition = cam.transform.position;
        
        

        // ถ้าผู้เล่นเดินเข้าใกล้ขอบซ้ายหรือขวาของกล้อง
        if (player.position.x < leftBoundary)
        {
            // เลื่อนกล้องไปทางซ้าย
            desiredPosition.x = player.position.x - camHalfWidth + boundaryThreshold;
        }
        else if (player.position.x > rightBoundary)
        {
            // เลื่อนกล้องไปทางขวา
            desiredPosition.x = player.position.x + camHalfWidth - boundaryThreshold;
        }
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);

        // คำนวณความเร็วของผู้เล่น
        float playerSpeed = Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x);

        // ปรับค่าความเร็วกล้องตามความเร็วของผู้เล่น
        float dynamicSmoothTime = baseSmoothTime / (playerSpeed + 1); // +1 เพื่อหลีกเลี่ยงการหารด้วยศูนย์
        
        // ติดตามผู้เล่นในแกน Y โดยปรับตำแหน่ง Y ของกล้อง
        desiredPosition.y = player.position.y + offset.y; // ใช้ offset.y เพื่อปรับตำแหน่งกล้องในแนวดิ่ง
        
        float smoothY = Mathf.SmoothDamp(cam.transform.position.y, desiredPosition.y, ref velocity.y, baseSmoothTimeY);
        
        // การเคลื่อนกล้องอย่างราบรื่น
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref velocity, baseSmoothTime);
    }
}
