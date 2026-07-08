using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using FluentValidation;

namespace DotCruz.Tenants.Application.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValid<T, TProperty, TValueObject>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, TValueObject> factory)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            if (value == null) return;
            if (value is string str && string.IsNullOrWhiteSpace(str)) return;

            try
            {
                factory(value);
            }
            catch (ErrorOnValidationException ex)
            {
                foreach (var error in ex.GetErrorsMessages())
                {
                    context.AddFailure(error);
                }
            }
        });
    }

    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValid<T, TProperty, TValueObject>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<T, TProperty, TValueObject> factory)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            if (value == null) return;
            if (value is string str && string.IsNullOrWhiteSpace(str)) return;

            try
            {
                factory(context.InstanceToValidate, value);
            }
            catch (ErrorOnValidationException ex)
            {
                foreach (var error in ex.GetErrorsMessages())
                {
                    context.AddFailure(error);
                }
            }
        });
    }
}
