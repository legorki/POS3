using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    internal class TextPost : Post
    {
        public string Content { get; }

        public TextPost(User user, string title,string content) : base(user, title)    
        {
            Content = content;
        }
        
    }

}
