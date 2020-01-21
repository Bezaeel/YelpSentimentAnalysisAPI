using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Yelp.Model.Model.DataModels;

namespace Yelp.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;
        public AnalyzeController(PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        [HttpGet]
        [Route("Run")]
        public ActionResult<string> perform([FromQuery]string sentimentText)
        {
            ModelInput sampleData = new ModelInput()
            {
                Col0 = sentimentText
            };

            //predict sentiment
            ModelOutput prediction = _predictionEnginePool.Predict(sampleData);
            string retVal = string.Empty;
            var probabilityToPercent = prediction.Probability * 100;
            if (prediction.Prediction)
            {
                retVal = $"'{sentimentText}' is {probabilityToPercent}% sentiment positive";
            }
            else
            {
                retVal = $"'{sentimentText}' is {probabilityToPercent}%  sentiment negative";
            }
            return retVal;
        }
    }
}