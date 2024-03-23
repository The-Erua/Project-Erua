using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorFilterAdjustment : MonoBehaviour
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    // 새벽 색상
    private Color earlyMorningColor = new Color(253.0f / 255.0f, 127.0f / 255.0f, 127.0f / 255.0f);

    void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
        colorAdjustments.active = false;
    }

    public void DeActiveColorAdjustment()
    {
        if (volume == null || colorAdjustments == null)
            return;
        // 마우스 오른쪽 버튼을 떼었을 때
        if (Input.GetMouseButtonUp(1))
        {
            colorAdjustments.active = false; // ColorAdjustments 비활성화
        }
    }

    public void ActiveColorAdjustment()
    {
        if (volume == null || colorAdjustments == null)
            return;

        volume.profile.TryGet(out colorAdjustments);
        colorAdjustments.active = true; // ColorAdjustments 활성화
    }
}