using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Scheduler
{
    public class ScheduledJobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ScheduledJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;

            var job = (IJob)serviceProvider.GetService(jobDetail.JobType);
            return job;
        }

        public void ReturnJob(IJob job) { }
    }
}
