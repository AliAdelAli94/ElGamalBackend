using ElGamal.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ElGamal.Controls
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;
                if (currentUrl.Contains("product-details"))
                {
                    string productID = currentUrl.Split('/').Last();
                    string apiUrl = WebConfigurationManager.AppSettings["WebApiUrl"] + "Product/GetProductById/" + productID;
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string json = client.DownloadString(apiUrl);

                    ProductDetailsDTO data = (new JavaScriptSerializer()).Deserialize<ProductDetailsDTO>(json);
                    if(data != null)
                    {
                        if(data.CurrentProduct != null)
                        {
                            this.AddMetaTagsToFacebook(data.CurrentProduct.name,
                                data.CurrentProduct.description, data.CurrentProduct.images.FirstOrDefault().imageUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void AddMetaTagsToFacebook(string title, string description, string imageUrl)
        {
            try
            {
                Header.Controls.Add(new HtmlMeta { Name = "og:title", Content = title });
                Header.Controls.Add(new HtmlMeta { Name = "og:description", Content = description });
                Header.Controls.Add(new HtmlMeta { Name = "og:url", Content = HttpContext.Current.Request.Url.AbsoluteUri });
                Header.Controls.Add(new HtmlMeta { Name = "og:image", Content = imageUrl });
                Header.Controls.Add(new HtmlMeta { Name = "og:image:alt", Content = "Product image" });
                Header.Controls.Add(new HtmlMeta { Name = "og:image:height", Content = "200" });
                Header.Controls.Add(new HtmlMeta { Name = "og:image:width", Content = "200" });

            }
            catch (Exception ex)
            {
            }
        }
    }
}