using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codealong_pie_project.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public string Address { get; set; }
        public string Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //overrida process och skapa eget HTML-element
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "mailto:" + Address);
            //Innanför tag?
            output.Content.SetContent(Content);


            //base.Process(context, output);
        }
    }
}
