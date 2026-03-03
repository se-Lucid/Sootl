using System;
using UnityEngine;

public class BobController : MonoBehaviour
{
    [Header("Settings")]
    public bool sway = true;
    public bool swayRotation = true;
    public bool bobOffset = true;
    public bool bobSway = true;

    float smooth = 10f;
    float smoothRot = 12f;

    Vector2 walkInput;
    Vector2 lookInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        //BobRotation();

        CompositePositionRotation();
    }
    void GetInput()
    {
        walkInput.x = Input.GetAxisRaw("Horizontal");
        walkInput.y = Input.GetAxisRaw("Vertical");

        lookInput.x = Input.GetAxisRaw("Mouse X");
        lookInput.y = Input.GetAxisRaw("Mouse Y");
    }

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;
    void Sway()
    {
        if (sway == false) { swayPos = Vector3.zero; return; }

        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    void SwayRotation()
    {
        if (swayRotation == false) { swayEulerRot = Vector3.zero; return; }

        Vector2 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot), Time.deltaTime * smoothRot);
    }

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;

    Vector3 bobPosition;

    void BobOffset()
    {
        //Get the variable that determines if the player is grounded
        speedCurve += Time.deltaTime * (5f) + 0.01f;

        if (bobOffset == false)
        {
            bobPosition = Vector3.zero; return;
        }
        else if (MathF.Abs(walkInput.x) > .05f || MathF.Abs(walkInput.y) > .05f)
        {
            bobPosition.y = (curveSin * (bobLimit.y + .025f));
        }
        else
        {
            bobPosition.y = (curveSin * bobLimit.y);
        }
    }
}

