using LittleApp.Common.Enums;
using LittleApp.Common.Exceptions;
using LittleApp.Common.Extensions;
using LittleApp.Common.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleApp.Api.ActionResults;

public class ValidationProblemDetailsResult : IActionResult
{
    public Task ExecuteResultAsync(ActionContext context)
    {
        if (context.ModelState.IsValid)
        {
            return Task.CompletedTask;
        }

        var invalidModelStates = context.ModelState
            .ToDictionary(ms => ms.Key)
            .Where(ms => ms.Value.Value.ValidationState == ModelValidationState.Invalid);

        var validationResults = new List<ValidationResult>();

        foreach (var modelState in invalidModelStates)
        {
            ModelErrorCollection errors = modelState.Value.Value.Errors;

            var validationResult = new ValidationResult
            {
                Property = modelState.Key.ToCamelCase(),
                Errors = errors
                    .Select(e => string.IsNullOrEmpty(e.ErrorMessage)
                        ? e.Exception.Message
                        : e.ErrorMessage)
                    .ToList()
            };

            validationResult.Errors = validationResult.Errors.Distinct().ToList();
            validationResults.Add(validationResult);
        }

        throw new ValidationException(validationResults.Select(result => new ExceptionDetail
        {
            ErrorCode = ErrorCode.RequestInvalid,
            Params = result
        }).ToList());
    }
}
