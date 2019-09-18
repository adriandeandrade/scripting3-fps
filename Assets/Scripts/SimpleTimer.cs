using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleTimer
{
   private float startTime;
   private float endTime;

   public SimpleTimer()
   {

   }

   public SimpleTimer(float time)
   {
       SetTimer(time);
   }

   public float GetRatio()
   {
       return Mathf.Min((Time.time - startTime) / (endTime - startTime), 1.0f);
   }

   public void SetTimer(float time)
   {
       startTime = Time.time;
       endTime = startTime + time;
   }
}
