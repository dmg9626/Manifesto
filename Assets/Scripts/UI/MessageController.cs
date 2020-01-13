using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    /// <summary>
    /// List of messages to show
    /// </summary>
    [SerializeField]
    private List<Message> messages;

    [Header("UI Elements")]
    
    /// <summary>
    /// UI Text element to show messages in
    /// </summary>
    [SerializeField]
    private Text textElement;

    /// <summary>
    /// Icon that tells the player to click
    /// </summary>
    [SerializeField]
    private Image leftClickIndicator;

    [Header("Other Settings")] 

    /// <summary>
    /// Duration of message fade transition
    /// </summary>
    [SerializeField]
    private float transitionDuration = 1;

    [SerializeField]
    private int currentMessageIndex = 0;

    void Start()
    {
        Message message = messages[currentMessageIndex];
        showmessageCoroutine = StartCoroutine(ShowMessage(messages[0]));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMessageIndex < messages.Count)
        {
            // Show next message when coroutine ends
            if (showmessageCoroutine == null)
            {
                currentMessageIndex++;
                Message message = messages[currentMessageIndex];

                showmessageCoroutine = StartCoroutine(ShowMessage(message));
            }
        }
        
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
        StartCoroutine(FadeIn(textElement));
        yield return new WaitForSeconds(transitionDuration);

        // Wait for duration of message duration
        yield return new WaitForSeconds(message.duration);

        // Fade out
        StartCoroutine(FadeOut(textElement));
        yield return new WaitForSeconds(transitionDuration);

        // Set coroutine instance to null before returning
        showmessageCoroutine = null;
        yield return null;
    }

    /// <summary>
    /// Smoothly fades a graphic from invisible to opaque
    /// </summary>
    /// <param name="element">UI element to fade</param>
    /// <returns></returns>
    IEnumerator FadeIn(Graphic element)
    {
        // Fade in
        for (float t = 0; t < 1; t+= Time.deltaTime / transitionDuration) {
            float alpha = Mathf.Lerp(0, 1, t);
            SetAlpha(element, alpha);

            yield return null;
        }
    }

    /// <summary>
    /// Smoothly fades a graphic from opaque to invisible
    /// </summary>
    /// <param name="element">UI element to fade</param>
    /// <returns></returns>
    IEnumerator FadeOut(Graphic element)
    {
        // Fade out
        for (float t = 1; t > 0; t-= Time.deltaTime / transitionDuration) {
            float alpha = Mathf.Lerp(0, 1, t);
            SetAlpha(element, alpha);

            yield return null;
        }
    }

    void SetAlpha(Graphic element, float alpha)
    {
        Color color = element.color;
        color.a = alpha;
        element.color = color;
    }
}
