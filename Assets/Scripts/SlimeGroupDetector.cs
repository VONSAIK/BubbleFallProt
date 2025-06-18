using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;

public class SlimeGroupDetector : MonoBehaviour, IService
{
    private HexGridMatrix _matrix;
    private EventBus _eventBus;
    private const int MinGroupSize = 3;

    public void Init()
    {
        _matrix = ServiceLocator.Current.Get<HexGridController>().GetMatrix();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscride<SlimeAttachedSignal>(OnSlimeAttached);
    }

    private void OnSlimeAttached(SlimeAttachedSignal signal)
    {
        Vector2Int? hexOpt = _matrix.GetHexOfSlime(signal.Slime);
        if (!hexOpt.HasValue)
        {
            Debug.LogWarning("—лайм не маЇ координат у с≥тц≥.");
            return;
        }

        Vector2Int hex = hexOpt.Value;
        SlimeColor color = signal.Slime.SlimeColor;

        HashSet<Vector2Int> group = _matrix.GetConnectedGroup(hex, color);

        if (group.Count >= MinGroupSize)
        {
            Debug.Log($"«найдено групу з {group.Count} слайм≥в.");
            _eventBus.Invoke(new SlimeGroupPoppedSignal(group));
        }
        else
        {
            _eventBus.Invoke(new SlimeStepDownSignal());
        }


        _matrix.DebugPrintGrid();
    }
    private IEnumerator InvokeStepDownNextFrame()
    {
        yield return null;
        
    }


}
