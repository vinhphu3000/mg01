/* ==============================================================================
 * CameraFirstPerson
 * @author jr.zeng
 * 2017/8/9 16:02:03
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


    public class CameraFirstPerson : MonoBehaviour
    {
        

        Vector2 mouseSensitivity = new Vector2(5, 5);
        
        Vector2 limitAngleX = new Vector2(-60, 60);
        //上下最大视角(Y视角)  
        Vector2 limitAngleY = new Vector2(-60, 60);

        float rotationY = 0F;

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            if (mouseX == 0 && mouseY == 0)
                return;

            //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
            float rotationX = transform.localEulerAngles.y + mouseX * mouseSensitivity.x;
            //rotationX = Mathf.Clamp(rotationX, limitAngleX.x, limitAngleX.y);

            //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
            rotationY += mouseY * mouseSensitivity.y;
            //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value   
            rotationY = Mathf.Clamp(rotationY, limitAngleY.x, limitAngleY.y);

            //总体设置一下相机角度  
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        void Start()
        {
            // Make the rigid body not change rotation  
            //if (rigidbody)
            //rigidbody.freezeRotation = true;
            
            //Cursor.lockState = CursorLockMode.Confined;
            //Cursor.visible = true;
        }

        

    }
