using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Dtos;
using FluentValidation;

namespace BankBros.Billing.Business.Validation.FluentValidation
{
    public class PayBillDtoValidator : AbstractValidator<PayBillDto>
    {
        public PayBillDtoValidator()
        {
            RuleFor(x => x.AccountNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Hesap Numarası"))
                .GreaterThan(1000)
                .WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Hesap Numarası", "{ComparisionValue}"))
                .LessThanOrEqualTo(2000)
                .WithMessage(string.Format(ValidationMessages.MustBeLessThan, "Hesap Numarası", "{ComparisionValue}"));
        }
    }
}
