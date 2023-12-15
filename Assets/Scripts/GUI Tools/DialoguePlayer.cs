using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialoguePlayer : MonoBehaviour
{
    [Header("Playback Settings")]
    [SerializeField, Range(0f, 1f)] protected float playbackSpeed;
    [SerializeField, Range(0f, 1f)] protected float randomAspect;
    [SerializeField, Range(0f, 10f)] protected float lifetime;
    [SerializeField, CanBeNull] protected Transform focusPosition;
    [Header("Dialogues")]
    [SerializeField] protected TMP_Text text;
    [SerializeField, Multiline] protected string[] dialogues;

    protected new AudioSource audio;
    protected Coroutine coroutine = null;
    protected Camera mainCamera;
    protected CameraController mainCameraController;
    protected Player localPlayer;

    protected Transform Target { 
        set
        {
            if (value != null)
                mainCameraController.Target = value;
        }
    }

    protected void Awake()
    {
        audio = GetComponent<AudioSource>();
        localPlayer = FindAnyObjectByType<Player>();
        mainCamera = Camera.main;
        mainCameraController = mainCamera.GetComponent<CameraController>();
    }

    protected IEnumerator Playback(int id)
    {
        localPlayer.enabled = false;
        text.text = "";
        Target = focusPosition;
        foreach (char _ch in dialogues[id])
        {
            text.text += _ch;
            audio.Play();
            yield return new WaitForSeconds(playbackSpeed + Random.Range(0f, randomAspect));
        }
        yield return new WaitForSeconds(lifetime);
        text.text = "";
        coroutine = null;
        localPlayer.enabled = true;
        Target = localPlayer.transform;
    }

    protected IEnumerator Playback()
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            coroutine = StartCoroutine(Playback(i));
            while (coroutine != null) yield return null;
        }
    }

    public void PlayDialogue(int id)
    {
        if (coroutine == null)
            coroutine = StartCoroutine(Playback(id));
    }

    public void PlayDialogue()
    {
        if (coroutine == null)
            StartCoroutine(Playback());
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            text.text = "";
            localPlayer.enabled = true;
            Target = localPlayer.transform;
        }
    }
}
