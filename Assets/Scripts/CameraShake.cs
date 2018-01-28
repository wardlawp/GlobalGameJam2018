using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera m_camera; // set this via inspector
    public float m_shake = 0;
    public float m_shakeDuration = 2f;
    public float m_shakeAmount = 0.7f;
    public float m_decreaseFactor = 1f;

    private Vector3 m_originalRelPos;

    void Start()
    {
        m_originalRelPos = transform.localPosition;
    }

    public void Shake ()
    {
        m_shake = m_shakeDuration;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_shake > 0)
        {
            m_camera.transform.localPosition = Random.insideUnitSphere * m_shakeAmount;
            m_shake = Mathf.Max(0f, m_shake - Time.deltaTime * m_decreaseFactor);
            if (m_shake == 0f)
            {
                transform.localPosition = m_originalRelPos;
            }
        }
    }
}
