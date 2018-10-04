using UnityEngine;
using System.Collections;

using mg.org;

public class MoveByADSW : MonoBehaviour
{

    [SerializeField]
    public Camera m_camera;
    CameraTPerson m_cameraTP;


    //人物移动速度
    public int moveSpeed = 20;


    Vector3 m_cameraDir;
    Vector3 m_movDir = new Vector3();

    //初始化人物位置
    public void Awake()
    {

        m_cameraTP = ComponentUtil.EnsureComponent<CameraTPerson>(m_camera.gameObject);
        m_cameraTP.SetTarget(transform);
        m_cameraTP.SetDefault(30, 30);

        ComponentUtil.EnsureComponent<CameraTPMouse>(m_camera.gameObject);

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //获取控制的方向， 上下左右， 
        float KeyVertical = Input.GetAxis("Vertical");
        float KeyHorizontal = Input.GetAxis("Horizontal");
        //Debug.Log("keyVertical" + KeyVertical);
        //Debug.Log("keyHorizontal" + KeyHorizontal);
        
        Move(KeyHorizontal, KeyVertical);
    }

    void Move(float dirX_, float dirZ_)
    {

        m_movDir.x = dirX_;
        m_movDir.z = dirZ_;

        if (m_movDir.x == 0 && m_movDir.z == 0)
            return;

        m_cameraDir = m_camera.transform.eulerAngles;


        if (m_movDir.sqrMagnitude > 1)
        {
            //最大速度长度保持是1
            m_movDir.Normalize();
        }

        m_movDir *= Time.deltaTime;

        var rotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(rotation.x, m_cameraDir.y, rotation.z);

        transform.Translate(m_movDir * moveSpeed, Space.Self);

        //计算面向
        //float angle = Vector3.Angle(m_movDir, Vector3.right) * -Mathf.Sign(dirZ_);  //轴方向不解,每个轴的正方向在哪里? 答: 因为是左手坐标系

    }



}
