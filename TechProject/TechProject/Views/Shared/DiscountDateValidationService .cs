using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tech.DataAccess.Repository.IRepository;

namespace TechProject.BackgroundServices
{
    public class DiscountDateValidationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DiscountDateValidationService> _logger;

        public DiscountDateValidationService(IServiceScopeFactory scopeFactory, ILogger<DiscountDateValidationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    _logger.LogInformation("Checking for discounts with StartDate == EndDate...");

                    var discounts = unitOfWork.Discount.GetAll().ToList();

                    foreach (var discount in discounts)
                    {
                        if (discount.StartDate.Date == discount.EndDate.Date && discount.IsActive)
                        {
                            discount.IsActive = false;
                            unitOfWork.Discount.Update(discount);
                            _logger.LogInformation($"Discount ID {discount.Id} deactivated because StartDate == EndDate.");
                        }
                    }

                    unitOfWork.Save();
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
