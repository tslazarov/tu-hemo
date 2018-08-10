using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Users
{
    public class FacebookPictureModel
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public int Height { get; set; }
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
    }
}