using System.Collections.Generic;
using Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Config
{
    public static class WebValidationUtils
    {
        public static List<ModelValidationError> AllModelErrors(this ModelStateDictionary modelState)
        {
            var result = new List<ModelValidationError>();


            foreach (KeyValuePair<string, ModelStateEntry> modelStatePair in modelState)
            {
                string key = modelStatePair.Key;
                ModelStateEntry modelStateItem = modelStatePair.Value;

                foreach (ModelError error in modelStateItem.Errors)
                {
                    var entry = new ModelValidationError
                    {
                        FieldName = key,
                        Error = error.ErrorMessage,
                        Description = error.Exception?.Message,
                        Value = modelStateItem.RawValue,
                        //Value = modelStateItem.AttemptedValue
                    };
                    result.Add(entry);
                }
            }


            return result;
        }
    }
}