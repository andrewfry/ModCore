using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Site
{
    public class JsonSuccessResult : JsonObjectResult
    {
        public override string Status { get { return "success"; } }

        public override string[] Errors { get { return null; } }

        public JsonSuccessResult(object value) : base(value, 200)
        {

        }
    }

    public class JsonFailResult : JsonObjectResult
    {
        public override string Status { get { return "fail"; } }

        private string[] _errors;
        public override string[] Errors { get { return _errors; } }

        public JsonFailResult(object value) : base(value, 200)
        {
            _errors = new string[0];
        }

        public JsonFailResult(object value, params string[] errors) : base(value, 200)
        {
            _errors = errors;
        }
    }

    public abstract class JsonObjectResult : ObjectResult
    {
        public abstract string Status { get; }

        public abstract string[] Errors { get; }

        public JsonObjectResult(object value, int statusCode) :
            base(value)
        {
            this.StatusCode = statusCode;
        }
    }
}
