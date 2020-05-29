using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Textract.Internal;
using Amazon.Textract;
using Amazon.Textract.Model;
using S3Object = Amazon.Textract.Model.S3Object;
using System.IO;

namespace TextRekognition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        // POST api/values
        [HttpPost]
        public IActionResult Post()
        {

            AmazonTextractClient objTextExtract = new AmazonTextractClient("AKIA23XZGIB6BEUBA5HM", "ePmW8ZsldWdQP3hmillAOMLu+A5b4ePgeBV3+/O5", Amazon.RegionEndpoint.USEast1);
            DetectDocumentTextRequest request = new DetectDocumentTextRequest();
            byte[] fileData = null;

            using (var ms = new MemoryStream())
            {
                HttpContext.Request.Form.Files[0].CopyTo(ms);
                fileData = ms.ToArray();
                string s = Convert.ToBase64String(fileData);
                // act on the Base64 data
            }
            request.Document = new Document
            {
                Bytes = new MemoryStream(fileData)
            };

            var task = UploadImageForRekognition(objTextExtract, request);
            task.Wait();
            var result = task.Result;
            return Ok(result);

        }

        private async Task<DetectDocumentTextResponse> UploadImageForRekognition(AmazonTextractClient objRekClient, DetectDocumentTextRequest objDetectText)
        {
            return await objRekClient.DetectDocumentTextAsync(objDetectText);
        }
    }
}
