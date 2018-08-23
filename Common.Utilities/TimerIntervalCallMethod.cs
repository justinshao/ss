using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
   public class TimerIntervalCallMethod<T>
   {
       public event CallMethodEventHandler callMethodEventHandler;
       public static Dictionary<string,DateTime>  DICTIONARY_LAST_EXCUTE_TIME = new Dictionary<string,DateTime>();
       private static object lockObj=new object();
       private static object LOCKLAST_EXCUTE_TIME = new object();
       private int timerInterval = 0;
       public TimerIntervalCallMethod(int timerInterval,string key) {
           this.timerInterval = timerInterval;
           this.key = key;
       }
       public void Excute(T obj) {
           if (CanExcute()) {
               if (callMethodEventHandler != null) {
                   SetLastExcuteTime();
                   callMethodEventHandler(obj);
               }
           }
       }
       public delegate void CallMethodEventHandler(T obj) ;
       private bool  CanExcute() {
           lock (lockObj) {
               if (GetLastExcuteTime().AddSeconds(timerInterval) < DateTime.Now || GetLastExcuteTime() == DateTime.MinValue){
                   return true;
               }
           }
           return false;
       }
       private void SetLastExcuteTime() {
           lock (LOCKLAST_EXCUTE_TIME) {
               if (DICTIONARY_LAST_EXCUTE_TIME.ContainsKey(key))
                     DICTIONARY_LAST_EXCUTE_TIME[key] = DateTime.Now;
               else
                   DICTIONARY_LAST_EXCUTE_TIME.Add(key,DateTime.Now);
           }
       }
       private DateTime GetLastExcuteTime()
       {
           lock (LOCKLAST_EXCUTE_TIME)
           {
               if (DICTIONARY_LAST_EXCUTE_TIME.ContainsKey(key))
                      return   DICTIONARY_LAST_EXCUTE_TIME[key];
               else
                  return DateTime.MinValue;
           }
       }

       public string key { get; set; }
   }
}
