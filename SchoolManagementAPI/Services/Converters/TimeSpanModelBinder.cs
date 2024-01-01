using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Services.Converters
{
    public class TimeSpanModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var valueAsString = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(valueAsString))
            {
                return Task.CompletedTask;
            }

            if (TimeSpan.TryParseExact(valueAsString, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out var timeSpan))
            {
                bindingContext.Result = ModelBindingResult.Success(timeSpan);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid TimeSpan format");
            }

            return Task.CompletedTask;
        }
    }

    public class TimeSpanModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(TimeSpan))
            {
                return new BinderTypeModelBinder(typeof(TimeSpanModelBinder));
            }

            return null;
        }
    }
}
