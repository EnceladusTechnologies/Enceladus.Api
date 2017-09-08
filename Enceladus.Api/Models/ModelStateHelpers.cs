using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Enceladus.Api.Models
{
    public static class ModelStateHelpers
    {
        public static string AggregateErrors(ModelStateDictionary result)
        {
            var toReturn = string.Empty;
            bool firstPass = true;
            foreach (var item in result.Values)
            {
                if (item.Errors.Count > 0)
                {

                    foreach (var error in item.Errors)
                    {
                        if (!firstPass)
                        {
                            toReturn += Environment.NewLine;
                        }
                        if (error.Exception != null)
                        {
                            toReturn += error.Exception.Message + "  ";
                            if (error.Exception.InnerException != null)
                                toReturn += error.Exception.InnerException.Message + "  ";
                        }
                        if (!string.IsNullOrEmpty(error.ErrorMessage))
                        {
                            toReturn += error.ErrorMessage + "  ";
                        }

                        firstPass = false;
                    }

                }
            }
            return toReturn;
        }
        public static string AggregateErrors(Exception result)
        {
            var toReturn = result.Message;
            if (result.InnerException != null)
                toReturn += Environment.NewLine + "   " + result.InnerException.Message;
            return toReturn;
        }
    }
}
