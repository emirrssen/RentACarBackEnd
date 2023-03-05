using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.BrandId).NotNull();
            RuleFor(x => x.ColorId).NotNull();
            RuleFor(x => x.ModelYear).NotNull();
            RuleFor(x => x.DailyPrice).NotNull();
            RuleFor(x => x.Description).NotNull();
            RuleFor(x => x.Name).MinimumLength(2);
            RuleFor(x => x.DailyPrice).GreaterThan(0);
        }
    }
}
