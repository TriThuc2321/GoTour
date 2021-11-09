using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoTour.Database
{
     public class ImagePlaceStream
    {
        public string id;
        public List<Stream> imgs;

        public ImagePlaceStream(string id, List<Stream> imgs)
        {
            this.id = id;
            this.imgs = imgs;
        }
    }
}
