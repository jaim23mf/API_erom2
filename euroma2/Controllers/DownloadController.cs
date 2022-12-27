﻿using euroma2.Models;
using Microsoft.AspNetCore.Mvc;

namespace euroma2.Controllers
{
    public class DownloadController : Controller
    {
        [HttpGet("FloorGltf/{id}")]
        [HttpGet("LogoImg/{id}")]
        [HttpGet("PromoImg/{id}")]
        [HttpGet("ReachImg/{id}")]
        [HttpGet("ServiceImg/{id}")]
        [HttpGet("StoreImg/{id}")]
        public IActionResult ReturnStream(string id)
        {
            string route = Request.Path.Value;
            
            var basePath = Path.Combine(Directory.GetCurrentDirectory()+ route);
            return File(System.IO.File.ReadAllBytes(basePath), Consts.MimeGltf, System.IO.Path.GetFileName(basePath));
        }
 
    }
}
