using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Filters
{
    public class ValidateModelFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var entry in context.ActionArguments)
            {
                if (entry.Value != null)
                {
                    var modelType = entry.Value.GetType();
                    var validateModelMethod = typeof(ApiBaseController) // 替换为你的控制器基类
                        .GetMethod("ValidateModel")
                        .MakeGenericMethod(modelType);

                    var validateTask = (Task)validateModelMethod.Invoke(context.Controller, new[] { entry.Value });
                    await validateTask;

                    var validationResult = (dynamic)validateTask.GetType().GetProperty("Result").GetValue(validateTask);

                    if (!validationResult.IsSuccess)
                    {
                        context.Result = new ObjectResult(validationResult)
                        {
                            StatusCode = validationResult.Status
                        };
                        return;
                    }
                }
            }

            await next();
        }
    }

}
