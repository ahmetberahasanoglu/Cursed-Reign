using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourglassCollectable : Collectable
{
    public override void OnCollect()
    {
      ActionsListener.OnHourglassCollected();
    }

   
}
