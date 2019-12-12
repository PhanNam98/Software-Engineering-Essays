using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_MLML.Model;
using Microsoft.ML;
using Microsoft.AspNetCore.Mvc;

namespace BDS_ML.Controllers
{
    public class PredictController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Predict(ModelInput input)
        {
            //load the model
            MLContext mLContext = new MLContext();
            //create predection engine related to the loaded train model
            ITransformer mlModel = mLContext.Model.Load(@"..\BDS_MLML.Model\MLModel.zip", out var modelInputSchema);
            var predEngine = mLContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            //Try model on sample data to predict price
            ModelOutput result = predEngine.Predict(input);

            ViewBag.Price = result.Score;
            ViewBag.PriceVN = result.Score*23172;
       
            return View(input);
        }
    }
}