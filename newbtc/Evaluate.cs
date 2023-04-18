using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newbtc
{
    public class Evaluate
    {
        public void MEvaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            IDataView predictions = model.Transform(testData);
            IEnumerable<float> actual =
            mlContext.Data.CreateEnumerable<ModelInput>(testData, true).Select(observed => observed.Price);
            IEnumerable<float> forecast =
            mlContext.Data.CreateEnumerable<ModelOutput>(predictions, true).Select(prediction => prediction.PredictedPrice[0]);
            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => (actualValue - forecastValue) / actualValue * 100);

            var MAE = metrics.Average(error => Math.Abs(error)); 
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); 

            // Output metrics
            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}%");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}%\n");
        }
    }
}
