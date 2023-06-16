using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        private ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        [SecuredOperation("user")]
        [ValidationAspect(typeof(CarValidator))]
        [TransactionScopeAspect]
        public IResult AddCar(Car car)
        {
            _carDal.Add(car);
            return new SuccessResult(Messages.CarAddedSuccessfully);
        }

        //[SecuredOperation("user")]
        [ValidationAspect(typeof(CarValidator))]
        [TransactionScopeAspect]
        public IResult DeleteCar(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.CarDeletedSuccessfully);
        }

        //[SecuredOperation("user")]
        [PerformanceAspect(10)]
        public IDataResult<List<Car>> GetAllCars()
        {
            var result = _carDal.GetAll();
            return new SuccessDataResult<List<Car>>(result, Messages.CarsListedSuccessfully);
        }
        
        //[SecuredOperation("user")]
        [PerformanceAspect(10)]
        public IDataResult<List<CarForListDto>> ListAllCars()
        {
            var result = _carDal.ListAllCars();
            return new SuccessDataResult<List<CarForListDto>>(result, Messages.CarsListedSuccessfully);
        }

        //[SecuredOperation("user")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(CarValidator))]
        public IResult UpdateCar(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.CarUpdatedSuccessfully);
        }
    }
}
