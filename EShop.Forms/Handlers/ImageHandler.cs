using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;

namespace EShop.Forms.Handlers
{
    public class ImageHandler : IHttpHandler
    {

        private static bool UseCached(HttpRequest req)
        {
            try
            {
                var ifmod = req.Headers["If-Modified-Since"];
                if (string.IsNullOrEmpty(ifmod))
                {
                    ifmod = req.Headers["Last-Modified"];
                }
                DateTime d;
                if (DateTime.TryParse(ifmod, out d))
                {
                    return d.ToUniversalTime().AddHours(4) >= DateTime.Now.ToUniversalTime();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            //if (UseCached(context.Request))
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.NotModified;
            //    context.Response.SuppressContent = true;
            //}
            //else
            //{
            var mode = request.QueryString["mode"] != null ? request.QueryString["mode"].ToLower() : "prev";
            int imageId = 1000000;
            int.TryParse(request.QueryString["id"], out imageId);

            var cacheKey = string.Format("{0}_{1}", mode, imageId);
            string localImagePath;
            if (context.Cache[cacheKey] == null)
            {

                var localImgFolder = context.Server.MapPath("~/Images");
                var fileNamePrefix = "ico";
                if (mode.Equals("full")) { fileNamePrefix = "img"; }
                var fileName = string.Format("{0}_{1}.jpg", fileNamePrefix, imageId);
                localImagePath = Path.Combine(localImgFolder, fileName);
                if (!File.Exists(localImagePath))
                {
                    localImagePath = Path.Combine(localImgFolder, string.Format("{0}_{1}.jpg", fileNamePrefix, 1000000));
                }
                context.Cache.Insert(cacheKey, localImagePath, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30), CacheItemPriority.Normal, null);

            }
            localImagePath = context.Cache[cacheKey] as string;

            var response = context.Response;
            response.BufferOutput = false;

            try
            {
                using (var fs = new FileStream(localImagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    response.ContentType = "image/gif";
                    response.AppendHeader("Content-Length", fs.Length.ToString(CultureInfo.InvariantCulture));
                    //response.Cache.SetLastModified(DateTime.Now);
                    //response.Cache.SetCacheability(HttpCacheability.Public);
                    fs.CopyTo(response.OutputStream, 64 * 1024);
                }
            }
            catch
            {

            }
        }
        //}
    }
}