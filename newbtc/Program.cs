
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using newbtc;
using PricePredictML.Livedata;
using System.Linq;
using System.Reflection;
using static Microsoft.ML.ForecastingCatalog;
ApiHelper.InitializeClient();
Priceprocessor live = new Priceprocessor();
live.Loadbtcinformation();

string rootDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../"));
string modelPath = Path.Combine(rootDir, "MLModel.zip");
string trainingFilePath = Path.Combine(rootDir, "Data", "trainingData.csv");
string testingFilePath = Path.Combine(rootDir, "Data", "testingData.csv");

MLContext mlContext = new MLContext();



IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(trainingFilePath, hasHeader: true);
IDataView testingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(testingFilePath, hasHeader: true);

var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(outputColumnName: nameof(ModelOutput.PredictedPrice),
    inputColumnName: nameof(ModelInput.Price),
    windowSize: 7,
    seriesLength: 30,
    trainSize: 90,
    horizon: 7,
    confidenceLevel: 0.95f,
    confidenceLowerBoundColumn: "Features",
    confidenceUpperBoundColumn: "Features");
    

SsaForecastingTransformer forecaster = forecastingPipeline.Fit(trainingDataView);
Evaluate x = new  Evaluate();
x.MEvaluate(testingDataView, forecaster, mlContext);
var forecastEngine = forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
forecastEngine.CheckPoint(mlContext, modelPath);

void Forecast(IDataView testData, int horizon, TimeSeriesPredictionEngine<ModelInput, ModelOutput> forecaster, MLContext mlContext)
{
    ModelOutput forecast = forecaster.Predict();
    IEnumerable<string> forecastOutput =
    mlContext.Data.CreateEnumerable<ModelInput>(testData, reuseRowObject: false)
        .Take(horizon)
        .Select((ModelInput price, int index) =>
        {

            float actualPrice = price.Price;

            float estimate = forecast.PredictedPrice[index];

            return
                   $"Actual Price: {actualPrice}\n" +
                   $"Forecast: {estimate}\n";
                  
       });
    Console.WriteLine("Price Forecast");
    Console.WriteLine("---------------------");
    foreach (var prediction in forecastOutput)
    {
        Console.WriteLine(prediction);
    }
}
Forecast(testingDataView, 7, forecastEngine, mlContext);
