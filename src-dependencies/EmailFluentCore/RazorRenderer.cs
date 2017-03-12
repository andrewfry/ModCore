using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorLight.Extensions;
using RazorLight.Templating;

namespace EmailFluentCore
{
    public class RazorRenderer : ITemplateRenderer
    {
        public RazorRenderer()
        {
        }

        public string Parse<T>(string template, T model, bool isHtml = true)
        {
            //var pageFactory = new DefaultPageFactory(a =>
            //{
            //    return new RazorLight.Compilation.CompilationResult()
            //    { 

            //    };
            //});
            //IPageLookup pageLookup = new DefaultPageLookup(pageFactory);

            //var engine = EngineFactory.CreateEmbedded(typeof(RazorRenderer));
            //var templateManager = 
            //var engineCore = new EngineCore()

            // var engine = new RazorLightEngine(pageLookup);

            var engine = EngineFactory.CreatePhysical("/");

            // string result = new LightRazorEngine().ParseString(template);
            try {
                return engine.ParseString(template, model, typeof(T));

            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
