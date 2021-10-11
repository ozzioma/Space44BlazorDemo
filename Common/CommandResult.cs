using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Common
{
    [Serializable]
    //[JsonObject(IsReference = false)]
    public class CommandResult<T> : ProblemDetails
    {
        public CommandResult()
        {
            Status = StatusCodes.Status200OK;
        }

        //[JsonPropertyName("response")]
        [JsonProperty("response")] public T Response { get; set; }

        //[JsonPropertyName("errorFlag")]
        [JsonProperty("errorFlag")] public bool ErrorFlag { get; set; }

        //[JsonPropertyName("errorMessage")]
        [JsonProperty("message")] public string Message { get; set; }


        //[JsonPropertyName("validationErrors")]
        [JsonProperty("validationErrors", IsReference = false)]
        public List<ModelValidationError> ValidationErrors { get; set; }
    }
}