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
    public class BrandManager : IBrandService
    {
        IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        [ValidationAspect(typeof(BrandValidator))]
        public IResult AddBrand(Brand brand)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _brandDal.Add(brand);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(BrandValidator))]
        public IResult DeleteBrand(Brand brand)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _brandDal.Delete(brand);
            return new SuccessResult();
        }

        public IDataResult<List<Brand>> GetAllBrands()
        {
            var result = _brandDal.GetAll();
            return new SuccessDataResult<List<Brand>>(result);
        }

        public IDataResult<Brand> GetBrandById(int brandId)
        {
            var result = _brandDal.Get(x => x.Id == brandId);
            return new SuccessDataResult<Brand>(result);
        }

        [ValidationAspect(typeof(BrandValidator))]
        public IResult UpdateBrand(Brand brand)
        {
            var aspectResults = AspectRules.Check(Results.ValidationResult);
            if (aspectResults != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _brandDal.Update(brand);
            return new SuccessResult();
        }
    }
}
