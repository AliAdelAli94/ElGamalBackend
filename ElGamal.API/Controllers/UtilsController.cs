using LawFirm.CommonUtilitis.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ElGamal.API.Controllers
{
    public class UtilsController : ApiController
    {
        [HttpPost]
        [Route("Utils/UploadImages/")]
        public async Task<HttpResponseMessage> UploadImages()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }

                var folderName = HttpContext.Current.Request.Form["FolderName"];
                var oldProp = HttpContext.Current.Request.Form["OldProp"];

                var provider = GetMultipartProvider(folderName);
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                string originalFileName = string.Empty;
                string extension = string.Empty;
                string newName = string.Empty;
                FileInfo uploadedFileInfo = null;
                string targetFile = string.Empty;
                List<string> filesUrls = new List<string>();

                foreach (var item in result.FileData)
                {
                    originalFileName = GetDeserializedFileName(item);
                    extension = originalFileName.Split('.')[1];
                    newName = Guid.NewGuid().ToString() + '.' + extension;
                    uploadedFileInfo = new FileInfo(item.LocalFileName);
                    targetFile = Path.Combine(uploadedFileInfo.DirectoryName, newName);
                    uploadedFileInfo.MoveTo(targetFile);
                    filesUrls.Add("/images/" + folderName + '/' + newName);
                }


                if (HttpContext.Current.Request.Form["EditMode"] == "true")
                {
                    List<string> imagesUrls = new List<string>();
                    imagesUrls = (HttpContext.Current.Request.Form["oldImagesUrls"] != null) ? HttpContext.Current.Request.Form["oldImagesUrls"].Split(';').ToList() : null;
                    if(imagesUrls != null)
                    {
                        string urlTemp = string.Empty;
                        foreach (var item in imagesUrls)
                        {
                            var url = new Uri(item);
                            oldProp = '~' + url.LocalPath;
                            oldProp = HttpContext.Current.Server.MapPath(oldProp);
                            if (File.Exists(oldProp))
                                File.Delete(oldProp);
                        }
                    }
                    
                }

                var response = Request.CreateResponse(HttpStatusCode.OK, filesUrls);

                return response;

            }
            catch (Exception ex)
            {
                ErrorLogger.LogDebug(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new ObjectContent(ex.GetType(), ex, new JsonMediaTypeFormatter()) };
            }
        }

        private MultipartFormDataStreamProvider GetMultipartProvider(string foldername)
        {
            var uploadFolder = HttpContext.Current.Server.MapPath("~/images/" + foldername);

            if (Directory.Exists(uploadFolder) == false) Directory.CreateDirectory(uploadFolder);

            return new MultipartFormDataStreamProvider(uploadFolder);
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        private string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }

    }
}
