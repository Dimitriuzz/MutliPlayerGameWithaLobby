using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private Camera m_MiniMapCamera;
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_InterpolationLinear;
    [SerializeField] private float m_InterpolationAngular;
    [SerializeField] private float m_CameraZOffset;
    [SerializeField] private float m_ForwardOffset;

    private PhotonView _myView;

    private void Start()
    {
        var cameras = FindObjectsOfType<Camera>();
        if (cameras[0].transform.name == "Main Camera")
        {
            m_Camera = cameras[0];
            m_MiniMapCamera = cameras[1];
        }
        else
        {
            m_Camera = cameras[1];
            m_MiniMapCamera = cameras[0];
        }

        _myView = GetComponent<PhotonView>();
        if (!_myView.IsMine)
        
            foreach (var c in FindObjectsOfType<Camera>()) Destroy(c.gameObject);
    }
    private void FixedUpdate()
    {
        if (m_Target == null || m_Camera == null) return;

        Vector2 camPos = m_Camera.transform.position;
        Vector2 targetPos = m_Target.position + m_Target.transform.up * m_ForwardOffset;

        Vector2 newCamPos = Vector2.Lerp(camPos, targetPos, m_InterpolationLinear * Time.deltaTime);

        m_Camera.transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset);

        m_MiniMapCamera.transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset-200);



        if(m_InterpolationAngular>0)
        {
            m_Camera.transform.rotation = Quaternion.Slerp(m_Camera.transform.rotation,
                m_Target.rotation, m_InterpolationAngular * Time.deltaTime);

        }
    }

    public void SetTarget(Transform newTarget)
    {
        m_Target = newTarget;
    }

}
