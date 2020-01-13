using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message
{
    [Header("Text Settings")]

    /// <summary>
    /// Text to show
    /// </summary>
    public string text;

    /// <summary>
    /// Font to use
    /// </summary>
    [SerializeField]
    private Font font;

    /// <summary>
    /// Font size
    /// </summary>
    [SerializeField]
    private int fontSize = 60;

    [Header("Other Settings")]

    /// <summary>
    /// Duration to show text
    /// </summary>
    public float duration = 3;

    /// <summary>
    /// If true, specified settings will be applied to boids
    /// </summary>
    public bool setBoidSettings = false;

    /// <summary>
    /// Settings to apply to boids (only used if setBoidSettings is true)
    /// </summary>
    public Boid.Settings boidSettings;
}
