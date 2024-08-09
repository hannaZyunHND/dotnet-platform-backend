using Hangfire;
using Hangfire.Annotations;
using Hangfire.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.FlashSale.Repository;
using PlatformWEBAPI.Services.Order.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Utility
{
    internal class RecurringJobsService : BackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IRecurringJobManager _recurringJobs;
        private readonly ILogger<RecurringJobScheduler> _logger;
        private readonly IFlashSaleRepository _flashSale;
        private readonly IExtraRepository _extra;
        private readonly IOrderRepository _orderRepository;

        public RecurringJobsService(
            [NotNull] IBackgroundJobClient backgroundJobs,
            [NotNull] IRecurringJobManager recurringJobs,
            [NotNull] ILogger<RecurringJobScheduler> logger,
            IFlashSaleRepository flashSale,
            IExtraRepository extra,
            IOrderRepository orderRepository)
        {
            _backgroundJobs = backgroundJobs ?? throw new ArgumentNullException(nameof(backgroundJobs));
            _recurringJobs = recurringJobs ?? throw new ArgumentNullException(nameof(recurringJobs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _flashSale = flashSale;
            _extra = extra;
            _orderRepository = orderRepository;
        }

        //public IRecurringJobManager RecurringJobs => _recurringJobs;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                //_backgroundJobs.Enqueue<Services>(x => x.LongRunning(JobCancellationToken.Null));
                //_backgroundJobs.Enqueue(() => )
                //_recurringJobs.RemoveIfExists("minutely");
                _recurringJobs.AddOrUpdate("minutely", () => _flashSale.AutoUpdateFlashSale(), Cron.Minutely);
                _recurringJobs.AddOrUpdate("backup", () => _extra.AutoBackupDatabase(), Cron.Weekly);
                //_recurringJobs.RemoveIfExists("sendMailNotiMessage");
                //_recurringJobs.RemoveIfExists("sendMailNotiFeedback");
                _recurringJobs.AddOrUpdate("sendMailToSupplier", () => _orderRepository.SendRequestOrderToSupplierInBackground(), Cron.Minutely);
                _recurringJobs.AddOrUpdate("sendMailNotiMessage", () => _orderRepository.ResponseGetLastChatDetailBySessionForCustomer(), Cron.MinuteInterval(10));
                _recurringJobs.AddOrUpdate("sendMailNotiFeedback", () => _orderRepository.GetOrderNotificationFeedback(), Cron.Daily);

            }
            catch (Exception e)
            {
                _logger.LogError("An exception occurred while creating recurring jobs.", e);
            }

            return Task.CompletedTask;
        }
    }
}
