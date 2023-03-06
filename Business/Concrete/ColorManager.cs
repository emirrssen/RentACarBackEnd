using Business.Abstract;
using Business.ValidationRules;
using Core.AspectMessages;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.AspectResults;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ColorManager : IColorService
    {
        IColorDal _colorDal;

        public ColorManager(IColorDal colorDal)
        {
            _colorDal = colorDal;
        }

        [ValidationAspect(typeof(ColorValidator))]
        public IResult AddColor(Color color)
        {
            var aspectResults = AspectRules.Check(Results.ValidationResult);
            if (aspectResults != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _colorDal.Add(color);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(ColorValidator))]
        public IResult DeleteColor(Color color)
        {
            var aspectResults = AspectRules.Check(Results.ValidationResult);
            if (aspectResults != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _colorDal.Delete(color);
            return new SuccessResult();
        }

        public IDataResult<List<Color>> GetAllColors()
        {
            var result = _colorDal.GetAll();
            return new SuccessDataResult<List<Color>>(result);
        }

        public IDataResult<Color> GetColorById(int colorId)
        {
            var result = _colorDal.Get(x => x.Id == colorId);
            return new SuccessDataResult<Color>(result);
        }

        [ValidationAspect(typeof(ColorValidator))]
        public IResult UpdateColor(Color color)
        {
            var aspectResults = AspectRules.Check(Results.ValidationResult);
            if (aspectResults != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _colorDal.Update(color);
            return new SuccessResult();
        }
    }
}
