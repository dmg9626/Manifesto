using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    [Header("Message Settings")]

    /// <summary>
    /// Duration of message fade transition
    /// </summary>
    [SerializeField]
    private float messageTransitionDuration = 1;

    /// <summary>
    /// UI Text element to show messages in
    /// </summary>
    [SerializeField]
    private Text textElement;

    /// <summary>
    /// List of messages to show
    /// </summary>
    [SerializeField]
    private List<Message> messages;


    [Header("Left Click Indicator Settings")]

    /// <summary>
    /// Icon that tells the player to click
    /// </summary>
    [SerializeField]
    private Image leftClickIndicator;
    
    /// <summary>
    /// Time to wait before showing left click indicator
    /// </summary>
    [SerializeField]
    private float leftClickDelay;

    /// <summary>
    /// Duration of fade in/out transition for left click indicator
    /// </summary>
    [SerializeField]
    private float leftClickFadeDuration;

    /// <summary>
    /// Time to keep left click indicator on screen for
    /// </summary>
    [SerializeField]
    private float leftClickShowDuration;

    private int currentMessageIndex = 0;

    void Start()
    {
        StartCoroutine(ShowLeftClickIndicator(leftClickDelay, leftClickFadeDuration, leftClickShowDuration));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMessageIndex < messages.Count)
        {
            // Show next message when coroutine ends
            if (showmessageCoroutine == null)
            {
                Message message = messages[currentMessageIndex];
                showmessageCoroutine = StartCoroutine(ShowMessage(message));
            }
        }
    }

    IEnumerator ShowLeftClickIndicator(float delay, float fadeDuration, float duration)
    {
        // Wait for delay before showing indicator
        SetAlpha(leftClickIndicator, 0);
        yield return new WaitForSeconds(delay);

        // Fade in
        StartCoroutine(FadeIn(leftClickIndicator, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);

        // Keep it on screen for a bit
        yield return new WaitForSeconds(duration);

        // Wait until the player clicks to hide the indicator
        while(!Input.GetMouseButton(0)) {
            yield return null;
        }

        // Fade out
        StartCoroutine(FadeOut(leftClickIndicator, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);
    }

    /// <summary>
    /// Running instance of ShowMessage (null if not currently running)
    /// </summary>
    private Coroutine showmessageCoroutine;

    /// <summary>
    /// Shows a message on the screen with fade in/fade out transitions
    /// </summary>
    /// <param name="message">Message to show</param>
    IEnumerator ShowMessage(Message message)
    {
        // Update UI with message text
        textElement.text = message.text;
        SetAlpha(textElement, 0);
        
        // Fade in
        StartCoroutine(FadeIn(textElement, messageTransitionDuration));
        yield return new WaitForSeconds(messageTransitionDuration);

        // Wait for duration of message duration
        yield return new WaitForSeconds(message.duration);

        // Fade out
        StartCoroutine(FadeOut(textElement, messageTransitionDuration));
        yield return new WaitForSeconds(messageTransitionDuration);

        // Increment current message index
        currentMessageIndex++;

        // Set coroutine instance to null before returning
        showmessageCoroutine = null;
        yield return null;
    }

    /// <summary>
    /// Smoothly fades a graphic from invisible to opaque
    /// </summary>
    /// <param name="element">UI element to fade</param>
    /// <returns></returns>
    IEnumerator FadeIn(Graphic element, float duration)
    {
        // Fade in
        for (float t = 0; t < 1; t+= Time.deltaTime / messageTransitionDuration) {
            float alpha = Mathf.Lerp(0, 1, t);
            SetAlpha(element, alpha);

            yield return null;
        }
        SetAlpha(element, 1);
    }

    /// <summary>
    /// Smoothly fades a graphic from opaque to invisible
    /// </summary>
    /// <param name="element">UI element to fade</param>
    /// <returns></returns>
    IEnumerator FadeOut(Graphic element, float duration)
    {
        // Fade out
        for (float t = 1; t > 0; t-= Time.deltaTime / messageTransitionDuration) {
            float alpha = Mathf.Lerp(0, 1, t);
            SetAlpha(element, alpha);

            yield return null;
        }
        SetAlpha(element, 0);
    }

    void SetAlpha(Graphic element, float alpha)
    {
        Color color = element.color;
        color.a = alpha;
        element.color = color;
    }
}
