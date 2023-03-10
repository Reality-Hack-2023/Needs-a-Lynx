
//   ==============================================================================
//   | Lynx HandTracking Sample : LynxInterfaces (2022)                           |
//   | Author : GC & Geoffrey Marhuenda & Cedric Morel Francoz                    |
//   |======================================                                      |
//   | LynxButton Script                                                          |
//   | Script to set a UI element as Button.                                      |
//   ==============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LynxButtonTimer : Button
{
    #region SCRIPT ATTRIBUTES
    // Public attributes
    [SerializeField] public Vector3 m_moveDelta = Vector3.forward;
    [SerializeField] public float m_moveDuration = 0.5f;
    [SerializeField] public bool m_isUsingScale = false;

    [SerializeField] private Image m_timerImage;
    [SerializeField] private float m_deltaTime = 2.0f;

    [SerializeField] public UnityEvent OnPress;
    [SerializeField] public UnityEvent OnUnpress;
    [SerializeField] public UnityEvent OnTimerPress;

    [SerializeField] public List<ColorBlock> themes = new List<ColorBlock>();

    // Private attributes
    private bool m_isRunning = false; // Avoid multiple press or unpress making the object in unstable state
    private bool m_isCurrentlyPressed = false; // Status of the current object
    private Vector3 m_basePose = Vector3.zero; // Store base position when pressed.

    private bool m_bIsRunning = false;

    private IEnumerator coroutine = null;

    // Properties
    public new bool IsPressed { get => m_isCurrentlyPressed; }
    #endregion


    #region UNITY API
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -m_moveDelta.z);
        m_isCurrentlyPressed = false;
        m_isRunning = false;
        m_bIsRunning = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion


    #region UI EVENTS
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        base.OnPointerUp(eventData);
        StartCoroutine(UnpressingCoroutine());

        m_bIsRunning = false;
        StopCoroutine(coroutine);

        m_timerImage.fillAmount = 0.0f;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        base.OnPointerDown(eventData);
        StartCoroutine(PressingCoroutine());

        if (!m_bIsRunning)
        {
            m_bIsRunning = true;
            coroutine = TimerCoroutine();
            StartCoroutine(coroutine);
        }
    }

    public void ChangeTheme(int index)
    {
        if(index <= themes.Count - 1) colors = themes[index];
        else  Debug.LogWarning("Index not found.");
    }
    #endregion


    #region UI ANIMATIONS
    /// <summary>
    /// Press animation forward.
    /// </summary>
    private IEnumerator PressingCoroutine()
    {
        if (!m_isRunning && !m_isCurrentlyPressed)
        {
            m_isRunning = true;

            float elapsedTime = 0.0f;


            m_basePose = this.transform.localPosition;
            Vector3 forwardPose = m_basePose;
            if (m_isUsingScale)
            {
                forwardPose.x += m_moveDelta.x * this.transform.localScale.x;
                forwardPose.y += m_moveDelta.y * this.transform.localScale.y;
                forwardPose.z += m_moveDelta.z * this.transform.localScale.z;
            }
            else
            {
                forwardPose += m_moveDelta;
            }


            // Forward
            while (elapsedTime < m_moveDuration)
            {
                this.transform.localPosition = Vector3.Lerp(m_basePose, forwardPose, elapsedTime / m_moveDuration);
                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
            }

            this.transform.localPosition = forwardPose;

            m_isCurrentlyPressed = true;
            m_isRunning = false;

            OnPress.Invoke();
        }
    }

    /// <summary>
    /// Press animation backward.
    /// </summary>
    private IEnumerator UnpressingCoroutine()
    {
        while (m_isRunning)
            yield return new WaitForEndOfFrame();

        if (m_isCurrentlyPressed)
        {
            m_isRunning = true;

            float elapsedTime = 0.0f;
            Vector3 forwardPose = this.transform.localPosition;

            // Backward
            while (elapsedTime < m_moveDuration)
            {
                this.transform.localPosition = Vector3.Lerp(forwardPose, m_basePose, elapsedTime / m_moveDuration);
                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
            }

            this.transform.localPosition = m_basePose;

            m_isCurrentlyPressed = false;
            m_isRunning = false;

            OnUnpress.Invoke();
        }
    }

    /// <summary>
    /// Timer animation.
    /// </summary>
    IEnumerator TimerCoroutine()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < m_deltaTime)
        {
            elapsedTime += Time.deltaTime;
            m_timerImage.fillAmount = elapsedTime / m_deltaTime;
            yield return new WaitForEndOfFrame();
        }

        OnTimerPress.Invoke();
        m_timerImage.fillAmount = 0f;

        m_bIsRunning = false;
    }
    #endregion
}