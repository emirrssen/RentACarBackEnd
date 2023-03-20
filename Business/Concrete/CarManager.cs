using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules;
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
        public IResult AddCar(Car car)
        {
            _carDal.Add(car);
            return new SuccessResult(Messages.CarAddedSuccessfully);
        }

        [ValidationAspect(typeof(CarValidator))]
        public IResult DeleteCar(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.CarDeletedSuccessfully);
        }

        [SecuredOperation("user")]
        public IDataResult<List<Car>> GetAllCars()
        {
            var result = _carDal.GetAll();
            return new SuccessDataResult<List<Car>>(result, Messages.CarsListedSuccessfully);
        }

        public IDataResult<Car> GetCarById(int carId)
        {
            var result = _carDal.Get(x => x.Id == carId);
            return new SuccessDataResult<Car>(result, Messages.CarListedSuccessfully);
        }

        public IDataResult<List<Car>> GetCarsByBrandId(int brandId)
        {
            var result = _carDal.GetAll(x => x.BrandId == brandId);
            return new SuccessDataResult<List<Car>>(result, Messages.CarsListedSuccessfully);
        }

        public IDataResult<List<Car>> GetCarsByColorId(int colorId)
        {
            var result = _carDal.GetAll(x => x.ColorId == colorId);
            return new SuccessDataResult<List<Car>>(result, Messages.CarsListedSuccessfully);    
        }

        public IDataResult<List<CarForListDto>> ListAllCars()
        {
            var result = _carDal.ListAllCars();
            return new SuccessDataResult<List<CarForListDto>>(result, Messages.CarsListedSuccessfully);
        }

        [ValidationAspect(typeof(CarValidator))]
        public IResult UpdateCar(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.CarUpdatedSuccessfully);
        }
    }
}
