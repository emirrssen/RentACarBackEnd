using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
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

        [CacheRemoveAspect("IBrandService.Get")]
        [ValidationAspect(typeof(BrandValidator))]
        [TransactionScopeAspect]
        [SecuredOperation("user")]
        public IResult AddBrand(Brand brand)
        {
            _brandDal.Add(brand);
            return new SuccessResult(Messages.BrandAddedSuccessfully);
        }

        [CacheRemoveAspect("IBrandService.Get")]
        [ValidationAspect(typeof(BrandValidator))]
        [TransactionScopeAspect]
        [SecuredOperation("user")]
        public IResult DeleteBrand(Brand brand)
        {
            _brandDal.Delete(brand);
            return new SuccessResult(Messages.BrandDeletedSuccessfully);
        }

        [CacheAspect]
        [PerformanceAspect(10)]
        //[SecuredOperation("user")]
        public IDataResult<List<Brand>> GetAllBrands()
        {
            var result = _brandDal.GetAll();
            return new SuccessDataResult<List<Brand>>(result, Messages.BrandsListedSuccessfully);
        }

        [CacheRemoveAspect("IBrandService.Get")]
        [ValidationAspect(typeof(BrandValidator))]
        [TransactionScopeAspect]
        [SecuredOperation("user")]
        public IResult UpdateBrand(Brand brand)
        {
            _brandDal.Update(brand);
            return new SuccessResult(Messages.BrandUpdatedSuccessfully);
        }
    }
}
