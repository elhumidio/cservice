using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class Retry
    {
        public static async void Do(
            Func<Task> action,
            TimeSpan retryInterval,
            int maxAttemptCount = 5)
        {
            await Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        public static async Task<T> Do<T>(
            Func<Task<T>> action,
            TimeSpan retryInterval,
            int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return await action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            var aggragateExcption = new AggregateException(exceptions);
            //Log.WriteKibanaLogToFile(aggragateExcption);
            throw aggragateExcption;
        }
    }
}
