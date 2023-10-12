using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    internal class ImagePost : Post
    {
        public  string Url { get; }

        public ImagePost(User user, string title,string url)      : base(user, title)
        {

            Url = url;
        }
        
    }

}
