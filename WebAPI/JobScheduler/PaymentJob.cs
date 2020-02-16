// using Common;
// using DataAccess.Services;
// using Microsoft.Extensions.Logging;
// using Quartz;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace WebAPI.JobScheduler
// {
//     [DisallowConcurrentExecution]
//     public class PaymentJob : IJob
//     {
//         private readonly ILogger<PaymentJob> _logger;

//         public PaymentJob(ILogger<PaymentJob> logger)
//         {
//             _logger = logger;
//         }

//         public Task Execute(IJobExecutionContext context)
//         {
//             //mabil semua data yang akan dikirim
//             try
//             {
//                 var dataPushNotif = new PushNotificationService().GetDataPaymentPushNotif();
//                 foreach (var item in dataPushNotif)
//                 {
//                     List<Notification.TargetNotif> dataUser = new List<Notification.TargetNotif>();
//                     dataUser.Add(new Notification.TargetNotif() { deviceToken = item.deviceToken.ToString(), deviceType = item.deviceType.ToString() });

//                     Notification.SendNotif(dataUser,
//                         "Wujudkan Impianmu.", "Jangan lupa segera lakukan pembayaran " + item.title_dream.ToString());
//                 }
//             }
//             catch (Exception e)
//             {
//                 _logger.LogInformation(e.Message);
//             }

//             return Task.CompletedTask;
//         }
//     }
// }
