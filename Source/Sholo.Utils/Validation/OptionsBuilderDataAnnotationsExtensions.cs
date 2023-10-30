using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Sholo.Utils.Validation;

[PublicAPI]
public static class OptionsBuilderDataAnnotationsExtensions
{
    public static OptionsBuilder<TOptions> ValidateDataAnnotations<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] TOptions>(this OptionsBuilder<TOptions> optionsBuilder, bool recurse, bool allowAsync = true)
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(sp => new MiniValidatorValidateOptions<TOptions>(sp, optionsBuilder.Name, recurse, allowAsync));
        return optionsBuilder;
    }
}
