using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectMoving : SpawnedObject
{
    [SerializeField] private float magnitude;
    [SerializeField] private float frequency;

    public enum States
    {
        UpDown,
        Scale,
        Rotation
    }

    private States states;

    private float frequency_scaler = 1f;
    private float magnitude_scaler = 0.001f;

    private Coroutine _objectChangeRoutine;

    private void Start()
    {
        states = States.UpDown;
        _objectChangeRoutine = null;
    }

    void Update()
    {
        switch (states)
        {
            case States.UpDown:
                UpDownMoving();
                break;
            case States.Rotation:
                RotationMoving();
                break;
        }
    }

    public States getState()
    {
        States current_states = states;
        return current_states;
    }

    private void UpDownMoving()
    {
        gameObject.transform.position += Vector3.up
                                         * Mathf.Sin(Time.time * frequency * frequency_scaler)
                                         * magnitude * magnitude_scaler;
    }

    private void RotationMoving()
    {
        gameObject.transform.rotation *= Quaternion.Euler(0f, 0.5f, 0f);
    }

    public IEnumerator ChangeWaving(bool stop, float transitionTime)
    {
        float timer = 0;
        while (timer < transitionTime)
        {
            if (stop)
                magnitude_scaler = Mathf.Lerp(magnitude_scaler, 0f, timer / transitionTime);
            else
                magnitude_scaler = Mathf.Lerp(magnitude_scaler, 0.001f, timer / transitionTime);
            yield return null;
            timer += Time.deltaTime;
        }

        _objectChangeRoutine = null;
    }

    public IEnumerator SelectedObjectTransform()
    {
        ChangeWaving(true, 0.1f);

        float timer = 0;
        float q;
        while (timer < 1f)
        {
            q = Mathf.Lerp(0.25f, 0.5f, timer / 1);
            gameObject.transform.localScale = new Vector3(q, q, q);
            yield return null;
            timer += Time.deltaTime;
        }

        states = States.Rotation;
    }

    public IEnumerator UnselectedObjectTransform()
    {
        float timer = 0;
        float q;
        while (timer < 1f)
        {
            q = Mathf.Lerp(0.5f, 0.25f, timer / 1);
            gameObject.transform.localScale = new Vector3(q, q, q);
            yield return null;
            timer += Time.deltaTime;
        }

        ChangeWaving(false, 0.1f);

        states = States.UpDown;
    }

}
