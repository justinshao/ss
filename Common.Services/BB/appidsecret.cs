using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.BB;

namespace Common.Services.BB
{
   public class appidsecret
   {
       public static string appid() {
           return PublicDAL.Appid();
       }
       public static string appsecret() {
           return PublicDAL.Appsecret();
       }
   }
}
