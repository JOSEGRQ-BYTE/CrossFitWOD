using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CrossFitWOD.Extensions
{
    public class SQLServerHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _Configuration; 
        public SQLServerHealthCheck(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_Configuration["ConnectionStrings:DefaultConnection"]))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT 1";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                }
                catch (DbException exception)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus, exception: exception);
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}

