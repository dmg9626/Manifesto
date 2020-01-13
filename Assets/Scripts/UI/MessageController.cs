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
    
    /// <summary>
    /// UI Text element to show messages in
    /// </summary>
    [SerializeField]
    private Text textElement;

    [Header("Other Settings")] 

    /// <summary>
    /// Duration of message fade transition
    /// </summary>
    [SerializeField]
    private float transitionDuration = 1;
    
    [SerializeField]
    private AnimationCurve fadeAnimationCurve;


    private int currentMessageIndex = 0;

    void Start()
    {
        showmessageCoroutine = StartCoroutine(ShowMessage(messages[0]));
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        // Set invisible before fading in
        SetAlpha(0);

        Debug.Log("Fading in...");
        // Fade in
        for (float t = 0; t < 1; t+= Time.deltaTime / transitionDuration) {
            // Update alpha with lerp function each frame
            float alpha = Mathf.Lerp(0, 1, t);
            Debug.Log("Alpha -> " + alpha);
            SetAlpha(alpha);

            yield return null;
        }
        SetAlpha(1);

        Debug.Log("Waiting...");
        // Wait for duration of message duration
        yield return new WaitForSeconds(message.duration);

        Debug.Log("Fading out...");

        // Fade out
        for (float t = 1; t > 0; t-= Time.deltaTime / transitionDuration) {
            // Update alpha with lerp function each frame
            float alpha = Mathf.Lerp(0, 1, t);
            Debug.Log("Alpha -> " + alpha);
            SetAlpha(alpha);

            yield return null;
        }
        SetAlpha(0);

        // Set coroutine instance to null before returning
        showmessageCoroutine = null;
        yield return null;
    }

    void SetAlpha(float alpha)
    {
        Color color = textElement.color;
        color.a = alpha;
        textElement.color = color;
    }
}
