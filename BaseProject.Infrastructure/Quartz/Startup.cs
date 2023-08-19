using BaseProject.Application.TaskScheduler.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace BaseProject.Infrastructure.Quartz;

public static class Startup
{
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        return services
            .AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                #region SendOtp

                var sendOtpJob = new JobKey("sendOtp");
                q.AddJob<SendOneTimePasswordJob>(opts => opts.WithIdentity(sendOtpJob));

                q.AddTrigger(opts => opts
                    .ForJob(sendOtpJob)
                    .WithIdentity("sendOtp-trigger")
                    .WithCronSchedule("0/1 * * * * ?"));

                #endregion
                
                
            }).AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}